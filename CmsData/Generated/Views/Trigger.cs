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
	[Table(Name="Triggers")]
	public partial class Trigger
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _TableName;
		
		private string _TriggerName;
		
		private DateTime _TriggerCreatedDate;
		
		private string _Code;
		
		
		public Trigger()
		{
		}

		
		
		[Column(Name="TableName", Storage="_TableName", DbType="nvarchar(128) NOT NULL")]
		public string TableName
		{
			get
			{
				return this._TableName;
			}

			set
			{
				if (this._TableName != value)
					this._TableName = value;
			}

		}

		
		[Column(Name="TriggerName", Storage="_TriggerName", DbType="nvarchar(128) NOT NULL")]
		public string TriggerName
		{
			get
			{
				return this._TriggerName;
			}

			set
			{
				if (this._TriggerName != value)
					this._TriggerName = value;
			}

		}

		
		[Column(Name="TriggerCreatedDate", Storage="_TriggerCreatedDate", DbType="datetime NOT NULL")]
		public DateTime TriggerCreatedDate
		{
			get
			{
				return this._TriggerCreatedDate;
			}

			set
			{
				if (this._TriggerCreatedDate != value)
					this._TriggerCreatedDate = value;
			}

		}

		
		[Column(Name="Code", Storage="_Code", DbType="nvarchar(4000)")]
		public string Code
		{
			get
			{
				return this._Code;
			}

			set
			{
				if (this._Code != value)
					this._Code = value;
			}

		}

		
    }

}
