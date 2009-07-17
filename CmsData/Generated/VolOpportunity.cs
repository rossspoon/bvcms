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
	[Table(Name="dbo.VolOpportunity")]
	public partial class VolOpportunity : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Description;
		
		private string _EmailNoCva;
		
		private string _EmailYesCva;
		
		private string _Email;
		
		private string _UrlKey;
		
		private string _ExtraQuestion;
		
   		
   		private EntitySet< VolInterest> _VolInterests;
		
   		private EntitySet< VolInterestCode> _VolInterestCodes;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnEmailNoCvaChanging(string value);
		partial void OnEmailNoCvaChanged();
		
		partial void OnEmailYesCvaChanging(string value);
		partial void OnEmailYesCvaChanged();
		
		partial void OnEmailChanging(string value);
		partial void OnEmailChanged();
		
		partial void OnUrlKeyChanging(string value);
		partial void OnUrlKeyChanged();
		
		partial void OnExtraQuestionChanging(string value);
		partial void OnExtraQuestionChanged();
		
    #endregion
		public VolOpportunity()
		{
			
			this._VolInterests = new EntitySet< VolInterest>(new Action< VolInterest>(this.attach_VolInterests), new Action< VolInterest>(this.detach_VolInterests)); 
			
			this._VolInterestCodes = new EntitySet< VolInterestCode>(new Action< VolInterestCode>(this.attach_VolInterestCodes), new Action< VolInterestCode>(this.detach_VolInterestCodes)); 
			
			
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

		
		[Column(Name="Description", UpdateCheck=UpdateCheck.Never, Storage="_Description", DbType="varchar(50)")]
		public string Description
		{
			get { return this._Description; }

			set
			{
				if (this._Description != value)
				{
				
                    this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}

			}

		}

		
		[Column(Name="EmailNoCva", UpdateCheck=UpdateCheck.Never, Storage="_EmailNoCva", DbType="varchar(2000)")]
		public string EmailNoCva
		{
			get { return this._EmailNoCva; }

			set
			{
				if (this._EmailNoCva != value)
				{
				
                    this.OnEmailNoCvaChanging(value);
					this.SendPropertyChanging();
					this._EmailNoCva = value;
					this.SendPropertyChanged("EmailNoCva");
					this.OnEmailNoCvaChanged();
				}

			}

		}

		
		[Column(Name="EmailYesCva", UpdateCheck=UpdateCheck.Never, Storage="_EmailYesCva", DbType="varchar(2000)")]
		public string EmailYesCva
		{
			get { return this._EmailYesCva; }

			set
			{
				if (this._EmailYesCva != value)
				{
				
                    this.OnEmailYesCvaChanging(value);
					this.SendPropertyChanging();
					this._EmailYesCva = value;
					this.SendPropertyChanged("EmailYesCva");
					this.OnEmailYesCvaChanged();
				}

			}

		}

		
		[Column(Name="email", UpdateCheck=UpdateCheck.Never, Storage="_Email", DbType="varchar(50)")]
		public string Email
		{
			get { return this._Email; }

			set
			{
				if (this._Email != value)
				{
				
                    this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}

			}

		}

		
		[Column(Name="UrlKey", UpdateCheck=UpdateCheck.Never, Storage="_UrlKey", DbType="varchar(15)")]
		public string UrlKey
		{
			get { return this._UrlKey; }

			set
			{
				if (this._UrlKey != value)
				{
				
                    this.OnUrlKeyChanging(value);
					this.SendPropertyChanging();
					this._UrlKey = value;
					this.SendPropertyChanged("UrlKey");
					this.OnUrlKeyChanged();
				}

			}

		}

		
		[Column(Name="ExtraQuestion", UpdateCheck=UpdateCheck.Never, Storage="_ExtraQuestion", DbType="varchar(80)")]
		public string ExtraQuestion
		{
			get { return this._ExtraQuestion; }

			set
			{
				if (this._ExtraQuestion != value)
				{
				
                    this.OnExtraQuestionChanging(value);
					this.SendPropertyChanging();
					this._ExtraQuestion = value;
					this.SendPropertyChanged("ExtraQuestion");
					this.OnExtraQuestionChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_VolInterest_VolOpportunity", Storage="_VolInterests", OtherKey="OpportunityCode")]
   		public EntitySet< VolInterest> VolInterests
   		{
   		    get { return this._VolInterests; }

			set	{ this._VolInterests.Assign(value); }

   		}

		
   		[Association(Name="FK_VolInterestCodes_VolOpportunity", Storage="_VolInterestCodes", OtherKey="OpportunityId")]
   		public EntitySet< VolInterestCode> VolInterestCodes
   		{
   		    get { return this._VolInterestCodes; }

			set	{ this._VolInterestCodes.Assign(value); }

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

   		
		private void attach_VolInterests(VolInterest entity)
		{
			this.SendPropertyChanging();
			entity.VolOpportunity = this;
		}

		private void detach_VolInterests(VolInterest entity)
		{
			this.SendPropertyChanging();
			entity.VolOpportunity = null;
		}

		
		private void attach_VolInterestCodes(VolInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.VolOpportunity = this;
		}

		private void detach_VolInterestCodes(VolInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.VolOpportunity = null;
		}

		
	}

}

