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
	[Table(Name="dbo.VolInterestInterestCodes")]
	public partial class VolInterestInterestCode : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _VolInterestId;
		
		private int _InterestCodeId;
		
   		
    	
		private EntityRef< VolInterest> _VolInterest;
		
		private EntityRef< VolInterestCode> _VolInterestCode;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnVolInterestIdChanging(int value);
		partial void OnVolInterestIdChanged();
		
		partial void OnInterestCodeIdChanging(int value);
		partial void OnInterestCodeIdChanged();
		
    #endregion
		public VolInterestInterestCode()
		{
			
			
			this._VolInterest = default(EntityRef< VolInterest>); 
			
			this._VolInterestCode = default(EntityRef< VolInterestCode>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="VolInterestId", UpdateCheck=UpdateCheck.Never, Storage="_VolInterestId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int VolInterestId
		{
			get { return this._VolInterestId; }

			set
			{
				if (this._VolInterestId != value)
				{
				
					if (this._VolInterest.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnVolInterestIdChanging(value);
					this.SendPropertyChanging();
					this._VolInterestId = value;
					this.SendPropertyChanged("VolInterestId");
					this.OnVolInterestIdChanged();
				}

			}

		}

		
		[Column(Name="InterestCodeId", UpdateCheck=UpdateCheck.Never, Storage="_InterestCodeId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int InterestCodeId
		{
			get { return this._InterestCodeId; }

			set
			{
				if (this._InterestCodeId != value)
				{
				
					if (this._VolInterestCode.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnInterestCodeIdChanging(value);
					this.SendPropertyChanging();
					this._InterestCodeId = value;
					this.SendPropertyChanged("InterestCodeId");
					this.OnInterestCodeIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_VolInterestInterestCodes_VolInterest", Storage="_VolInterest", ThisKey="VolInterestId", IsForeignKey=true)]
		public VolInterest VolInterest
		{
			get { return this._VolInterest.Entity; }

			set
			{
				VolInterest previousValue = this._VolInterest.Entity;
				if (((previousValue != value) 
							|| (this._VolInterest.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._VolInterest.Entity = null;
						previousValue.VolInterestInterestCodes.Remove(this);
					}

					this._VolInterest.Entity = value;
					if (value != null)
					{
						value.VolInterestInterestCodes.Add(this);
						
						this._VolInterestId = value.Id;
						
					}

					else
					{
						
						this._VolInterestId = default(int);
						
					}

					this.SendPropertyChanged("VolInterest");
				}

			}

		}

		
		[Association(Name="FK_VolInterestInterestCodes_VolInterestCodes", Storage="_VolInterestCode", ThisKey="InterestCodeId", IsForeignKey=true)]
		public VolInterestCode VolInterestCode
		{
			get { return this._VolInterestCode.Entity; }

			set
			{
				VolInterestCode previousValue = this._VolInterestCode.Entity;
				if (((previousValue != value) 
							|| (this._VolInterestCode.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._VolInterestCode.Entity = null;
						previousValue.VolInterestInterestCodes.Remove(this);
					}

					this._VolInterestCode.Entity = value;
					if (value != null)
					{
						value.VolInterestInterestCodes.Add(this);
						
						this._InterestCodeId = value.Id;
						
					}

					else
					{
						
						this._InterestCodeId = default(int);
						
					}

					this.SendPropertyChanged("VolInterestCode");
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

