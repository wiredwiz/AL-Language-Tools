# Remaining AL Object Types Grammar Design

## Goal

Add grammar support for all 14 remaining AL object types to the ANTLR4 parser, bringing the grammar to full BC26 coverage.

## Currently Supported

`codeunit`, `table`, `page`, `query`, `report`, `xmlport`

## Object Types to Add

| Object type | New grammar file | Imports |
|---|---|---|
| `enum` | `ALEnumParser.g4` | `ALPropertyParser` |
| `enumextension` | `ALEnumExtensionParser.g4` | `ALEnumParser` |
| `interface` | `ALInterfaceParser.g4` | `ALPropertyParser` |
| `tableextension` | `ALTableExtensionParser.g4` | `ALTableParser` |
| `pageextension` | `ALPageExtensionParser.g4` | `ALPageParser` |
| `pagecustomization` | `ALPageCustomizationParser.g4` | `ALPageParser` |
| `reportextension` | `ALReportExtensionParser.g4` | `ALReportParser`, `ALPageExtensionParser` |
| `profile` | `ALProfileParser.g4` | `ALPropertyParser` |
| `profileextension` | `ALProfileExtensionParser.g4` | `ALProfileParser` |
| `permissionset` | `ALPermissionSetParser.g4` | `ALPropertyParser` |
| `permissionsetextension` | `ALPermissionSetExtensionParser.g4` | `ALPermissionSetParser` |
| `entitlement` | `ALEntitlementParser.g4` | `ALPropertyParser` |
| `dotnet` | `ALDotNetParser.g4` | `ALPropertyParser` |
| `controladdin` | `ALControlAddInParser.g4` | `ALPropertyParser` |

Each extension type imports its base type's grammar file, inheriting rules without duplication. This is the same pattern already used by `ALReportParser` and `ALXmlPortParser` importing `ALPageParser`.

## Architecture

### Lexer Change

`PROFILEEXTENSION` does not exist in `ALLexer.g4`. Add it as a new dedicated token consistent with `TABLEEXTENSION`, `PAGEEXTENSION`, `ENUMEXTENSION`, `REPORTEXTENSION`, and `PERMISSIONSETEXTENSION` (all of which are already present).

### Shared Grammar Changes

Two changes to existing files before any new parsers are written.

#### 1. Expanded `permissionSpecifier` in `ALPropertyParser.g4`

The current rule only handles `tabledata ObjectId = r`. BC26 permissionsets also use object-type specifiers (`codeunit * = X`, `page "X" = rimd`). Updated rule:

```antlr
permissionSpecifier
    : TABLEDATA objectId EQUAL identifier
    | {!TokenMatches("tabledata")}? identifier (objectId | ASTERISK) EQUAL identifier
    ;
```

The `{!TokenMatches("tabledata")}?` predicate on the second alternative prevents ANTLR4 from silently falling through when `TABLEDATA` (which also appears in the `identifier` rule) would otherwise match both alternatives. The object type keyword (`codeunit`, `page`, `table`, etc.) is matched as `identifier` — no new tokens needed.

#### 2. Layout/action modification verbs added to `ALPageParser.g4`

`pageextension`, `pagecustomization`, and `reportextension`'s requestpage all use position operators to modify existing controls and actions. Defining them in `ALPageParser.g4` means they are inherited by all three via import.

```antlr
pageLayoutModification
    : {TokenMatches("addafter") || TokenMatches("addbefore") ||
       TokenMatches("addfirst") || TokenMatches("addlast")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageLayoutEntity* RIGHTCBRACE
    | {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageGenericProperty* triggerDeclaration* RIGHTCBRACE
    | {TokenMatches("movebefore") || TokenMatches("moveafter") ||
       TokenMatches("movefirst") || TokenMatches("movelast")}?
      identifier LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN
    ;

pageActionModification
    : {TokenMatches("addafter") || TokenMatches("addbefore") ||
       TokenMatches("addfirst") || TokenMatches("addlast")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageAction* RIGHTCBRACE
    | {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageActionEntity* RIGHTCBRACE
    | {TokenMatches("movebefore") || TokenMatches("moveafter") ||
       TokenMatches("movefirst") || TokenMatches("movelast")}?
      identifier LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN
    ;
```

### New Grammar Files

#### `ALEnumParser.g4` (imports `ALPropertyParser`)

