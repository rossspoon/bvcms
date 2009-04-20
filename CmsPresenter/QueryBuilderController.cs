/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using CmsData;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Data.Linq;
using System.Data;

namespace CMSPresenter
{
    [DataObject]
    public class QueryBuilderController
    {
        private IQueryView view;
        private QueryBuilderClause Qb;
        private CMSDataContext Db;

        public QueryBuilderController(IQueryView ev)
        {
            Db = new CMSDataContext(Util.ConnectionString);
            Qb = Db.QueryBuilderScratchPad();
            view = ev;
            if (view.QueryId.HasValue && view.QueryId.Value != Qb.QueryId)
            {
                var existing = Db.LoadQueryById(view.QueryId.Value);
                if (existing != null)
                {
                    Qb.CopyFromAll(existing);
                    view.Description = Qb.Description;
                    Qb.Description = Util.ScratchPad;
                    Db.SubmitChanges();
                    view.SelectedId = Qb.QueryId;
                    EditClause(view.SelectedId);
                }
            }
            view.QueryId = Qb.QueryId;
            if (view.SelectedId == 0)
                view.SelectedId = Qb.QueryId;
        }

        public bool IsPublic { get { return Qb.IsPublic; } }
        private int level;
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<QueryClauseDisplay> ConditionList()
        {
            level = 0;
            return ClauseAndSubs(new List<QueryClauseDisplay>(), Qb);
        }
        private List<QueryClauseDisplay> ClauseAndSubs(List<QueryClauseDisplay> list, QueryBuilderClause qc)
        {
            list.Add(new QueryClauseDisplay { Level = (level * 25), Clause = qc });
            level++;
            var q = qc.Clauses.OrderBy(c => c.ClauseOrder);
            foreach (var c in q)
                list = ClauseAndSubs(list, c);
            level--;
            return list;
        }

        public void EditClause(int id)
        {
            var c = Db.LoadQueryById(id);
            if (c == null)
                return;
            //view.CategoryText = c.FieldInfo.CategoryTitle;
            view.FieldText = c.FieldInfo.Name;
            UpdateExpressionView();
            view.ComparisonText = c.Comparison;
            view.TextValue = c.TextValue;
            view.DateValue = c.DateValue;
            view.DivOrgValue = c.DivOrg;
            view.SubDivOrgValue = c.SubDivOrg;
            view.OrganizationValue = c.Organization;
            view.StartDateValue = c.StartDate;
            view.EndDateValue = c.EndDate;
            view.SelectMultiple = c.HasMultipleCodes;
            view.CodeValue = c.CodeIdValue;
            view.AddToGroupEnabled = c.IsGroup;
            view.AddEnabled = !c.IsFirst;
            view.DaysValue = c.Days;
            view.QuartersValue = c.Quarters;
            view.TagsValue = c.Tags;
            view.SavedQueryIdDesc = c.SavedQueryIdDesc;
        }
        public bool UpdateClause(int id)
        {
            if (id == 0)
                return false;
            var c = Db.LoadQueryById(id);
            if (c == null || (c.IsFirst && view.FieldText != "Group"))
                return false;
            UpdateClause(c);
            return true;
        }
        private void UpdateClause(QueryBuilderClause c)
        {
            c.Field = view.FieldText;
            c.Comparison = view.ComparisonText;
            c.TextValue = view.TextValue;
            c.DateValue = view.DateValue;
            c.DivOrg = view.DivOrgValue ?? 0;
            c.SubDivOrg = view.SubDivOrgValue ?? 0;
            c.Organization = view.OrganizationValue ?? 0;
            c.StartDate = view.StartDateValue;
            c.EndDate = view.EndDateValue;
            c.CodeIdValue = view.CodeValue;
            c.Days = view.DaysValue;
            c.Quarters = view.QuartersValue;
            c.Tags = view.TagsValue;
            c.SavedQueryIdDesc = view.SavedQueryIdDesc;
            Db.SubmitChanges();
        }
        private void NewClause(QueryBuilderClause gc, int order)
        {
            var c = new QueryBuilderClause();
            c.ClauseOrder = order;
            gc.Clauses.Add(c);
            gc.ReorderClauses();
            UpdateClause(c);
        }
        public void CopyAsNew(int i)
        {
            var Qb = Db.LoadQueryById(i).Clone();
            if (!Qb.IsGroup)
            {
                var g = new QueryBuilderClause();
                g.SetQueryType(QueryType.Group);
                g.SetComparisonType(CompareType.AllTrue);
                Qb.Parent = g;
                Qb = g;
            }
            Db.SubmitChanges();
            view.QueryId = Qb.QueryId;
        }
        public void InsertGroupAbove(int i)
        {
            var cc = Db.LoadQueryById(i);
            var g = new QueryBuilderClause();
            g.SetQueryType(QueryType.Group);
            g.SetComparisonType(CompareType.AllTrue);
            g.ClauseOrder = cc.ClauseOrder;
            if (cc.IsFirst)
                cc.Parent = g;
            else
            {
                var currParent = cc.Parent;
                // find all clauses from cc down at same level
                var q = from c in cc.Parent.Clauses
                        orderby c.ClauseOrder
                        where c.ClauseOrder >= cc.ClauseOrder
                        select c;
                foreach (var c in q)
                    c.Parent = g;   // change to new parent
                g.Parent = currParent;
            }
            if (cc.SavedBy.HasValue())
            {
                g.SavedBy = Util.UserName;
                g.Description = cc.Description;
                g.CreatedOn = cc.CreatedOn;
                cc.IsPublic = false;
                cc.Description = null;
                cc.SavedBy = null;
            }
            Db.SubmitChanges();
            if (g.IsFirst)
            {
                Qb = g;
                view.QueryId = g.QueryId;
            }
        }

