using Antlr4.Runtime;
using Strumenta.Entity.Mapping;
using Strumenta.Sharplasu.Model;
using Strumenta.Sharplasu.Parsing;
using Strumenta.Sharplasu.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strumenta.Entity.Parser
{
    public class EntitySharplasuParser : SharpLasuParser<Module, AntlrEntityParser, AntlrEntityParser.Module_declarationContext, SharplasuANTLRToken>
    {
        protected override Lexer CreateANTLRLexer(ICharStream charStream)
        {
            return new AntlrEntityLexer(charStream);
        }

        protected override AntlrEntityParser CreateANTLRParser(ITokenStream tokenStream)
        {
            return new AntlrEntityParser(tokenStream);
        }

        protected override Module ParseTreeToAst(AntlrEntityParser.Module_declarationContext parseTreeRoot, bool considerPosition = true, List<Issue> issues = null, Source source = null)
        {

            return new EntityParseTreeToAstTransformer(issues).Transform(parseTreeRoot) as Module;
        }

        protected override AntlrEntityParser.Module_declarationContext InvokeRootRule(AntlrEntityParser parser)
        {
            return parser.module_declaration();
        }

        public EntitySharplasuParser() : base(new ANTLRTokenFactory()) { }
    }
}
