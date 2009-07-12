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
	[Table(Name="dbo.VolInterest")]
	public partial class VolInterest : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _PeopleId;
		
		private int? _ImgId;
		
		private bool? _IsDocument;
		
		private DateTime? _Created;
		
		private int? _OpportunityCode;
		
		private string _Question;
		
   		
   		private EntitySet< VolInterestInterestCode> _VolInterestInterestCodes;
		
    	
		private EntityRef< Person> _Person;
		
		private EntityRef< VolOpportunity> _VolOpportunity;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnImgIdChanging(int? value);
		partial void OnImgIdChanged();
		
		partial void OnIsDocumentChanging(bool? value);
		partial void OnIsDocumentChanged();
		
		partial void OnCreatedChanging(DateTime? value);
		partial void OnCreatedChanged();
		
		partial void OnOpportunityCodeChanging(int? value);
		partial void OnOpportunityCodeChanged();
		
		partial void OnQuestionChanging(string value);
		partial void OnQuestionChanged();
		
    #endregion
		public VolInterest()
		{
			
			this._VolInterestInterestCodes = new EntitySet< VolInterestInterestCode>(new Action< VolInterestInterestCode>(this.attach_VolInterestInterestCodes), new Action< VolInterestInterestCode>(this.detach_VolInterestInterestCodes)); 
			
			
			this._Person = default(EntityRef< Person>); 
			
			this._VolOpportunity = default(EntityRef< VolOpportunity>); 
			
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

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="ImgId", UpdateCheck=UpdateCheck.Never, Storage="_ImgId", DbType="int")]
		public int? ImgId
		{
			get { return this._ImgId; }

			set
			{
				if (this._ImgId != value)
				{
				
                    this.OnImgIdChanging(value);
					this.SendPropertyChanging();
					this._ImgId = value;
					this.SendPropertyChanged("ImgId");
					this.OnImgIdChanged();
				}

			}

		}

		
		[Column(Name="IsDocument", UpdateCheck=UpdateCheck.Never, Storage="_IsDocument", DbType="bit")]
		public bool? IsDocument
		{
			get { return this._IsDocument; }

			set
			{
				if (this._IsDocument != value)
				{
				
                    this.OnIsDocumentChanging(value);
					this.SendPropertyChanging();
					this._IsDocument = value;
					this.SendPropertyChanged("IsDocument");
					this.OnIsDocumentChanged();
				}

			}

		}

		
		[Column(Name="Created", UpdateCheck=UpdateCheck.Never, Storage="_Created", DbType="datetime")]
		public DateTime? Created
		{
			get { return this._Created; }

			set
			{
				if (this._Created != value)
				{
				
                    this.OnCreatedChanging(value);
					this.SendPropertyChanging();
					this._Created = value;
					this.SendPropertyChanged("Created");
					this.OnCreatedChanged();
				}

			}

		}

		
		[Column(Name="OpportunityCode", UpdateCheck=UpdateCheck.Never, Storage="_OpportunityCode", DbType="int")]
		public int? OpportunityCode
		{
			get { return this._OpportunityCode; }

			set
			{
				if (this._OpportunityCode != value)
				{
				
					if (this._VolOpportunity.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOpportunityCodeChanging(value);
					this.SendPropertyChanging();
					this._OpportunityCode = value;
					this.SendPropertyChanged("OpportunityCode");
					this.OnOpportunityCodeChanged();
				}

			}

		}

		
		[Column(Name="question", UpdateCheck=UpdateCheck.Never, Storage="_Question", DbType="varchar(50)")]
		public string Question
		{
			get { return this._Question; }

			set
			{
				if (this._Question != value)
				{
				
                    this.OnQuestionChanging(value);
					this.SendPropertyChanging();
					this._Question = value;
					this.SendPropertyChanged("Question");
					this.OnQuestionChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_VolInterestInterestCodes_VolInterest", Storage="_VolInterestInterestCodes", OtherKey="VolInterestId")]
   		public EntitySet< VolInterestInterestCode> VolInterestInterestCodes
   		{
   		    get { return this._VolInterestInterestCodes; }

			set	{ this._VolInterestInterestCodes.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_VolInterest_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
		public Person Person
		{
			get { return this._Person.Entity; }

			set
			{
				Person previousValue = this._Person.Entity;
				if (((previousValue != value) 
							|| (this._Person.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Person.Entity = null;
						previousValue.VolInterests.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.VolInterests.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int?);
						
					}

					this.SendPropertyChanged("Person");
				}

			}

		}

		
		[Association(Name="FK_VolInterest_VolOpportunity", Storage="_VolOpportunity", ThisKey="OpportunityCode", IsForeignKey=true)]
		public VolOpportunity VolOpportunity
		{
			get { return this._VolOpportunity.Entity; }

			set
			{
				VolOpportunity previousValue = this._VolOpportunity.Entity;
				if (((previousValue != value) 
							|| (this._VolOpportunity.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._VolOpportunity.Entity = null;
						previousValue.VolInterests.Remove(this);
					}

					this._VolOpportunity.Entity = value;
					if (value != null)
					{
						value.VolInterests.Add(this);
						
						this._OpportunityCode = value.Id;
						
					}

					else
					{
						
						this._OpportunityCode = default(int?);
						
					}

					this.SendPropertyChanged("VolOpportunity");
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

   		
		private void attach_VolInterestInterestCodes(VolInterestInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.VolInterest = this;
		}

		private void detach_VolInterestInterestCodes(VolInterestInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.VolInterest = null;
		}

		
	}

}

