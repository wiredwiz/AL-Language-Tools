parser grammar ALPreprocessorParser;

options { tokenVocab=ALLexer; }

preprocessorDirective
    : DEFINE IDENTIFIER
    ;