using Strumenta.Entity;
using Strumenta.Entity.Parser;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.SymbolResolution;
using System.Xml.Linq;

namespace Strumenta.Entity
{    
    internal class Program
    {
        static internal Module GetCompilationUnit()
        {
            var cu = new Module(
                name: "example",
                imports: new List<Import>()
                {
                    new Import("standard"),
                },
                types: new List<TypeDecl>(),
                entities: new List<ClassDecl>()
                {
                    new ClassDecl(
                        name: "base",
                        superclass: null,
                        features: new List<FeatureDecl>()
                        {
                            new FeatureDecl("name", new ReferenceByName<TypeDecl>("string")),
                            new FeatureDecl("description", new ReferenceByName<TypeDecl>("string"))
                        }
                    ),
                    new ClassDecl(
                        name: "aged",
                        superclass: new ReferenceByName<ClassDecl>("base"),
                        features: new List<FeatureDecl>()
                        {
                            new FeatureDecl("age", new ReferenceByName<TypeDecl>("integer"))
                        }                  
                    ),
                    new ClassDecl(
                        name: "person",                       
                        superclass: new ReferenceByName<ClassDecl>("aged"),
                        features: new List<FeatureDecl>()
                        {
                            new FeatureDecl("speed", new ReferenceByName<TypeDecl>("integer"), new IntegerLiteralExpression(2))
                        }
                    ),
                    new ClassDecl(
                        name: "athlete",
                        superclass: new ReferenceByName<ClassDecl>("person"),
                        features: new List<FeatureDecl>()
                        {
                            new FeatureDecl("speed", new ReferenceByName<TypeDecl>("integer"), 
                                new OperatorExpression(
                                    left: new ReferenceExpression(
                                        context: new ReferenceByName<ClassDecl>("person"),
                                        target: new ReferenceByName<FeatureDecl>("speed")
                                    ),
                                    right: new IntegerLiteralExpression(2),
                                    operatorType: Operator.Multiplication
                                )
                            )
                        }
                    ),
                    new ClassDecl(
                        name: "building",
                        superclass: new ReferenceByName<ClassDecl>("base"),
                        features: new List<FeatureDecl>()
                        {
                            new FeatureDecl("address", new ReferenceByName<TypeDecl>("string"))
                        }
                    ),
                    new ClassDecl(
                        name: "car",
                        superclass: new ReferenceByName<ClassDecl>("aged"),
                        features: new List<FeatureDecl>()
                        {
                            new FeatureDecl("kilometers", new ReferenceByName<TypeDecl>("integer"),
                            new OperatorExpression(
                                    left: new ReferenceExpression(
                                        context: null,
                                        target: new ReferenceByName<FeatureDecl>("age")
                                    ),
                                    right: new IntegerLiteralExpression(10000),
                                    operatorType: Operator.Multiplication
                                )
                            )
                        }
                    ),
                }
            );
            cu.AssignParents();
            return cu;
        }        

        static void Main(string[] args)
        {
            EntitySharplasuParser parser = new EntitySharplasuParser();
            var result = parser.Parse(new FileInfo("../../../Examples/example.entity"));   
            SimpleModuleFinder moduleFinder = new SimpleModuleFinder();            
            ExampleSemantics semantics = new ExampleSemantics(moduleFinder);
            semantics.SemanticEnrichment(result.Root);
            Console.WriteLine(result.Root.MultiLineString());            
            foreach (var issue in semantics.Issues)
            {
                Console.WriteLine(issue.Message);
            }
            result.Root.AssertAllReferencesResolved();
        }
    }
}
