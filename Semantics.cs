using Strumenta.Entity.Semantics;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.SymbolResolution;
using Strumenta.Sharplasu.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Strumenta.Sharplasu.Traversing;

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
        public IModuleFinder ModuleFinder { get; set; }

        Scope ModuleLevelTypes(Node ctx)
        {
            var scope = new Scope();
            var module = ctx.FindAncestorOfType<Module>();
            if (module != null)
            {
                // let's define types
                module.Types.ForEach(type => scope.Define(type));
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
                // let's define features
                scope.Define(superclass.Referred);
                if (superclass.Referred.Superclass != null && superclass.Referred.Superclass.Resolved)
                    scope.Parent = ClassHierarchyEntities(superclass.Referred.Superclass.Referred);
            }

            return scope;
        }

        Scope ClassLevelTypes(ClassDecl ctx)
        {
            var scope = new Scope();
            var superclass = ctx.Superclass;
            if (superclass != null && superclass.Resolved)
            {
                // let's define superclasses
                superclass.Referred.Features.ForEach(type => scope.Define(type));
                if (superclass.Referred.Superclass != null && superclass.Referred.Superclass.Resolved)
                    scope.Parent = ClassLevelTypes(superclass.Referred.Superclass.Referred);
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
            SymbolResolver.ScopeFor(typeof(ReferenceExpression).GetProperty("Context"), (ReferenceExpression reference) =>
            {
                var scope = ClassHierarchyEntities(reference.FindAncestorOfType<ClassDecl>());
                return scope;
            });
            SymbolResolver.ScopeFor(typeof(ReferenceExpression).GetProperty("Target"), (ReferenceExpression reference) =>
            {
                var scope = new Scope();
                var classParent = reference.FindAncestorOfType<ClassDecl>();
                if (classParent != null)
                    scope.Parent = ClassLevelTypes(classParent);
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
        }
    }    
}
