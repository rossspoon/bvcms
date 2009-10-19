using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData
{
	[Table(Name="dbo.Verse")]
	public partial class Verse : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _VerseRef;
		
		private string _VerseText;
		
		private string _Version;
		
		private int? _Book;
		
		private int? _Chapter;
		
		private int? _VerseNum;
		
		private DateTime? _CreatedOn;
		
		private int? _CreatedBy;
		
   		
   		private EntitySet< VerseCategoryXref> _VerseCategoryXrefs;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnVerseRefChanging(string value);
		partial void OnVerseRefChanged();
		
		partial void OnVerseTextChanging(string value);
		partial void OnVerseTextChanged();
		
		partial void OnVersionChanging(string value);
		partial void OnVersionChanged();
		
		partial void OnBookChanging(int? value);
		partial void OnBookChanged();
		
		partial void OnChapterChanging(int? value);
		partial void OnChapterChanged();
		
		partial void OnVerseNumChanging(int? value);
		partial void OnVerseNumChanged();
		
		partial void OnCreatedOnChanging(DateTime? value);
		partial void OnCreatedOnChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
    #endregion
		public Verse()
		{
			
			this._VerseCategoryXrefs = new EntitySet< VerseCategoryXref>(new Action< VerseCategoryXref>(this.attach_VerseCategoryXrefs), new Action< VerseCategoryXref>(this.detach_VerseCategoryXrefs)); 
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="id", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get { return this._Id; }

			set
			{
				if (this._Id != value)
				{
				
                    this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}

			}

		}

		
		[Column(Name="VerseRef", UpdateCheck=UpdateCheck.Never, Storage="_VerseRef", DbType="nvarchar(100)")]
		public string VerseRef
		{
			get { return this._VerseRef; }

			set
			{
				if (this._VerseRef != value)
				{
				
                    this.OnVerseRefChanging(value);
					this.SendPropertyChanging();
					this._VerseRef = value;
					this.SendPropertyChanged("VerseRef");
					this.OnVerseRefChanged();
				}

			}

		}

		
		[Column(Name="VerseText", UpdateCheck=UpdateCheck.Never, Storage="_VerseText", DbType="nvarchar")]
		public string VerseText
		{
			get { return this._VerseText; }

			set
			{
				if (this._VerseText != value)
				{
				
                    this.OnVerseTextChanging(value);
					this.SendPropertyChanging();
					this._VerseText = value;
					this.SendPropertyChanged("VerseText");
					this.OnVerseTextChanged();
				}

			}

		}

		
		[Column(Name="Version", UpdateCheck=UpdateCheck.Never, Storage="_Version", DbType="nvarchar(100)")]
		public string Version
		{
			get { return this._Version; }

			set
			{
				if (this._Version != value)
				{
				
                    this.OnVersionChanging(value);
					this.SendPropertyChanging();
					this._Version = value;
					this.SendPropertyChanged("Version");
					this.OnVersionChanged();
				}

			}

		}

		
		[Column(Name="Book", UpdateCheck=UpdateCheck.Never, Storage="_Book", DbType="int")]
		public int? Book
		{
			get { return this._Book; }

			set
			{
				if (this._Book != value)
				{
				
                    this.OnBookChanging(value);
					this.SendPropertyChanging();
					this._Book = value;
					this.SendPropertyChanged("Book");
					this.OnBookChanged();
				}

			}

		}

		
		[Column(Name="Chapter", UpdateCheck=UpdateCheck.Never, Storage="_Chapter", DbType="int")]
		public int? Chapter
		{
			get { return this._Chapter; }

			set
			{
				if (this._Chapter != value)
				{
				
                    this.OnChapterChanging(value);
					this.SendPropertyChanging();
					this._Chapter = value;
					this.SendPropertyChanged("Chapter");
					this.OnChapterChanged();
				}

			}

		}

		
		[Column(Name="VerseNum", UpdateCheck=UpdateCheck.Never, Storage="_VerseNum", DbType="int")]
		public int? VerseNum
		{
			get { return this._VerseNum; }

			set
			{
				if (this._VerseNum != value)
				{
				
                    this.OnVerseNumChanging(value);
					this.SendPropertyChanging();
					this._VerseNum = value;
					this.SendPropertyChanged("VerseNum");
					this.OnVerseNumChanged();
				}

			}

		}

		
		[Column(Name="CreatedOn", UpdateCheck=UpdateCheck.Never, Storage="_CreatedOn", DbType="datetime")]
		public DateTime? CreatedOn
		{
			get { return this._CreatedOn; }

			set
			{
				if (this._CreatedOn != value)
				{
				
                    this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}

			}

		}

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int")]
		public int? CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_VerseCategoryXref_Verse", Storage="_VerseCategoryXrefs", OtherKey="VerseId")]
   		public EntitySet< VerseCategoryXref> VerseCategoryXrefs
   		{
   		    get { return this._VerseCategoryXrefs; }

			set	{ this._VerseCategoryXrefs.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
	#endregion
	
		public event PropertyChangingEventHandler PropertyChanging;
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
				this.PropertyChanging(this, emptyChangingEventArgs);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

   		
		private void attach_VerseCategoryXrefs(VerseCategoryXref entity)
		{
			this.SendPropertyChanging();
			entity.Verse = this;
		}

		private void detach_VerseCategoryXrefs(VerseCategoryXref entity)
		{
			this.SendPropertyChanging();
			entity.Verse = null;
		}

		
	}

}

