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
	[Table(Name="dbo.Program")]
	public partial class Program : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Name;
		
		private bool? _BFProgram;
		
   		
   		private EntitySet< Division> _Divisions;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnBFProgramChanging(bool? value);
		partial void OnBFProgramChanged();
		
    #endregion
		public Program()
		{
			
			this._Divisions = new EntitySet< Division>(new Action< Division>(this.attach_Divisions), new Action< Division>(this.detach_Divisions)); 
			
			
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

		
		[Column(Name="BFProgram", UpdateCheck=UpdateCheck.Never, Storage="_BFProgram", DbType="bit")]
		public bool? BFProgram
		{
			get { return this._BFProgram; }

			set
			{
				if (this._BFProgram != value)
				{
				
                    this.OnBFProgramChanging(value);
					this.SendPropertyChanging();
					this._BFProgram = value;
					this.SendPropertyChanged("BFProgram");
					this.OnBFProgramChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_Division_Program", Storage="_Divisions", OtherKey="ProgId")]
   		public EntitySet< Division> Divisions
   		{
   		    get { return this._Divisions; }

			set	{ this._Divisions.Assign(value); }

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

   		
		private void attach_Divisions(Division entity)
		{
			this.SendPropertyChanging();
			entity.Program = this;
		}

		private void detach_Divisions(Division entity)
		{
			this.SendPropertyChanging();
			entity.Program = null;
		}

		
	}

}

