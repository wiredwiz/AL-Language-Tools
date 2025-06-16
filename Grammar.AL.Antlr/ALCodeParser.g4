parser grammar ALCodeParser;

options { tokenVocab=ALLexer; }

/*
 * AL Code Parser
 This parser is designed to parse AL code, including variable declarations,
 * method declarations, control structures, expressions and codeunits.
 */

@parser::members
{  // Define our member text lists
   List<string> Builtins = new List<string>()
      {
         "action", "array", "automation", "biginteger", "bigtext", "blob", "boolean", "byte",
         "char", "clienttype", "code", "codeunit", "completiontriggererrorlevel", "connectiontype",
         "database", "dataclassification", "datascope", "date", "dateformula", "datetime", "decimal",
         "defaultlayout", "dialog", "dictionary", "dotnet", "dotnetassembly", "dotnettypedeclaration",
         "duration", "enum", "errorinfo", "errortype", "executioncontext", "executionmode", "fieldclass",
         "fieldref", "fieldtype", "file", "filterpagebuilder", "guid", "instream", "integer", "joker",
         "keyref", "list", "moduledependencyinfo", "moduleinfo", "none", "notification",
         "notificationscope", "objecttype", "option", "outstream", "page", "pageresult", "query",
         "record", "recordid", "recordref", "report", "reportformat", "securityfilter", "securityfiltering",
         "table", "tableconnectiontype", "tablefilter", "testaction", "testfield", "testfilterfield",
         "testpage", "testpermissions", "testrequestpage", "text", "textbuilder", "textconst", "textencoding",
         "time", "transactionmodel", "transactiontype", "variant", "verbosity", "version", "xmlport",
         "httpcontent", "httpheaders", "httpclient", "httprequestmessage", "httpresponsemessage",
         "jsontoken", "jsonvalue", "jsonarray", "jsonobject", "view", "views", "xmlattribute",
         "xmlattributecollection", "xmlcomment", "xmlcdata", "xmldeclaration", "xmldocument",
         "xmldocumenttype", "xmlelement", "xmlnamespacemanager", "xmlnametable", "xmlnode",
         "xmlnodelist", "xmlprocessinginstruction", "xmlreadoptions", "xmltext", "xmlwriteoptions",
         "webserviceactioncontext", "webserviceactionresultcode", "sessionsettings;"
      };

      List<string> VariableTypes = new List<string>()
      {
         "any", "biginteger", "bigtext", "blob", "boolean", "byte", "char", "code",
         "codeunit", "companyproperty", "database", "datatransfer", "date", "dateformula",
         "datetime", "debugger", "decimal", "dialog", "dictionary", "dotnet", "duration",
         "enum", "errorinfo", "fieldref", "file", "filterpagebuilder", "guid", "httpclient",
         "httpcontent", "httpheaders", "httprequestmessage", "httpresponsemessage",
         "instream", "integer", "isolatedstorage", "jsonarray", "jsonobject", "jsontoken", "jsonvalue",
         "keyref", "label", "list", "media", "mediaset", "moduledependencyinfo", "moduleinfo",
         "navapp", "none", "notification", "numbersequence", "option", "outstream", "page", "productname", "query",
         "record", "recordid", "recordref", "report", "requestpage", "session", "sessioninformation",
         "sessionsettings", "system", "taskscheduler", "testaction", "testfield",
         "testfilter", "testfilterfield", "testpage", "testpart", "testrequestpage", "text",
         "textbuilder", "textconst", "time", "variant", "version", "webserviceactioncontext",
         "xmlattribute", "xmlattributecollection", "xmlcdata", "xmlcomment", "xmldeclaration",
         "xmldocument", "xmldocumenttype", "xmlelement", "xmlnamespacemanager", "xmlnametable",
         "xmlnode", "xmlnodelist", "xmlport", "xmlprocessinginstruction", "xmlreadoptions", "xmltext",
         "xmlwriteoptions", "action", "auditcategory", "clienttype", "commitbehavior", "dataclassification",
         "datascope", "defaultlayout", "errorbehavior", "errortype", "executioncontext", "executionmode",
         "fieldclass", "fieldtype", "inherentpermissionsscope", "isolationlevel", "notificationscope",
         "objecttype", "pagebackgroundtaskerrorlevel", "permissionobjecttype", "reportformat", "reportlayouttype",
         "securityfilter", "securityoperationresult", "tableconnectiontype", "telemetryscope", "testpermissions",
         "textencoding", "transactionmodel", "transactiontype", "verbosity", "webserviceactionresultcode"
      };

      List<string> VarAppObjects = new List<string>() {"codeunit", "page", "requestpage", "dotnet", "enum", "query", "report", "xmlport"};

      List<string> SimpleFieldTypes = new List<string>()
      {
         "integer", "biginteger", "decimal", "enum", "boolean", "binary", "blob", "date", "time", "datetime", "dateformula", "duration", "recordid", "tablefilter", "option", "guid"
      };

      // Positioning keywords for various entities (Groups, Areas, Actions, Controls)
      List<string> GroupPosKeywords = new List<string>()
      {
         "addfirst", "addlast", "addafter", "addbefore", "movefirst", "movelast", "moveafter", "movebefore", "modify"
      };

      List<string> AreaPosKeywords = new List<string>()
      {
         "addfirst", "addlast", "movefirst", "movelast"
      };

      List<string> ActionPosKeywords = new List<string>()
      {
         "addafter", "addbefore", "moveafter", "movebefore", "modify"
      };

      List<string> ControlPosKeywords = new List<string>()
      {
         "addafter", "addbefore", "moveafter", "movebefore", "modify"
      };

      List<string> ObsoleteStateValues = new List<string>()
      {
         "pending", "no"
      };


      // Define helper methods for token text matching

      bool TokenMatches(string text)
      {
         return _input.Lt(1).Text.ToLowerInvariant() == text.ToLowerInvariant();
      }

      bool TokenMatches(List<string> values)
      {
         return values.Contains(_input.Lt(1).Text.ToLowerInvariant());
      }
}

