using Strumenta.Entity;
using Strumenta.Entity.Parser;
using Strumenta.Entity.Semantics;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.SymbolResolution;
using Strumenta.Sharplasu.Traversing;
using Strumenta.Sharplasu.Validation;
using System.Xml.Linq;

namespace Strumenta.Entity
{    
    internal class Program
    {               
        static void Main(string[] args)
        {
            EntitySharplasuParser parser = new EntitySharplasuParser();
            var result = parser.Parse(new FileInfo("../../../Examples/example.entity"));
            SimpleModuleFinder moduleFinder = new SimpleModuleFinder();            
            ExampleSemantics semantics = new ExampleSemantics(moduleFinder);            
            List<Issue> issues = semantics.SemanticEnrichment(result.Root);
                       
            foreach (var issue in issues)
            {
                Console.WriteLine(issue.Message);
            }                
        }
    }
}
