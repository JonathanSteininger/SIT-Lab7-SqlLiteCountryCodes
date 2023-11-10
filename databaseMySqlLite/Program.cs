using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace databaseMySqlLite
{
    internal class Program
    {
        const string CONNECTION_STRING = @"Data Source=./world.db;Version=3;";
        static void Main(string[] args)
        {
            Console.WriteLine("? = print codes\n# = quit\n");
            string line;
            try
            {
                while ((line = Console.ReadLine()) != "#")
                {
                    ProcessLine(line);
                }
            }catch (Exception e)
            {
                Console.WriteLine($"Error occured. try entering a country code, a single 3 letter word. exeption: {e.ToString()}");
            }
        }

        //method to process the users iniputs
        static void ProcessLine(string line)
        {
            line = line.Trim().ToUpper();
            if(line.Length <= 0 )
            {
                return;
            }
            if (line[0] == '?')
            {
                PrintCodes();
            }
            ProcessThing($"Select Name, Population from city Where CountryCode = '{line.ToUpper().Trim(' ')}'");
        }

        //prints every country code into the console
        private static void PrintCodes()
        {
            string executionCommand = "Select CountryCode from city";
            try
            {
                HashSet<string> codes = new HashSet<string>();
                using (SQLiteConnection conn = new SQLiteConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(executionCommand, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) codes.Add(reader.GetString(0));
                        reader.Close();
                        conn.Close();
                    }
                }
                foreach (string code in codes) Console.WriteLine(code);
                Console.WriteLine("Total Codes: {0}\n\n", codes.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //Processes the command;
        static void ProcessThing(string executionCommand)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                using (SQLiteConnection conn = new SQLiteConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(executionCommand, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {

                        while (reader.Read())
                        {
                            builder.Clear();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if(i != 0) builder.Append("\t");
                                builder.Append(reader.GetValue(i).ToString());
                            }
                            Console.WriteLine(builder.ToString());
                        }
                        reader.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }


}