/*
 * Generic constructs
 */

identifier
   : IDENTIFIER
   | ABS
   | ACTION
   | ACTIONREF
   | ACTIVATE
   | ACTIVE
   | ACTIVECONTROLONOPEN
   | ADDLINK
   | ADDTEXT
   | ALTSEARCHFIELD
   | APPLICATIONPATH
   | ARRAY
   | ARRAYLEN
   | ASCENDING
   | ASSERTERROR
   | ASSISTEDIT
   | AUDITCATEGORY
   | AUTOCALCFIELD
   | AUTOENTER
   | AUTOFORMATEXPR
   | AUTOFORMATTYPE
   | AUTOINCREMENT
   | AUTOMATION
   | AUTOPOSITION
   | AUTOREPEAT
   | AUTOREPLACE
   | AUTOSAVE
   | AUTOSPLITKEY
   | AUTOUPDATE
   | AVERAGE
   | BACKCOLOR
   | BACKTRANSPARENT
   | BACKUPKEY
   | BEEP
   | BEGIN
   | BIGINTEGER
   | BIGTEXT
   | BINARY
   | BITMAP
   | BITMAPLIST
   | BITMAPPOS
   | BLANKNUMBERS
   | BLANKZERO
   | BLOB
   | BOOLEAN
   | BORDER
   | BORDERCOLOR
   | BORDERSTYLE
   | BORDERWIDTH
   | BOTTOMMARGIN
   | BREAK
   | BYTE
   | CALCDATE
   | CALCFIELD
   | CALCFIELDS
   | CALCFORMULA
   | CALCSUM
   | CALCSUMS
   | CALLFIELDVALIDATE
   | CANCEL
   | CAPTION
   | CAPTIONBAR
   | CAPTIONCLASS
   | CAPTIONML
   | CARDPAGEID
   | CASE
   | CHANGECOMPANY
   | CHAR
   | CHARALLOWED
   | CHARTPARTID
   | CHECKLICENSEFILE
   | CLASS
   | CLEAR
   | CLEARALL
   | CLEARLASTERROR
   | CLEARMARKS
   | CLEARONLOOKUP
   | CLIENTTYPE
   | CLOSE
   | CLOSINGDATE
   | CLOSINGDATES
   | CLUSTERED
   | CODE
   | CODECOVERAGELOG
   | CODEUNIT
   | COMMANDLINE
   | COMMIT
   | COMMITBEHAVIOR
   | COMPANYNAME
   | COMPLETIONTRIGGERERRORLEVEL
   | COMPRESSARRAY
   | COMPRESSED
   | CONFIRM
   | CONNECTIONTYPE
   | CONSISTENT
   | CONST
   | CONTAINERTYPE
   | CONTEXTURL
   | CONTINUE
   | CONTROLADDIN
   | CONTROLTYPE
   | CONVERTSTR
   | COPY
   | COPYARRAY
   | COPYFILTER
   | COPYFILTERS
   | COPYLINKS
   | COPYSTR
   | COPYSTREAM
   | COUNT
   | COUNTAPPROX
   | CREATE
   | CREATEDATETIME
   | CREATEGUID
   | CREATEINSTREAM
   | CREATEOUTSTREAM
   | CREATETEMPFILE
   | CURRENTDATETIME
   | CURRENTEXECUTIONMODE
   | CURRENTKEY
   | CURRENTKEYINDEX
   | CURRENTTRANSACTIONTYPE
   | CURRPAGE
   | CURRREPORT
   | CUSTOMACTION
   | DATABASE
   | DATACAPTIONEXPR
   | DATACAPTIONFIELDS
   | DATACLASSIFICATION
   | DATAITEMINDENT
   | DATAITEMLINK
   | DATAITEMLINKREFERENCE
   | DATAITEMSEPARATOR
   | DATAITEMTABLE
   | DATAITEMTABLEVIEW
   | DATALENGTH
   | DATAPERCOMPANY
   | DATASCOPE
   | DATATRANSFER
   | DATE
   | DATE2DMY
   | DATE2DWY
   | DATEFORMULA
   | DATETIME
   | DATI2VARIANT
   | DEBUGGER
   | DECIMAL
   | DECIMALPLACES
   | DECIMALPLACESMAX
   | DECIMALPLACESMIN
   | DEFAULT
   | DEFAULTFIELDSVALIDATION
   | DEFAULTLAYOUT
   | DELAYEDINSERT
   | DELCHR
   | DELETE
   | DELETEALL
   | DELETEALLOWED
   | DELETELINK
   | DELETELINKS
   | DELSTR
   | DESCRIPTION
   | DEVICEFONTNAME
   | DIALOG
   | DICTIONARY
   | DIMENSIONS
   | DIRECTION
   | DIVISOR
   | DMY2DATE
   | DO
   | DOTNET
   | DOTNETASSEMBLY
   | DOTNETTYPEDECLARATION
   | DOWNLOAD
   | DOWNLOADFROMSTREAM
   | DOWNTO
   | DRILLDOWN
   | DRILLDOWNPAGEID
   | DROPDOWN
   | DT2DATE
   | DT2TIME
   | DUPLICATE
   | DURATION
   | DWY2DATE
   | EDITABLE
   | ELLIPSIS
   | ELSE
   | ENABLED
   | ENABLEDIFERROR
   | ENABLEDIFINSERTION
   | ENABLEDIFSELECTION
   | ENCODING
   | END
   | ENTITLEMENT
   | ENUM
   | ENUMEXTENSION
   | ENVIRON
   | EOS
   | ERASE
   | ERROR
   | ERRORBEHAVIOR
   | ERRORCONTEXT
   | ERRORINFO
   | ERRORTYPE
   | EVALUATE
   | EXECUTIONCONTEXT
   | EXECUTIONMODE
   | EVENT
   | EXIST
   | EXISTS
   | EXIT
   | EXPORT
   | EXPORTOBJECTS
   | EXTENDEDDATATYPE
   | EXTENDS
   | FIELD
   | FIELDACTIVE
   | FIELDCAPTION
   | FIELDCLASS
   | FIELDCOUNT
   | FIELDENDDELIMITER
   | FIELDERROR
   | FIELDEXIST
   | FIELDGROUP
   | FIELDGROUPS
   | FIELDINDEX
   | FIELDNAME
   | FIELDNO
   | FIELDREF
   | FIELDSEPARATOR
   | FIELDTYPE
   | FIELDSTARTDELIMITER
   | FIELDVALIDATE
   | FILE
   | FILEFORMAT
   | FILENAME
   | FILTER
   | FILTERGROUP
   | FILTERPAGEBUILDER
   | FIND
   | FINDFIRST
   | FINDLAST
   | FINDSET
   | FOCUSABLE
   | FOCUSONCLICK
   | FONTBOLD
   | FONTITALIC
   | FONTNAME
   | FONTSIZE
   | FONTSTRIKETHRU
   | FONTUNDERLINE
   | FOR
   | FOREACH
   | FORECOLOR
   | FORMAT
   | FREEZECOLUMNID
   | FUNCTION
   | FUNCTIONTYPE
   | GET
   | GETFILTER
   | GETFILTERS
   | GETLASTERRORTEXT
   | GETPOSITION
   | GETRANGEMAX
   | GETRANGEMIN
   | GETRECORD
   | GETSTAMP
   | GETSUBTEXT
   | GETTABLE
   | GETURL
   | GETVIEW
   | GLOBALLANGUAGE
   | GROUPTOTALFIELDS
   | GROUPTYPE
   | GUIALLOWED
   | GUID
   | HANDLERFUNCTIONS
   | HASFILTER
   | HASLINKS
   | HASVALUE
   | HEADINGHEIGHT
   | HEIGHT
   | HIDEVALUE
   | HORZALIGN
   | HORZGLUE
   | HORZGRID
   | HTTPCONTENT
   | HTTPHEADERS
   | HTTPCLIENT
   | HTTPREQUESTMESSAGE
   | HTTPRESPONSEMESSAGE
   | HYPERLINK
   | ID
   | IF
   | IMAGE
   | IMPLEMENTS
   | IMPORT
   | IMPORTANCE
   | IMPORTOBJECTS
   | IN
   | INCLUDEINDATASET
   | INCOLUMN
   | INCOLUMNHEADING
   | INCSTR
   | INDATASET
   | INDENT
   | INDENTATION
   | INDENTATIONCOLUMNNAME
   | INDENTATIONCONTROLS
   | INFOOTERBAR
   | INFRAME
   | INHERENTPERMISSIONSSCOPE
   | INIT
   | INITVALUE
   | INLINEEDITING
   | INMATRIX
   | INMATRIXHEADING
   | INPAGE
   | INPUT
   | INSERT
   | INSERTALLOWED
   | INSSTR
   | INSTREAM
   | INSTRUCTIONALTEXTML
   | INTEGER
   | INTERFACE
   | INTERNAL
   | INVALIDACTIONAPPEARANCE
   | ISACTION
   | ISAUTOMATION
   | ISBINARY
   | ISBOOLEAN
   | ISCHAR
   | ISCLEAR
   | ISCODE
   | ISCODEUNIT
   | ISDATE
   | ISDATEFORMULA
   | ISDECIMAL
   | ISEMPTY
   | ISFILE
   | ISINSTREAM
   | ISINTEGER
   | ISNULLGUID
   | ISOLATIONLEVEL
   | ISOPTION
   | ISOUTSTREAM
   | ISRECORD
   | ISTEXT
   | ISTIME
   | ISTRANSACTIONTYPE
   | JOKER
   | JSONTOKEN
   | JSONVALUE
   | JSONARRAY
   | JSONOBJECT
   | KEEPWITHNEXT
   | KEY
   | KEYCOUNT
   | KEYGROUPDISABLE
   | KEYGROUPENABLE
   | KEYGROUPENABLED
   | KEYGROUPS
   | KEYINDEX
   | KEYREF
   | LABEL
   | LANGUAGE
   | LEADERDOTS
   | LEFTMARGIN
   | LEN
   | LENGTH
   | LINKEDINTRANSACTION
   | LINKEDOBJECT
   | LINKFIELDS
   | LINKSALLOWED
   | LINKTABLE
   | LINKTABLEFORCEINSERT
   | LIST
   | LOCAL
   | LOCKTABLE
   | LOCKTIMEOUT
   | LOGHEIGHT
   | LOGWIDTH
   | LOOKUP
   | LOOKUPMODE
   | LOOKUPPAGEID
   | LOWERCASE
   | MAINTAINSIFTINDEX
   | MAINTAINSQLINDEX
   | MARK
   | MARKEDONLY
   | MATRIXCOLUMNWIDTH
   | MATRIXSOURCETABLE
   | MAX
   | MAXIMIZABLE
   | MAXIMIZEDONOPEN
   | MAXITERATION
   | MAXLENGTH
   | MAXOCCURS
   | MAXSTRLEN
   | MAXVALUE
   | MEDIA
   | MEDIASET
   | MENUITEMTYPE
   | MENULEVEL
   | MESSAGE
   | MIN
   | MINIMIZABLE
   | MINIMIZEDONOPEN
   | MINOCCURS
   | MINVALUE
   | MODIFY
   | MODIFYALL
   | MODIFYALLOWED
   | MODULEDEPENDENCYINFO
   | MODULEINFO
   | MULTILINE
   | MULTIPLENEWLINES
   | NAME
   | NAMESPACE
   | NEWPAGEPERGROUP
   | NEXT
   | NEXTCONTROL
   | NODENAME
   | NODETYPE
   | NONE
   | NOTIFICATION
   | NOTIFICATIONSCOPE
   | NORMALDATE
   | NOTBLANK
   | NUMBER
   | NUMERIC
   | OBJECTTYPE
   | OCCURRENCE
   | OCX
   | OF
   | ONACTION
   | ONACTIVATE
   | ONAFTERASSIGNFIELD
   | ONAFTERASSIGNVARIABLE
   | ONAFTEREXPORTRECORD
   | ONAFTERFORMATFIELD
   | ONAFTERGETFIELD
   | ONAFTERGETRECORD
   | ONAFTERIMPORTRECORD
   | ONAFTERINITRECORD
   | ONAFTERINPUT
   | ONAFTERINSERTRECORD
   | ONAFTERMODIFYRECORD
   | ONAFTERTESTRUN
   | ONAFTERVALIDATE
   | ONASSISTEDIT
   | ONBEFOREEVALUATEFIELD
   | ONBEFOREEXPORTRECORD
   | ONBEFOREIMPORTRECORD
   | ONBEFOREINPUT
   | ONBEFOREINSERTRECORD
   | ONBEFOREMODIFYRECORD
   | ONBEFOREPASSFIELD
   | ONBEFOREPASSVARIABLE
   | ONBEFOREPUTRECORD
   | ONBEFORETESTRUN
   | ONCLOSEPAGE
   | ONCONTROLADDIN
   | ONDEACTIVATE
   | ONDELETE
   | ONDELETERECORD
   | ONDRILLDOWN
   | ONFINDRECORD
   | ONFORMAT
   | ONINIT
   | ONINITREPORT
   | ONINITXMLPORT
   | ONINPUTCHANGE
   | ONINSERT
   | ONINSERTRECORD
   | ONLOOKUP
   | ONMODIFY
   | ONMODIFYRECORD
   | ONNEWRECORD
   | ONNEXTRECORD
   | ONOPENPAGE
   | ONPOSTDATAITEM
   | ONPOSTREPORT
   | ONPOSTSECTION
   | ONPOSTXMLPORT
   | ONPREDATAITEM
   | ONPREREPORT
   | ONPRESECTION
   | ONPREXMLITEM
   | ONPREXMLPORT
   | ONPUSH
   | ONQUERYCLOSEPAGE
   | ONRENAME
   | ONTIMER
   | ONVALIDATE
   | OPEN
   | OPTION
   | OPTIONCAPTION
   | OPTIONCAPTIONML
   | OPTIONSTRING
   | OPTIONVALUE
   | ORDER
   | ORIENTATION
   | OSVERSION
   | OUTSTREAM
   | PADCHAR
   | PADSTR
   | PAGE
   | PAGEBACKGROUNDTASKERRORLEVEL
   | PAGECUSTOMIZATION
   | PAGEEXTENSION
   | PAGENAMES
   | PAGENAMESML
   | PAGENO
   | PAGERESULT
   | PAGEPARTID
   | PAGETYPE
   | PAPERSIZE
   | PAPERSOURCE
   | PAPERSOURCEDEFAULTPAGE
   | PAPERSOURCEFIRSTPAGE
   | PAPERSOURCELASTPAGE
   | PAPERSOURCEOTHERPAGES
   | PARENTCONTROL
   | PARTTYPE
   | PASSWORDTEXT
   | PASTEISVALID
   | PERCENTAGE
   | PERMANENTASSIST
   | PERMISSIONOBJECTTYPE
   | PERMISSIONS
   | PERMISSIONSET
   | PERMISSIONSETEXTENSION
   | PLACEINBOTTOM
   | POPULATEALLFIELDS
   | POS
   | POWER
   | PREVIEW
   | PRINTONEVERYPAGE
   | PRINTONLYIFDETAIL
   | PROCEDURE
   | PROCESSINGONLY
   | PROFILE
   | PROGRAM
   | PROMOTED
   | PROMOTEDCATEGORY
   | PROMOTEDISBIG
   | PROTECTED
   | PROVIDERID
   | PUSHACTION
   | QUERY
   | QUERYREPLACE
   | QUIT
   | RANDOM
   | RANDOMIZE
   | READ
   | READCONSISTENCY
   | READPERMISSION
   | READTEXT
   | REC
   | RECORD
   | RECORDID
   | RECORDLEVELLOCKING
   | RECORDREF
   | RECORDSEPARATOR
   | RELATION
   | RENAME
   | REPEAT
   | REPORT
   | REPORTEXTENSION
   | REPORTFORMAT
   | REPORTLAYOUTTYPE
   | REQFILTERFIELDS
   | REQFILTERHEADING
   | REQFILTERHEADINGML
   | REQUESTOPTIONSPAGE
   | RESET
   | RIGHTMARGIN
   | ROUND
   | ROUNDDATETIME
   | ROWHEIGHT
   | RUN
   | RUNCOMMAND
   | RUNMODAL
   | RUNOBJECT
   | RUNONCLIENT
   | RUNPAGELINK
   | RUNPAGEONREC
   | RUNPAGEVIEW
   | SAVEASEXCEL
   | SAVEASHTML
   | SAVEASPDF
   | SAVEASXML
   | SAVECOLUMNWIDTHS
   | SAVECONTROLINFO
   | SAVEPOSANDSIZE
   | SAVERECORD
   | SAVEVALUES
   | SECRETTEXT
   | SECTIONHEIGHT
   | SECTIONWIDTH
   | SECURITYFILTER
   | SECURITYFILTERING
   | SECURITYOPERATIONRESULT
   | SEEK
   | SELECTLATESTVERSION
   | SELECTSTR
   | SERIALNUMBER
   | SESSIONSETTINGS
   | SETAUTOCALCFIELDS
   | SETCURRENTKEY
   | SETFILTER
   | SETPERMISSIONFILTER
   | SETPOSITION
   | SETRANGE
   | SETRECFILTER
   | SETRECORD
   | SETSELECTIONFILTER
   | SETSTAMP
   | SETTABLE
   | SETTABLEVIEW
   | SETVIEW
   | SHAPESTYLE
   | SHELL
   | SHORTCUTKEY
   | SHOWASTREE
   | SHOWCAPTION
   | SHOWPRINTSTATUS
   | SHOWSTATUS
   | SID
   | SIGNDISPLACEMENT
   | SINGLEINSTANCE
   | SIZABLE
   | SLEEP
   | SORTING
   | SOURCEEXPR
   | SOURCEFIELD
   | SOURCETABLE
   | SOURCETABLEPLACEMENT
   | SOURCETABLERECORD
   | SOURCETABLETEMPORARY
   | SOURCETABLEVIEW
   | SOURCETYPE
   | SQLINDEX
   | STARTPOS
   | STARTSESSION
   | STOPSESSION
   | STRCHECKSUM
   | STRLEN
   | STRMENU
   | STRPOS
   | STRSUBSTNO
   | STYLE
   | STYLEEXPR
   | SUBPAGELINK
   | SUBPAGEVIEW
   | SUBTYPE
   | SUM
   | SUMINDEXFIELDS
   | SUPPRESSDISPOSE
   | SYNCHRONIZEALLLOGINS
   | SYNCHRONIZESINGLELOGIN
   | SYSTEM
   | SYSTEMACTION
   | SYSTEMPARTID
   | TABLE
   | TABLEBOXID
   | TABLECAPTION
   | TABLECONNECTIONTYPE
   | TABLEDATA
   | TABLEEXTENSION
   | TABLEFILTER
   | TABLEIDEXPR
   | TABLENAME
   | TABLENO
   | TABLERELATION
   | TELEMETRYSCOPE
   | TEMPORARY
   | TEMPORARYPATH
   | TESTACTION
   | TESTFIELD
   | TESTPAGE
   | TESTPERMISSIONS
   | TESTREQUESTPAGE
   | TESTTABLERELATION
   | TEXT
   | TEXTBUILDER
   | TEXTCONST
   | TEXTENCODING
   | TEXTMODE
   | TEXTPOS
   | TEXTTYPE
   | THEN
   | TIME
   | TIMERINTERVAL
   | TITLE
   | TO
   | TODAY
   | TOOLTIP
   | TOOLTIPML
   | TOPLINEONLY
   | TOPMARGIN
   | TOTALFIELDS
   | TOTALSCAUSEDBY
   | TRANSACTIONMODEL
   | TRANSACTIONTYPE
   | TRANSFERFIELDS
   | TRIGGER
   | UNTIL
   | TRUNC
   | TYPE
   | UPDATE
   | UPDATECONTROLS
   | UPDATEEDITABLE
   | UPDATEFONTBOLD
   | UPDATEFORECOLOR
   | UPDATEINDENT
   | UPDATEONACTION
   | UPDATEONACTIVATE
   | UPDATESELECTED
   | UPLOAD
   | UPLOADINTOSTREAM
   | UPPERCASE
   | UPPERLIMIT
   | USEREQUESTPAGE
   | USERID
   | USESYSTEMPRINTER
   | USING
   | VALIDATE
   | VALIDATETABLERELATION
   | VALUE
   | VALUESALLOWED
   | VAR
   | VARIABLEACTIVE
   | VARIABLENAME
   | VARIANT
   | VARIANT2DATE
   | VARIANT2TIME
   | VERBOSITY
   | VERSION
   | VERTALIGN
   | VERTGLUE
   | VERTGRID
   | VIEW
   | VIEWS
   | VISIBLE
   | WEBSERVICEACTIONCONTEXT
   | WEBSERVICEACTIONRESULTCODE
   | WHERE
   | WHILE
   | WIDTH
   | WINDOWSLANGUAGE
   | WITH
   | WITHEVENTS
   | WORDDATE
   | WRITE
   | WRITEMODE
   | WRITEPERMISSION
   | WRITETEXT
   | XMLATTRIBUTE
   | XMLATTRIBUTECOLLECTION
   | XMLCOMMENT
   | XMLCDATA
   | XMLDECLARATION
   | XMLDOCUMENT
   | XMLDOCUMENTTYPE
   | XMLELEMENT
   | XMLNAMESPACEMANAGER
   | XMLNAMETABLE
   | XMLNODE
   | XMLNODELIST
   | XMLPORT
   | XMLPROCESSINGINSTRUCTION
   | XMLREADOPTIONS
   | XMLTEXT
   | XMLVERSIONNO
   | XMLWRITEOPTIONS
   | XPOS
   | XREC
   | YIELD
   | YPOS
   ;

