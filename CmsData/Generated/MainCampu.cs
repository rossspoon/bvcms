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
	[Table(Name="dbo.MainCampus")]
	public partial class MainCampu : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Campus;
		
   		
   		private EntitySet< Organization> _Organizations;
		
   		private EntitySet< Person> _People;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnCampusChanging(string value);
		partial void OnCampusChanged();
		
    #endregion
		public MainCampu()
		{
			
			this._Organizations = new EntitySet< Organization>(new Action< Organization>(this.attach_Organizations), new Action< Organization>(this.detach_Organizations)); 
			
			this._People = new EntitySet< Person>(new Action< Person>(this.attach_People), new Action< Person>(this.detach_People)); 
			
			
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

		
		[Column(Name="Campus", UpdateCheck=UpdateCheck.Never, Storage="_Campus", DbType="varchar(50)")]
		public string Campus
		{
			get { return this._Campus; }

			set
			{
				if (this._Campus != value)
				{
				
                    this.OnCampusChanging(value);
					this.SendPropertyChanging();
					this._Campus = value;
					this.SendPropertyChanged("Campus");
					this.OnCampusChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_Organizations_MainCampus", Storage="_Organizations", OtherKey="CampusId")]
   		public EntitySet< Organization> Organizations
   		{
   		    get { return this._Organizations; }

			set	{ this._Organizations.Assign(value); }

   		}

		
   		[Association(Name="FK_People_MainCampus", Storage="_People", OtherKey="CampusId")]
   		public EntitySet< Person> People
   		{
   		    get { return this._People; }

			set	{ this._People.Assign(value); }

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

   		
		private void attach_Organizations(Organization entity)
		{
			this.SendPropertyChanging();
			entity.MainCampu = this;
		}

		private void detach_Organizations(Organization entity)
		{
			this.SendPropertyChanging();
			entity.MainCampu = null;
		}

		
		private void attach_People(Person entity)
		{
			this.SendPropertyChanging();
			entity.MainCampu = this;
		}

		private void detach_People(Person entity)
		{
			this.SendPropertyChanging();
			entity.MainCampu = null;
		}

		
	}

}

