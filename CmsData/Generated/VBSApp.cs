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
	[Table(Name="dbo.VBSApp")]
	public partial class VBSApp : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _PeopleId;
		
		private int? _ImgId;
		
		private bool? _IsDocument;
		
		private DateTime? _Uploaded;
		
		private string _Request;
		
		private bool? _ActiveInAnotherChurch;
		
		private string _GradeCompleted;
		
		private int? _OrgId;
		
		private int? _DivId;
		
		private bool? _Inactive;
		
		private bool? _PubPhoto;
		
		private string _UserInfo;
		
		private bool? _MedAllergy;
		
   		
    	
		private EntityRef< Organization> _Organization;
		
		private EntityRef< Person> _Person;
		
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
		
		partial void OnUploadedChanging(DateTime? value);
		partial void OnUploadedChanged();
		
		partial void OnRequestChanging(string value);
		partial void OnRequestChanged();
		
		partial void OnActiveInAnotherChurchChanging(bool? value);
		partial void OnActiveInAnotherChurchChanged();
		
		partial void OnGradeCompletedChanging(string value);
		partial void OnGradeCompletedChanged();
		
		partial void OnOrgIdChanging(int? value);
		partial void OnOrgIdChanged();
		
		partial void OnDivIdChanging(int? value);
		partial void OnDivIdChanged();
		
		partial void OnInactiveChanging(bool? value);
		partial void OnInactiveChanged();
		
		partial void OnPubPhotoChanging(bool? value);
		partial void OnPubPhotoChanged();
		
		partial void OnUserInfoChanging(string value);
		partial void OnUserInfoChanged();
		
		partial void OnMedAllergyChanging(bool? value);
		partial void OnMedAllergyChanged();
		
    #endregion
		public VBSApp()
		{
			
			
			this._Organization = default(EntityRef< Organization>); 
			
			this._Person = default(EntityRef< Person>); 
			
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

		
		[Column(Name="Uploaded", UpdateCheck=UpdateCheck.Never, Storage="_Uploaded", DbType="datetime")]
		public DateTime? Uploaded
		{
			get { return this._Uploaded; }

			set
			{
				if (this._Uploaded != value)
				{
				
                    this.OnUploadedChanging(value);
					this.SendPropertyChanging();
					this._Uploaded = value;
					this.SendPropertyChanged("Uploaded");
					this.OnUploadedChanged();
				}

			}

		}

		
		[Column(Name="Request", UpdateCheck=UpdateCheck.Never, Storage="_Request", DbType="varchar(140)")]
		public string Request
		{
			get { return this._Request; }

			set
			{
				if (this._Request != value)
				{
				
                    this.OnRequestChanging(value);
					this.SendPropertyChanging();
					this._Request = value;
					this.SendPropertyChanged("Request");
					this.OnRequestChanged();
				}

			}

		}

		
		[Column(Name="ActiveInAnotherChurch", UpdateCheck=UpdateCheck.Never, Storage="_ActiveInAnotherChurch", DbType="bit")]
		public bool? ActiveInAnotherChurch
		{
			get { return this._ActiveInAnotherChurch; }

			set
			{
				if (this._ActiveInAnotherChurch != value)
				{
				
                    this.OnActiveInAnotherChurchChanging(value);
					this.SendPropertyChanging();
					this._ActiveInAnotherChurch = value;
					this.SendPropertyChanged("ActiveInAnotherChurch");
					this.OnActiveInAnotherChurchChanged();
				}

			}

		}

		
		[Column(Name="GradeCompleted", UpdateCheck=UpdateCheck.Never, Storage="_GradeCompleted", DbType="varchar(15)")]
		public string GradeCompleted
		{
			get { return this._GradeCompleted; }

			set
			{
				if (this._GradeCompleted != value)
				{
				
                    this.OnGradeCompletedChanging(value);
					this.SendPropertyChanging();
					this._GradeCompleted = value;
					this.SendPropertyChanged("GradeCompleted");
					this.OnGradeCompletedChanged();
				}

			}

		}

		
		[Column(Name="OrgId", UpdateCheck=UpdateCheck.Never, Storage="_OrgId", DbType="int")]
		public int? OrgId
		{
			get { return this._OrgId; }

			set
			{
				if (this._OrgId != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrgIdChanging(value);
					this.SendPropertyChanging();
					this._OrgId = value;
					this.SendPropertyChanged("OrgId");
					this.OnOrgIdChanged();
				}

			}

		}

		
		[Column(Name="DivId", UpdateCheck=UpdateCheck.Never, Storage="_DivId", DbType="int")]
		public int? DivId
		{
			get { return this._DivId; }

			set
			{
				if (this._DivId != value)
				{
				
                    this.OnDivIdChanging(value);
					this.SendPropertyChanging();
					this._DivId = value;
					this.SendPropertyChanged("DivId");
					this.OnDivIdChanged();
				}

			}

		}

		
		[Column(Name="Inactive", UpdateCheck=UpdateCheck.Never, Storage="_Inactive", DbType="bit")]
		public bool? Inactive
		{
			get { return this._Inactive; }

			set
			{
				if (this._Inactive != value)
				{
				
                    this.OnInactiveChanging(value);
					this.SendPropertyChanging();
					this._Inactive = value;
					this.SendPropertyChanged("Inactive");
					this.OnInactiveChanged();
				}

			}

		}

		
		[Column(Name="PubPhoto", UpdateCheck=UpdateCheck.Never, Storage="_PubPhoto", DbType="bit")]
		public bool? PubPhoto
		{
			get { return this._PubPhoto; }

			set
			{
				if (this._PubPhoto != value)
				{
				
                    this.OnPubPhotoChanging(value);
					this.SendPropertyChanging();
					this._PubPhoto = value;
					this.SendPropertyChanged("PubPhoto");
					this.OnPubPhotoChanged();
				}

			}

		}

		
		[Column(Name="UserInfo", UpdateCheck=UpdateCheck.Never, Storage="_UserInfo", DbType="varchar(15)")]
		public string UserInfo
		{
			get { return this._UserInfo; }

			set
			{
				if (this._UserInfo != value)
				{
				
                    this.OnUserInfoChanging(value);
					this.SendPropertyChanging();
					this._UserInfo = value;
					this.SendPropertyChanged("UserInfo");
					this.OnUserInfoChanged();
				}

			}

		}

		
		[Column(Name="MedAllergy", UpdateCheck=UpdateCheck.Never, Storage="_MedAllergy", DbType="bit")]
		public bool? MedAllergy
		{
			get { return this._MedAllergy; }

			set
			{
				if (this._MedAllergy != value)
				{
				
                    this.OnMedAllergyChanging(value);
					this.SendPropertyChanging();
					this._MedAllergy = value;
					this.SendPropertyChanged("MedAllergy");
					this.OnMedAllergyChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_VBSApp_Organizations", Storage="_Organization", ThisKey="OrgId", IsForeignKey=true)]
		public Organization Organization
		{
			get { return this._Organization.Entity; }

			set
			{
				Organization previousValue = this._Organization.Entity;
				if (((previousValue != value) 
							|| (this._Organization.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Organization.Entity = null;
						previousValue.VBSApps.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.VBSApps.Add(this);
						
						this._OrgId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrgId = default(int?);
						
					}

					this.SendPropertyChanged("Organization");
				}

			}

		}

		
		[Association(Name="FK_VBSApp_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.VBSApps.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.VBSApps.Add(this);
						
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