namespaceIdentifier
   : identifier namespaceAccessor?
   ;

namespaceAccessor
   : PERIOD identifier namespaceAccessor?
   ;

objectId
   : simpleObjectId
   | qualifiedObjectId;

objectName
   : identifier
   ;

simpleObjectId
   : identifier
   | INTEGER_LITERAL;

qualifiedObjectId
   : namespaceIdentifier PERIOD identifier
   ;

booleanLiteral
   : TRUE
   | FALSE
   ;

numberLiteral
   : FLOAT_LITERAL
   | INTEGER_LITERAL
   ;

objectLiteral
   : (namespaceIdentifier PERIOD)? identifier
   ;

enumerationLiteral
   : objectLiteral SCOPE identifier
   ;

systemEnumerationLiteral
   : OBJECTTYPE SCOPE identifier #ObjectTypeLiteral
   | TRANSACTIONTYPE SCOPE identifier #TransactionTypeLiteral
   | EXECUTIONMODE SCOPE identifier #ExecutionModeLiteral
   | SECURITYFILTER SCOPE identifier #SecurityFilterLiteral
   | TEXTENCODING SCOPE identifier #TextEncodingLiteral
   | REPORTFORMAT SCOPE identifier #ReportFormatLiteral
   | DEFAULTLAYOUT SCOPE identifier #DefaultLayoutLiteral
   | CONNECTIONTYPE SCOPE identifier #ConnectionTypeLiteral
   | CLIENTTYPE SCOPE identifier #ClientTypeLiteral
   | COMMITBEHAVIOR SCOPE identifier #CommitBehaviorLiteral
   | DATASCOPE SCOPE identifier #DataScopeLiteral
   | DATACLASSIFICATION SCOPE identifier #DataClassificationLiteral
   | ACTION SCOPE identifier #ActionLiteral
   | AUDITCATEGORY SCOPE identifier #AuditCategoryLiteral
   | EXECUTIONCONTEXT SCOPE identifier #ExecutionContextLiteral
   | ERRORBEHAVIOR SCOPE identifier #ErrorBehaviorLiteral
   | ERRORTYPE SCOPE identifier #ErrorTypeLiteral
   | ERRORCONTEXT SCOPE identifier #ErrorContextLiteral
   | FIELDCLASS SCOPE identifier #FieldClassLiteral
   | FIELDTYPE SCOPE identifier #FieldTypeLiteral
   | INHERENTPERMISSIONSSCOPE SCOPE identifier #InherentPermissionsScopeLiteral
   | ISOLATIONLEVEL SCOPE identifier #IsolationLevelLiteral
   | NOTIFICATIONSCOPE SCOPE identifier #NotificationScopeLiteral
   | OBJECTTYPE SCOPE identifier #ObjectTypeLiteral
   | PAGEBACKGROUNDTASKERRORLEVEL SCOPE identifier #PageBackgroundTaskErrorLevelLiteral
   | PERMISSIONOBJECTTYPE SCOPE identifier #PermissionObjectTypeLiteral
   | REPORTLAYOUTTYPE SCOPE identifier #ReportLayoutTypeLiteral
   | SECURITYOPERATIONRESULT SCOPE identifier #SecurityOperationResultLiteral
   | TABLECONNECTIONTYPE SCOPE identifier #TableConnectionTypeLiteral
   | TELEMETRYSCOPE SCOPE identifier #TelemetryScopeLiteral
   | TESTPERMISSIONS SCOPE identifier #TestPermissionsLiteral
   | TRANSACTIONMODEL SCOPE identifier #TransactionModelLiteral
   | TRANSACTIONTYPE SCOPE identifier #TransactionTypeLiteral
   | VERBOSITY SCOPE identifier #VerbosityLiteral
   | WEBSERVICEACTIONRESULTCODE SCOPE identifier #WebServiceActionResultCodeLiteral
   | CODEUNIT SCOPE objectLiteral #CodeunitLiteral
   | PAGE SCOPE objectLiteral #PageLiteral
   | PAGEEXTENSION SCOPE objectLiteral #PageExtensionLiteral
   | QUERY SCOPE objectLiteral #QueryLiteral
   | REPORT SCOPE objectLiteral #ReportLiteral
   | XMLPORT SCOPE objectLiteral #XmlPortLiteral
   | DATABASE SCOPE objectLiteral #DatabaseLiteral
   | ENUM SCOPE objectLiteral #EnumLiteral
   | TESTPAGE SCOPE objectLiteral #TestPageLiteral
   | TESTREQUESTPAGE SCOPE objectLiteral #TestRequestPageLiteral
   ;

