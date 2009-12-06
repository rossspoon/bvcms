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
	[Table(Name="VerseSummary")]
	public partial class VerseSummary
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Id;
		
		private string _Reference;
		
		private string _ShortText;
		
		private int? _CategoryCount;
		
		private int? _Book;
		
		private int? _Chapter;
		
		private int? _VerseNum;
		
		
		public VerseSummary()
		{
		}

		
		
		[Column(Name="id", Storage="_Id", DbType="int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
					this._Id = value;
			}

		}

		
		[Column(Name="Reference", Storage="_Reference", DbType="nvarchar(203)")]
		public string Reference
		{
			get
			{
				return this._Reference;
			}

			set
			{
				if (this._Reference != value)
					this._Reference = value;
			}

		}

		
		[Column(Name="ShortText", Storage="_ShortText", DbType="nvarchar")]
		public string ShortText
		{
			get
			{
				return this._ShortText;
			}

			set
			{
				if (this._ShortText != value)
					this._ShortText = value;
			}

		}

		
		[Column(Name="CategoryCount", Storage="_CategoryCount", DbType="int")]
		public int? CategoryCount
		{
			get
			{
				return this._CategoryCount;
			}

			set
			{
				if (this._CategoryCount != value)
					this._CategoryCount = value;
			}

		}

		
		[Column(Name="Book", Storage="_Book", DbType="int")]
		public int? Book
		{
			get
			{
				return this._Book;
			}

			set
			{
				if (this._Book != value)
					this._Book = value;
			}

		}

		
		[Column(Name="Chapter", Storage="_Chapter", DbType="int")]
		public int? Chapter
		{
			get
			{
				return this._Chapter;
			}

			set
			{
				if (this._Chapter != value)
					this._Chapter = value;
			}

		}

		
		[Column(Name="VerseNum", Storage="_VerseNum", DbType="int")]
		public int? VerseNum
		{
			get
			{
				return this._VerseNum;
			}

			set
			{
				if (this._VerseNum != value)
					this._VerseNum = value;
			}

		}

		
    }

}
