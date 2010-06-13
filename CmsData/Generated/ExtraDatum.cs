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
	[Table(Name="dbo.ExtraData")]
	public partial class ExtraDatum : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Data;
		
		private DateTime? _Stamp;
		
   		
   		private EntitySet< Contribution> _Contributions;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDataChanging(string value);
		partial void OnDataChanged();
		
		partial void OnStampChanging(DateTime? value);
		partial void OnStampChanged();
		
    #endregion
		public ExtraDatum()
		{
			
			this._Contributions = new EntitySet< Contribution>(new Action< Contribution>(this.attach_Contributions), new Action< Contribution>(this.detach_Contributions)); 
			
			
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

		
		[Column(Name="Data", UpdateCheck=UpdateCheck.Never, Storage="_Data", DbType="varchar")]
		public string Data
		{
			get { return this._Data; }

			set
			{
				if (this._Data != value)
				{
				
                    this.OnDataChanging(value);
					this.SendPropertyChanging();
					this._Data = value;
					this.SendPropertyChanged("Data");
					this.OnDataChanged();
				}

			}

		}

		
		[Column(Name="Stamp", UpdateCheck=UpdateCheck.Never, Storage="_Stamp", DbType="datetime")]
		public DateTime? Stamp
		{
			get { return this._Stamp; }

			set
			{
				if (this._Stamp != value)
				{
				
                    this.OnStampChanging(value);
					this.SendPropertyChanging();
					this._Stamp = value;
					this.SendPropertyChanged("Stamp");
					this.OnStampChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_Contribution_ExtraData", Storage="_Contributions", OtherKey="ExtraDataId")]
   		public EntitySet< Contribution> Contributions
   		{
   		    get { return this._Contributions; }

			set	{ this._Contributions.Assign(value); }

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

   		
		private void attach_Contributions(Contribution entity)
		{
			this.SendPropertyChanging();
			entity.ExtraDatum = this;
		}

		private void detach_Contributions(Contribution entity)
		{
			this.SendPropertyChanging();
			entity.ExtraDatum = null;
		}

		
	}

}

