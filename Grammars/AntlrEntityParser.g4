parser grammar AntlrEntityParser;

options { tokenVocab=AntlrEntityLexer; }

module_declaration:
    MODULE name=ID
    imports+=module_import*
    types+=type_declaration*
    entities+=entity_declaration*
;

module_import
    : IMPORT name=ID
    ;

type_declaration:
    TYPE name=ID
;

entity_declaration:
    KW_CLASS name=ID (COLON base_class=ID)? LCRLY
        features+=feature_declaration*
    RCRLY
;

feature_declaration:
    name=ID COLON type=ID assignment?
;

assignment:
    EQ expression
;

expression
    : (context=ID DOT)? target=ID                       #reference_expression
    | left=expression op=ADD right=expression           #operator_expression
    | left=expression op=SUB right=expression           #operator_expression
    | left=expression op=MUL right=expression           #operator_expression
    | left=expression op=DIV right=expression           #operator_expression
    | literal                                           #literal_expression
    ;

literal
    : value=STRING  #string_literal
    | value=INTEGER #integer_literal
    | value=BOOLEAN #boolean_literal
    | value=REAL    #real_literal
    ;
