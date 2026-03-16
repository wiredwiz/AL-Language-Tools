parser grammar ALParser;

options { tokenVocab=ALLexer; }

import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   ;

compileUnit
	: EOF
   | alUnit EOF
	;

/*

applicationObjectType
   : CODEUNIT | PAGE | PAGEEXTENSION | PAGECUSTOMIZATION | DOTNET | ENUM | ENUMEXTENSION | VALUE | QUERY
   | REPORT | TABLE | TABLEEXTENSION | XMLPORT | PROFILE | CONTROLADDIN | REPORTEXTENSION | INTERFACE
   | PERMISSIONSET | PERMISSIONSETEXTENSION | ENTITLEMENT;
 */