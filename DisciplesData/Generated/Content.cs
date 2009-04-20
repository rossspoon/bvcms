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
	[Table(Name="dbo.Content")]
	public partial class Content : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _ContentID;
		
		private string _Title;
		
		private string _ContentName;
		
		private string _Body;
		
		private DateTime? _CreatedOn;
		
		private string _CreatedBy;
		
		private DateTime? _ModifiedOn;
		
		private string _ModifiedBy;
		
   		
   		private EntitySet< Group> _Groups;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnContentIDChanging(int value);
		partial void OnContentIDChanged();
		
		partial void OnTitleChanging(string value);
		partial void OnTitleChanged();
		
		partial void OnContentNameChanging(string value);
		partial void OnContentNameChanged();
		
		partial void OnBodyChanging(string value);
		partial void OnBodyChanged();
		
		partial void OnCreatedOnChanging(DateTime? value);
		partial void OnCreatedOnChanged();
		
		partial void OnCreatedByChanging(string value);
		partial void OnCreatedByChanged();
		
		partial void OnModifiedOnChanging(DateTime? value);
		partial void OnModifiedOnChanged();
		
		partial void OnModifiedByChanging(string value);
		partial void OnModifiedByChanged();
		
    #endregion
		public Content()
		{
			
			this._Groups = new EntitySet< Group>(new Action< Group>(this.attach_Groups), new Action< Group>(this.detach_Groups)); 
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="ContentID", UpdateCheck=UpdateCheck.Never, Storage="_ContentID", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ContentID
		{
			get { return this._ContentID; }

			set
			{
				if (this._ContentID != value)
				{
				
                    this.OnContentIDChanging(value);
					this.SendPropertyChanging();
					this._ContentID = value;
					this.SendPropertyChanged("ContentID");
					this.OnContentIDChanged();
				}

			}

		}

		
		[Column(Name="Title", UpdateCheck=UpdateCheck.Never, Storage="_Title", DbType="nvarchar(500)")]
		public string Title
		{
			get { return this._Title; }

			set
			{
				if (this._Title != value)
				{
				
                    this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}

			}

		}

		
		[Column(Name="ContentName", UpdateCheck=UpdateCheck.Never, Storage="_ContentName", DbType="nvarchar(50) NOT NULL")]
		public string ContentName
		{
			get { return this._ContentName; }

			set
			{
				if (this._ContentName != value)
				{
				
                    this.OnContentNameChanging(value);
					this.SendPropertyChanging();
					this._ContentName = value;
					this.SendPropertyChanged("ContentName");
					this.OnContentNameChanged();
				}

			}

		}

		
		[Column(Name="Body", UpdateCheck=UpdateCheck.Never, Storage="_Body", DbType="nvarchar")]
		public string Body
		{
			get { return this._Body; }

			set
			{
				if (this._Body != value)
				{
				
                    this.OnBodyChanging(value);
					this.SendPropertyChanging();
					this._Body = value;
					this.SendPropertyChanged("Body");
					this.OnBodyChanged();
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

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="nvarchar(50)")]
		public string CreatedBy
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

		
		[Column(Name="ModifiedOn", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedOn", DbType="datetime")]
		public DateTime? ModifiedOn
		{
			get { return this._ModifiedOn; }

			set
			{
				if (this._ModifiedOn != value)
				{
				
                    this.OnModifiedOnChanging(value);
					this.SendPropertyChanging();
					this._ModifiedOn = value;
					this.SendPropertyChanged("ModifiedOn");
					this.OnModifiedOnChanged();
				}

			}

		}

		
		[Column(Name="ModifiedBy", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedBy", DbType="nvarchar(50)")]
		public string ModifiedBy
		{
			get { return this._ModifiedBy; }

			set
			{
				if (this._ModifiedBy != value)
				{
				
                    this.OnModifiedByChanging(value);
					this.SendPropertyChanging();
					this._ModifiedBy = value;
					this.SendPropertyChanged("ModifiedBy");
					this.OnModifiedByChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="Groups__WelcomeText", Storage="_Groups", OtherKey="ContentId")]
   		public EntitySet< Group> Groups
   		{
   		    get { return this._Groups; }

			set	{ this._Groups.Assign(value); }

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

   		
		private void attach_Groups(Group entity)
		{
			this.SendPropertyChanging();
			entity.WelcomeText = this;
		}

		private void detach_Groups(Group entity)
		{
			this.SendPropertyChanging();
			entity.WelcomeText = null;
		}

		
	}

}

