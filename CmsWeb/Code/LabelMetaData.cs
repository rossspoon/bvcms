using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CmsWeb.Code
{
    public class UIText : Attribute, IMetadataAware
    {
        private readonly string label;
//        private readonly string classes;
//        private readonly string labelclasses;
        public UIText(string label/*, string classes, string labelclasses = null*/)
        {
            this.label = label;
//            this.classes = classes;
//            this.labelclasses = labelclasses;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["label"] = label;
//            metadata.AdditionalValues["classes"] = classes;
//            metadata.AdditionalValues["labelclasses"] = labelclasses;
            metadata.TemplateHint = "Text";
        }
    }
    public class UIDate : Attribute, IMetadataAware
    {
        private readonly string label;
        private readonly string classes;
        public UIDate(string label, string classes)
        {
            this.label = label;
            this.classes = classes;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["label"] = label;
            metadata.AdditionalValues["classes"] = classes;
            metadata.TemplateHint = "Date";
        }
    }
    public class UIEmail : Attribute, IMetadataAware
    {
        private readonly string label;
        private readonly string classes;
        public UIEmail(string label, string classes)
        {
            this.label = label;
            this.classes = classes;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["label"] = label;
            metadata.AdditionalValues["classes"] = classes;
            metadata.TemplateHint = "Email";
        }
    }
    public class UIBool : Attribute, IMetadataAware
    {
        private readonly string truetext;
        private readonly string falsetext;
        private readonly string classes;
        public UIBool(string truetext, string falsetext, string classes)
        {
            this.truetext = truetext;
            this.falsetext = falsetext;
            this.classes = classes;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["truetext"] = truetext;
            metadata.AdditionalValues["falsetext"] = truetext;
            metadata.AdditionalValues["classes"] = classes;
            metadata.TemplateHint = "Bool";
        }
    }
    public class UICode : Attribute, IMetadataAware
    {
        private readonly string label;
        private readonly string classes;
        public UICode(string label, string classes)
        {
            this.label = label;
            this.classes = classes;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["label"] = label;
            metadata.AdditionalValues["classes"] = classes;
            metadata.TemplateHint = "Code";
        }
    }
}