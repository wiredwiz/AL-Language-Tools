parser grammar ALParser;

options { tokenVocab=ALLexer; }

import ALCodeunitParser, ALTableParser, ALPageParser;

alUnit
   : codeunitDeclaration
   | table
   | page
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