```antlr
enumValue
    : VALUE LEFTPAREN INTEGER_LITERAL SEMICOLON identifier RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

enum
    : namespaceDeclaration? usingDeclarations?
      ENUM INTEGER_LITERAL identifier
      LEFTCBRACE keyValueProperty* enumValue* RIGHTCBRACE
    ;
```

#### `ALEnumExtensionParser.g4` (imports `ALEnumParser`)

Inherits `enumValue` from `ALEnumParser`.

```antlr
enumextension
    : namespaceDeclaration? usingDeclarations?
      ENUMEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE enumValue* RIGHTCBRACE
    ;
```

#### `ALInterfaceParser.g4` (imports `ALPropertyParser`)

Interface methods have signatures but no body. The terminating semicolon is optional (both forms appear in BC26 sources). Object-level `keyValueProperty*` handles obsolete state properties (`ObsoleteState`, `ObsoleteReason`, `ObsoleteTag`). Imports `ALPropertyParser` (rather than just `ALCodeParser`) to make `keyValueProperty` available; `ALPropertyParser` already transitively imports `ALCodeParser`, so `methodAttribute`, `parameterList`, and `returnValue` remain accessible.

```antlr
interfaceMethodDeclaration
    : attributes=methodAttribute*?
      PROCEDURE name=identifier
      LEFTPAREN parameters=parameterList? RIGHTPAREN
      returnVal=returnValue? SEMICOLON?
    ;

interface
    : namespaceDeclaration? usingDeclarations?
      INTERFACE identifier
      LEFTCBRACE keyValueProperty* interfaceMethodDeclaration* RIGHTCBRACE
    ;
```

#### `ALTableExtensionParser.g4` (imports `ALTableParser`)

`ALTableParser.g4` already contains a correct `tableExtension` rule (with `tableProperties`, `tableExtEntities`, and `codeDeclarations`), along with `tableExtFieldGroups` and `tableExtFieldGroup` for extension-specific field groups using the `addlast` verb. The new `ALTableExtensionParser.g4` imports `ALTableParser` and inherits this rule without redefining it.

The existing `tableExtension` rule in `ALTableParser.g4` is moved to `ALTableExtensionParser.g4` as part of this work to keep each object type in its own file. The `ALTableParser.g4` import chain remains unchanged since `ALParser.g4` will now import `ALTableExtensionParser` which itself imports `ALTableParser`.

The inherited rule structure:
```antlr
// Defined in ALTableParser, inherited by ALTableExtensionParser via import:
// tableExtension
//     : namespaceDeclaration? usingDeclarations?
//       TABLEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
//       LEFTCBRACE tableProperties tableExtEntities? codeDeclarations? RIGHTCBRACE
//     ;
//
// tableExtEntities allows: tableFields, tableKeys, tableExtFieldGroups
// tableExtFieldGroups uses the addlast verb — distinct from tableFieldGroups (base table only)
```

#### `ALPageExtensionParser.g4` (imports `ALPageParser`)

Inherits `pageProperties`, `pageLayoutEntity`, `pageAction`, and the modification verbs from `ALPageParser`.

```antlr
pageExtensionLayoutSection
    : LAYOUT LEFTCBRACE pageLayoutModification* RIGHTCBRACE
    ;

pageExtensionActionSection
    : ACTIONS LEFTCBRACE pageActionModification* RIGHTCBRACE
    ;

pageextension
    : namespaceDeclaration? usingDeclarations?
      PAGEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE
          pageProperties
          pageExtensionLayoutSection?
          pageExtensionActionSection?
          codeDeclarations?
      RIGHTCBRACE
    ;
```

#### `ALPageCustomizationParser.g4` (imports `ALPageParser`)

Imports `ALPageParser` directly (not `ALPageExtensionParser`) so it does not inherit `add*`/`move*` verbs — only `modify` is valid in a customization. Uses the `customizes` soft keyword instead of `extends`. No object ID, no code, no triggers — only property modifications.

