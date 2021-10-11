using System;

namespace PelianalytiikkaTyökalu
{
    class Käyttölliittymätaso
    {
        //tämä on se varsinainen ohjelma
        //täällä kysytään käyttäjältä mitä se haluaa tehdä ja toimitaan sen mukaan
        //varmaan vaan joku iso switch case häsmäkkä
        static void Main(string[] args)
        {
            string server;
            string databaseName;
            string uid;
            string pwd;
            bool connectionStatus;


            //Kerätään käyttäjältä tarvittavat tiedot tietokantaan yhdistämistä varten
            Console.Write("Address of the database: ");
            server = Console.ReadLine();
            Console.Write("Name of the database: ");
            databaseName = Console.ReadLine();
            Console.Write("Username: ");
            uid = Console.ReadLine();
            Console.Write("Password: ");
            pwd = Console.ReadLine();
            Console.WriteLine();

            //tehdään tietokanta-olio ja yhdistetään siihen
            Tietokantataso database= new Tietokantataso(server, databaseName, uid, pwd);
            connectionStatus = database.OpenConnection();


            while (connectionStatus)
            {
                Console.WriteLine("\nWhat are we going to do?\n" +
                    "1. Close program\n" +
                    "2. Show all games by a developer\n" +
                    "3. Playercount for a game\n" +
                    "4. Show latest game sessions");
                //ja jatkuu niin paljon kun halutaan eri kyselyitä

                int selection = Convert.ToInt32(Console.ReadLine());

                switch (selection)
                {
                    default:
                        Console.WriteLine("\nerror");
                        break;
                    case 1:
                        return;
                    case 2:
                        Console.Write("\nGive the name of the developer: ");
                        string developerName = Console.ReadLine();
                        database.ShowAllGamesByDeveloper(developerName);
                        break;
                    case 3:
                        Console.Write("\nGive the name of the game: ");
                        string gameName = Console.ReadLine();
                        database.PlayerCount(gameName);
                        break;
                    case 4:
                        Console.Write("\nSee the latest game sessions by ID (1-5): ");
                        string gameSessions = Console.ReadLine();
                        database.PlayerCount(gameSessions);
                        break;

                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
            }
            database.CloseConnection();
        }
    }
}
