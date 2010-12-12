using System;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Data.Linq;
using System.Linq.Expressions;

namespace CmsData
{
    public static class EntityExtension
    {
        //public static DataContext GetDataContext(this IQueryable query)
        //{
        //    Expression expr = query.Expression;
        //    while (expr != null)
        //    {
        //        switch (expr.NodeType)
        //        {
        //            case ExpressionType.Call:
        //                MethodCallExpression call = (MethodCallExpression)expr;
        //                if (call.Method.IsStatic && call.Arguments.Count > 0)
        //                    expr = call.Arguments[0];
        //                else
        //                    expr = call.Object;
        //                continue;
        //            case ExpressionType.Constant:
        //                var con = expr as ConstantExpression;
        //                var table = con.Value as ITable;
        //                if (table != null)
        //                    return table.Context;
        //                return con.Value as DataContext;
        //        }
        //    }
        //    return null;
        //}
        //public static DataContext GetDataContext(this INotifyPropertyChanging item)
        //{
        //    return GetDataContext<DataContext>(item, false);
        //}

        //public static DataContext GetDataContext(this INotifyPropertyChanging item, bool throwIfNotFound)
        //{
        //    return GetDataContext<DataContext>(item, throwIfNotFound);
        //}

        //public static ContextType GetDataContext<ContextType>(this INotifyPropertyChanging item, bool throwIfNotFound)
        //    where ContextType : DataContext
        //{
        //    var changeTracker = FindChangeTracker(item);
        //    if (changeTracker != null)
        //        return GetDataContextFromChangeTracker<ContextType>(changeTracker);
        //    else
        //        if (throwIfNotFound)
        //            throw new InvalidOperationException("No context attached to this object");
        //        else
        //            return null;
        //}

        //private static object FindChangeTracker(INotifyPropertyChanging item)
        //{
        //    object changeTracker = null;
        //    var eventDelegate = GetEventDelegateFrom(item);
        //    if (eventDelegate != null)
        //        changeTracker = (from d in eventDelegate.GetInvocationList()
        //                         where IsChangeTracker(d.Target)
        //                         select d.Target).First();
        //    return changeTracker;
        //}

        //private static PropertyChangingEventHandler GetEventDelegateFrom(INotifyPropertyChanging item)
        //{
        //    var itemType = item.GetType();
        //    var eventField = itemType.GetField("PropertyChanging", 
        //        BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (eventField == null)
        //        return null;
        //    var eventDelegate = (PropertyChangingEventHandler)eventField.GetValue(item);
        //    return eventDelegate;
        //}

        //private static DataContextType GetDataContextFromChangeTracker<DataContextType>(object changeTracker)
        //{
        //    var typeOfValue = changeTracker.GetType();
        //    var servicesField = typeOfValue.GetField("services", 
        //        BindingFlags.NonPublic | BindingFlags.Instance);
        //    var services = servicesField.GetValue(changeTracker);
        //    var serviesType = services.GetType();
        //    var contextProperty = serviesType.GetProperty("Context", 
        //        BindingFlags.Instance | BindingFlags.Public);

        //    return (DataContextType)contextProperty.GetValue(services, null);
        //}

        //private static bool IsChangeTracker(object value)
        //{
        //    var baseType = value.GetType().BaseType;
        //    return baseType.Name == "ChangeTracker";
        //}
        public static void SetNoLock(this CMSDataContext ds) 
        { 
            ds.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED"); 
        }
    }
}