/*
 * Property declarations
 */

keyValueProperty
   : identifier EQUAL (STRING_LITERAL | INTEGER_LITERAL | FLOAT_LITERAL | identifier | booleanLiteral) SEMICOLON
   ;

identifierList
   : identifier (COMMA identifier)*
   ;

keyIdentifierListProperty
   : identifier EQUAL identifierList? SEMICOLON
   ;

permissionSpecifier
   : TABLEDATA objectId EQUAL identifier
   ;

permissionSpecifiers
   : permissionSpecifier (COMMA permissionSpecifier)*;

permissionsProperty
   : PERMISSIONS EQUAL permissionSpecifiers SEMICOLON
   ;

/*
 * Type declarations
 */

sizeDeclaration : LEFTBRACKET INTEGER_LITERAL RIGHTBRACKET;

builtinType
   : identifier;
   /*
   : ACTION | ARRAY | AUTOMATION | BIGINTEGER | BIGTEXT | BLOB | BOOLEAN | BYTE | CHAR | CLIENTTYPE | CODE
   | CODEUNIT | COMPLETIONTRIGGERERRORLEVEL | CONNECTIONTYPE | DATABASE | DATACLASSIFICATION | DATASCOPE
   | DATE | DATEFORMULA | DATETIME | DECIMAL | DEFAULTLAYOUT | DIALOG | DICTIONARY | DOTNET | DOTNETASSEMBLY
   | DOTNETTYPEDECLARATION | DURATION | ENUM | ERRORINFO | ERRORTYPE | EXECUTIONCONTEXT | EXECUTIONMODE
   | FIELDCLASS | FIELDREF | FIELDTYPE | FILE | FILTERPAGEBUILDER | GUID | INSTREAM | INTEGER | JOKER
   | KEYREF | LIST | MODULEDEPENDENCYINFO | MODULEINFO | NONE | NOTIFICATION | NOTIFICATIONSCOPE | OBJECTTYPE
   | OPTION | OUTSTREAM | PAGE | PAGERESULT | QUERY | RECORD | RECORDID | RECORDREF | REPORT | REPORTFORMAT
   | SECURITYFILTER | SECURITYFILTERING | TABLE | TABLECONNECTIONTYPE | TABLEFILTER | TESTACTION | TESTFIELD
   | TESTFILTERFIELD | TESTPAGE | TESTPERMISSIONS | TESTREQUESTPAGE | TEXT | TEXTBUILDER | TEXTCONST
   | TEXTENCODING | TIME | TRANSACTIONMODEL | TRANSACTIONTYPE | VARIANT | VERBOSITY | VERSION | XMLPORT
   | HTTPCONTENT | HTTPHEADERS | HTTPCLIENT | HTTPREQUESTMESSAGE | HTTPRESPONSEMESSAGE | JSONTOKEN
   | JSONVALUE | JSONARRAY | JSONOBJECT | VIEW | VIEWS | XMLATTRIBUTE | XMLATTRIBUTECOLLECTION | XMLCOMMENT
   | XMLCDATA | XMLDECLARATION | XMLDOCUMENT | XMLDOCUMENTTYPE | XMLELEMENT | XMLNAMESPACEMANAGER
   | XMLNAMETABLE | XMLNODE | XMLNODELIST | XMLPROCESSINGINSTRUCTION | XMLREADOPTIONS | XMLTEXT
   | XMLWRITEOPTIONS | WEBSERVICEACTIONCONTEXT | WEBSERVICEACTIONRESULTCODE | SESSIONSETTINGS;
*/

