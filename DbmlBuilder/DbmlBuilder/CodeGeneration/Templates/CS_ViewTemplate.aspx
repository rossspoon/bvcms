<%@ Page Language="C#"%>
<%@ Import namespace="DbmlBuilder.Utilities"%>
<%@ Import Namespace="DbmlBuilder.TableSchema" %>
<%@ Import Namespace="DbmlBuilder" %>
<%
    Table view = Db.Service.GetSchema("#VIEW#", TableType.View);
%>
namespace <%=Db.Service.GeneratedNamespace %>.View
{
	[Table(Name="<%=view.Name %>")]
	public partial class <%=view.ClassName %>
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		<%
            foreach (TableColumn col in view.Columns)
			{
        %>
		private <%=col.VarType%> _<%=col.Name%>;
		<% 
			}
		%>
		
		public <%=view.ClassName%>()
		{
		}
		
		<%
			foreach(TableColumn col in view.Columns)
			{
        %>
		[Column(Name="<%=col.ColumnName%>", Storage="_<%=col.Name%>"<%=col.FullDbType%>)]
		public <%=col.VarType%> <%=col.Name%>
		{
			get
			{
				return this._<%=col.Name%>;
			}
			set
			{
				if (this._<%=col.Name%> != value)
					this._<%=col.Name%> = value;
			}
		}
		<% 
			}
		%>
    }
}