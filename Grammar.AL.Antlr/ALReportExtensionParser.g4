parser grammar ALReportExtensionParser;

options { tokenVocab=ALLexer; }

import ALReportParser, ALPageExtensionParser;

reportExtensionDatasetModification
    : {TokenMatches("addafter") || TokenMatches("addbefore") ||
       TokenMatches("addfirst") || TokenMatches("addlast")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE (reportDataItem | reportColumn)* RIGHTCBRACE
    | {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE reportDataItemProperty* RIGHTCBRACE
    ;

reportExtensionDataset
    : {TokenMatches("dataset")}? IDENTIFIER
      LEFTCBRACE reportExtensionDatasetModification* RIGHTCBRACE
    ;

reportExtensionRequestPage
    : {TokenMatches("requestpage")}? IDENTIFIER
      LEFTCBRACE pageExtensionLayoutSection? pageExtensionActionSection? codeDeclarations? RIGHTCBRACE
    ;

reportextension
    : namespaceDeclaration? usingDeclarations?
      REPORTEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE
          reportExtensionDataset?
          reportExtensionRequestPage?
          codeDeclarations?
      RIGHTCBRACE
    ;
