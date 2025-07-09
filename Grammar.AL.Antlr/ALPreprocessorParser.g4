parser grammar ALPreprocessorParser;

options { tokenVocab=ALLexer; }

directiveTerminator
    : DIRECTIVE_NEW_LINE
    | EOF
    ;

directiveExpression
    : IDENTIFIER
    | LEFTPAREN directiveExpression RIGHTPAREN
    | BANG directiveExpression
    | directiveExpression OP_AND directiveExpression
    | directiveExpression OP_OR directiveExpression
    ;

preprocessorDirective
    : DEFINE IDENTIFIER directiveTerminator
    | UNDEF IDENTIFIER directiveTerminator
    | PRAGMA DIRECTIVE_TEXT directiveTerminator
    | REGION DIRECTIVE_TEXT directiveTerminator
    | ENDREGION DIRECTIVE_TEXT directiveTerminator
    | IF directiveExpression directiveTerminator
    | ELIF directiveExpression directiveTerminator
    | ELSE directiveTerminator
    | ENDIF directiveTerminator
    ;

compileDirectives
    : preprocessorDirective*
    ;