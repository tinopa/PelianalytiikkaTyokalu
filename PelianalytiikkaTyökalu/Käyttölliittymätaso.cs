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

            Console.WriteLine("Address of the database: ");
            server = Console.ReadLine();
            Console.WriteLine("Name of the database: ");
            databaseName = Console.ReadLine();
            Console.WriteLine("Username: ");
            uid = Console.ReadLine();
            Console.WriteLine("Password: ");
            pwd = Console.ReadLine();


            Tietokantataso database= new Tietokantataso(server, databaseName, uid, pwd);
            connectionStatus = database.OpenConnection();


            while (connectionStatus)
            {
                Console.WriteLine("What are we going to do?\n" +
                    "1.  Show all games by a developer\n" +
                    "2. \n"); 
                //ja jatkuu niin paljon kun halutaan eri kyselyitä

                int valinta = Convert.ToInt32(Console.ReadLine());

                switch (valinta)
                {
                    default:
                        Console.WriteLine("error");
                        break;
                    case 1:
                        Console.WriteLine("Give the name of the developer: ");
                        string nimi = Console.ReadLine();
                        database.ShowAllGamesByDeveloper(nimi);
                        break;
                }
            }

        }
    }
}
