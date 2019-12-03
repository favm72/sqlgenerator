using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ConsoleApp1
{
	class Program
	{		
		static void Main(string[] args)
		{
			//DBDiagramIO diagramIO = new DBDiagramIO();
			//diagramIO.GenerateScript();
			Oracle oracle = new Oracle();
			oracle.GenerateScript();
		}
	}
}
