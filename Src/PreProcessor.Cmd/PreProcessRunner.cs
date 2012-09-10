using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Amido.Common.Exceptions;
using Amido.SystemEx.Contracts;
using Amido.SystemEx.IO;
using Amido.SystemEx.Xml.Serialisation;
using log4net;
using Microsoft.Test.CommandLineParsing;

namespace Amido.PreProcessor.Cmd
{
    public class PreProcessRunner
    {
        private static readonly ILog log = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetType());

        private IFileSystem fileSystem;
        private IPropertyManager propertyManager;
        private ITokenisationRunner tokeniser;

        public PreProcessRunner(IFileSystem fileSystem, IPropertyManager propertyManager, ITokenisationRunner tokeniser)
        {
            Args.NotNull(fileSystem, "fileSystem");
            Args.NotNull(propertyManager, "propertyManager");
            Args.NotNull(tokeniser, "tokeniser");

            this.fileSystem = fileSystem;
            this.propertyManager = propertyManager;
            this.tokeniser = tokeniser;
        }

        public void Run(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");

            CommandLineArguments parser = new CommandLineArguments();
            CommandLineParser.ParseArguments(parser, args);

            log.Info("Commandline parameters:");
            log.InfoFormat("Environment: {0}", parser.Environment);
            log.InfoFormat("ManifestFile: {0}", parser.ManifestFile);
            Process(parser);
        }

        private void Process(CommandLineArguments parser)
        {
            var processorManfiest = GetProcessorManfiest(parser.ManifestFile);

            Tuple<string, string> configSettings = GetConfigFilePaths(parser.Environment, processorManfiest, parser.ManifestFile);

            IDictionary<string, string> properties;

            properties = LoadProperties(configSettings, parser);

            ProcessManifest(properties, processorManfiest, parser);
        }

        private Tuple<string, string> GetConfigFilePaths(string environment, PreProcessorManifest processorManfiest, string manifestFile)
        {
            Tuple<string, string> configSettings = null;

            configSettings = GetEnvironmentSpecificPropertyConfiguration(environment, processorManfiest, manifestFile);

            if (configSettings == null && processorManfiest.Default.PropertyConfigurationManifest != null)
            {
                configSettings =  GetDefaultPropertyConfiguration(environment, processorManfiest, manifestFile);
            }
            
            if (configSettings == null)
            {
                throw new InvalidOperationException(string.Format("Property configuration for environment '{0}' not found.", environment));
            }

            return configSettings;
        }

        private Tuple<string, string> GetEnvironmentSpecificPropertyConfiguration(string environment, PreProcessorManifest processorManfiest, string manifestFile)
        {
            string staticFile = null;
            string overrideFile = null;

            if (processorManfiest.Environments == null)
            {
                return null;
            }

            var env = processorManfiest.Environments.FirstOrDefault(x => x.Name == environment);
            if (env == null)
            {
                return null;
            }

            var configSettings = env.Configuration;
            if (configSettings == null)
            {
                return null;
            }

            var root = fileSystem.GetDirectoryFullPath(manifestFile);

            staticFile = ResolvePath(root, env.Configuration.StaticFile);

            if (configSettings.OverrideFile != null)
            {
                overrideFile = ResolvePath(root, configSettings.OverrideFile);
            }

            return new Tuple<string, string>(staticFile, overrideFile);
        }

