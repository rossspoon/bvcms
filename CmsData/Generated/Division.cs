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
	[Table(Name="dbo.Division")]
	public partial class Division : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Name;
		
		private int? _ProgId;
		
		private int? _SortOrder;
		
   		
   		private EntitySet< DivOrg> _DivOrgs;
		
    	
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
		
		partial void OnSortOrderChanging(int? value);
		partial void OnSortOrderChanged();
		
    #endregion
		public Division()
		{
			
			this._DivOrgs = new EntitySet< DivOrg>(new Action< DivOrg>(this.attach_DivOrgs), new Action< DivOrg>(this.detach_DivOrgs)); 
			
			
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

		
		[Column(Name="SortOrder", UpdateCheck=UpdateCheck.Never, Storage="_SortOrder", DbType="int")]
		public int? SortOrder
		{
			get { return this._SortOrder; }

			set
			{
				if (this._SortOrder != value)
				{
				
                    this.OnSortOrderChanging(value);
					this.SendPropertyChanging();
					this._SortOrder = value;
					this.SendPropertyChanged("SortOrder");
					this.OnSortOrderChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_DivOrg_Division", Storage="_DivOrgs", OtherKey="DivId")]
   		public EntitySet< DivOrg> DivOrgs
   		{
   		    get { return this._DivOrgs; }

			set	{ this._DivOrgs.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Division_Program", Storage="_Program", ThisKey="ProgId", IsForeignKey=true)]
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
						previousValue.Divisions.Remove(this);
					}

					this._Program.Entity = value;
					if (value != null)
					{
						value.Divisions.Add(this);
						
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

   		
		private void attach_DivOrgs(DivOrg entity)
		{
			this.SendPropertyChanging();
			entity.Division = this;
		}

		private void detach_DivOrgs(DivOrg entity)
		{
			this.SendPropertyChanging();
			entity.Division = null;
		}

		
	}

}

