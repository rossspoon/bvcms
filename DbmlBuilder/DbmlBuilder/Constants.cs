using System;

namespace DbmlBuilder
{
    public class TemplateName
    {
        public const string CLASS = "EntityTemplate";

        public const string DYNAMIC_SCAFFOLD = "DynamicScaffold";
        public const string GENERATED_SCAFFOLD_CODE_BEHIND = "GeneratedScaffoldCodeBehind";
        public const string GENERATED_SCAFFOLD_MARKUP = "GeneratedScaffoldMarkup";
        public const string STORED_PROCEDURE = "SPTemplate";
        public const string TABLE_FUNCTION = "TableFunctionTemplate";
        public const string STRUCTS = "StructsTemplate";
        public const string CONTEXT = "ContextTemplate";
        public const string VIEW = "ViewTemplate";
        public const string WEBFORM1 = "WebForm_aspx";
        public const string WEBFORM2 = "WebForm_cs";
    }

    public class TemplateVariable
    {
        public const string ARGUMENT_LIST = "#ARGLIST#";
        public const string BIND_LIST = "#BINDLIST#";
        public const string CLASS_NAME = "#CLASSNAME#";
        public const string CLASS_NAME_COLLECTION = "#CLASSNAMECOLLECTION#";
        public const string COLUMNS_STRUCT = "#COLUMNSSTRUCT#";
        public const string CONTROL_PROPERTY = "#CONTROLPROPERTY#";
        public const string DROP_LIST = "#DROPLIST#";
        public const string EDITOR_ROWS = "#EDITORROWS#";
        public const string FK_VAR = "#FKVAR#";
        public const string FOREIGN_CLASS = "#FOREIGNCLASS#";
        public const string FOREIGN_PK = "#FOREIGNPK#";
        public const string FOREIGN_TABLE = "#FOREIGNTABLE#";
        public const string GETTER = "#GETTER#";
        public const string GRID_ROWS = "#GRIDROWS#";
        public const string INSERT = "#INSERT#";
        public const string JAVASCRIPT_BLOCK = "#JAVASCRIPTBLOCK#";
        public const string LANGUAGE = "#LANGUAGE#";
        public const string LANGUAGE_EXTENSION = "#LANGEXTENSION#";
        public const string MANY_METHODS = "#MANYMETHODS#";
        public const string MAP_TABLE = "#MAPTABLE#";
        public const string MASTER_PAGE = "#MASTERPAGE#";
        public const string METHOD_BODY = "#METHODBODY#";
        public const string METHOD_LIST = "#METHODLIST#";
        public const string METHOD_NAME = "#METHODNAME#";
        public const string METHOD_TYPE = "#METHODTYPE#";
        public const string NAMESPACE_USING = "#NAMESPACE_USING#";
        public const string PAGE_BIND_LIST = "#PAGEBINDLIST#";
        public const string PAGE_CLASS = "#PAGECLASS#";
        public const string PAGE_CODE = "#PAGECODE#";
        public const string PAGE_FILE = "#PAGEFILE#";
        public const string PARAMETERS = "#PARAMS#";
        public const string PK = "#PK#";
        public const string PK_PROP = "#PKPROP#";
        public const string PK_VAR = "#PKVAR#";
        public const string PROPERTY_LIST = "#PROPLIST#";
        public const string PROPERTY_NAME = "#PROPNAME#";
        public const string PROPERTY_TYPE = "#PROPTYPE#";
        public const string PROVIDER = "#PROVIDER#";
        public const string SET_LIST = "#SETLIST#";
        public const string SETTER = "#SETTER#";
        public const string STORED_PROCEDURE_NAME = "#SPNAME#";
        public const string STRUCT_ASSIGNMENTS = "#STRUCTASSIGNMENTS#";
        public const string STRUCT_LIST = "#STRUCTLIST#";
        public const string SUMMARY = "#SUMMARY#";
        public const string TABLE_NAME = "#TABLENAME#";
        public const string TABLE_SCHEMA = "#TABLESCHEMA#";
        public const string UPDATE = "#UPDATE#";
    }

    public class ReservedColumnName
    {
        public const string CREATED_BY = "CreatedBy";
        public const string CREATED_ON = "CreatedOn";
        public const string DELETED = "Deleted";
        public const string IS_ACTIVE = "IsActive";
        public const string IS_DELETED = "IsDeleted";
        public const string MODIFIED_BY = "ModifiedBy";
        public const string MODIFIED_ON = "ModifiedOn";
    }

