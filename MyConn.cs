using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApp1
{
	public class MyConn
	{
		string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=omni;Integrated Security=True";

		public event EventHandler<SqlDataReader> OnRead;

		public void ExecQuery(string query)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand())
				{
					command.Connection = connection;
					command.CommandText = query;
					connection.Open();
					command.CommandType = System.Data.CommandType.Text;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.HasRows)
						{							
							while (reader.Read())
							{
								OnRead.Invoke(this, reader);
							}
						}
					}
				}
			}
		}
	}
}