using FileCopy_Move.Classes;
using System;
using System.Configuration;

namespace FileCopy_Move
{
    class Program
    {
        static void Main(string[] args)
        {
            FileActions FA = new FileActions();

            if (args != null)
            {
                if (args[0] == "DriveCheck")
                {
                    if (FA.checkForSpecificDrive())
                    {
                        FA.SetDestPathComp(true);
                    }
                }
            }
            else
            {
                FA.SetDestPathComp(false);
            }

            switch (ConfigurationManager.AppSettings["Usage"].ToLower())
            {
                case "move": Console.WriteLine(FA.RunMoveWorkflow()); break;
                case "copy": Console.WriteLine(FA.RunCopyWorkflow()); break;
                default: Console.WriteLine("Function not implemented. Please change \"Usage\"."); break;
            }
        }
    }
}
