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
	[Table(Name="dbo.RecLeague")]
	public partial class RecLeague : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _DivId;
		
		private string _AgeDate;
		
		private decimal? _ExtraFee;
		
		private string _ExpirationDt;
		
		private decimal? _ShirtFee;
		
		private string _EmailMessage;
		
		private string _EmailSubject;
		
		private string _EmailAddresses;
		
   		
    	
		private EntityRef< Division> _Division;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnDivIdChanging(int value);
		partial void OnDivIdChanged();
		
		partial void OnAgeDateChanging(string value);
		partial void OnAgeDateChanged();
		
		partial void OnExtraFeeChanging(decimal? value);
		partial void OnExtraFeeChanged();
		
		partial void OnExpirationDtChanging(string value);
		partial void OnExpirationDtChanged();
		
		partial void OnShirtFeeChanging(decimal? value);
		partial void OnShirtFeeChanged();
		
		partial void OnEmailMessageChanging(string value);
		partial void OnEmailMessageChanged();
		
		partial void OnEmailSubjectChanging(string value);
		partial void OnEmailSubjectChanged();
		
		partial void OnEmailAddressesChanging(string value);
		partial void OnEmailAddressesChanged();
		
    #endregion
		public RecLeague()
		{
			
			
			this._Division = default(EntityRef< Division>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="DivId", UpdateCheck=UpdateCheck.Never, Storage="_DivId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int DivId
		{
			get { return this._DivId; }

			set
			{
				if (this._DivId != value)
				{
				
					if (this._Division.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnDivIdChanging(value);
					this.SendPropertyChanging();
					this._DivId = value;
					this.SendPropertyChanged("DivId");
					this.OnDivIdChanged();
				}

			}

		}

		
		[Column(Name="AgeDate", UpdateCheck=UpdateCheck.Never, Storage="_AgeDate", DbType="varchar(50)")]
		public string AgeDate
		{
			get { return this._AgeDate; }

			set
			{
				if (this._AgeDate != value)
				{
				
                    this.OnAgeDateChanging(value);
					this.SendPropertyChanging();
					this._AgeDate = value;
					this.SendPropertyChanged("AgeDate");
					this.OnAgeDateChanged();
				}

			}

		}

		
		[Column(Name="ExtraFee", UpdateCheck=UpdateCheck.Never, Storage="_ExtraFee", DbType="money")]
		public decimal? ExtraFee
		{
			get { return this._ExtraFee; }

			set
			{
				if (this._ExtraFee != value)
				{
				
                    this.OnExtraFeeChanging(value);
					this.SendPropertyChanging();
					this._ExtraFee = value;
					this.SendPropertyChanged("ExtraFee");
					this.OnExtraFeeChanged();
				}

			}

		}

		
		[Column(Name="ExpirationDt", UpdateCheck=UpdateCheck.Never, Storage="_ExpirationDt", DbType="varchar(50)")]
		public string ExpirationDt
		{
			get { return this._ExpirationDt; }

			set
			{
				if (this._ExpirationDt != value)
				{
				
                    this.OnExpirationDtChanging(value);
					this.SendPropertyChanging();
					this._ExpirationDt = value;
					this.SendPropertyChanged("ExpirationDt");
					this.OnExpirationDtChanged();
				}

			}

		}

		
		[Column(Name="ShirtFee", UpdateCheck=UpdateCheck.Never, Storage="_ShirtFee", DbType="money")]
		public decimal? ShirtFee
		{
			get { return this._ShirtFee; }

			set
			{
				if (this._ShirtFee != value)
				{
				
                    this.OnShirtFeeChanging(value);
					this.SendPropertyChanging();
					this._ShirtFee = value;
					this.SendPropertyChanged("ShirtFee");
					this.OnShirtFeeChanged();
				}

			}

		}

		
		[Column(Name="EmailMessage", UpdateCheck=UpdateCheck.Never, Storage="_EmailMessage", DbType="varchar")]
		public string EmailMessage
		{
			get { return this._EmailMessage; }

			set
			{
				if (this._EmailMessage != value)
				{
				
                    this.OnEmailMessageChanging(value);
					this.SendPropertyChanging();
					this._EmailMessage = value;
					this.SendPropertyChanged("EmailMessage");
					this.OnEmailMessageChanged();
				}

			}

		}

		
		[Column(Name="EmailSubject", UpdateCheck=UpdateCheck.Never, Storage="_EmailSubject", DbType="varchar(50)")]
		public string EmailSubject
		{
			get { return this._EmailSubject; }

			set
			{
				if (this._EmailSubject != value)
				{
				
                    this.OnEmailSubjectChanging(value);
					this.SendPropertyChanging();
					this._EmailSubject = value;
					this.SendPropertyChanged("EmailSubject");
					this.OnEmailSubjectChanged();
				}

			}

		}

		
		[Column(Name="EmailAddresses", UpdateCheck=UpdateCheck.Never, Storage="_EmailAddresses", DbType="varchar(150)")]
		public string EmailAddresses
		{
			get { return this._EmailAddresses; }

			set
			{
				if (this._EmailAddresses != value)
				{
				
                    this.OnEmailAddressesChanging(value);
					this.SendPropertyChanging();
					this._EmailAddresses = value;
					this.SendPropertyChanged("EmailAddresses");
					this.OnEmailAddressesChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_RecLeague_Division", Storage="_Division", ThisKey="DivId", IsForeignKey=true)]
		public Division Division
		{
			get { return this._Division.Entity; }

			set
			{
				Division previousValue = this._Division.Entity;
				if (((previousValue != value) 
							|| (this._Division.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Division.Entity = null;
						previousValue.RecLeagues.Remove(this);
					}

					this._Division.Entity = value;
					if (value != null)
					{
						value.RecLeagues.Add(this);
						
						this._DivId = value.Id;
						
					}

					else
					{
						
						this._DivId = default(int);
						
					}

					this.SendPropertyChanged("Division");
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