/*
 * Method
 */

methodName
   : identifier;

methodDeclaration
   : methodAttribute*? LOCAL? PROCEDURE identifier LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

/*
 * Method attributes
 */

attributeIdentifier
   : identifier COLON;

attributeArgument
   : (attributeIdentifier)? (STRING_LITERAL | INTEGER_LITERAL	| FLOAT_LITERAL | identifier | booleanLiteral | systemEnumerationLiteral | enumerationLiteral);

attributeArgumentList
   : attributeArgument (COMMA attributeArgument)*?;

methodAttribute
   : LEFTBRACKET identifier (LEFTPAREN attributeArgumentList? RIGHTPAREN)? RIGHTBRACKET;

/*
 * Variables
 */

dictionaryKey
   : variableTypeDeclaration
   ;

dictionaryDataType
   : variableTypeDeclaration
   ;

labelText
   : STRING_LITERAL
   ;

labelMaxLength
   : {TokenMatches("MaxLength")}? identifier EQUAL INTEGER_LITERAL
   ;

labelComment
   : {TokenMatches("Comment")}? identifier EQUAL STRING_LITERAL
   ;

labelLocked
   : {TokenMatches("Locked")}? identifier EQUAL (TRUE | FALSE)
   ;

labelArgument
   : labelMaxLength
   | labelComment
   | labelLocked
   ;

