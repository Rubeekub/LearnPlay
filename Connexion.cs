using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay
{
    internal class Connexion
    {
        private static string? _connectionString;
        public Connexion()
        {
            // Priorité : variable d'environnement - On va vérifier s'il existe un environnement de test nommé TACHES_DB - Si oui, on prend la valeur stockée de cet environnement comme chaîne de caractères.
            var envCs = Environment.GetEnvironmentVariable("TACHES_DB");
            if (!string.IsNullOrEmpty(envCs))
            {
                _connectionString = envCs;
            }
            else
            {
                // sinon configuration depuis appsettings.json
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                _connectionString = config.GetConnectionString("LearnPlayDb");
                //Console.WriteLine("Utilisation de DefaultConnection");
            }
        }

        public SqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("La chaîne de connexion n'a pas été définie.");

            return new SqlConnection(_connectionString);
        }

    }
}