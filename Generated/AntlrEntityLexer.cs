//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Grammars\AntlrEntityLexer.g4 by ANTLR 4.13.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Strumenta.Entity.Parser {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.CLSCompliant(false)]
public partial class AntlrEntityLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		ENTITY=1, MODULE=2, IMPORT=3, RETURN=4, LET=5, NEW=6, COLON=7, SEMI=8, 
		COMMA=9, DOT=10, LSQRD=11, RSQRD=12, LCRLY=13, RCRLY=14, LPAREN=15, RPAREN=16, 
		ADD=17, SUB=18, MUL=19, DIV=20, EQ=21, STRING=22, INTEGER=23, BOOLEAN=24, 
		ID=25, COMMENT=26, WS=27;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"ENTITY", "MODULE", "IMPORT", "RETURN", "LET", "NEW", "COLON", "SEMI", 
		"COMMA", "DOT", "LSQRD", "RSQRD", "LCRLY", "RCRLY", "LPAREN", "RPAREN", 
		"ADD", "SUB", "MUL", "DIV", "EQ", "STRING", "INTEGER", "BOOLEAN", "ID", 
		"COMMENT", "WS"
	};


	public AntlrEntityLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public AntlrEntityLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'entity'", "'module'", "'import'", "'return'", "'let'", "'new'", 
		"':'", "';'", "','", "'.'", "'['", "']'", "'{'", "'}'", "'('", "')'", 
		"'+'", "'-'", "'*'", "'/'", "'='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "ENTITY", "MODULE", "IMPORT", "RETURN", "LET", "NEW", "COLON", "SEMI", 
		"COMMA", "DOT", "LSQRD", "RSQRD", "LCRLY", "RCRLY", "LPAREN", "RPAREN", 
		"ADD", "SUB", "MUL", "DIV", "EQ", "STRING", "INTEGER", "BOOLEAN", "ID", 
		"COMMENT", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "AntlrEntityLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static AntlrEntityLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,27,177,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,1,0,1,0,1,0,1,0,
		1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,
		3,1,3,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,6,1,6,1,7,
		1,7,1,8,1,8,1,9,1,9,1,10,1,10,1,11,1,11,1,12,1,12,1,13,1,13,1,14,1,14,
		1,15,1,15,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,1,20,1,21,1,21,
		5,21,124,8,21,10,21,12,21,127,9,21,1,21,1,21,1,22,1,22,1,22,5,22,134,8,
		22,10,22,12,22,137,9,22,3,22,139,8,22,1,23,1,23,1,23,1,23,1,23,1,23,1,
		23,1,23,1,23,3,23,150,8,23,1,24,4,24,153,8,24,11,24,12,24,154,1,25,1,25,
		5,25,159,8,25,10,25,12,25,162,9,25,1,25,3,25,165,8,25,1,25,1,25,1,25,1,
		25,1,26,4,26,172,8,26,11,26,12,26,173,1,26,1,26,1,125,0,27,1,1,3,2,5,3,
		7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,
		33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,1,0,
		21,2,0,69,69,101,101,2,0,78,78,110,110,2,0,84,84,116,116,2,0,73,73,105,
		105,2,0,89,89,121,121,2,0,77,77,109,109,2,0,79,79,111,111,2,0,68,68,100,
		100,2,0,85,85,117,117,2,0,76,76,108,108,2,0,80,80,112,112,2,0,82,82,114,
		114,2,0,87,87,119,119,1,0,49,57,1,0,48,57,2,0,70,70,102,102,2,0,65,65,
		97,97,2,0,83,83,115,115,2,0,65,90,97,122,2,0,10,10,13,13,3,0,9,10,13,13,
		32,32,184,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,
		0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,
		1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,
		0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,
		1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,
		0,1,55,1,0,0,0,3,62,1,0,0,0,5,69,1,0,0,0,7,76,1,0,0,0,9,83,1,0,0,0,11,
		87,1,0,0,0,13,91,1,0,0,0,15,93,1,0,0,0,17,95,1,0,0,0,19,97,1,0,0,0,21,
		99,1,0,0,0,23,101,1,0,0,0,25,103,1,0,0,0,27,105,1,0,0,0,29,107,1,0,0,0,
		31,109,1,0,0,0,33,111,1,0,0,0,35,113,1,0,0,0,37,115,1,0,0,0,39,117,1,0,
		0,0,41,119,1,0,0,0,43,121,1,0,0,0,45,138,1,0,0,0,47,149,1,0,0,0,49,152,
		1,0,0,0,51,156,1,0,0,0,53,171,1,0,0,0,55,56,7,0,0,0,56,57,7,1,0,0,57,58,
		7,2,0,0,58,59,7,3,0,0,59,60,7,2,0,0,60,61,7,4,0,0,61,2,1,0,0,0,62,63,7,
		5,0,0,63,64,7,6,0,0,64,65,7,7,0,0,65,66,7,8,0,0,66,67,7,9,0,0,67,68,7,
		0,0,0,68,4,1,0,0,0,69,70,7,3,0,0,70,71,7,5,0,0,71,72,7,10,0,0,72,73,7,
		6,0,0,73,74,7,11,0,0,74,75,7,2,0,0,75,6,1,0,0,0,76,77,7,11,0,0,77,78,7,
		0,0,0,78,79,7,2,0,0,79,80,7,8,0,0,80,81,7,11,0,0,81,82,7,1,0,0,82,8,1,
		0,0,0,83,84,7,9,0,0,84,85,7,0,0,0,85,86,7,2,0,0,86,10,1,0,0,0,87,88,7,
		1,0,0,88,89,7,0,0,0,89,90,7,12,0,0,90,12,1,0,0,0,91,92,5,58,0,0,92,14,
		1,0,0,0,93,94,5,59,0,0,94,16,1,0,0,0,95,96,5,44,0,0,96,18,1,0,0,0,97,98,
		5,46,0,0,98,20,1,0,0,0,99,100,5,91,0,0,100,22,1,0,0,0,101,102,5,93,0,0,
		102,24,1,0,0,0,103,104,5,123,0,0,104,26,1,0,0,0,105,106,5,125,0,0,106,
		28,1,0,0,0,107,108,5,40,0,0,108,30,1,0,0,0,109,110,5,41,0,0,110,32,1,0,
		0,0,111,112,5,43,0,0,112,34,1,0,0,0,113,114,5,45,0,0,114,36,1,0,0,0,115,
		116,5,42,0,0,116,38,1,0,0,0,117,118,5,47,0,0,118,40,1,0,0,0,119,120,5,
		61,0,0,120,42,1,0,0,0,121,125,5,34,0,0,122,124,9,0,0,0,123,122,1,0,0,0,
		124,127,1,0,0,0,125,126,1,0,0,0,125,123,1,0,0,0,126,128,1,0,0,0,127,125,
		1,0,0,0,128,129,5,34,0,0,129,44,1,0,0,0,130,139,5,48,0,0,131,135,7,13,
		0,0,132,134,7,14,0,0,133,132,1,0,0,0,134,137,1,0,0,0,135,133,1,0,0,0,135,
		136,1,0,0,0,136,139,1,0,0,0,137,135,1,0,0,0,138,130,1,0,0,0,138,131,1,
		0,0,0,139,46,1,0,0,0,140,141,7,2,0,0,141,142,7,11,0,0,142,143,7,8,0,0,
		143,150,7,0,0,0,144,145,7,15,0,0,145,146,7,16,0,0,146,147,7,9,0,0,147,
		148,7,17,0,0,148,150,7,0,0,0,149,140,1,0,0,0,149,144,1,0,0,0,150,48,1,
		0,0,0,151,153,7,18,0,0,152,151,1,0,0,0,153,154,1,0,0,0,154,152,1,0,0,0,
		154,155,1,0,0,0,155,50,1,0,0,0,156,160,5,35,0,0,157,159,8,19,0,0,158,157,
		1,0,0,0,159,162,1,0,0,0,160,158,1,0,0,0,160,161,1,0,0,0,161,164,1,0,0,
		0,162,160,1,0,0,0,163,165,5,13,0,0,164,163,1,0,0,0,164,165,1,0,0,0,165,
		166,1,0,0,0,166,167,5,10,0,0,167,168,1,0,0,0,168,169,6,25,0,0,169,52,1,
		0,0,0,170,172,7,20,0,0,171,170,1,0,0,0,172,173,1,0,0,0,173,171,1,0,0,0,
		173,174,1,0,0,0,174,175,1,0,0,0,175,176,6,26,1,0,176,54,1,0,0,0,9,0,125,
		135,138,149,154,160,164,173,2,6,0,0,0,1,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace Strumenta.Entity.Parser