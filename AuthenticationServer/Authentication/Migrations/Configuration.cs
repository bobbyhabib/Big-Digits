using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Dijits.Authentication.Entities;
using Dijits.Authentication.Global.Enum;

namespace Dijits.Authentication.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<Dijits.Authentication.AuthContext.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AuthContext.AuthContext context)
        {
            if (context.Clients.Count() > 0)
            {
                return;
            }

            context.Clients.AddRange(BuildClientsList());
            context.SaveChanges();
        }

        private static IEnumerable<Entities.Client> BuildClientsList()
        {

            List<Client> ClientsList = new List<Client>
            {
                new Client
                { 
                    Id = "ngAuthApp",
                    Secret = Helper.Helper.GetHash("abc@123"), 
                    Name ="AngularJS front-end Application", 
                    ApplicationType =  ApplicationTypes.JavaScript, 
                    Active = true, 
                    RefreshTokenLifeTime = 7200, 
                    AllowedOrigin = "http://ngauthenticationweb.azurewebsites.net"
                },
                new Client
                { 
                    Id = "consoleApp", 
                    Secret = Helper.Helper.GetHash("123@abc"), 
                    Name ="Console Application", 
                    ApplicationType = ApplicationTypes.NativeConfidential, 
                    Active = true, 
                    RefreshTokenLifeTime = 14400, 
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }
    }
}
