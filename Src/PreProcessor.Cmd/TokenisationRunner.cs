using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Amido.SystemEx.Contracts;
using Amido.SystemEx.IO;
using log4net;

namespace Amido.PreProcessor.Cmd
{
    public class TokenisationRunner : ITokenisationRunner
    {
        private static readonly ILog log = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetType());
        private const string TemplateFileExtension = ".template";
        private IFileSystem fileSystem;
        private ITokeniser tokeniser;
        
        public TokenisationRunner(IFileSystem fileSystem, ITokeniser tokeniser)
        {
            Args.NotNull(fileSystem, "fileSystem");
            Args.NotNull(tokeniser, "tokeniser");

            this.fileSystem = fileSystem;
            this.tokeniser = tokeniser;
        }

        public void ProcessSingleTemplate(IDictionary<string, string> properties, string sourceFile, string destinationFile)
        {
            Args.NotNull(properties, "properties");
            Args.NotNullOrEmpty(sourceFile, "sourceFile");

            try
            {
                sourceFile = TryReplace(properties, sourceFile);
                destinationFile = TryReplace(properties, destinationFile);

                if (!this.fileSystem.FileExists(sourceFile)) throw new FileNotFoundException(string.Format("SourceFile not found at {0}.", sourceFile));

                if (null == destinationFile)
                {
                    if (!sourceFile.EndsWith(TemplateFileExtension))
                    {
                        throw new InvalidOperationException(string.Format("Unable to use convention to derive the Destination file path because the source path does not end in '{1}'.  Source file path is {0}.", sourceFile, TemplateFileExtension));
                    }

                    destinationFile = sourceFile.Substring(0, sourceFile.Length - TemplateFileExtension.Length);
                    log.DebugFormat("Derived destination file for source {0} is {1}.", sourceFile, destinationFile);
                }

                //if (this.fileSystem.FileExists(destinationFile))
                //{
                //    BackUpDestinationFile(destinationFile);
                //}

                var template = this.fileSystem.ReadAllText(sourceFile);

                template = this.TryReplace(properties, template);

                var destinationDirectory = this.fileSystem.GetDirectoryFullPath(destinationFile);
                if (!this.fileSystem.DirectoryExists(destinationDirectory))
                {
                    log.DebugFormat("Creating directory{0}...", destinationDirectory);
                    this.fileSystem.CreateDirectory(destinationDirectory);
                }

                this.fileSystem.WriteAllText(destinationFile, template);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Error occurred while processing template {0} and destination file {1}.  Please refer to inner exception for details.",
                        sourceFile, destinationFile), ex);
            }
        }

        //private void BackUpDestinationFile(string destinationFile)
        //{
        //    log.DebugFormat("Destination file {0} already exists.  Creating backup...", destinationFile);

        //    var backupfile = string.Format("{0}.bak", destinationFile);

        //    if (this.fileSystem.FileExists(backupfile))
        //    {
        //        this.fileSystem.DeleteFile(backupfile);
        //    }

        //    this.fileSystem.CopyFile(destinationFile, backupfile);
        //    this.fileSystem.DeleteFile(destinationFile);
        //}

        private string TryReplace(IDictionary<string, string> properties, string template)
        {
            IList<string> tokensNotFound;
            template = this.tokeniser.TryReplace(template, properties, out tokensNotFound);

            if (tokensNotFound.Count > 0)
            {
                throw new InvalidOperationException(string.Format("{0} token(s) has not been matched.  Unmatched strings: {1}.", tokensNotFound.Count, string.Join(", ", tokensNotFound.ToArray())));
            }

            return template;
        }
    }
}