    public class ConfigurationPropertyName
    {
        public const string APPEND_WITH = "appendWith";
        public const string ADDITIONAL_NAMESPACES = "additionalNamespaces";
        public const string CONNECTION_STRING_NAME = "connectionStringName";
        public const string DEFAULT_PROVIDER = "defaultProvider";
        public const string ENABLE_TRACE = "enableTrace";
        public const string EXCLUDE_PROCEDURE_LIST = "excludeProcedureList";
        public const string EXCLUDE_TABLE_LIST = "excludeTableList";
        public const string EXTRACT_CLASS_NAME_FROM_SP_NAME = "extractClassNameFromSPName";
        public const string FIX_DATABASE_OBJECT_CASING = "fixDatabaseObjectCasing";
        public const string FIX_PLURAL_CLASS_NAMES = "fixPluralClassNames";
        public const string GENERATE_LAZY_LOADS = "generateLazyLoads";
        public const string GENERATE_NULLABLE_PROPERTIES = "generateNullableProperties";
        public const string GENERATE_ODS_CONTROLLERS = "generateODSControllers";
        public const string GENERATE_RELATED_TABLES_AS_PROPERTIES = "generateRelatedTablesAsProperties";
        public const string GENERATED_NAMESPACE = "generatedNamespace";
        public const string INCLUDE_PROCEDURE_LIST = "includeProcedureList";
        public const string INCLUDE_TABLE_LIST = "includeTableList";
        public const string MANY_TO_MANY_SUFFIX = "manyToManySuffix";
        public const string PROVIDER_TO_USE = "provider";
        public const string REGEX_DICTIONARY_REPLACE = "regexDictionaryReplace";
        public const string REGEX_IGNORE_CASE = "regexIgnoreCase";
        public const string REGEX_MATCH_EXPRESSION = "regexMatchExpression";
        public const string REGEX_REPLACE_EXPRESSION = "regexReplaceExpression";
        public const string RELATED_TABLE_LOAD_PREFIX = "relatedTableLoadPrefix";
        public const string REMOVE_UNDERSCORES = "removeUnderscores";
        public const string SET_PROPERTY_DEFAULTS_FROM_DATABASE = "setPropertyDefaultsFromDatabase";
        public const string SP_STARTS_WITH = "spStartsWith";
        public const string STRIP_COLUMN_TEXT = "stripColumnText";
        public const string STRIP_PARAM_TEXT = "stripParamText";
        public const string STRIP_STORED_PROCEDURE_TEXT = "stripSPText";
        public const string STRIP_TABLE_TEXT = "stripTableText";
        public const string STRIP_VIEW_TEXT = "stripViewText";
        public const string TEMPLATE_DIRECTORY = "templateDirectory";
        public const string USE_EXTENDED_PROPERTIES = "useExtendedProperties"; //SQL 2000/2005 Only
        public const string USE_STORED_PROCEDURES = "useSPs";
    }

    public class ClassName
    {
        public const string STORED_PROCEDURES = "SPs";
        public const string TABLES = "Tables";
        public const string VIEWS = "Views";
    }

    public static class FileExtension
    {
        public const string ASPX = "aspx";
        public const string CS = "cs";
        public const string DOT_ASPX = ".aspx";
        public const string DOT_CS = ".cs";
    }

    public class SqlSchemaVariable
    {
        public const string COLUMN_DEFAULT = "DefaultSetting";
        public const string COLUMN_NAME = "ColumnName";
        public const string CONSTRAINT_TYPE = "constraintType";
        public const string DATA_TYPE = "DataType";
        public const string DEFAULT = "DEFAULT";
        public const string FOREIGN_KEY = "FOREIGN KEY";
        public const string IS_COMPUTED = "IsComputed";
        public const string IS_IDENTITY = "IsIdentity";
        public const string IS_NULLABLE = "IsNullable";
        public const string MAX_LENGTH = "MaxLength";
        public const string MODE = "mode";
        public const string MODE_INOUT = "INOUT";
        public const string NAME = "Name";
        public const string PARAMETER_PREFIX = "@";
        public const string PRIMARY_KEY = "PRIMARY KEY";
        public const string TABLE_NAME = "TableName";
        public const string SCHEMA_NAME = "Owner";
    }
}