labelArgs
   : labelArgument (COMMA labelArgument)*?
   ;

optionValue
   : identifier
   ;

optionValueList
   : optionValue (COMMA optionValue)*?
   ;

dimensions
   : INTEGER_LITERAL (COMMA INTEGER_LITERAL)*?
   ;

variableTypeDeclaration
   : DICTIONARY OF LEFTBRACKET dictionaryKey COMMA dictionaryDataType RIGHTBRACKET #DictionaryVariable
   | LIST OF LEFTBRACKET variableTypeDeclaration RIGHTBRACKET #ListVariable
   | ARRAY LEFTBRACKET dimensions RIGHTBRACKET OF variableTypeDeclaration #ArrayVariable
   | LABEL labelText (COMMA labelArgs)? #LabelVariable
   | RECORD objectId TEMPORARY? #RecordVariable
   | TEXT sizeDeclaration? #TextVariable
   | CODE sizeDeclaration #CodeVariable
   | TEXTCONST identifier EQUAL STRING_LITERAL #TextConstantVariable
   | INTEGER #IntegerVariable
   | DECIMAL #DecimalVariable
   | BOOLEAN #BooleanVariable
   | CHAR #CharVariable
   | DATE #DateVariable
   | TIME #TimeVariable
   | DATETIME #DatetimeVariable
   | DURATION #DurationVariable
   | GUID #GuidVariable
   | OPTION #OptionVariable
   | INSTREAM #InStreamVariable
   | OUTSTREAM #OutStreamVariable
   | FILE #FileVariable
   | LABEL #LabelVariable
   | VARIANT #VariantVariable
   | BIGINTEGER #BigIntegerVariable
   | BYTE #ByteVariable
   | RECORD #RecordVariable
   | RECORDREF #RecordRefVariable
   | FIELDREF #FieldRefVariable
   | KEYREF #KeyRefVariable
   | RECORDID #RecordIdVariable
   | HTTPCLIENT #HttpClientVariable
   | HTTPHEADERS #HttpHeadersVariable
   | HTTPREQUESTMESSAGE #HttpRequestMessageVariable
   | HTTPRESPONSEMESSAGE #HttpResponseMessageVariable
   | HTTPCONTENT #HttpContentVariable
   | JSONOBJECT #JsonObjectVariable
   | JSONARRAY #JsonArrayVariable
   | JSONVALUE #JsonValueVariable
   | JSONTOKEN #JsonTokenVariable
   | XMLDOCUMENT #XmlDocumentVariable
   | XMLELEMENT #XmlElementVariable
   | XMLATTRIBUTE #XmlAttributeVariable
   | XMLNODE #XmlNodeVariable
   | TEXTBUILDER #TextBuilderVariable
   | FILTERPAGEBUILDER #FilterPageBuilderVariable
   | NOTIFICATION #NotificationVariable
   | SESSIONSETTINGS #SessionSettingsVariable
   | CODEUNIT objectId #CodeunitVariable
   | PAGE objectId #PageVariable
   | DOTNET objectId #DotNetVariable
   | ENUM objectId #EnumVariable
   | QUERY objectId #QueryVariable
   | REPORT objectId #ReportVariable
   | XMLPORT objectId #XmlPortVariable
   ;

