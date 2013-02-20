using System;
using System.Web.Mvc;

namespace CmsWeb.Code
{
    public class UIField : Attribute, IMetadataAware
    {
        private readonly string label;
        private readonly string classes;
        public UIField(string label, string classes)
        {
            this.label = label;
            this.classes = classes;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["label"] = label;
            metadata.AdditionalValues["classes"] = classes;
            metadata.TemplateHint = "Field";
        }
    }
}