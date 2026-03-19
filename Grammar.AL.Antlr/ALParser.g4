parser grammar ALParser;

options { tokenVocab=ALLexer; }

// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser and ALXmlPortParser — ANTLR4 deduplicates imported rules.
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser, ALXmlPortParser,
       ALEnumParser, ALEnumExtensionParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   | report
   | xmlPort
   | enum
   | enumextension
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