parameterName
   : identifier
   ;

parameterDeclaration
   : VAR? parameterName COLON variableTypeDeclaration
   ;

parameterList
   : parameterDeclaration (SEMICOLON parameterDeclaration)*
   ;

variableName
   : identifier;

variableNameList
	: variableName (COMMA variableName)*
   ;

variableDeclaration
   : variableNameList COLON variableTypeDeclaration SEMICOLON
   ;

variableDeclarations
   : variableDeclaration*
   ;

varBlock
   : VAR variableDeclarations
   ;

protectedVarBlock
   : PROTECTED VAR variableDeclaration
   ;

returnValue
   : identifier? returnType
   ;

returnType
   : COLON variableTypeDeclaration
   ;

/*
 * Trigger declarations
 */

triggerName : identifier;

triggerDeclaration
   : TRIGGER triggerName LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

/*
 * Code declaration elements
 */

codeEntity
    : varBlock
    | protectedVarBlock
    | triggerDeclaration
    | methodDeclaration
    ;

codeDeclarations
    : codeEntity+
    ;

/*
 * AL IF statement logic
 */

ifCondition
   : IF expression THEN;

ifElse
   : ELSE statement?;

ifStatement
   : ifCondition statement (ifElse)?;

/*
 * AL WHILE statement logic
 */

