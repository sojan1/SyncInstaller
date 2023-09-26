using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncInstaller
{
    internal class TableInfo //: ITableInfo
    {
        public string ConnString { get; private set; }

        public TableInfo(string connString)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException(nameof(connString));
            ConnString = connString;
        }

        public DataTable GetTables()
        {
            DataTable data = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(GetTablesQuery(), conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        data.Load(reader);
                    }
                }
                return data;
            }
        }
        public IEnumerable<string> GetTableNames()
        {
            List<string> tables = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(GetTablesQuery(), conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["TableName"].ToString());
                        }
                    }
                }
                return tables;
            }
        }
        private string GetTablesQuery()
        {
            return @"
                IF EXISTS(SELECT 1 FROM sys.tables where name = '_clnt') --AND EXISTS (SELECT 1 from _clnt)
                BEGIN
                    IF EXISTS(SELECT 1 FROM _clnt)
                            SELECT [TableID] AS 'TableID'
                                ,[ParentID] AS 'ParentID'
                                ,[TableName] AS 'TableName'
                                ,[Description] AS 'Description' 
                            FROM [_tableData] td
                                INNER JOIN [SYS].[TABLES] st 
                                    ON td.[TableName] = st.[name]
                        UNION
                            SELECT st.[object_id] AS 'TableID'
                                ,-1 AS 'ParentID'
                                ,lut.[TableName] AS 'TableName'
                                ,lut.[Description] AS 'Description' 
                            FROM [_lut_tbls] lut 
                                INNER JOIN [SYS].[TABLES] st 
                                    ON lut.[TableName] = st.[name]
                        UNION
                            SELECT [object_id] AS 'TableID'
                                ,-1 AS 'ParentID'
                                ,[Name] AS 'TableName'
                                , ''  AS 'Description' 
                            FROM [SYS].[TABLES] 
                            WHERE [Name] LIKE '[_]%' 
                                AND [Name] <> '_db'
                                AND [Name] NOT LIKE '%[_]tracking'
                        UNION
                            SELECT st.[object_id] AS 'TableID'
                                ,-1 AS 'ParentID'
                                ,g.[TableName] AS 'TableName'
                                ,g.[Description] AS 'Description' 
                            FROM [_gph] g 
                                INNER JOIN [SYS].[TABLES] st 
                                    ON g.[TableName]=st.[name]
                                        ORDER BY TableName,TableID
                    ELSE
                        SELECT [object_id] AS 'TableID'
                                ,-1 AS 'ParentID'
                                ,[name] AS 'TableName'
                                ,'' AS 'Description' 
                        FROM [sys].[tables] 
                        WHERE [name] NOT LIKE '%[_]tracking'
                            AND [Name] <> '_db'
                            AND [name] not in ('dtproperties', 'sysdiagrams', 'schema_info', 'scope_config', 'scope_info', 'scope_parameters', 'scope_templates')
                END
                ELSE
                    SELECT [object_id] AS 'TableID'
                            ,-1 AS 'ParentID'
                            ,[name] AS 'TableName'
                            ,'' AS 'Description' 
                    FROM [sys].[tables] 
                    WHERE [name] NOT LIKE '%[_]tracking'
                        AND [Name] <> '_db'
                        AND [name] not in ('dtproperties', 'sysdiagrams', 'schema_info', 'scope_config', 'scope_info', 'scope_parameters', 'scope_templates')
                ";
        }


        //public DbSyncTableDescription GetDescription(string tableName)
        //{
        //    return GetDescription(tableName, null);
        //}

 

        public Dictionary<string, List<string>> GetTablesAndColumns(IEnumerable<string> tableNames, out string problem)
        {
            Dictionary<string, List<string>> tablesAndColumns = GetTablesAndColumns(tableNames);


            //List<DbSyncTableDescription> result = new List<DbSyncTableDescription>();
            //foreach (KeyValuePair<string, List<string>> table in tablesAndColumns)
            //{
            //    DbSyncTableDescription desc = GetDescriptionForTable(table.Key, table.Value, out problem);
            //    if (problem != null)
            //    {
            //        return new List<DbSyncTableDescription>(0);
            //    }
            //    result.Add(desc);
            //}
            problem = null;
            return tablesAndColumns;
        }



        //public IEnumerable<DbSyncTableDescription> GetDescriptions(IEnumerable<string> tableNames, out string problem)
        //{
        //    Dictionary<string, List<string>> tablesAndColumns = GetTablesAndColumns(tableNames);
        //    List<DbSyncTableDescription> result = new List<DbSyncTableDescription>();
        //    foreach (KeyValuePair<string, List<string>> table in tablesAndColumns)
        //    {
        //        DbSyncTableDescription desc = GetDescriptionForTable(table.Key, table.Value, out problem);
        //        if (problem != null)
        //        {
        //            return new List<DbSyncTableDescription>(0);
        //        }
        //        result.Add(desc);
        //    }
        //    problem = null;
        //    return result;
        //}



        /// <summary>
        /// Returns the table description that only contains columns from the list provided (may contain fewer if they were not valid for Sync use)
        /// Returns the table description wilth all columns if no column filter
        /// No check is made on the tableName however.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        //public DbSyncTableDescription GetDescription(string tableName, IEnumerable<string> columnNames)
        //{
        //    KeyValuePair<string, List<string>> tablesAndColumns = GetTablesAndColumns(new string[] { tableName }).First();
        //    IEnumerable<string> columns = columnNames == null || columnNames.Count() == 0 ? tablesAndColumns.Value
        //        : columnNames.Intersect(tablesAndColumns.Value);
        //    return GetDescriptionForTable(tablesAndColumns.Key, columns.ToList(), out string problem);
        //}

        //private DbSyncTableDescription GetDescriptionForTable(string tableName, List<string> columnNames, out string problem)
        //{
        //    problem = null;
        //    if (string.IsNullOrEmpty(tableName))
        //    {
        //        problem = string.Format("No table data exists for '{0}'.", tableName);
        //    }
        //    if (columnNames == null || columnNames.Count == 0)
        //    {
        //        problem = string.Format("No column data exists for '{0}'.", tableName);
        //    }
        //    if (problem != null)
        //    {
        //        return new DbSyncTableDescription(tableName);
        //    }
        //    using (SqlConnection conn = new SqlConnection(ConnString))
        //    {
        //        conn.Open();
        //        DbSyncTableDescription tableDesc = SqlSyncDescriptionBuilder.GetDescriptionForTable(tableName, new Collection<string>(columnNames), conn);

        //        if (columnNames.Contains("GEO_ID"))
        //        {
        //            foreach (var column in tableDesc.Columns)
        //            {
        //                column.IsPrimaryKey = column.UnquotedName == "GEO_ID";
        //            }
        //        }
        //        return tableDesc;
        //    }
        //}


        private Dictionary<string, List<string>> GetTablesAndColumns(IEnumerable<string> tables)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            if (tables == null || tables.Count() == 0)
            {
                return result;
            }
            StringBuilder script = new StringBuilder();
            //script.Append(@"
            //    SELECT 
            //        OBJECT_NAME([OBJECT_ID]) AS 'TableName', 
            //        [NAME] AS 'ColumnName'
            //    FROM [SYS].[COLUMNS]
            //    WHERE [NAME] NOT IN ('rts','Commands')
            //        AND TYPE_NAME(user_type_id) NOT IN ('geography','geometry')
            //        AND [is_computed] = 0
            //        AND [is_identity] = 0

            //    ");

            script.Append(@"
                SELECT 
                    OBJECT_NAME([OBJECT_ID]) AS 'TableName', 
                    [NAME] AS 'ColumnName'
                FROM [SYS].[COLUMNS]
                WHERE [NAME] NOT IN ('rts','Commands')
                    AND TYPE_NAME(user_type_id) NOT IN ('geography','geometry')
                    AND system_type_id NOT IN(241)
                    AND [is_computed] = 0
                    AND [is_identity] = 0

                ");
            ;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(script.ToString(), conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader["TableName"].ToString();
                            if (tables.Contains(tableName))
                            {
                                if (!result.ContainsKey(tableName))
                                {
                                    result.Add(tableName, new List<string>());
                                }
                                result[tableName].Add(reader["ColumnName"].ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
