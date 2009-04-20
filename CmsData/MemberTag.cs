using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData
{
	[Table(Name="dbo.MemberTags")]
	public partial class MemberTag : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Name;
		
		private int? _ProgId;
		
   		
   		private EntitySet< OrgMemMemTag> _OrgMemMemTags;
		
    	
		private EntityRef< Program> _Program;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnProgIdChanging(int? value);
		partial void OnProgIdChanged();
		
    #endregion
		public MemberTag()
		{
			
			this._OrgMemMemTags = new EntitySet< OrgMemMemTag>(new Action< OrgMemMemTag>(this.attach_OrgMemMemTags), new Action< OrgMemMemTag>(this.detach_OrgMemMemTags)); 
			
			
			this._Program = default(EntityRef< Program>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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

		
		[Column(Name="Name", UpdateCheck=UpdateCheck.Never, Storage="_Name", DbType="varchar(50)")]
		public string Name
		{
			get { return this._Name; }

			set
			{
				if (this._Name != value)
				{
				
                    this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}

			}

		}

		
		[Column(Name="ProgId", UpdateCheck=UpdateCheck.Never, Storage="_ProgId", DbType="int")]
		public int? ProgId
		{
			get { return this._ProgId; }

			set
			{
				if (this._ProgId != value)
				{
				
					if (this._Program.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnProgIdChanging(value);
					this.SendPropertyChanging();
					this._ProgId = value;
					this.SendPropertyChanged("ProgId");
					this.OnProgIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_OrgMemMemTags_MemberTags", Storage="_OrgMemMemTags", OtherKey="MemberTagId")]
   		public EntitySet< OrgMemMemTag> OrgMemMemTags
   		{
   		    get { return this._OrgMemMemTags; }

			set	{ this._OrgMemMemTags.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_MemberTags_Program", Storage="_Program", ThisKey="ProgId", IsForeignKey=true)]
		public Program Program
		{
			get { return this._Program.Entity; }

			set
			{
				Program previousValue = this._Program.Entity;
				if (((previousValue != value) 
							|| (this._Program.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Program.Entity = null;
						previousValue.MemberTags.Remove(this);
					}

					this._Program.Entity = value;
					if (value != null)
					{
						value.MemberTags.Add(this);
						
						this._ProgId = value.Id;
						
					}

					else
					{
						
						this._ProgId = default(int?);
						
					}

					this.SendPropertyChanged("Program");
				}

			}

		}

		
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

   		
		private void attach_OrgMemMemTags(OrgMemMemTag entity)
		{
			this.SendPropertyChanging();
			entity.MemberTag = this;
		}

		private void detach_OrgMemMemTags(OrgMemMemTag entity)
		{
			this.SendPropertyChanging();
			entity.MemberTag = null;
		}

		
	}

}