whileConditional
   : WHILE expression DO;

whileStatement
   : whileConditional statement;

/*
 * AL FOR statement logic
 */

forValue
   : expression
   ;

forControl
   : FOR identifier ASSGN expression (TO | DOWNTO) expression DO;

forStatement
   : forControl statement;

/*
 * AL FOREACH statement logic
 */

forEachControl:
	FOREACH identifier IN expression DO;

forEachStatement: forEachControl statement;

/*
 * AL CASE statement logic
 */

caseValue
   : expression
   ;

caseSet
   : caseValue (COMMA caseValue)*?;

caseRange
   : caseValue RANGE caseValue;

caseValueCondition
   : (caseSet | caseRange) COLON statement?;

caseElse
   : ELSE statement? SEMICOLON?;

caseBody
   : (caseValueCondition (SEMICOLON caseValueCondition?)*?)? caseElse?;

caseControl
   : CASE expression OF;

caseStatement
   : caseControl caseBody END;

/*
 * AL CONTINUE statement logic
 */

continueStatement
   : CONTINUE;

/*
 * AL BREAK statement logic
 */

breakStatement: BREAK;

/*
 * AL REPEAT UNTIL statement logic
 */

untilCondition
   : UNTIL expression;

repeatUntilStatement
   : REPEAT statementList SEMICOLON? untilCondition;

/*
 * AL WITH statement logic
 * (DEPRECATED in Dynamics 365 Business Central 2020, release wave 2)
 */

withControl
   : WITH identifier DO;

withStatement
   : withControl statement;

/*
 *AL Exit statement logic
 */

 exitStatement
   : EXIT (LEFTPAREN expression RIGHTPAREN)?;

/*
 * AL generic statement statement logic
 */

statementLine
   : ifStatement
   | forStatement
   | forEachStatement
   | caseStatement
   | withStatement
   | whileStatement
   | repeatUntilStatement
   | exitStatement
   | continueStatement
   | breakStatement
   | expression;

statementBlock
   : BEGIN statementList END;

statement
   : (statementLine | statementBlock)
   ;

statementList
   : (statementLine (SEMICOLON statementLine?)*?)?;

/*
 * AL expression logic
 */

indexAccessorValue
   : INTEGER_LITERAL
   | identifier
   ;

indexAccessorSet
   : indexAccessorValue (COMMA indexAccessorValue)*;

valueSet
   : expression (COMMA expression)*;

expression
   : LEFTPAREN expression RIGHTPAREN #ParenthesisExpression
   | booleanLiteral #BooleanLiteralExpression
   | DATE_LITERAL #DateLiteralExpression
   | TIME_LITERAL #TimeLiteralExpression
   | DATETIME_LITERAL #DatetimeLiteralExpression
   | STRING_LITERAL #StringLiteralExpression
   | FLOAT_LITERAL #FloatLiteralExpression
   | INTEGER_LITERAL	#IntegerLiteralExpression
   | identifier #IdentifierExpression
   | expression PERIOD methodName LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | methodName LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | expression SCOPE identifier # ScopeExpression
   | expression PERIOD identifier #MemberAccessExpression
   | expression LEFTBRACKET indexAccessorSet RIGHTBRACKET #IndexExpression
   | LEFTBRACKET valueSet? RIGHTBRACKET #SetExpression
   | NOT expression #NotExpression
   | MINUS expression #NegativeExpression
   | expression ASTERISK expression	#MultiplyExpression
   | expression BACKSLASH expression #DivideExpression
   | expression DIV expression #IntegerDivideExpression
   | expression MOD expression #ModulusExpression
   | expression PLUS expression #AddExpression
   | expression MINUS expression #SubtractExpression
   | expression (LESSTHAN | GREATERTHAN | LESSTHANEQUAL | GREATERTHANEQUAL | NOTEQUAL | EQUAL) expression #ComparisonExpression
   | expression (AND | OR | XOR) expression #LogicalComparisonExpression
   | expression CONDITION expression COLON expression #TernaryExpression
   | expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) expression #AssignmentExpression
   | expression IN LEFTBRACKET valueSet? RIGHTBRACKET #InRangeExpression
   | GUIALLOWED #GuiAllowedFunctionExpression
   ;

methodCallArguments
   : expression (COMMA expression)*
   ;

/*
* Codeunit structure
*/

namespaceDeclaration
   : {TokenMatches("namespace")}? identifier namespaceIdentifier SEMICOLON;

usingDeclaration
   : {TokenMatches("using")}? identifier namespaceIdentifier SEMICOLON;

usingDeclarations
   : usingDeclaration+;

codeunitProperty
   : {TokenMatches("Obsolete")}? identifier EQUAL STRING_LITERAL SEMICOLON
   | {TokenMatches("ObsoleteState")}? identifier EQUAL {TokenMatches(ObsoleteStateValues)}? identifier SEMICOLON
   | {TokenMatches("ObsoleteTag")}? identifier EQUAL STRING_LITERAL SEMICOLON
   | permissionsProperty
   | keyValueProperty
   | keyIdentifierListProperty
   ;

codeunitProperties
   : codeunitProperty*;

codeunitDeclaration
   : namespaceDeclaration? usingDeclarations? CODEUNIT objectId? objectName LEFTCBRACE codeunitProperties codeDeclarations RIGHTCBRACE;