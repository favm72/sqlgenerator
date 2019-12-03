using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
	public class MySchm
	{
		public string table_name { get; set; }
		public string column_name { get; set; }
		public string type_name { get; set; }
		public bool is_identity { get; set; }
		public bool is_nullable { get; set; }
		public bool is_key { get; set; }

		public const string querySchema = @"
					select 
					t.object_id, 
					t.name as table_name, 
					c.name as column_name, 
					y.name as type_name, 
					c.is_identity, 
					c.is_nullable,
					cast(case x.is_key when 1 then 1 else 0 end as bit) as is_key
					from sys.tables t
					inner join sys.columns c on c.object_id = t.object_id
					inner join sys.types y on y.user_type_id = c.user_type_id
					left join (
						SELECT table_name, column_name, 1 as is_key
						FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
						WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
						AND TABLE_SCHEMA = 'dbo'
					) x on x.table_name = t.name and x.column_name = c.name
		";

		public static string getForeignKeys(string table_name) {
			return "EXEC sp_fkeys '" + table_name + "'";
		} 
	}
}
