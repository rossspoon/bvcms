using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData.View
{
	[Table(Name="oldroles")]
	public partial class Oldrole
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int? _Groupid;
		
		private string _Rolename;
		
		private string _Userid;
		
		
		public Oldrole()
		{
		}

		
		
		[Column(Name="groupid", Storage="_Groupid", DbType="int")]
		public int? Groupid
		{
			get
			{
				return this._Groupid;
			}

			set
			{
				if (this._Groupid != value)
					this._Groupid = value;
			}

		}

		
		[Column(Name="rolename", Storage="_Rolename", DbType="nvarchar(40)")]
		public string Rolename
		{
			get
			{
				return this._Rolename;
			}

			set
			{
				if (this._Rolename != value)
					this._Rolename = value;
			}

		}

		
		[Column(Name="userid", Storage="_Userid", DbType="nvarchar(256) NOT NULL")]
		public string Userid
		{
			get
			{
				return this._Userid;
			}

			set
			{
				if (this._Userid != value)
					this._Userid = value;
			}

		}

		
    }

}
