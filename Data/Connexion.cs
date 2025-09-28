using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LearnPlay.Data
{
    internal class Connexion
    {
        private SqlConnection _con;
            public string GetConnectionString()
            {
                string connectionString;

                // Définition de l'environnement (défini au début de program.cs)

                // recupere l environment **
                // environment de TESTS doit exister 
                string env = Environment.GetEnvironmentVariable("TESTS");
                if (!string.IsNullOrEmpty(env))
                {
                    Console.Write("s{env}");
                    connectionString = env;
                    return connectionString;
                }


                //configuration de la connexion a la base de donnee
                var builder = new ConfigurationBuilder();
                var configRoot = builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables()
                    .Build();

                connectionString = configRoot.GetConnectionString("DefaultConnection");
                //Console.WriteLine("Utilisation de DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Chaîne de connexion non trouvée dans appsettings.json");
                }
                return connectionString;
            }


            public SqlConnection OpenConnection()
            {
                _con = new SqlConnection(GetConnectionString());
                _con.Open();
                return _con;
            }
        }
    }
}
    }
}