```antlr
// Restrict modify blocks to properties only — no triggers, no code
pageCustomizationLayoutEntity : pageGenericProperty ;

pageCustomizationActionEntity : pageGenericProperty ;

pageCustomizationLayoutModification
    : {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageCustomizationLayoutEntity* RIGHTCBRACE
    ;

pageCustomizationActionModification
    : {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageCustomizationActionEntity* RIGHTCBRACE
    ;

pageCustomizationLayoutSection
    : LAYOUT LEFTCBRACE pageCustomizationLayoutModification* RIGHTCBRACE
    ;

pageCustomizationActionSection
    : ACTIONS LEFTCBRACE pageCustomizationActionModification* RIGHTCBRACE
    ;

pagecustomization
    : namespaceDeclaration? usingDeclarations?
      PAGECUSTOMIZATION identifier
      {TokenMatches("customizes")}? IDENTIFIER identifier
      LEFTCBRACE
          pageCustomizationLayoutSection?
          pageCustomizationActionSection?
      RIGHTCBRACE
    ;
```

Separate `pageCustomizationLayoutEntity` and `pageCustomizationActionEntity` rules (properties only, no `triggerDeclaration`) enforce the constraint that customizations contain no code.

#### `ALReportExtensionParser.g4` (imports `ALReportParser`, `ALPageExtensionParser`)

Inherits `reportDataItem`, `reportColumn`, `reportDataItemProperty` from `ALReportParser`. Inherits `pageExtensionLayoutSection` and `pageExtensionActionSection` from `ALPageExtensionParser` for the requestpage modifications.

```antlr
reportExtensionDatasetModification
    : {TokenMatches("addafter") || TokenMatches("addbefore") ||
       TokenMatches("addfirst") || TokenMatches("addlast")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE reportDataItem* RIGHTCBRACE
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
      LEFTCBRACE pageExtensionLayoutSection? pageExtensionActionSection? RIGHTCBRACE
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
```

#### `ALProfileParser.g4` (imports `ALPropertyParser`)

Pure key/value properties: `Caption`, `ProfileDescription`, `RoleCenter`, `Enabled`, `Customizations`.

```antlr
profile
    : namespaceDeclaration? usingDeclarations?
      PROFILE identifier
      LEFTCBRACE keyValueProperty* RIGHTCBRACE
    ;
```

#### `ALProfileExtensionParser.g4` (imports `ALProfileParser`)

Uses the new `PROFILEEXTENSION` dedicated token added to `ALLexer.g4`.

```antlr
profileextension
    : namespaceDeclaration? usingDeclarations?
      PROFILEEXTENSION identifier EXTENDS identifier
      LEFTCBRACE keyValueProperty* RIGHTCBRACE
    ;
```

#### `ALPermissionSetParser.g4` (imports `ALPropertyParser`)

Adds `includedPermissionSetsProperty` for the `IncludedPermissionSets = id1, id2;` list form. Uses the expanded `permissionsProperty` from the shared grammar changes.

```antlr
includedPermissionSetsProperty
    : {TokenMatches("IncludedPermissionSets")}? identifier EQUAL identifierList SEMICOLON
    ;

permissionSetProperty
    : permissionsProperty
    | includedPermissionSetsProperty
    | keyValueProperty
    ;

permissionset
    : namespaceDeclaration? usingDeclarations?
      PERMISSIONSET INTEGER_LITERAL identifier
      LEFTCBRACE permissionSetProperty* RIGHTCBRACE
    ;
```

#### `ALPermissionSetExtensionParser.g4` (imports `ALPermissionSetParser`)

Inherits `permissionSetProperty` (and therefore `permissionsProperty` and `includedPermissionSetsProperty`).

```antlr
permissionsetextension
    : namespaceDeclaration? usingDeclarations?
      PERMISSIONSETEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE permissionSetProperty* RIGHTCBRACE
    ;
```

#### `ALEntitlementParser.g4` (imports `ALPropertyParser`)

`ObjectEntitlements` takes a comma-separated list of identifiers — distinct from `keyValueProperty`'s single value.

```antlr
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
```

#### `ALDotNetParser.g4` (imports `ALPropertyParser`)

The `dotnet` object has no ID or name — it is a file-level container. `DOTNET` is a dedicated token; `assembly` and `type` are soft keywords (there is no `ASSEMBLY` token — only `DOTNETASSEMBLY` which matches the keyword `dotnetassembly`).

Assembly and type names can be either quoted string literals or dot-separated identifier chains (e.g. `Microsoft.Dynamics.Nav.MX`). A `dotNetName` rule handles both forms:

