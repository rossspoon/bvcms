using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsData
{
    public partial class QueryBuilderClause
    {
        public string Serialize()
        {
            XmlWriter w;
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = new UTF8Encoding(false);
            var sb = new StringBuilder();
            using (w = XmlWriter.Create(sb, settings))
            {
                w.WriteStartElement("Search");
                w.WriteComment("{0}/{1}".Fmt(Util.ServerLink("/QueryBuilder/Main"), this.QueryId));
                WriteClause(w, this);
                w.WriteEndElement();
            }
            return sb.ToString();
        }
        public void Serialize(XmlWriter w)
        { 
            using (w)
            {
                w.WriteStartElement("Search");
                WriteClause(w, this);
                w.WriteEndElement();
            }
        }
        private void WriteClause(XmlWriter w, QueryBuilderClause clause)
        {
            w.WriteStartElement("Condition");
            w.WriteAttributeString("ClauseOrder", clause.ClauseOrder.ToString());
            w.WriteAttributeString("Field", clause.Field);
            if (clause.Description.HasValue())
                w.WriteAttributeString("Description", clause.Description);
            w.WriteAttributeString("Comparison", clause.Comparison);
            if (clause.TextValue.HasValue())
                w.WriteAttributeString("TextValue", clause.TextValue);
            if (clause.DateValue.HasValue)
                w.WriteAttributeString("DateValue", clause.DateValue.ToString());
            if (clause.CodeIdValue.HasValue())
                w.WriteAttributeString("CodeIdValue", clause.CodeIdValue);
            if (clause.StartDate.HasValue)
                w.WriteAttributeString("StartDate", clause.StartDate.ToString());
            if (clause.EndDate.HasValue)
                w.WriteAttributeString("EndDate", clause.EndDate.ToString());
            if (clause.Program > 0)
                w.WriteAttributeString("Program", clause.Program.ToString());
            if (clause.Division > 0)
                w.WriteAttributeString("Division", clause.Division.ToString());
            if (clause.Organization > 0)
                w.WriteAttributeString("Organization", clause.Organization.ToString());
            if (clause.Days > 0)
                w.WriteAttributeString("Days", clause.Days.ToString());
            if (clause.Quarters.HasValue())
                w.WriteAttributeString("Quarters", clause.Quarters);
            if (clause.Tags.HasValue())
                w.WriteAttributeString("Tags", clause.Tags);
            if (clause.Schedule > 0)
                w.WriteAttributeString("Schedule", clause.Schedule.ToString());
            if (clause.Age.HasValue)
                w.WriteAttributeString("Age", clause.Age.ToString());
            foreach (var qc in clause.Clauses)
                WriteClause(w, qc);
            w.WriteEndElement();
        }
    }
}