        public void SaveQuery()
        {
            var saveto = Db.QueryBuilderClauses.FirstOrDefault(c =>
                c.SavedBy == Util.UserName && c.Description == view.Description);
            if (saveto == null)
            {
                saveto = new QueryBuilderClause();
                Db.QueryBuilderClauses.InsertOnSubmit(saveto);
            }
            saveto.CopyFromAll(Qb); // save Qb on top of existing
            saveto.SavedBy = Util.UserName;
            saveto.Description = view.Description;
            saveto.IsPublic = view.IsPublic;
            view.Description = saveto.Description; // sync both display and textbox
            Db.SubmitChanges();
        }
        public void AddConditionToGroup(int id)
        {
            var c = Db.LoadQueryById(id);
            if (c == null)
                return;
            NewClause(c, c.MaxClauseOrder() + 1);
        }
        public void AddConditionAfterCurrent(int id)
        {
            var c = Db.LoadQueryById(id);
            NewClause(c.Parent, c.ClauseOrder + 1);
        }
        public bool DeleteCondition(int id)
        {
            var c = Db.LoadQueryById(id);
            if (c == null || c.IsFirst)
                return false;
            view.SelectedId = c.Parent.QueryId;
            view.AddToGroupEnabled = true;
            view.AddEnabled = !c.Parent.IsFirst;
            Db.DeleteQueryBuilderClauseOnSubmit(c);
            Db.SubmitChanges();
            return true;
        }

