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
            Console.WriteLine("Address of the database: ");
            server = Console.ReadLine();
            Console.WriteLine("Name of the database: ");
            databaseName = Console.ReadLine();
            Console.WriteLine("Username: ");
            uid = Console.ReadLine();
            Console.WriteLine("Password: ");
            pwd = Console.ReadLine();

            //tehdään tietokanta-olio ja yhdistetään siihen
            Tietokantataso database= new Tietokantataso(server, databaseName, uid, pwd);
            connectionStatus = database.OpenConnection();


            while (connectionStatus)
            {
                Console.WriteLine("What are we going to do?\n" +
                    "1.  Show all games by a developer\n" +
                    "2. \n"); 
                //ja jatkuu niin paljon kun halutaan eri kyselyitä

                int selection = Convert.ToInt32(Console.ReadLine());

                switch (selection)
                {
                    default:
                        Console.WriteLine("error");
                        break;
                    case 1:
                        Console.WriteLine("Give the name of the developer: ");
                        string developerName = Console.ReadLine();
                        database.ShowAllGamesByDeveloper(developerName);
                        break;
                }
            }
            database.CloseConnection();
        }
    }
}
