parser grammar ALParser;

options { tokenVocab=ALLexer; }

// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser — ANTLR4 deduplicates imported rules.
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   | report
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