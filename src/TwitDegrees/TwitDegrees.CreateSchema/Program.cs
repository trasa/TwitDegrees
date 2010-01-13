using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackfin.Core.NHibernate;
using TwitDegrees.Core.Services;

namespace TwitDegrees.CreateSchema
{
    class Program
    {
        static void Main(string[] args)
        {
            service = new TwitterNHibernateConfigurationService();
            NHibernateSession.Init(new SimpleSessionStorage(), service);
            BuildSchema();
        }

        private static TwitterNHibernateConfigurationService service;

        private static void BuildSchema()
        {
            if (!VerifyToContinue())
            {
                return;
            }

            Console.WriteLine("Ok, here we go.");
            service.CreateSchema(true);
            Console.WriteLine("It is finished.");
        }

        private static bool VerifyToContinue()
        {
            Console.WriteLine();
            Console.Write("This is going to write schema information to the database - are you sure you want to continue? [Y|N]");
            string s = Console.ReadLine();
            if (s == null || s.Trim().ToUpperInvariant() != "Y")
            {
                Console.WriteLine("Fine, nevermind then.");
                return false;
            }
            return true;
        }

    }
}
