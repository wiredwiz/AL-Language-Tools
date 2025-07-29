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
    : IDENTIFIER #identifierExpression
    | LEFTPAREN directiveExpression RIGHTPAREN #parenthesizedExpression
    | NOT directiveExpression #notExpression
    | directiveExpression AND directiveExpression #andExpression
    | directiveExpression OR directiveExpression #orExpression
    ;

preprocessorDirective
    : DEFINE IDENTIFIER directiveTerminator #defineDirective
    | UNDEF IDENTIFIER directiveTerminator #undefDirective
    | PRAGMA IMPLICITWITH (DISABLE | RESTORE) directiveTerminator #pragmaImplicitWithDirective
    | PRAGMA WARNING (DISABLE | RESTORE) warningList directiveTerminator #pragmaWarningDirective
    | REGION DIRECTIVE_TEXT? directiveTerminator #regionDirective
    | ENDREGION DIRECTIVE_TEXT? directiveTerminator #regionEndDirective
    | IF directiveExpression directiveTerminator #ifDirective
    | ELIF directiveExpression directiveTerminator #elseifDirective
    | ELSE directiveTerminator #elseDirective
    | ENDIF directiveTerminator #endifDirective
    ;

compileDirectives
    : preprocessorDirective* EOF
    ;