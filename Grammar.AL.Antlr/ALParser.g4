parser grammar ALParser;

options { tokenVocab=ALLexer; }

// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser, ALXmlPortParser, ALPageExtensionParser,
// ALPageCustomizationParser, and ALReportExtensionParser — ANTLR4 deduplicates imported rules.
// ALTableParser is kept explicit here: it defines the `table` rule used in alUnit below.
// It also arrives transitively through ALTableExtensionParser.
import ALCodeunitParser,
       ALTableParser, ALTableExtensionParser,
       ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
       ALQueryParser,
       ALReportParser, ALReportExtensionParser,
       ALXmlPortParser,
       ALEnumParser, ALEnumExtensionParser,
       ALInterfaceParser,
       ALProfileParser, ALProfileExtensionParser,
       ALPermissionSetParser, ALPermissionSetExtensionParser,
       ALEntitlementParser,
       ALDotNetParser,
       ALControlAddInParser;

alUnit
   : codeunitDeclaration
   | table           | tableExtension
   | page            | pageextension      | pagecustomization
   | query
   | report          | reportextension
   | xmlPort
   | enum            | enumextension
   | interface
   | profile         | profileextension
   | permissionset   | permissionsetextension
   | entitlement
   | dotnet
   | controladdin
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