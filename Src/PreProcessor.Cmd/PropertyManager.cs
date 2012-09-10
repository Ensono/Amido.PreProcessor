using System;
using System.Collections.Generic;
using System.Reflection;
using Amido.SystemEx.Contracts;
using Amido.SystemEx.IO;
using Amido.SystemEx.Xml.Serialisation;
using log4net;

namespace Amido.PreProcessor.Cmd
{
    public class PropertyManager : IPropertyManager
    {
        private IFileSystem fileSystem;
        private static readonly ILog log = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetType());
        
        public PropertyManager(IFileSystem fileSystem)
        {
            
            Args.NotNull(fileSystem, "fileSystem");

            this.fileSystem = fileSystem;
        }

        public IDictionary<string, string> LoadProperties(string propertyFile)
        {
            Args.NotNullOrEmpty(propertyFile, "propertyFile");

            IDictionary<string, string> propertiesDictionary = this.LoadStaticProperties(propertyFile);

            LogDictionaryInfo(propertiesDictionary, "Merged");

            return propertiesDictionary;
        }

        public IDictionary<string, string> LoadProperties(string propertyFile, string overrideFile)
        {
            Args.NotNullOrEmpty(propertyFile, "propertyFile");
            Args.NotNullOrEmpty(overrideFile, "overrideFile");

            IDictionary<string, string> propertiesDictionary = this.LoadStaticProperties(propertyFile);

            log.Debug("Loading overrides ...");
            IDictionary<string, string> overridesDictionary = this.GetPropertiesFromFile(overrideFile);
            LogDictionaryDebug(overridesDictionary, "Overrides");
            log.Debug("Overrides loaded.");

            propertiesDictionary = this.MergeProperties(propertiesDictionary, overridesDictionary, true);

            LogDictionaryInfo(propertiesDictionary, "Merged");
            
            return propertiesDictionary;
        }

        private IDictionary<string, string> LoadStaticProperties(string propertyFile)
        {
            Args.NotNullOrEmpty(propertyFile, "propertyFile");

            log.Debug("Loading system properties ...");
            IDictionary<string, string> sysPropertiesDictionary = this.AddSystemProperties();
            LogDictionaryDebug(sysPropertiesDictionary, "System");
            log.Debug("System properties loaded.");

            log.Debug("Loading static properties ...");
            IDictionary<string, string> staticPropertiesDictionary = this.GetPropertiesFromFile(propertyFile);
            LogDictionaryDebug(staticPropertiesDictionary, "Static");
            log.Debug("Static properties loaded.");

            staticPropertiesDictionary = this.MergeProperties(sysPropertiesDictionary, staticPropertiesDictionary, false);

            return staticPropertiesDictionary;
        }

        private void LogDictionaryDebug(IDictionary<string, string> dictionary, string dictionaryName)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("{0} properties are: ", dictionaryName);
                foreach (var item in dictionary)
                {
                    log.DebugFormat("name:{0} value:{1}", item.Key, item.Value);
                }
            }
        }

        private void LogDictionaryInfo(IDictionary<string, string> dictionary, string dictionaryName)
        {
            if (log.IsInfoEnabled)
            {
                log.InfoFormat("{0} properties are: ", dictionaryName);
                foreach (var item in dictionary)
                {
                    log.InfoFormat("name:{0} value:{1}", item.Key, item.Value);
                }
            }
        }

        private IDictionary<string, string> MergeProperties(IDictionary<string, string> master,
                                                           IDictionary<string, string> slave,
                                                            bool overrideAllowed)
        {
            foreach (var item in slave)
            {
                if (master.ContainsKey(item.Key))
                {
                    if (overrideAllowed)
                    {
                        log.DebugFormat("Overriding property {0}.  Old value {1}. New value {2}", item.Key, master[item.Key], item.Value);
                        master[item.Key] = item.Value;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            string.Format("Dictionary already contains property key {0} with value {1}.",
                                          item.Key, item.Value));
                    }
                }
                else
                {
                    master.Add(item.Key, item.Value);
                }
            }

            return master;
        }


        private IDictionary<string, string> AddSystemProperties()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("computer.name", Environment.MachineName);
            dictionary.Add("computer.fullyQualifiedName", System.Net.Dns.GetHostEntry("").HostName);
            dictionary.Add("computer.programFiles.directory", Environment.GetEnvironmentVariable("programFiles"));
            if (Environment.GetEnvironmentVariable("programFiles(x86)") == null)
            {
                dictionary.Add("computer.programFilesx86.directory", Environment.GetEnvironmentVariable("programFiles"));
            }
            else
            {
                dictionary.Add("computer.programFilesx86.directory", Environment.GetEnvironmentVariable("programFiles(x86)"));
            }


            dictionary.Add("computer.currentUser.Name", System.Security.Principal.WindowsIdentity.GetCurrent().Name);

            return dictionary;
        }


        private IDictionary<string, string> GetPropertiesFromFile(string propertyFile)
        {
            Args.NotNullOrEmpty(propertyFile, "propertyFile");

            PreProcessorConfig config = new SerialisationManager(fileSystem).DeserializeXmlFile<PreProcessorConfig>(propertyFile);

            return this.LoadDictionary(config);
        }


        private IDictionary<string, string> LoadDictionary(PreProcessorConfig config)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in config.Properties)
            {
                if (dictionary.ContainsKey(item.Name))
                {
                    
                    throw new InvalidOperationException(
                        string.Format("Dictionary already contains property key {0}.",
                                        item.Name));
                }
                else
                {
                    dictionary.Add(item.Name, item.Value);
                }
            }

            return dictionary;
        }


        
    }
}