﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
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
    public partial class PropertyConfigurationManifest {
        
        private PropertyConfigurationManifestEnvironment[] environmentsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Environment", IsNullable=false)]
        public PropertyConfigurationManifestEnvironment[] Environments {
            get {
                return this.environmentsField;
            }
            set {
                this.environmentsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class PropertyConfigurationManifestEnvironment {
        
        private PropertyConfigurationManifestEnvironmentConfiguration configurationField;
        
        private string nameField;
        
        /// <remarks/>
        public PropertyConfigurationManifestEnvironmentConfiguration Configuration {
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
    public partial class PropertyConfigurationManifestEnvironmentConfiguration {
        
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
}