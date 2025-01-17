using Strumenta.Entity.Semantics;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.SymbolResolution;
using Strumenta.Sharplasu.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Strumenta.Sharplasu.Traversing;
using System.Reflection;
using System.Text;

namespace Strumenta.Entity
{
    public interface IModuleFinder
    {
        Module FindModule(string moduleName);
    }

    public class SimpleModuleFinder : IModuleFinder
    {
        private Dictionary<string, Module> modules = new Dictionary<string, Module>();

        public SimpleModuleFinder()
        {
            // let's register the standard module by default
            RegisterModule(EntityStandardLibrary.StandardModule);
        }

        public void RegisterModule(Module module)
        {
            modules[module.Name] = module;
        }

        public Module FindModule(string moduleName)
        {
            Module module;
            modules.TryGetValue(moduleName, out module);
            return module;
        }
    }

    public class ExampleSemantics
    {
        public DeclarativeLocalSymbolResolver SymbolResolver { get; set; }
        public List<Issue> Issues { get; set; } = new List<Issue>();
        private IModuleFinder ModuleFinder { get; set; }
        public TypeCalculator TypeCalculator { get; set; }      

        public List<Issue> SemanticEnrichment(Node node)
        {
            SymbolResolver.ResolveSymbols(node);
            node.WalkDescendants<Expression>().ToList().ForEach(expression => {
                TypeCalculator.SetTypeIfNeeded(expression);
            });
            return Issues;
        }

        Scope ModuleLevelTypes(Node ctx)
        {
            var scope = new Scope();
            var module = ctx.FindAncestorOfType<Module>();
            if (module != null)
            {
                // let's define types
                module.Types.ForEach(type => scope.Define(type));
                module.Entities.ForEach(type => scope.Define(type));
                foreach (var import in module.Imports)
                {
                    SymbolResolver.ResolveSymbols(import);
                    if (import.Module.Referred?.Entities != null)
                    {
                        foreach (var entity in import.Module.Referred.Entities)
                        {
                            scope.Define(entity);
                        }
                    }
                    if (import.Module.Referred?.Types != null)
                    {
                        foreach (var type in import.Module.Referred.Types)
                        {
                            scope.Define(type);
                        }
                    }
                }
            }

            return scope;
        }
        
        Scope ClassHierarchyEntities(ClassDecl ctx)
        {
            var scope = new Scope();
            var superclass = ctx.Superclass;            
            if (superclass != null && superclass.Resolved)
            {
                // let's define the superclass
                scope.Define(superclass.Referred);                
                scope.Parent = ClassHierarchyEntities(superclass.Referred);
            }

            return scope;
        }

        Scope ClassElements(ClassDecl ctx)
        {
            var scope = new Scope();

            ctx.Features.ForEach(type => scope.Define(type));
            var superclass = ctx.Superclass;
            if (superclass != null && superclass.Resolved)
            {
                // let's define superclasses
                scope.Parent = ClassElements(superclass.Referred);
            }

            return scope;
        }

        Scope ClassLevelTypes(ClassDecl ctx)
        {
            var scope = new Scope();
            
            ctx.Features.ForEach(feature => scope.Define(feature));
            var superclass = ctx.Superclass;
            if (superclass != null && superclass.Resolved)
            {
                // let's define superclasses
                scope.Parent = ClassLevelTypes(superclass.Referred);                
            }

            return scope;
        }

        public ExampleSemantics(IModuleFinder moduleFinder)
        {
            ModuleFinder = moduleFinder;

            SymbolResolver = new DeclarativeLocalSymbolResolver(Issues);
            SymbolResolver.ScopeFor(typeof(ClassDecl).GetProperty("Superclass"), (ClassDecl classDecl) =>
            {
                var scope = new Scope();                
                classDecl.FindAncestorOfType<Module>().Entities.ForEach(it => scope.Define(it));
                return scope;
            });
            SymbolResolver.ScopeFor(typeof(FeatureDecl).GetProperty("Type"), (FeatureDecl feature) =>
            {
                var scope = ModuleLevelTypes(feature);                
                return scope;
            });
            SymbolResolver.ScopeFor(typeof(ReferenceExpression).GetProperty("Target"), (ReferenceExpression reference) =>
            {
                var scope = new Scope();

                var classParent = reference.FindAncestorOfType<ClassDecl>();
                if (classParent != null)
                   scope.Parent = ClassLevelTypes(classParent);                

                if (reference.Context != null)
                {
                    SymbolResolver.ResolveNode(reference.Context);

                    var type = TypeCalculator.GetType(reference.Context) as ClassDecl;

                    if (type != null)
                    {
                        type.Features.ForEach(it => scope.Define(it));
                    }
                }

                return scope;
            });
            SymbolResolver.ScopeFor(typeof(Import).GetProperty("Module"), (Import import) =>
            {
                var scope = new Scope();
                if(moduleFinder.FindModule(import.Module.Name) != null)
                {
                   scope.Define(moduleFinder.FindModule(import.Module.Name));
                }                    
                return scope;
            });

            TypeCalculator = new EntityTypeCalculator(SymbolResolver);                
        }
    }

