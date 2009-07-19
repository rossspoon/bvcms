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
		
   		private EntitySet< Organization> _Organizations;
		
   		private EntitySet< RecReg> _RecRegs;
		
   		private EntitySet< Recreation> _Recreations;
		
   		private EntitySet< Promotion> _FromPromotions;
		
   		private EntitySet< Promotion> _ToPromotions;
		
    	
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
			
			this._Organizations = new EntitySet< Organization>(new Action< Organization>(this.attach_Organizations), new Action< Organization>(this.detach_Organizations)); 
			
			this._RecRegs = new EntitySet< RecReg>(new Action< RecReg>(this.attach_RecRegs), new Action< RecReg>(this.detach_RecRegs)); 
			
			this._Recreations = new EntitySet< Recreation>(new Action< Recreation>(this.attach_Recreations), new Action< Recreation>(this.detach_Recreations)); 
			
			this._FromPromotions = new EntitySet< Promotion>(new Action< Promotion>(this.attach_FromPromotions), new Action< Promotion>(this.detach_FromPromotions)); 
			
			this._ToPromotions = new EntitySet< Promotion>(new Action< Promotion>(this.attach_ToPromotions), new Action< Promotion>(this.detach_ToPromotions)); 
			
			
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

		
   		[Association(Name="FK_Organizations_Division", Storage="_Organizations", OtherKey="DivisionId")]
   		public EntitySet< Organization> Organizations
   		{
   		    get { return this._Organizations; }

			set	{ this._Organizations.Assign(value); }

   		}

		
   		[Association(Name="FK_Participant_Division", Storage="_RecRegs", OtherKey="DivId")]
   		public EntitySet< RecReg> RecRegs
   		{
   		    get { return this._RecRegs; }

			set	{ this._RecRegs.Assign(value); }

   		}

		
   		[Association(Name="FK_Recreation_Division", Storage="_Recreations", OtherKey="DivId")]
   		public EntitySet< Recreation> Recreations
   		{
   		    get { return this._Recreations; }

			set	{ this._Recreations.Assign(value); }

   		}

		
   		[Association(Name="FromPromotions__FromDivision", Storage="_FromPromotions", OtherKey="FromDivId")]
   		public EntitySet< Promotion> FromPromotions
   		{
   		    get { return this._FromPromotions; }

			set	{ this._FromPromotions.Assign(value); }

   		}

		
   		[Association(Name="ToPromotions__ToDivision", Storage="_ToPromotions", OtherKey="ToDivId")]
   		public EntitySet< Promotion> ToPromotions
   		{
   		    get { return this._ToPromotions; }

			set	{ this._ToPromotions.Assign(value); }

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

		
		private void attach_Organizations(Organization entity)
		{
			this.SendPropertyChanging();
			entity.Division = this;
		}

		private void detach_Organizations(Organization entity)
		{
			this.SendPropertyChanging();
			entity.Division = null;
		}

		
		private void attach_RecRegs(RecReg entity)
		{
			this.SendPropertyChanging();
			entity.Division = this;
		}

		private void detach_RecRegs(RecReg entity)
		{
			this.SendPropertyChanging();
			entity.Division = null;
		}

		
		private void attach_Recreations(Recreation entity)
		{
			this.SendPropertyChanging();
			entity.Division = this;
		}

		private void detach_Recreations(Recreation entity)
		{
			this.SendPropertyChanging();
			entity.Division = null;
		}

		
		private void attach_FromPromotions(Promotion entity)
		{
			this.SendPropertyChanging();
			entity.FromDivision = this;
		}

		private void detach_FromPromotions(Promotion entity)
		{
			this.SendPropertyChanging();
			entity.FromDivision = null;
		}

		
		private void attach_ToPromotions(Promotion entity)
		{
			this.SendPropertyChanging();
			entity.ToDivision = this;
		}

		private void detach_ToPromotions(Promotion entity)
		{
			this.SendPropertyChanging();
			entity.ToDivision = null;
		}

		
	}

}

