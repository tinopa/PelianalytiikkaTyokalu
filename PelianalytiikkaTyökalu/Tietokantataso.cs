using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PelianalytiikkaTyökalu
{
    class Tietokantataso
    {
        /*
         * showGames(string developer)
         * playTimeTotal(string game)
         * playTimeAverage(string game)
         * sessionAverageTime(string game)
         * playerCount(string Game)
         * showGameSessions(string player, string game)
         * showGameEvents(int gameSessionID)
         * highestSpendingPlayer(string game)
         * showTransactions(string player)
         * showHighestTransaction(string Game)
         * 
         * ehkä
         * activePlayers(string game)
         * retentionRate(string Game, ??)
         */
        public string connetionString = null;
        public MySqlConnection cnn;
        

        public Tietokantataso(string server, string database, string uid, string pwd)
        {
            //tietokantaan yhdistämiseen tarvittavat tiedot
            connetionString = "server=" + server + ";database="+ database + ";uid=" + uid + ";pwd=" + pwd + ";";
            cnn = new MySqlConnection(connetionString);
        }


        public bool OpenConnection()
        {
            try
            {
                cnn.Open();
                Console.WriteLine("Connection Open ! ");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! ");
                return false;
            }
        }


        public MySqlDataReader DatabaseQuery(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, cnn);
            MySqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }


        public void ShowAllGamesByDeveloper(string nimi)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT Peli.Nimi FROM  Peli, Pelistudio WHERE  Peli.Studio_ID = Pelistudio.Studio_ID AND pelistudio.Nimi = " + nimi + ";");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string gameName = reader.GetString(reader.GetOrdinal("Nimi"));
                    Console.WriteLine(gameName);
                }
            }
        }
    }
}
