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
	[Table(Name="ActivityAll")]
	public partial class ActivityAll
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Machine;
		
		private DateTime? _ActivityDate;
		
		private string _Name;
		
		private int _UserId;
		
		private string _Activity;
		
		
		public ActivityAll()
		{
		}

		
		
		[Column(Name="Machine", Storage="_Machine", DbType="varchar(50)")]
		public string Machine
		{
			get
			{
				return this._Machine;
			}

			set
			{
				if (this._Machine != value)
					this._Machine = value;
			}

		}

		
		[Column(Name="ActivityDate", Storage="_ActivityDate", DbType="datetime")]
		public DateTime? ActivityDate
		{
			get
			{
				return this._ActivityDate;
			}

			set
			{
				if (this._ActivityDate != value)
					this._ActivityDate = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(50) NOT NULL")]
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

		
		[Column(Name="UserId", Storage="_UserId", DbType="int NOT NULL")]
		public int UserId
		{
			get
			{
				return this._UserId;
			}

			set
			{
				if (this._UserId != value)
					this._UserId = value;
			}

		}

		
		[Column(Name="Activity", Storage="_Activity", DbType="varchar(200)")]
		public string Activity
		{
			get
			{
				return this._Activity;
			}

			set
			{
				if (this._Activity != value)
					this._Activity = value;
			}

		}

		
    }

}
