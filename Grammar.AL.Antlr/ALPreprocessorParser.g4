parser grammar ALPreprocessorParser;

options { tokenVocab=ALLexer; }

directiveTerminator
    : DIRECTIVE_NEW_LINE
    | EOF
    ;

warningList
    : IDENTIFIER (COMMA IDENTIFIER)*
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
    | PRAGMA IMPLICITWITH (DISABLE | RESTORE) directiveTerminator
    | PRAGMA WARNING (DISABLE | RESTORE) warningList directiveTerminator
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