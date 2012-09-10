﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace Amido.PreProcessor.Cmd {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class PreProcessorManifest {
        
        private PreProcessorManifestEnvironment[] environmentsField;
        
        private PreProcessorManifestDefault defaultField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Environment", IsNullable=false)]
        public PreProcessorManifestEnvironment[] Environments {
            get {
                return this.environmentsField;
            }
            set {
                this.environmentsField = value;
            }
        }
        
        /// <remarks/>
        public PreProcessorManifestDefault Default {
            get {
                return this.defaultField;
            }
            set {
                this.defaultField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class PreProcessorManifestEnvironment {
        
        private Group[] groupsField;
        
        private PreProcessorManifestEnvironmentConfiguration configurationField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Group", IsNullable=false)]
        public Group[] Groups {
            get {
                return this.groupsField;
            }
            set {
                this.groupsField = value;
            }
        }
        
        /// <remarks/>
        public PreProcessorManifestEnvironmentConfiguration Configuration {
            get {
                return this.configurationField;
            }
            set {
                this.configurationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Group {
        
        private Command[] commandsField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Command", IsNullable=false)]
        public Command[] Commands {
            get {
                return this.commandsField;
            }
            set {
                this.commandsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Command {
        
        private string sourceField;
        
        private string destinationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Destination {
            get {
                return this.destinationField;
            }
            set {
                this.destinationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class PreProcessorManifestEnvironmentConfiguration {
        
        private string staticFileField;
        
        private string overrideFileField;
        
        /// <remarks/>
        public string StaticFile {
            get {
                return this.staticFileField;
            }
            set {
                this.staticFileField = value;
            }
        }
        
        /// <remarks/>
        public string OverrideFile {
            get {
                return this.overrideFileField;
            }
            set {
                this.overrideFileField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class PreProcessorManifestDefault {
        
        private Group[] groupsField;
        
        private string propertyConfigurationManifestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Group", IsNullable=false)]
        public Group[] Groups {
            get {
                return this.groupsField;
            }
            set {
                this.groupsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertyConfigurationManifest {
            get {
                return this.propertyConfigurationManifestField;
            }
            set {
                this.propertyConfigurationManifestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Groups {
        
        private Group[] groupField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Group")]
        public Group[] Group {
            get {
                return this.groupField;
            }
            set {
                this.groupField = value;
            }
        }
    }
}
