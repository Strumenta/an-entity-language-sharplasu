using Antlr4.Runtime;
using Strumenta.Entity.Parser;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.Parsing;
using Strumenta.Sharplasu.Transformation;
using Strumenta.Sharplasu.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Strumenta.Entity.Parser.AntlrEntityParser;

namespace Strumenta.Entity.Mapping;

public class EntityParseTreeToAstTransformer : ASTTransformer
{
    public EntityParseTreeToAstTransformer(List<Issue> issues) : base(issues, false)
    {
        RegisterNodeFactory<Module>(typeof(Module_declarationContext), (source) =>
        {
            var it = (source as Module_declarationContext);
            return new Module(
                name: it.name.Text,
                imports: it.module_import().Select(x => Transform(x) as Import).ToList(),
                types: it.type_declaration().Select(x => Transform(x) as TypeDecl).ToList(),
                entities: it.entity_declaration().Select(x => Transform(x) as ClassDecl).ToList()
            ).WithParseTreeNode(it) as Module;
        });
        RegisterNodeFactory<Import>(typeof(Module_importContext), (source) =>
        {
            var it = (source as Module_importContext);
            return new Import(
                it.name.Text
            ).WithParseTreeNode(it) as Import;
        });
        RegisterNodeFactory<TypeDecl>(typeof(Type_declarationContext), (source) =>
        {
            var it = (source as Type_declarationContext);
            return new TypeDecl(
                it.name.Text
            ).WithParseTreeNode(it) as TypeDecl;
        });
        RegisterNodeFactory<ClassDecl>(typeof(Entity_declarationContext), (source) =>
        {
            var it = (source as Entity_declarationContext);
            return new ClassDecl(
                name: it.name.Text,
                superclass: it.base_class?.Text != null ? new ReferenceByName<ClassDecl>(it.base_class?.Text) : null,
                features: it.feature_declaration().Select(x => Transform(x as Feature_declarationContext) as FeatureDecl).ToList()
            ).WithParseTreeNode(it) as ClassDecl;
        });
        RegisterNodeFactory<FeatureDecl>(typeof(Feature_declarationContext), (source) =>
        {
            var it = (source as Feature_declarationContext);
            return new FeatureDecl(
                name: it.name.Text,
                type: new ReferenceByName<TypeDecl>(it.type.Text),
                value: it.assignment() != null ? Transform(it.assignment()) as Expression : null
            ).WithParseTreeNode(it) as FeatureDecl;
        });        
        RegisterNodeFactory<Expression>(typeof(AssignmentContext), (source) =>
        {
            var it = (source as AssignmentContext);
            return Transform(it.expression()).WithOrigin(new ParseTreeOrigin(it)) as Expression;
        });
        RegisterNodeFactory<Expression>(typeof(ExpressionContext), (source) =>
        {            
            return source switch
            {
                Reference_expressionContext reference => new ReferenceExpression(  
                    reference.context != null ?
                    Transform(reference.context).WithOrigin(new ParseTreeOrigin(reference.context)) as Expression : null,
                     new ReferenceByName<FeatureDecl>(reference.target.Text)
                ),       
                Literal_expressionContext literal => Transform(literal.literal()).WithOrigin(new ParseTreeOrigin(literal)) as Expression,
                Operator_expressionContext operation =>
                    new OperatorExpression(
                        left: Transform(operation.left).WithOrigin(new ParseTreeOrigin(operation.left)) as Expression,
                        right: Transform(operation.right).WithOrigin(new ParseTreeOrigin(operation.right)) as Expression,
                        operatorType: operation.op switch
                        {
                            IToken it when operation.ADD() != null => Operator.Addition,
                            IToken it when operation.SUB() != null => Operator.Subtraction,
                            IToken it when operation.MUL() != null => Operator.Multiplication,
                            IToken it when operation.DIV() != null => Operator.Division
                        }
                    )
            };
        });
        RegisterNodeFactory<Expression>(typeof(LiteralContext), (source) =>
        {
            return source switch
            {
                String_literalContext strLit => new StringLiteralExpression(strLit.value.Text                
                ),
                Integer_literalContext intLit => new IntegerLiteralExpression(long.Parse(intLit.value.Text)
                ),
                Boolean_literalContext boolLit => new BooleanLiteralExpression(bool.Parse(boolLit.value.Text)
                ),
                Real_literalContext realLit => new RealLiteralExpression(double.Parse(realLit.value.Text)
                )
            };
        });
    }
}
