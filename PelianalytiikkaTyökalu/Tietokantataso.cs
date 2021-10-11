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

         * playTimeAverage(string game)
         * sessionAverageTime(string game)
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
            connetionString = "server=" + server + ";database="+ database + ";uid=" + uid + ";pwd=" + pwd + ";SSL Mode=None";
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


        public void CloseConnection()
        {
            cnn.Close();
        }


        public MySqlDataReader DatabaseQuery(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, cnn);
            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void ShowAllGamesByDeveloper(string gameName)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT Peli.Nimi FROM  Peli, Pelistudio " +
                "WHERE  Peli.Studio_ID = Pelistudio.Studio_ID " +
                "AND pelistudio.Nimi = \"" + gameName + "\";");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Games by " + gameName + ":");
                while (reader.Read())
                {
                    string result = reader.GetString(reader.GetOrdinal("Nimi"));
                    Console.WriteLine(result);
                }
                reader.Close();
            }
        }

        public void PlayerCount(string gameName)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT SUM(Pelaaja_ID) FROM Pelaaja " +
                "WHERE Pelaaja_ID IN(" +
                "SELECT Pelaaja_ID FROM Pelaa, Peli " +
                "WHERE Pelaa.Peli_ID = Peli.Peli_ID AND Peli.Nimi = \"" + gameName + "\");");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Playercount for " + gameName + ":");
                while (reader.Read())
                {
                    int result = reader.GetInt32(reader.GetOrdinal("SUM(Pelaaja_ID)"));
                    Console.WriteLine(result);
                }
                reader.Close();
            }
        }

        public void PlayTimeTotal(string gameName)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT SUM(TIMESTAMPDIFF(HOUR, Aloitusaika, Loppuaika)) " +
                "AS Peliaika FROM Pelisessio, Peli " +
                "WHERE Pelisessio.Peli_ID = Peli.Peli_ID AND Peli.Nimi = \"" + gameName + "\";");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Total played hours for " + gameName + ":");
                while (reader.Read())
                {
                    int result = reader.GetInt32(reader.GetOrdinal("Peliaika"));
                    Console.WriteLine(result);
                }
                reader.Close();
            }
        }

        public void PlayTimeAverage(string gameName)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT AVG(TIMESTAMPDIFF(HOUR, Aloitusaika, Loppuaika)) " +
                "AS Peliaika FROM Pelisessio, Peli " +
                "WHERE Pelisessio.Peli_ID = Peli.Peli_ID AND Peli.Nimi = \"" + gameName + "\";");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Average played hours for " + gameName + ":");
                while (reader.Read())
                {
                    float result = reader.GetFloat(reader.GetOrdinal("Peliaika"));
                    Console.WriteLine(result);
                }
                reader.Close();
            }
        }

        public void ShowGameSessions(string playerNick, string gameName)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT DATE(Aloitusaika) AS paivamaara, TIMESTAMPDIFF(HOUR, Aloitusaika, Loppuaika) as Peliaika, Sessio_ID " +
                "FROM Pelisessio, Pelaaja, Peli, Pelaa " +
                "WHERE Pelisessio.Peli_ID = Peli.Peli_ID " + 
                "AND Peli.Nimi = \"" + gameName + "\" " +
                "AND Pelisessio.Pelaaja_ID = Pelaaja.Pelaaja_ID " +
                "AND Pelaaja.Pelaaja_ID = Pelaa.Pelaaja_ID " +
                "AND Pelaa.Nimimerkki = \"" + playerNick + "\"; ");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Date, played hours and session ID's for " + playerNick + " in " + gameName + ":");
                while (reader.Read())
                {
                    string date = reader.GetString(reader.GetOrdinal("paivamaara"));
                    int gameTime = reader.GetInt32(reader.GetOrdinal("Peliaika"));
                    int id = reader.GetInt32(reader.GetOrdinal("Sessio_ID"));
                    Console.WriteLine(date + " " + gameTime + " " + id);
                }
                reader.Close();
            }
        }

        public void ShowGameEvents(int sessionID)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT Aikaleima, Tyyppi_Nimi FROM Pelitapahtuma, Pelitapahtuma_Tyyppi " +
                "WHERE Pelitapahtuma.Tyyppi_ID = Pelitapahtuma_Tyyppi.Tyyppi_ID " +
                "AND Sessio_ID =\"" + sessionID + "\" ;");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Gameevents in session " + sessionID + ":");
                while (reader.Read())
                {
                    string timestamp = reader.GetString(reader.GetOrdinal("Aikaleima"));
                    string gameEvent = reader.GetString(reader.GetOrdinal("Tyyppi_Nimi"));
                    Console.WriteLine(timestamp + " " + gameEvent);
                }
                reader.Close();
            }
        }

        public void GameSession(string gameSessions)
        {
            MySqlDataReader reader = DatabaseQuery("SELECT Aloitusaika, Loppuaika FROM pelisessio " +
                "WHERE Peli_ID = ANY");

            if (reader != null && reader.HasRows)
            {
                Console.WriteLine("Latest game sessions " + ":");
                while (reader.Read())
                {
                    String results = reader.GetString(reader.GetOrdinal("Aloitusaika, Loppuaika"));
                    Console.WriteLine(results);
                }
            }
        }

    }
}
