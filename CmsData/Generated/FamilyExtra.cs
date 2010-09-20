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
	[Table(Name="dbo.FamilyExtra")]
	public partial class FamilyExtra : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _FamilyId;
		
		private string _Field;
		
		private DateTime _TransactionTime;
		
		private string _Data;
		
   		
    	
		private EntityRef< Family> _Family;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnFamilyIdChanging(int value);
		partial void OnFamilyIdChanged();
		
		partial void OnFieldChanging(string value);
		partial void OnFieldChanged();
		
		partial void OnTransactionTimeChanging(DateTime value);
		partial void OnTransactionTimeChanged();
		
		partial void OnDataChanging(string value);
		partial void OnDataChanged();
		
    #endregion
		public FamilyExtra()
		{
			
			
			this._Family = default(EntityRef< Family>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="FamilyId", UpdateCheck=UpdateCheck.Never, Storage="_FamilyId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int FamilyId
		{
			get { return this._FamilyId; }

			set
			{
				if (this._FamilyId != value)
				{
				
					if (this._Family.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnFamilyIdChanging(value);
					this.SendPropertyChanging();
					this._FamilyId = value;
					this.SendPropertyChanged("FamilyId");
					this.OnFamilyIdChanged();
				}

			}

		}

		
		[Column(Name="Field", UpdateCheck=UpdateCheck.Never, Storage="_Field", DbType="varchar(50) NOT NULL", IsPrimaryKey=true)]
		public string Field
		{
			get { return this._Field; }

			set
			{
				if (this._Field != value)
				{
				
                    this.OnFieldChanging(value);
					this.SendPropertyChanging();
					this._Field = value;
					this.SendPropertyChanged("Field");
					this.OnFieldChanged();
				}

			}

		}

		
		[Column(Name="TransactionTime", UpdateCheck=UpdateCheck.Never, Storage="_TransactionTime", DbType="datetime NOT NULL", IsPrimaryKey=true)]
		public DateTime TransactionTime
		{
			get { return this._TransactionTime; }

			set
			{
				if (this._TransactionTime != value)
				{
				
                    this.OnTransactionTimeChanging(value);
					this.SendPropertyChanging();
					this._TransactionTime = value;
					this.SendPropertyChanged("TransactionTime");
					this.OnTransactionTimeChanged();
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

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_FamilyExtra_Family", Storage="_Family", ThisKey="FamilyId", IsForeignKey=true)]
		public Family Family
		{
			get { return this._Family.Entity; }

			set
			{
				Family previousValue = this._Family.Entity;
				if (((previousValue != value) 
							|| (this._Family.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Family.Entity = null;
						previousValue.FamilyExtras.Remove(this);
					}

					this._Family.Entity = value;
					if (value != null)
					{
						value.FamilyExtras.Add(this);
						
						this._FamilyId = value.FamilyId;
						
					}

					else
					{
						
						this._FamilyId = default(int);
						
					}

					this.SendPropertyChanged("Family");
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

   		
	}

}

