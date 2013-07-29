using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData.View
{
	[Table(Name="PreviousMemberCounts")]
	public partial class PreviousMemberCount
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _OrganizationId;
		
		private int? _Prevcount;
		
		
		public PreviousMemberCount()
		{
		}

		
		
		[Column(Name="OrganizationId", Storage="_OrganizationId", DbType="int NOT NULL")]
		public int OrganizationId
		{
			get
			{
				return this._OrganizationId;
			}

			set
			{
				if (this._OrganizationId != value)
					this._OrganizationId = value;
			}

		}

		
		[Column(Name="prevcount", Storage="_Prevcount", DbType="int")]
		public int? Prevcount
		{
			get
			{
				return this._Prevcount;
			}

			set
			{
				if (this._Prevcount != value)
					this._Prevcount = value;
			}

		}

		
    }

}
