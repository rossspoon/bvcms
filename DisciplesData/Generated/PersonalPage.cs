using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData
{
	[Table(Name="dbo.PersonalPage")]
	public partial class PersonalPage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _UserId;
		
		private string _Picture;
		
		private DateTime? _Birthday;
		
		private string _Spouse;
		
		private string _Mobile;
		
		private string _Work;
		
		private string _Home;
		
		private string _Address;
		
		private string _Csz;
		
		private string _Activities;
		
		private string _Interests;
		
		private string _Music;
		
		private string _TVShows;
		
		private string _Employer;
		
		private string _WorkPosition;
		
		private string _WorkLocation;
		
		private string _WorkDescription;
		
		private bool? _IsPublic;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnUserIdChanging(string value);
		partial void OnUserIdChanged();
		
		partial void OnPictureChanging(string value);
		partial void OnPictureChanged();
		
		partial void OnBirthdayChanging(DateTime? value);
		partial void OnBirthdayChanged();
		
		partial void OnSpouseChanging(string value);
		partial void OnSpouseChanged();
		
		partial void OnMobileChanging(string value);
		partial void OnMobileChanged();
		
		partial void OnWorkChanging(string value);
		partial void OnWorkChanged();
		
		partial void OnHomeChanging(string value);
		partial void OnHomeChanged();
		
		partial void OnAddressChanging(string value);
		partial void OnAddressChanged();
		
		partial void OnCszChanging(string value);
		partial void OnCszChanged();
		
		partial void OnActivitiesChanging(string value);
		partial void OnActivitiesChanged();
		
		partial void OnInterestsChanging(string value);
		partial void OnInterestsChanged();
		
		partial void OnMusicChanging(string value);
		partial void OnMusicChanged();
		
		partial void OnTVShowsChanging(string value);
		partial void OnTVShowsChanged();
		
		partial void OnEmployerChanging(string value);
		partial void OnEmployerChanged();
		
		partial void OnWorkPositionChanging(string value);
		partial void OnWorkPositionChanged();
		
		partial void OnWorkLocationChanging(string value);
		partial void OnWorkLocationChanged();
		
		partial void OnWorkDescriptionChanging(string value);
		partial void OnWorkDescriptionChanged();
		
		partial void OnIsPublicChanging(bool? value);
		partial void OnIsPublicChanged();
		
    #endregion
		public PersonalPage()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", DbType="int NOT NULL", IsPrimaryKey=true)]
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

		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", DbType="varchar(50)")]
		public string UserId
		{
			get { return this._UserId; }

			set
			{
				if (this._UserId != value)
				{
				
                    this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}

			}

		}

		
		[Column(Name="Picture", UpdateCheck=UpdateCheck.Never, Storage="_Picture", DbType="varchar(50)")]
		public string Picture
		{
			get { return this._Picture; }

			set
			{
				if (this._Picture != value)
				{
				
                    this.OnPictureChanging(value);
					this.SendPropertyChanging();
					this._Picture = value;
					this.SendPropertyChanged("Picture");
					this.OnPictureChanged();
				}

			}

		}

		
		[Column(Name="Birthday", UpdateCheck=UpdateCheck.Never, Storage="_Birthday", DbType="datetime")]
		public DateTime? Birthday
		{
			get { return this._Birthday; }

			set
			{
				if (this._Birthday != value)
				{
				
                    this.OnBirthdayChanging(value);
					this.SendPropertyChanging();
					this._Birthday = value;
					this.SendPropertyChanged("Birthday");
					this.OnBirthdayChanged();
				}

			}

		}

		
		[Column(Name="Spouse", UpdateCheck=UpdateCheck.Never, Storage="_Spouse", DbType="varchar(50)")]
		public string Spouse
		{
			get { return this._Spouse; }

			set
			{
				if (this._Spouse != value)
				{
				
                    this.OnSpouseChanging(value);
					this.SendPropertyChanging();
					this._Spouse = value;
					this.SendPropertyChanged("Spouse");
					this.OnSpouseChanged();
				}

			}

		}

		
		[Column(Name="Mobile", UpdateCheck=UpdateCheck.Never, Storage="_Mobile", DbType="varchar(50)")]
		public string Mobile
		{
			get { return this._Mobile; }

			set
			{
				if (this._Mobile != value)
				{
				
                    this.OnMobileChanging(value);
					this.SendPropertyChanging();
					this._Mobile = value;
					this.SendPropertyChanged("Mobile");
					this.OnMobileChanged();
				}

			}

		}

		
		[Column(Name="Work", UpdateCheck=UpdateCheck.Never, Storage="_Work", DbType="varchar(50)")]
		public string Work
		{
			get { return this._Work; }

			set
			{
				if (this._Work != value)
				{
				
                    this.OnWorkChanging(value);
					this.SendPropertyChanging();
					this._Work = value;
					this.SendPropertyChanged("Work");
					this.OnWorkChanged();
				}

			}

		}

		
		[Column(Name="Home", UpdateCheck=UpdateCheck.Never, Storage="_Home", DbType="varchar(50)")]
		public string Home
		{
			get { return this._Home; }

			set
			{
				if (this._Home != value)
				{
				
                    this.OnHomeChanging(value);
					this.SendPropertyChanging();
					this._Home = value;
					this.SendPropertyChanged("Home");
					this.OnHomeChanged();
				}

			}

		}

		
		[Column(Name="Address", UpdateCheck=UpdateCheck.Never, Storage="_Address", DbType="varchar(80)")]
		public string Address
		{
			get { return this._Address; }

			set
			{
				if (this._Address != value)
				{
				
                    this.OnAddressChanging(value);
					this.SendPropertyChanging();
					this._Address = value;
					this.SendPropertyChanged("Address");
					this.OnAddressChanged();
				}

			}

		}

		
		[Column(Name="CSZ", UpdateCheck=UpdateCheck.Never, Storage="_Csz", DbType="varchar(50)")]
		public string Csz
		{
			get { return this._Csz; }

			set
			{
				if (this._Csz != value)
				{
				
                    this.OnCszChanging(value);
					this.SendPropertyChanging();
					this._Csz = value;
					this.SendPropertyChanged("Csz");
					this.OnCszChanged();
				}

			}

		}

		
		[Column(Name="Activities", UpdateCheck=UpdateCheck.Never, Storage="_Activities", DbType="varchar(500)")]
		public string Activities
		{
			get { return this._Activities; }

			set
			{
				if (this._Activities != value)
				{
				
                    this.OnActivitiesChanging(value);
					this.SendPropertyChanging();
					this._Activities = value;
					this.SendPropertyChanged("Activities");
					this.OnActivitiesChanged();
				}

			}

		}

		
		[Column(Name="Interests", UpdateCheck=UpdateCheck.Never, Storage="_Interests", DbType="varchar(500)")]
		public string Interests
		{
			get { return this._Interests; }

			set
			{
				if (this._Interests != value)
				{
				
                    this.OnInterestsChanging(value);
					this.SendPropertyChanging();
					this._Interests = value;
					this.SendPropertyChanged("Interests");
					this.OnInterestsChanged();
				}

			}

		}

		
		[Column(Name="Music", UpdateCheck=UpdateCheck.Never, Storage="_Music", DbType="varchar(500)")]
		public string Music
		{
			get { return this._Music; }

			set
			{
				if (this._Music != value)
				{
				
                    this.OnMusicChanging(value);
					this.SendPropertyChanging();
					this._Music = value;
					this.SendPropertyChanged("Music");
					this.OnMusicChanged();
				}

			}

		}

		
		[Column(Name="TVShows", UpdateCheck=UpdateCheck.Never, Storage="_TVShows", DbType="varchar(500)")]
		public string TVShows
		{
			get { return this._TVShows; }

			set
			{
				if (this._TVShows != value)
				{
				
                    this.OnTVShowsChanging(value);
					this.SendPropertyChanging();
					this._TVShows = value;
					this.SendPropertyChanged("TVShows");
					this.OnTVShowsChanged();
				}

			}

		}

		
		[Column(Name="Employer", UpdateCheck=UpdateCheck.Never, Storage="_Employer", DbType="varchar(50)")]
		public string Employer
		{
			get { return this._Employer; }

			set
			{
				if (this._Employer != value)
				{
				
                    this.OnEmployerChanging(value);
					this.SendPropertyChanging();
					this._Employer = value;
					this.SendPropertyChanged("Employer");
					this.OnEmployerChanged();
				}

			}

		}

		
		[Column(Name="WorkPosition", UpdateCheck=UpdateCheck.Never, Storage="_WorkPosition", DbType="varchar(50)")]
		public string WorkPosition
		{
			get { return this._WorkPosition; }

			set
			{
				if (this._WorkPosition != value)
				{
				
                    this.OnWorkPositionChanging(value);
					this.SendPropertyChanging();
					this._WorkPosition = value;
					this.SendPropertyChanged("WorkPosition");
					this.OnWorkPositionChanged();
				}

			}

		}

		
		[Column(Name="WorkLocation", UpdateCheck=UpdateCheck.Never, Storage="_WorkLocation", DbType="varchar(100)")]
		public string WorkLocation
		{
			get { return this._WorkLocation; }

			set
			{
				if (this._WorkLocation != value)
				{
				
                    this.OnWorkLocationChanging(value);
					this.SendPropertyChanging();
					this._WorkLocation = value;
					this.SendPropertyChanged("WorkLocation");
					this.OnWorkLocationChanged();
				}

			}

		}

		
		[Column(Name="WorkDescription", UpdateCheck=UpdateCheck.Never, Storage="_WorkDescription", DbType="varchar(100)")]
		public string WorkDescription
		{
			get { return this._WorkDescription; }

			set
			{
				if (this._WorkDescription != value)
				{
				
                    this.OnWorkDescriptionChanging(value);
					this.SendPropertyChanging();
					this._WorkDescription = value;
					this.SendPropertyChanged("WorkDescription");
					this.OnWorkDescriptionChanged();
				}

			}

		}

		
		[Column(Name="IsPublic", UpdateCheck=UpdateCheck.Never, Storage="_IsPublic", DbType="bit")]
		public bool? IsPublic
		{
			get { return this._IsPublic; }

			set
			{
				if (this._IsPublic != value)
				{
				
                    this.OnIsPublicChanging(value);
					this.SendPropertyChanging();
					this._IsPublic = value;
					this.SendPropertyChanged("IsPublic");
					this.OnIsPublicChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
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

   		
	}

}

