using System;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using UtilityExtensions;

namespace CmsData
{
    // from http://blog.matthidinger.com/2008/05/09/LINQToSQLAuditTrail.aspx

    public static class AuditExtensions
    {
        public static bool HasAttribute(this Type t, Type attrType)
        {
            return t.GetCustomAttributes(attrType, true) != null;
        }

        public static bool HasAttribute(this PropertyInfo pi, Type attrType)
        {
            return pi.GetCustomAttributes(attrType, false) != null;
        }


        private static string GetPropertyValue(PropertyInfo pi, object input)
        {
            object tmp = pi.GetValue(input, null);
            return (tmp == null) ? string.Empty : tmp.ToString();
        }

        private static string GetPropertyValue(object input)
        {
            return (input == null) ? string.Empty : input.ToString();
        }

        public static void InsertAudit(this DataContext db, Audit entity)
        {
            var auditTable = db.GetTable<Audit>();
            auditTable.InsertOnSubmit(entity);
        }

        private static Audit CreateAudit<TEntity>(Table<TEntity> table, int key) where TEntity : class
        {
            Audit audit = new Audit();
            audit.TableName = table.ToString().Substring(6).TrimEnd(')');
            audit.TableKey = key;
            audit.UserName = Util.UserName;
            audit.AuditDate = Util.Now;
            return audit;
        }

        public static void Audit<TEntity>(this DataContext db, Func<TEntity, int> tableKeySelector) where TEntity : class
        {
            Audit<TEntity, TEntity>(db, tableKeySelector);
        }

        public static void Audit<TBaseEntity, TSubEntity>(this DataContext db, Func<TSubEntity, int> tableKeySelector)
            where TBaseEntity : class
            where TSubEntity : TBaseEntity
        {
            AuditInserts<TBaseEntity, TSubEntity>(db, tableKeySelector);
            AuditUpdates<TBaseEntity, TSubEntity>(db, tableKeySelector);
            AuditDeletes<TBaseEntity, TSubEntity>(db, tableKeySelector);
        }

        private static void AuditInserts<TEntity, TSubEntity>(DataContext db, Func<TSubEntity, int> tableKeySelector)
            where TEntity : class
            where TSubEntity : TEntity
        {
            var inserts = db.GetChangeSet().Inserts.OfType<TSubEntity>();

            Table<TEntity> table = db.GetTable<TEntity>();
            PropertyInfo[] props = typeof(TSubEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach (TSubEntity item in inserts)
            {
                // Get the Primary Key for our table by Invoking the tableKeySelector delegate on the current TSubEntity item
                int key = tableKeySelector.Invoke(item);

                // Create the Audit
                Audit audit = CreateAudit<TEntity>(table, key);
                audit.Action = "Insert";

                // Loop through every property in our inserted entity
                foreach (PropertyInfo pi in props)
                {
                    // This code checks to see if the property is a LINQ to SQL column. You may change this if you need.
                    if (pi.HasAttribute(typeof(ColumnAttribute)))
                    {
                        //// I chose to ignore any Id columns in the auditing, again, you may change this
                        //if (pi.Name.EndsWith("Id"))
                        //    continue;

                        // Creat the AuditValue row and add it to our current Audit
                        AuditValue values = new AuditValue();
                        values.MemberName = pi.Name.SplitUpperCaseToString();
                        values.NewValue = GetPropertyValue(pi, item);

                        audit.AuditValues.Add(values);
                    }
                }
                db.InsertAudit(audit);
            }
        }
        private static void AuditDeletes<TEntity, TSubEntity>(DataContext db, Func<TSubEntity, int> tableKeySelector)
            where TEntity : class
            where TSubEntity : TEntity
        {
            var deletes = db.GetChangeSet().Deletes.OfType<TSubEntity>();
            Table<TEntity> table = db.GetTable<TEntity>();

            PropertyInfo[] props = typeof(TSubEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach (TSubEntity item in deletes)
            {
                int key = tableKeySelector.Invoke(item);

                Audit audit = CreateAudit<TEntity>(table, key);
                audit.Action = "Delete";

                foreach (PropertyInfo pi in props)
                {
                    if (pi.HasAttribute(typeof(ColumnAttribute)))
                    {
                        //if (pi.Name.EndsWith("Id"))
                        //    continue;

                        AuditValue values = new AuditValue();
                        values.MemberName = pi.Name.SplitUpperCaseToString();
                        values.OldValue = GetPropertyValue(pi, item);

                        audit.AuditValues.Add(values);
                    }
                }

                db.InsertAudit(audit);
            }
        }
        private static void AuditUpdates<TEntity, TSubEntity>(DataContext db, Func<TSubEntity, int> tableKey)
            where TEntity : class
            where TSubEntity : TEntity
        {
            var updates = db.GetChangeSet().Updates.OfType<TSubEntity>();
            Table<TEntity> table = db.GetTable<TEntity>();

            foreach (TSubEntity item in updates)
            {
                int key = tableKey.Invoke(item);

                Audit audit = CreateAudit<TEntity>(table, key);
                audit.Action = "Update";

                ModifiedMemberInfo[] mmi = table.GetModifiedMembers(item);

                foreach (ModifiedMemberInfo mi in mmi)
                {
                    AuditValue values = new AuditValue();
                    values.MemberName = mi.Member.Name.SplitUpperCaseToString();

                    values.OldValue = GetPropertyValue(mi.OriginalValue);
                    values.NewValue = GetPropertyValue(mi.CurrentValue);

                    audit.AuditValues.Add(values);
                }
                db.InsertAudit(audit);
            }
        }
    }
}
