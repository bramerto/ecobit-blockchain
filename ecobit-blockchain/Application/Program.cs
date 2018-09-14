using System.IO;
using Application.Controllers;
using Application.Util;

namespace Application
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Main(args, DependencyFactory.Resolve<ILogger>());
        }

        public static void Main(string[] args, ILogger logger)
        {
            logger.Log("Welcome to the Ecobit Blockchain Connector");
            
            if (args.Length > 0 && File.Exists(args[0]))
            {
                var controller = new TransactionController();
                controller.SaveTransactionXml(File.ReadAllText(args[0]));

                return;
            } 

            logger.Log("No existing file specified");
        }
    }
}    