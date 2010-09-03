/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace CmsData
{
    public partial class QueryBuilderClause
    {
        private FieldClass _FieldInfo;
        public FieldClass FieldInfo
        {
            get
            {
                if (_FieldInfo == null || _FieldInfo.Name != Field)
                    _FieldInfo = FieldClass.Fields[Field];
                return _FieldInfo;
            }
        }
        public void SetComparisonType(CompareType value)
        {
            Comparison = value.ToString();
        }
        public void SetQueryType(QueryType value)
        {
            Field = value.ToString();
        }
        public CompareType ComparisonType
        {
            get { return CompareClass.Convert(Comparison); }
        }
        private CompareClass _Compare;
        public CompareClass Compare
        {
            get
            {
                if (_Compare == null)
                    _Compare = CompareClass.Comparisons.Single(cm =>
                        cm.FieldType == FieldInfo.Type && cm.CompType == ComparisonType);
                return _Compare;
            }
        }
        public override string ToString()
        {
            if (!IsGroup)
                return Compare.ToString(this);

            var sb = new StringBuilder();
            if (IsFirst)
                sb.Append("Select records where ");
            switch (ComparisonType)
            {
                case CompareType.AllTrue:
                    sb.Append("ALL of these are TRUE");
                    break;
                case CompareType.AnyTrue:
                    sb.Append("ANY ONE of these is TRUE");
                    break;
                case CompareType.AllFalse:
                    sb.Append("ALL of these are FALSE");
                    break;
                case CompareType.AnyFalse:
                    sb.Append("ANY ONE of these is FALSE");
                    break;
            }
            return sb.ToString();
        }
        internal void SetIncludeDeceased()
        {
            var c = this;
            while (c.Parent != null)
                c = c.Parent;
            c.includeDeceased = true;
        }
        private bool includeDeceased = false;
        public Expression<Func<Person, bool>> Predicate()
        {
            var parm = Expression.Parameter(typeof(Person), "p");
            var tree = ExpressionTree(parm);
            if (tree == null)
                tree = Expressions.CompareConstant(parm, "PeopleId", CompareType.NotEqual, 0);
            if (includeDeceased == false)
                tree = Expression.And(tree, Expressions.CompareConstant(parm,
                     "DeceasedDate", CompareType.Equal, new DateTime?()));
            if (Util.OrgMembersOnly)
                tree = Expression.And(OrgMembersOnly(parm), tree);
            return Expression.Lambda<Func<Person, bool>>(tree, parm);
        }
        private Expression OrgMembersOnly(ParameterExpression parm)
        {
            var tag = DbUtil.Db.OrgMembersOnlyTag;
            var db = this.GetDataContext() as CMSDataContext;
            Expression<Func<Person, bool>> pred = p =>
                p.Tags.Any(t => t.Id == tag.Id);
            //db.TaggedPeople(tag.Id).Select(t => t.PeopleId).Contains(p.PeopleId);
            return Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
        }
        private bool InAllAnyFalse
        {
            get
            {
                return Parent.IsGroup
                    && (Parent.ComparisonType == CompareType.AllFalse
                        || Parent.ComparisonType == CompareType.AnyFalse);
            }
        }
        private bool AnyFalseTrue
        {
            get
            {
                return ComparisonType == CompareType.AnyTrue
                    || ComparisonType == CompareType.AnyFalse;
            }
        }
        private Expression ExpressionTree(ParameterExpression parm)
        {
            Expression expr = null;
            if (IsGroup)
            {
                foreach (var clause in Clauses.OrderBy(c => c.ClauseOrder))
                    if (expr == null)
                        expr = clause.ExpressionTree(parm);
                    else
                    {
                        var right = clause.ExpressionTree(parm);
                        if (right != null && expr != null)
                            if (AnyFalseTrue)
                                expr = Expression.Or(expr, right);
                            else
                                expr = Expression.And(expr, right);
                    }
                return expr;
            }
            else
            {
                expr = Compare.Expression(this, parm);
                if (InAllAnyFalse)
                    expr = Expression.Not(expr);
                return expr;
            }
        }
        public IQueryable<int> PeopleIds()
        {
            var Db = EntityExtension.GetDataContext(this) as CMSDataContext;
            return Db.People.Where(Predicate()).Select(p => p.PeopleId);
        }
        public int MaxClauseOrder()
        {
            int max = 0;
            if (Clauses.Count() > 0)
                max = Clauses.Max(qc => qc.ClauseOrder);
            return max;
        }
        public void ReorderClauses()
        {
            var q = from c in Clauses
                    orderby c.ClauseOrder
                    select c;
            int n = 1;
            foreach (var c in q)
            {
                c.ClauseOrder = n;
                n += 2;
            }
        }
        public bool HasMultipleCodes
        {
            get
            {
                var e = Compare;
                return e.CompType == CompareType.OneOf
                    || e.CompType == CompareType.NotOneOf;
            }
        }
        private bool IsCode
        {
            get
            {
                var e = Compare;
                return e.FieldType == FieldType.Bit
                    || e.FieldType == FieldType.NullBit
                    || e.FieldType == FieldType.Code
                    || e.FieldType == FieldType.CodeStr
                    || e.FieldType == FieldType.DateField;
            }
        }
        private enum Part { Id = 0, Code = 1 }
        private string GetCodeIdValuePart(string value, Part part)
        {
            if (value != null && value.Contains(","))
                return value.SplitStr(",", 2)[(int)part];
            return value;
        }
        internal string CodeValues
        {
            get
            {
                if (IsCode)
                    if (HasMultipleCodes)
                        return string.Join(",", (from s in CodeIdValue.SplitStr(";")
                                                 select GetCodeIdValuePart(s, Part.Code)).ToArray());
                    else
                        return GetCodeIdValuePart(CodeIdValue, Part.Code);
                return "";
            }
        }
        internal string CodeIds
        {
            get
            {
                if (IsCode)
                    if (HasMultipleCodes)
                    {
                        var q = from s in CodeIdValue.SplitStr(";")
                                select GetCodeIdValuePart(s, Part.Id);
                        return string.Join(",", q.ToArray());
                    }
                    else
                        return GetCodeIdValuePart(CodeIdValue, Part.Id);
                return "";
            }
        }
        internal int[] CodeIntIds
        {
            get
            {
                if (IsCode)
                    if (HasMultipleCodes)
                    {
                        var q = from s in CodeIdValue.SplitStr(";")
                                select GetCodeIdValuePart(s, Part.Id).ToInt();
                        return q.ToArray();
                    }
                    else
                        return new int[] { GetCodeIdValuePart(CodeIdValue, Part.Id).ToInt() };
                return null;
            }
        }
        internal string[] CodeStrIds
        {
            get
            {
                if (IsCode)
                    if (HasMultipleCodes)
                    {
                        var q = from s in CodeIdValue.SplitStr(";")
                                select GetCodeIdValuePart(s, Part.Id);
                        return q.ToArray();
                    }
                    else
                        return new string[] { GetCodeIdValuePart(CodeIdValue, Part.Id) };
                return null;
            }
        }
        public bool IsFirst
        {
            get { return IsGroup && Parent == null; }
        }
        public bool IsGroup
        {
            get { return FieldInfo.Type == FieldType.Group; }
        }
        partial void OnValidate(System.Data.Linq.ChangeAction action)
        {
            switch (action)
            {
                case System.Data.Linq.ChangeAction.Insert:
                case System.Data.Linq.ChangeAction.Update:
                    CreatedOn = Util.Now;
                    break;
            }
        }
        public QueryBuilderClause Clone()
        {
            var q = new QueryBuilderClause();
            q.CopyFrom(this);
            var Db = this.GetDataContext() as CMSDataContext;
            Db.QueryBuilderClauses.InsertOnSubmit(q);
            foreach (var c in Clauses)
                q.Clauses.Add(c.Clone());
            return q;
        }
        private void CopyFrom(QueryBuilderClause from)
        {
            ClauseOrder = from.ClauseOrder;
            Description = from.Description;
            Field = from.Field;
            Comparison = from.Comparison;
            TextValue = from.TextValue;
            DateValue = from.DateValue;
            CodeIdValue = from.CodeIdValue;
            StartDate = from.StartDate;
            EndDate = from.EndDate;
            Program = from.Program;
            Schedule = from.Schedule;
            Division = from.Division;
            Organization = from.Organization;
            Days = from.Days;
            Quarters = from.Quarters;
            Tags = from.Tags;
            SavedQueryIdDesc = from.SavedQueryIdDesc;
        }
        public void CopyFromAll(QueryBuilderClause from)
        {
            foreach (var c in Clauses)
                DeleteClause(c);
            CopyFrom(from);
            foreach (var c in from.Clauses)
                Clauses.Add(c.Clone());
        }
        private void DeleteClause(QueryBuilderClause qb)
        {
            var Db = this.GetDataContext() as CMSDataContext;
            foreach (var c in qb.Clauses)
                DeleteClause(c);
            Db.QueryBuilderClauses.DeleteOnSubmit(qb);
        }
        public void CleanSlate()
        {
            foreach (var c in Clauses)
                DeleteClause(c);
            SetQueryType(QueryType.Group);
            SetComparisonType(CompareType.AllTrue);
            var Db = this.GetDataContext() as CMSDataContext;
            Db.SubmitChanges();
        }
        public static QueryBuilderClause NewGroupClause()
        {
            var qb = new QueryBuilderClause();
            qb.SetQueryType(QueryType.Group);
            qb.SetComparisonType(CompareType.AllTrue);
            return qb;
        }
        public QueryBuilderClause AddNewClause(QueryType type, CompareType op, object value)
        {
            var qb = new QueryBuilderClause();
            qb.ClauseOrder = qb.MaxClauseOrder() + 1;
            qb.SetQueryType(type);
            this.Clauses.Add(qb);
            qb.SetComparisonType(op);
            switch (qb.FieldInfo.Type)
            {
                case FieldType.NullBit:
                case FieldType.Bit:
                case FieldType.Code:
                    qb.CodeIdValue = value.ToString();
                    break;
                case FieldType.Date:
                    qb.DateValue = (DateTime?)value;
                    break;
                case FieldType.Number:
                case FieldType.NullNumber:
                case FieldType.NullInteger:
                case FieldType.String:
                case FieldType.StringEqual:
                case FieldType.Integer:
                case FieldType.IntegerEqual:
                    qb.TextValue = value.ToString();
                    break;
                default:
                    throw new ArgumentException("type not allowed");
            }
            return qb;
        }
    }
}