        public void UpdateExpressionView()
        {
            // reset to default state first
            view.RightPanelVisible = true;
            view.TextVisible = true;
            view.TextValue = "";
            view.CodeVisible = false;
            view.DateVisible = false;
            view.DivOrgVisible = false;
            view.SubDivOrgVisible = false;
            view.EndDateVisible = false;
            view.StartDateVisible = false;
            view.OrganizationVisible = false;
            view.DaysVisible = false;
            view.WeekVisible = false;
            view.SavedQueryVisible = false;
            view.QuartersVisible = false;
            view.TagsVisible = false;
            view.CodeDataSource = null;
            view.CompareDataSource = null;
            view.CodeValue = null;
            if (!FieldClass.Fields.ContainsKey(view.FieldText))
                return; // leave at default state

            var fieldMap = FieldClass.Fields[view.FieldText];
            var cc = Db.LoadQueryById(view.SelectedId);
            if (cc == null)
                return;
            if (fieldMap.Type == FieldType.Group)
            {
                view.CompareDataSource = from co in CompareClass.Comparisons
                                         where co.FieldType == FieldType.Group
                                         select co.CompType;
                view.RightPanelVisible = false;
                view.UpdateButtonEnabled = cc.IsGroup;
                return;
            }
            view.UpdateButtonEnabled = !cc.IsGroup;
            view.CompareDataSource = from c in CompareClass.Comparisons
                                     where c.FieldType == fieldMap.Type
                                     select c.CompType;
            view.TextVisible = false;
            view.DataTextField = fieldMap.DataTextField;
            view.DataValueField = fieldMap.DataValueField;

            view.SubDivOrgVisible = fieldMap.HasParam("SubDivOrg");
            view.DivOrgVisible = fieldMap.HasParam("DivOrg");
            view.OrganizationVisible = fieldMap.HasParam("Organization");
            view.DaysVisible = fieldMap.HasParam("Days");
            view.WeekVisible = fieldMap.HasParam("Week");
            view.SavedQueryVisible = fieldMap.HasParam("SavedQueryIdDesc");
            view.QuartersVisible = fieldMap.HasParam("Quarters");
            view.TagsVisible = fieldMap.HasParam("Tags");
            view.StartDateVisible = fieldMap.HasParam("StartDate");
            view.EndDateVisible = fieldMap.HasParam("EndDate");
            var cvctl = new CodeValueController();
            switch (fieldMap.Type)
            {
                case FieldType.Bit:
                case FieldType.NullBit:
                    view.CodeVisible = true;
                    view.CodeDataSource = cvctl.BitCodes();
                    break;
                case FieldType.String:
                case FieldType.DateString:
                    view.TextVisible = true;
                    break;
                case FieldType.NullNumber:
                case FieldType.Number:
                    view.NumberVisible = true;
                    break;
                case FieldType.NullInteger:
                case FieldType.Integer:
                    view.IntegerVisible = true;
                    break;
                case FieldType.Code:
                case FieldType.CodeStr:
                    view.CodeVisible = true;
                    view.CodeDataSource = Util.CallMethod(cvctl, fieldMap.DataSource);
                    view.CheckCodesDataSource = Util.CallMethod(cvctl, fieldMap.DataSource);
                    break;
                case FieldType.Date:
                    view.DateVisible = true;
                    break;
                case FieldType.DateField:
                    view.CodeVisible = true;
                    view.CodeDataSource = Util.CallMethod(cvctl, fieldMap.DataSource);
                    break;
            }
        }
        public int Count { get; set; }

        public int FetchPeopleListCount(int startRowIndex, int maximumRows,
            string sortExpression, int QueryId)
        {
            return Count;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<TaggedPersonInfo> FetchPeopleList(int startRowIndex, int maximumRows,
            string sortExpression, int QueryId)
        {
            var q = PersonQuery(QueryId);
            Count = q.Count();
            q = PersonSearchController.ApplySort(q, sortExpression);
            q = q.Skip(startRowIndex).Take(maximumRows);
            return new PersonSearchController().FetchPeopleList(q);
        }
        public IQueryable<Person> PersonQuery(int QueryId)
        {
            if (!view.ResultsVisible)
                return null;

            var db = new CMSDataContext(Util.ConnectionString);
            db.SetNoLock();
            var Qb = db.LoadQueryById(QueryId);
            var q = db.People.Where(Qb.Predicate());
            return q;
        }

        public void TagAll(int? QueryId)
        {
            var Qb = Db.LoadQueryById(QueryId.Value);
            var q = Db.People.Where(Qb.Predicate());
            Db.TagAll(q);
        }
        public void UnTagAll(int? QueryId)
        {
            var Qb = Db.LoadQueryById(QueryId.Value);
            var q = Db.People.Where(Qb.Predicate());
            Db.UnTagAll(q);
        }

        public static IEnumerable<CategoryClass> FieldCategories()
        {
            var q = from c in CategoryClass.Categories
                    where c.Title != "Grouping"
                    select c;
            return q;
        }
    }
}