    public abstract class TypeCalculator
    {
        public virtual IType GetType(Node node)
        {
            return SetTypeIfNeeded(node);
        }

        public IType StrictlyGetType(Node node)
        {
            var type = SetTypeIfNeeded(node);
            if (type == null)
                throw new InvalidOperationException($"Cannot get type for node {node}");
            return type;
        }

        public abstract IType CalculateType(Node node);

        public virtual IType SetTypeIfNeeded(Node node)
        {
            if (node.GetTypeSemantics() == null)
            {
                var calculatedType = CalculateType(node);
                node.SetTypeSemantics(calculatedType);
            }
            return node.GetTypeSemantics();
        }
    }

    public class EntityTypeCalculator : TypeCalculator
    {
        private DeclarativeLocalSymbolResolver SymbolResolver { get; set; }

        public EntityTypeCalculator(DeclarativeLocalSymbolResolver sr)
        {
            SymbolResolver = sr;
        }

        private IType GetTypeOfReference<T, S>(T refHolder, PropertyInfo refAccessor)
            where T : Node
            where S : Node, Named            
        {
            ReferenceByName<S> refValue = refAccessor.GetValue(refHolder) as ReferenceByName<S>;
            if (refValue != null && !refValue.Resolved)
            {
                SymbolResolver?.ResolveProperty(refAccessor, refHolder);                             
            }
            else if (refValue != null && refValue.Resolved != false)
                return GetType(refValue.Referred as Node);
            
            return null;
        }

        public override IType CalculateType(Node node)
        {
            switch (node)
            {
                case OperatorExpression opExpr:
                    var leftType = GetType(opExpr.Left);
                    var rightType = GetType(opExpr.Right);
                    if (leftType == null || rightType == null)
                        return null;
                    switch (opExpr.Operator)
                    {
                        case Operator.Addition:
                            if (leftType == EntityStandardLibrary.StringType && rightType == EntityStandardLibrary.StringType)
                                return EntityStandardLibrary.StringType;
                            else if (leftType == EntityStandardLibrary.StringType && rightType == EntityStandardLibrary.IntegerType)
                                return EntityStandardLibrary.StringType;
                            else if (leftType == EntityStandardLibrary.IntegerType && rightType == EntityStandardLibrary.IntegerType)
                                return EntityStandardLibrary.IntegerType;
                            else
                                throw new NotImplementedException($"Unsupported operand types for addition: {leftType}, {rightType}");
                        case Operator.Multiplication:
                        case Operator.Division:
                        case Operator.Subtraction:
                            if (leftType == EntityStandardLibrary.IntegerType && rightType == EntityStandardLibrary.IntegerType)
                                return EntityStandardLibrary.IntegerType;
                            else
                                throw new NotImplementedException($"Unsupported operand types for multiplication: {leftType}, {rightType}");
                        default:
                            throw new NotImplementedException($"Operator not supported: {opExpr.Operator}");
                    }
                case ReferenceExpression refExpr:
                    {
                        SymbolResolver.ResolveNode(refExpr);
                        return GetTypeOfReference<ReferenceExpression, FeatureDecl>(refExpr, typeof(ReferenceExpression).GetProperty("Target"));
                    }             
                case FeatureDecl featureDecl:
                    if (featureDecl.Type.Resolved)                        
                        return featureDecl.Type.Referred;
                    else
                        return null;
                case StringLiteralExpression _:
                    return EntityStandardLibrary.StringType;
                case BooleanLiteralExpression _:
                    return EntityStandardLibrary.BooleanType;
                case IntegerLiteralExpression _:
                    return EntityStandardLibrary.IntegerType;                
                default:
                    throw new NotImplementedException($"Type calculation not implemented for node type {node.GetType()}");
            }
        }
    }

}
