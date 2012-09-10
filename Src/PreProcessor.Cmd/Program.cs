using System;
using Amido.SystemEx.IO;
using log4net;

namespace Amido.PreProcessor.Cmd
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static int EXIT_CODE_OK = 0;
        private static int EXIT_CODE_ERROR = 1;

        static int Main(string[] args)
        {

            log.Info("Processing started.");

            try
            {
                var fso = new FileSystem();
                var runner = new PreProcessRunner(fso, new PropertyManager(fso), new TokenisationRunner(fso, new Tokeniser()));
                runner.Run(args);
                log.Info("Processing complete.");

                return EXIT_CODE_OK;
                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(ex.StackTrace);
                return EXIT_CODE_ERROR;
            }
        }
    }
}