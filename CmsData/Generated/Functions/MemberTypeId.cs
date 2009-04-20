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
	[Table(Name="MemberTypeIds")]
	public partial class MemberTypeId
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _MemberTypeIdX;
		
		
		public MemberTypeId()
		{
		}

		
		
		[Column(Name="member_type_id", Storage="_MemberTypeIdX", DbType="int NOT NULL")]
		public int MemberTypeIdX
		{
			get
			{
				return this._MemberTypeIdX;
			}

			set
			{
				if (this._MemberTypeIdX != value)
					this._MemberTypeIdX = value;
			}

		}

		
    }

}