```antlr
dotNetName
    : STRING_LITERAL
    | identifier (PERIOD identifier)*
    ;

dotNetType
    : {TokenMatches("type")}? IDENTIFIER
      LEFTPAREN dotNetName SEMICOLON dotNetName RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

dotNetAssembly
    : {TokenMatches("assembly")}? IDENTIFIER
      LEFTPAREN dotNetName RIGHTPAREN
      LEFTCBRACE keyValueProperty*? dotNetType* RIGHTCBRACE
    ;

dotnet
    : namespaceDeclaration? usingDeclarations?
      DOTNET
      LEFTCBRACE dotNetAssembly* RIGHTCBRACE
    ;
```

Imports `ALPropertyParser` (not just `ALCoreParser`) to make `keyValueProperty` available.

#### `ALControlAddInParser.g4` (imports `ALPropertyParser`)

Events and procedures have signatures but no body. The terminating semicolon is optional — both forms appear in the Lanham ACE Warehousing sources. `EVENT` and `PROCEDURE` are dedicated tokens; LL(*) lookahead unambiguously distinguishes them from `keyValueProperty` entries (which start with a plain `identifier` never preceded by a dedicated keyword token).

Imports `ALPropertyParser` (not just `ALCodeParser`) to make `keyValueProperty` available; `ALPropertyParser` transitively imports `ALCodeParser`, so `parameterList` and `returnValue` remain accessible.

```antlr
controlAddInEventDeclaration
    : EVENT identifier LEFTPAREN parameterList? RIGHTPAREN SEMICOLON?
    ;

controlAddInProcedureDeclaration
    : PROCEDURE identifier LEFTPAREN parameterList? RIGHTPAREN returnValue? SEMICOLON?
    ;

controlAddInMember
    : controlAddInEventDeclaration
    | controlAddInProcedureDeclaration
    | keyValueProperty
    ;

controladdin
    : namespaceDeclaration? usingDeclarations?
      CONTROLADDIN identifier
      LEFTCBRACE controlAddInMember* RIGHTCBRACE
    ;
```

### `ALParser.g4` Updates

```antlr
import ALCodeunitParser, ALTableParser, ALTableExtensionParser,
       ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
       ALQueryParser, ALReportParser, ALReportExtensionParser,
       ALXmlPortParser, ALEnumParser, ALEnumExtensionParser,
       ALInterfaceParser, ALProfileParser, ALProfileExtensionParser,
       ALPermissionSetParser, ALPermissionSetExtensionParser,
       ALEntitlementParser, ALDotNetParser, ALControlAddInParser;

alUnit
    : codeunitDeclaration
    | table          | tableExtension
    | page           | pageextension    | pagecustomization
    | query
    | report         | reportextension
    | xmlPort
    | enum           | enumextension
    | interface
    | profile        | profileextension
    | permissionset  | permissionsetextension
    | entitlement
    | dotnet
    | controladdin
    ;
```

Note: `tableExtension` retains its camelCase rule name (as defined in `ALTableParser.g4`) for consistency with the existing rule. All other new rules use lowercase names consistent with their keyword tokens.

## Test Strategy

Each new grammar file gets a corresponding `*ParserTests.cs` in the `ANTLR AL Parser Tests` project, following the established pattern:

- **Unit tests**: Inline AL source strings covering the minimal form, key property variants, and (for extension types) both add and modify verbs.
- **Real-file batch tests**: A `[Theory]` with `[MemberData]` that collects up to 40 `.TypeSuffix.al` files from the BC26 directories and asserts zero parse errors on each. This catches real-world patterns that inline tests miss.

### Source file paths for batch tests

| Type | Directory pattern | Suffix |
|---|---|---|
| enum | `BC26/**/Source` | `*.Enum.al` |
| enumextension | `BC26/**/Source` | `*.EnumExt.al` |
| interface | `BC26/**/Source` | `*.Interface.al` |
| tableextension | `BC26/**/Source` | `*.TableExt.al` |
| pageextension | `BC26/**/Source` | `*.PageExt.al` |
| pagecustomization | `BC26/**/Source` | `*.PageCust.al` |
| reportextension | `BC26/**/Source` | `*.ReportExt.al` |
| profile | `BC26/**/Source` | `*.Profile.al` |
| profileextension | `BC26/**/Source` | `*.ProfileExt.al` |
| permissionset | `BC26/**/Source` | `*.PermissionSet.al` |
| permissionsetextension | `BC26/**/Source` | `*.PermissionSetExt.al` |
| entitlement | `BC26/**/Source` | `*.Entitlement.al` |
| dotnet | `BC26/**/DotNet*` | `dotnet*.al` |
| controladdin | Lanham ACE Warehousing `Source/Control Addins` | `*.al` |
