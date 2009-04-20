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
	[Table(Name="PodcastSummary")]
	public partial class PodcastSummary
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int? _UserId;
		
		private DateTime? _LastPosted;
		
		private int? _Count;
		
		private string _Username;
		
		
		public PodcastSummary()
		{
		}

		
		
		[Column(Name="UserId", Storage="_UserId", DbType="int")]
		public int? UserId
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

		
		[Column(Name="lastPosted", Storage="_LastPosted", DbType="datetime")]
		public DateTime? LastPosted
		{
			get
			{
				return this._LastPosted;
			}

			set
			{
				if (this._LastPosted != value)
					this._LastPosted = value;
			}

		}

		
		[Column(Name="Count", Storage="_Count", DbType="int")]
		public int? Count
		{
			get
			{
				return this._Count;
			}

			set
			{
				if (this._Count != value)
					this._Count = value;
			}

		}

		
		[Column(Name="Username", Storage="_Username", DbType="varchar(50) NOT NULL")]
		public string Username
		{
			get
			{
				return this._Username;
			}

			set
			{
				if (this._Username != value)
					this._Username = value;
			}

		}

		
    }

}
