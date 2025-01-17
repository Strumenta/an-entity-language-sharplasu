using Strumenta.Entity.Parser;
using Strumenta.Entity;
using Strumenta.Sharplasu.Validation;
using Strumenta.Sharplasu.SymbolResolution;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.Traversing;
using Strumenta.Entity.Semantics;

namespace Tests
{
    [TestClass]
    public sealed class Tests
    {
        [TestMethod]
        public void TestSymbolResolution()
        {
            EntitySharplasuParser parser = new EntitySharplasuParser();
            string code = @"
module example

import standard

class base {
	name string
	description string
}

class aged : base {
	age integer
}

class car : aged {
	kilometers integer = age * 10000	
}
            ";
            var result = parser.Parse(code);

            SimpleModuleFinder moduleFinder = new SimpleModuleFinder();
            ExampleSemantics semantics = new ExampleSemantics(moduleFinder);
            List<Issue> issues = semantics.SemanticEnrichment(result.Root);
            Assert.AreEqual(0, issues.Count);
            result.Root.AssertAllReferencesResolved();
        }

        [TestMethod]
        public void TestTypeCalculation()
        {
            EntitySharplasuParser parser = new EntitySharplasuParser();
            string code = @"module example

import standard

class base {
	name string
	description string
}

class aged : base {
	age integer
}

class person : aged {
	location address
	speed integer = 2
}

class address {
	note base
	city string
	street string
	number integer	
}

class athlete : person {			
	deliveryNote string = location.note.description
	luckyNumber integer = location.number + 3
}

class car : aged {
	kilometers integer = age * 10000	
}";
            var result = parser.Parse(code);

            SimpleModuleFinder moduleFinder = new SimpleModuleFinder();
            ExampleSemantics semantics = new ExampleSemantics(moduleFinder);
            List<Issue> issues = semantics.SemanticEnrichment(result.Root);
            Assert.AreEqual(0, issues.Count);
            result.Root.AssertAllExpressionsHaveTypes();
            Assert.AreEqual("string",
                result.Root.Entities[4].Features[0].Value.GetTypeSemantics().Name
            );
        }

        [TestMethod]
        public void TestTypeCalculationFailsWithUnresolveSymbols()
        {
            EntitySharplasuParser parser = new EntitySharplasuParser();
            string code = @"module example

import standard

class base {
	name string
	description string
}

class aged : base {
	age integer
}

class person : aged {
	location address
	speed integer = 2
}

class address {
	note base
	city string
	street string
	number integer	
}

class athlete : person {			
	deliveryNote string = location.note.description
	luckyNumber integer = location.number + 3
}

class car : aged {
	kilometers integer = undeclared * 10000	
}";
            var result = parser.Parse(code);

            SimpleModuleFinder moduleFinder = new SimpleModuleFinder();
            ExampleSemantics semantics = new ExampleSemantics(moduleFinder);
            List<Issue> issues = semantics.SemanticEnrichment(result.Root);
            result.Root.AssertSomeExpressionsDoNotHaveTypes(2);
            result.Root.AssertNotAllReferencesResolved();
        }
    }

    public static class Utils
    {
        public static void AssertAllExpressionsHaveTypes(this Node node)
        {
            var expressionsWithoutType = node.WalkDescendants<Strumenta.Entity.Expression>().Where(x => x.GetTypeSemantics() == null).ToList();
            Assert.AreEqual(0, expressionsWithoutType.Count);
        }

        public static void AssertSomeExpressionsDoNotHaveTypes(this Node node, int count)
        {
            var expressionsWithoutType = node.WalkDescendants<Strumenta.Entity.Expression>().Where(x => x.GetTypeSemantics() == null).ToList();
            Assert.AreEqual(count, expressionsWithoutType.Count);
        }
    }
}
