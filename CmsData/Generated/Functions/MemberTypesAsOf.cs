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
	[Table(Name="MemberTypesAsOf")]
	public partial class MemberTypesAsOf
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _MemberTypeId;
		
		
		public MemberTypesAsOf()
		{
		}

		
		
		[Column(Name="member_type_id", Storage="_MemberTypeId", DbType="int NOT NULL")]
		public int MemberTypeId
		{
			get
			{
				return this._MemberTypeId;
			}

			set
			{
				if (this._MemberTypeId != value)
					this._MemberTypeId = value;
			}

		}

		
    }

}
