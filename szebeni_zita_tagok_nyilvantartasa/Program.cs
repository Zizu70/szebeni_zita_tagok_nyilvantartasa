using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szebeni_zita_tagok_nyilvantartasa
{
    internal class Program
    {
        static List<Tagok> tagokList = new List<Tagok>();
        static MySqlConnection connection = null;
        static MySqlCommand command = null;
        static void Main(string[] args)
        {
            try
            {
                beolvasas();
                
                kiiratas();
                ujTagFelvetel();
               
                int frissAzon = 1013;
                string ujNev = "Völgyi Viola";
                int ujSzulev = 2001;
                int ujIrszam = 9999;
                string ujOrsz = "HR";
                tagFrissites(frissAzon, ujNev, ujSzulev, ujIrszam, ujOrsz);
                

                int toroltTag = 1015;              
                tagTorles(toroltTag);
                
                tagokList.Clear();
                beolvasas();
                kiiratas();
                
                Console.WriteLine("\n*** Program vége! ***");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }

            Console.ReadLine();

        }

        private static void beolvasas()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Clear();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "tagdij";
            sb.CharacterSet = "utf8";
            connection = new MySqlConnection(sb.ConnectionString);
            command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT * FROM `ugyfel`";
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Tagok ujtag = new Tagok(dr.GetInt32("azon"), dr.GetString("nev"), dr.GetInt32("szulev"), dr.GetInt32("irszam"), dr.GetString("orsz"));
                        tagokList.Add(ujtag);
                    }
                }
                connection.Close();
            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            Console.WriteLine("\n***** Beolvasás megtörtént *****\n");

        }

        private static void kiiratas()
        {
            foreach (Tagok tagok in tagokList)
            {
                Console.WriteLine(tagok); 
            }

            Console.WriteLine("\n***** Kiiratás megtörtént *****");
        }

        private static void ujTagFelvetel()
        {
            Tagok ujtag = new Tagok(1014, "Szent Gotthárd", 2000, 2025, "H");
            command.CommandText = "INSERT INTO `ugyfel` (`azon`,`nev`,`szulev`,`irszam`,`orsz`) VALUES (@azon, @nev, @szulev, @irszam, @orsz)";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", ujtag.azon);
            command.Parameters.AddWithValue("@nev", ujtag.nev);
            command.Parameters.AddWithValue("@szulev", ujtag.szulev);
            command.Parameters.AddWithValue("@irszam", ujtag.irszam);
            command.Parameters.AddWithValue("@orsz", ujtag.orsz);


            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
                connection.Close();

            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
          
            Console.WriteLine("\n***** Új tag felvétele megtörtént *****");
        }


        private static void tagFrissites(int azon, string ujNev, int ujSzulev, int ujIrszam, string ujOrsz)
        {
            command.CommandText = "UPDATE `ugyfel` SET `nev` = @ujNev, `szulev` = @ujSzulev, `irszam` = @ujIrszam, `orsz` = @ujOrsz WHERE `azon` = @azon";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", azon);
            command.Parameters.AddWithValue("@ujNev", ujNev);
            command.Parameters.AddWithValue("@ujSzulev", ujSzulev);
            command.Parameters.AddWithValue("@ujIrszam", ujIrszam);
            command.Parameters.AddWithValue("@ujOrsz", ujOrsz);

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
                Console.WriteLine("\n***** Tag frissítése sikeres *****");
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        private static void tagTorles(int azon)
        {
            command.CommandText = "DELETE FROM `ugyfel` WHERE `azon` = @azon";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", azon);

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();

                Console.WriteLine("\n***** Tag törlése sikeres *****");

                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
