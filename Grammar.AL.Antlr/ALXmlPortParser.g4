parser grammar ALXmlPortParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser, ALPageParser;

xmlPortProperty
    : permissionsProperty
    | keyValueProperty
    ;

xmlPortProperties : xmlPortProperty* ;

// fieldelement and fieldattribute source is always TableVar.Field
xmlPortFieldSource : identifier PERIOD identifier ;

// Leaf elements — no child schema elements
xmlPortFieldElement
    : {TokenMatches("fieldelement")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=xmlPortFieldSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

xmlPortTextAttribute
    : {TokenMatches("textattribute")}? IDENTIFIER
      LEFTPAREN name=identifier RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

xmlPortFieldAttribute
    : {TokenMatches("fieldattribute")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=xmlPortFieldSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

// Union rule — referenced recursively by container elements below.
// ANTLR4 handles mutual recursion cleanly: recursion is never left-edge.
xmlPortSchemaElement
    : xmlPortTextElement
    | xmlPortTableElement
    | xmlPortFieldElement
    | xmlPortTextAttribute
    | xmlPortFieldAttribute
    ;

// Properties within a tableelement block. Structured properties listed first
// so LL(*) prediction resolves them before keyValueProperty.
xmlPortTableElementProperty
    : tableViewProperty           // SourceTableView = sorting(...) where(...)
    | dataItemLinkProperty        // DataItemLink = field = DataItem.Field
    | dataItemTableFilterProperty // DataItemTableFilter = "F"=CONST(V),...
    | keyValueProperty            // single key = value
    ;

// Container elements — can hold child elements then triggers
xmlPortTextElement
    : {TokenMatches("textelement")}? IDENTIFIER
      LEFTPAREN name=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?          // 1. properties
          xmlPortSchemaElement*?      // 2. child elements/attributes (recursive)
          triggerDeclaration*?        // 3. triggers last
      RIGHTCBRACE
    ;

xmlPortTableElement
    : {TokenMatches("tableelement")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          xmlPortTableElementProperty*?  // 1. properties (DataItemLink, SourceTableView, etc.)
          xmlPortSchemaElement*?         // 2. child elements/attributes (recursive)
          triggerDeclaration*?           // 3. triggers last
      RIGHTCBRACE
    ;

xmlPortSchema
    : {TokenMatches("schema")}? IDENTIFIER
      LEFTCBRACE xmlPortSchemaElement* RIGHTCBRACE
    ;

// Reuses pageProperties, pageLayoutSection, pageActionSection from ALPageParser (imported above)
xmlPortRequestPage
    : {TokenMatches("requestpage")}? IDENTIFIER
      LEFTCBRACE pageProperties pageLayoutSection? pageActionSection? codeDeclarations? RIGHTCBRACE
    ;

xmlPort
    : namespaceDeclaration? usingDeclarations?
      XMLPORT INTEGER_LITERAL identifier
      LEFTCBRACE
          xmlPortProperties
          xmlPortSchema?
          xmlPortRequestPage?
          codeDeclarations?
      RIGHTCBRACE
    ;