        private Tuple<string, string> GetDefaultPropertyConfiguration(string environment, PreProcessorManifest processorManfiest, string manifestFile)
        {
            string staticFile = null;
            string overrideFile = null;

            var propertyConfigurationManifestFileAbsolutePath = ResolvePath(fileSystem.GetDirectoryFullPath(manifestFile), processorManfiest.Default.PropertyConfigurationManifest);

            var propertyConfigurationManifest = new SerialisationManager(this.fileSystem).DeserializeXmlFile<PropertyConfigurationManifest>(propertyConfigurationManifestFileAbsolutePath);

            if (propertyConfigurationManifest.Environments == null)
            {
                throw new XmlDidNotPassValidationException("Environments element missing from property configuration manifest.");
            }

            var env = propertyConfigurationManifest.Environments.FirstOrDefault(x => x.Name == environment);
            if (env == null)
            {
                throw new InvalidOperationException(string.Format("Environment '{0}' not found in property configuration manifest file.", environment));
            }

            var configSettings = env.Configuration;
            if (configSettings == null)
            {
                throw new InvalidOperationException(string.Format("Environment Configuration '{0}' not found in property configuration manifest file.", environment));
            }

            var root = fileSystem.GetDirectoryFullPath(propertyConfigurationManifestFileAbsolutePath);

            staticFile = ResolvePath(root, configSettings.StaticFile);

            if (configSettings.OverrideFile != null)
            {
                overrideFile = ResolvePath(root, configSettings.OverrideFile);
            }

            return new Tuple<string, string>(staticFile, overrideFile);

        }
        private PreProcessorManifest GetProcessorManfiest(string manifestFile)
        {
            var manifest = new SerialisationManager(this.fileSystem).DeserializeXmlFile<PreProcessorManifest>(manifestFile);

            if (manifest.Default == null)
            {
                throw new XmlDidNotPassValidationException("Default element missing from manifest.");
            }

            foreach (var group in manifest.Default.Groups)
            {
                ValidateGroup(group);
            }

            if (manifest.Environments != null)
            {
                foreach (var environment in manifest.Environments)
                {
                    if (environment.Configuration != null)
                    {
                        ValidateConfiguration(environment.Configuration);
                    }
                    if (environment.Groups != null)
                    {
                        foreach (var group in environment.Groups)
                        {
                            ValidateGroup(group);
                        }
                    }
                }
            }
            return manifest;
        }

        private static void ValidateConfiguration(PreProcessorManifestEnvironmentConfiguration configuration)
        {
            if (configuration.StaticFile == null)
            {
                throw new XmlDidNotPassValidationException("StaticFile element is missing.");
            }
        }

        private static void ValidateGroup(Group group)
        {
            if (group.Name == null) throw new XmlDidNotPassValidationException("Group Name is missing.");

            foreach (var command in group.Commands)
            {
                if (command.Source == null)
                    throw new XmlDidNotPassValidationException(string.Format("Command Source is missing in Group {0}.",
                                                                             group.Name));
                if (command.Destination == null)
                {
                    log.DebugFormat(
                        "Destination missing from xml for group '{0}' and source '{1}'.  Using Convention to derive destination",
                        group.Name, command.Source);
                }
            }
        }

        private void ProcessManifest(IDictionary<string, string> properties, PreProcessorManifest manifest, CommandLineArguments parser)
        {
            Group[] groups = null;
            if (manifest.Environments != null)
            {
                var env = manifest.Environments.FirstOrDefault(x => x.Name == parser.Environment);
                if (env != null)
                {
                    if (env.Groups != null)
                    {
                        groups = env.Groups;
                    }
                }
            }
            if (groups == null)
            {
                log.DebugFormat(
                    "Group config for Environment {0} not found in PreProcessorManifest file.  Using Default group.",
                    parser.Environment);

                groups = manifest.Default.Groups;
                if (groups == null )
                {
                    throw new XmlDidNotPassValidationException(string.Format("No specific Groups specified for environment {0} but no DefaultGroups specified either.", parser.Environment));
                }
            }
            else
            {
                log.DebugFormat(
                    "Using group config for Environment {0} in PreProcessorManifest file.",
                    parser.Environment);
                
            }

            var root = fileSystem.GetDirectoryFullPath(parser.ManifestFile);

            foreach (var group in groups)
            {
                log.InfoFormat("PreProcess group: {0}", group.Name);

                foreach (var command in group.Commands)
                {
                    var source = ResolvePath(root, command.Source);
                    var destination = ResolvePath(root, command.Destination);

                    log.InfoFormat("PreProcess Source: {0}.  Destination:{1}", source, destination);
                    tokeniser.ProcessSingleTemplate(properties, source, destination);
                }   
            }
        }

        private string ResolvePath(string rootPath, string childPath)
        {
            if (childPath ==null)
            {
                return null;
            }

            return fileSystem.CombinePath(rootPath, childPath);
        }

        private IDictionary<string, string> LoadProperties(Tuple<string, string> configSettings, CommandLineArguments parser)
        {
            IDictionary<string, string> properties;

            if (configSettings.Item2 == null)
            {
                properties = this.propertyManager.LoadProperties(configSettings.Item1);
            }
            else
            {
                properties = this.propertyManager.LoadProperties(configSettings.Item1, configSettings.Item2);
            }
           
            return properties;
        }
    }
}
