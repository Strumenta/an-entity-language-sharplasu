# Symbol Resolution and Type Calculation With a Entity Language

This project shows how to add semantic enrichment to a parser based on the [Sharplasu](https://github.com/Strumenta/sharplasu) library:
- how to perform symbol resolution
- how to calculate types of expressions

It works on a simple language, but uses common patterns for symbol resolution.

This project already includes the compiled ANTLR parser. If you change the grammar and want to regenerate it, you can use the following command (assuming you have [setup ANTLR](https://tomassetti.me/antlr-mega-tutorial/#chapter19)).

```
antlr4 Grammars/*.g4 -Dlanguage=CSharp -Xexact-output-dir -o ./Generated/ -no-visitor -no-listener -package "Strumenta.Entity.Parser"
```

This is the companion repository of the article [Resolve Symbols and Calculate Types with Sharplasu](https://tomassetti.me/resolve-symbols-and-calculate-types-with-sharplasu/).
