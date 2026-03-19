parser grammar ALEntitlementParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

objectEntitlementsProperty
    : {TokenMatches("ObjectEntitlements")}? identifier EQUAL identifierList SEMICOLON
    ;

entitlementProperty
    : objectEntitlementsProperty
    | keyValueProperty     // Type, Id, RoleType
    ;

entitlement
    : namespaceDeclaration? usingDeclarations?
      ENTITLEMENT identifier
      LEFTCBRACE entitlementProperty* RIGHTCBRACE
    ;
