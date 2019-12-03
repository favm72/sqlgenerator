using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
	public class DBDiagramIO
	{
		List<MySchm> data = new List<MySchm>();
		string query = "";
		string foreignKeys = "";

		void LoadData()
		{
			MyConn conn = new MyConn();
			conn.OnRead += Conn_OnRead;
			conn.ExecQuery(MySchm.querySchema);
		}		

		private void Conn_OnRead(object sender, System.Data.SqlClient.SqlDataReader reader)
		{
			MySchm structure = new MySchm();
			structure.column_name = reader["column_name"].ToString();
			structure.table_name = reader["table_name"].ToString();
			structure.type_name = reader["type_name"].ToString();
			structure.is_identity = (bool)reader["is_identity"];
			structure.is_nullable = (bool)reader["is_nullable"];
			structure.is_key = (bool)reader["is_key"];
			data.Add(structure);
		}

		public void GenerateScript()
		{
			LoadData();
			var tables = data.Select(x => x.table_name).Distinct().ToList();			
			MyConn conn;

			foreach (string table in tables)
			{
				query += "\nTable " + table + " {";
				var columns = data.Where(x => x.table_name == table).ToList();
				foreach (var column in columns)
				{
					query += "\n" + column.column_name;
					switch (column.type_name)
					{
						case "nvarchar": query += " varchar"; break;
						case "datetime2": query += " datetime"; break;
						default: query += " " + column.type_name; break;
					}
					if (column.is_key)
						query += " [pk]";
				}
				query += "\n}";

				conn = new MyConn();
				conn.OnRead += Conn_OnRead1;
				conn.ExecQuery(MySchm.getForeignKeys(table));
			}
			System.IO.File.WriteAllText("script.txt", query + foreignKeys);			
			Console.ReadLine();
		}

		private void Conn_OnRead1(object sender, System.Data.SqlClient.SqlDataReader reader)
		{
			foreignKeys += "\nRef: " + reader["FKTABLE_NAME"].ToString() + "." + reader["FKCOLUMN_NAME"].ToString() + " > " + reader["PKTABLE_NAME"].ToString() + "." + reader["PKCOLUMN_NAME"].ToString();
		}
	}
}
