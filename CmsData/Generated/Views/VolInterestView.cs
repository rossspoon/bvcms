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
	[Table(Name="VolInterestView")]
	public partial class VolInterestView
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Opportunity;
		
		private string _Name;
		
		private DateTime? _Created;
		
		private string _Interest;
		
		private string _CVAStatus;
		
		
		public VolInterestView()
		{
		}

		
		
		[Column(Name="Opportunity", Storage="_Opportunity", DbType="varchar(50)")]
		public string Opportunity
		{
			get
			{
				return this._Opportunity;
			}

			set
			{
				if (this._Opportunity != value)
					this._Opportunity = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(36)")]
		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
					this._Name = value;
			}

		}

		
		[Column(Name="Created", Storage="_Created", DbType="datetime")]
		public DateTime? Created
		{
			get
			{
				return this._Created;
			}

			set
			{
				if (this._Created != value)
					this._Created = value;
			}

		}

		
		[Column(Name="Interest", Storage="_Interest", DbType="varchar(50)")]
		public string Interest
		{
			get
			{
				return this._Interest;
			}

			set
			{
				if (this._Interest != value)
					this._Interest = value;
			}

		}

		
		[Column(Name="CVAStatus", Storage="_CVAStatus", DbType="varchar(50)")]
		public string CVAStatus
		{
			get
			{
				return this._CVAStatus;
			}

			set
			{
				if (this._CVAStatus != value)
					this._CVAStatus = value;
			}

		}

		
    }

}
