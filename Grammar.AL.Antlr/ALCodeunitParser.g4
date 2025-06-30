parser grammar ALCodeunitParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser;

/*
* Codeunit structure
*/

codeunitProperty
   : {TokenMatches("Obsolete")}? identifier EQUAL STRING_LITERAL SEMICOLON
   | {TokenMatches("ObsoleteState")}? identifier EQUAL {TokenMatches(ObsoleteStateValues)}? identifier SEMICOLON
   | {TokenMatches("ObsoleteTag")}? identifier EQUAL STRING_LITERAL SEMICOLON
   | permissionsProperty
   | keyValueProperty
   | keyIdentifierListProperty
   ;

codeunitProperties
   : codeunitProperty+;

codeunitDeclaration
   : namespaceDeclaration? usingDeclarations? CODEUNIT objectId? objectName LEFTCBRACE codeunitProperties? codeDeclarations? RIGHTCBRACE;