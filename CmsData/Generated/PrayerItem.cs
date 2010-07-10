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
	[Table(Name="dbo.PrayerItem")]
	public partial class PrayerItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _PeopleId;
		
		private string _Text;
		
		private int? _Type;
		
		private int? _Link;
		
		private DateTime? _Submitted;
		
		private int? _Approved;
		
		private int? _TimesPrayedFor;
		
		private int? _Visibility;
		
		private DateTime? _Expires;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnTextChanging(string value);
		partial void OnTextChanged();
		
		partial void OnTypeChanging(int? value);
		partial void OnTypeChanged();
		
		partial void OnLinkChanging(int? value);
		partial void OnLinkChanged();
		
		partial void OnSubmittedChanging(DateTime? value);
		partial void OnSubmittedChanged();
		
		partial void OnApprovedChanging(int? value);
		partial void OnApprovedChanged();
		
		partial void OnTimesPrayedForChanging(int? value);
		partial void OnTimesPrayedForChanged();
		
		partial void OnVisibilityChanging(int? value);
		partial void OnVisibilityChanged();
		
		partial void OnExpiresChanging(DateTime? value);
		partial void OnExpiresChanged();
		
    #endregion
		public PrayerItem()
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

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="Text", UpdateCheck=UpdateCheck.Never, Storage="_Text", DbType="varchar(500)")]
		public string Text
		{
			get { return this._Text; }

			set
			{
				if (this._Text != value)
				{
				
                    this.OnTextChanging(value);
					this.SendPropertyChanging();
					this._Text = value;
					this.SendPropertyChanged("Text");
					this.OnTextChanged();
				}

			}

		}

		
		[Column(Name="Type", UpdateCheck=UpdateCheck.Never, Storage="_Type", DbType="int")]
		public int? Type
		{
			get { return this._Type; }

			set
			{
				if (this._Type != value)
				{
				
                    this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}

			}

		}

		
		[Column(Name="Link", UpdateCheck=UpdateCheck.Never, Storage="_Link", DbType="int")]
		public int? Link
		{
			get { return this._Link; }

			set
			{
				if (this._Link != value)
				{
				
                    this.OnLinkChanging(value);
					this.SendPropertyChanging();
					this._Link = value;
					this.SendPropertyChanged("Link");
					this.OnLinkChanged();
				}

			}

		}

		
		[Column(Name="Submitted", UpdateCheck=UpdateCheck.Never, Storage="_Submitted", DbType="datetime")]
		public DateTime? Submitted
		{
			get { return this._Submitted; }

			set
			{
				if (this._Submitted != value)
				{
				
                    this.OnSubmittedChanging(value);
					this.SendPropertyChanging();
					this._Submitted = value;
					this.SendPropertyChanged("Submitted");
					this.OnSubmittedChanged();
				}

			}

		}

		
		[Column(Name="Approved", UpdateCheck=UpdateCheck.Never, Storage="_Approved", DbType="int")]
		public int? Approved
		{
			get { return this._Approved; }

			set
			{
				if (this._Approved != value)
				{
				
                    this.OnApprovedChanging(value);
					this.SendPropertyChanging();
					this._Approved = value;
					this.SendPropertyChanged("Approved");
					this.OnApprovedChanged();
				}

			}

		}

		
		[Column(Name="TimesPrayedFor", UpdateCheck=UpdateCheck.Never, Storage="_TimesPrayedFor", DbType="int")]
		public int? TimesPrayedFor
		{
			get { return this._TimesPrayedFor; }

			set
			{
				if (this._TimesPrayedFor != value)
				{
				
                    this.OnTimesPrayedForChanging(value);
					this.SendPropertyChanging();
					this._TimesPrayedFor = value;
					this.SendPropertyChanged("TimesPrayedFor");
					this.OnTimesPrayedForChanged();
				}

			}

		}

		
		[Column(Name="Visibility", UpdateCheck=UpdateCheck.Never, Storage="_Visibility", DbType="int")]
		public int? Visibility
		{
			get { return this._Visibility; }

			set
			{
				if (this._Visibility != value)
				{
				
                    this.OnVisibilityChanging(value);
					this.SendPropertyChanging();
					this._Visibility = value;
					this.SendPropertyChanged("Visibility");
					this.OnVisibilityChanged();
				}

			}

		}

		
		[Column(Name="Expires", UpdateCheck=UpdateCheck.Never, Storage="_Expires", DbType="datetime")]
		public DateTime? Expires
		{
			get { return this._Expires; }

			set
			{
				if (this._Expires != value)
				{
				
                    this.OnExpiresChanging(value);
					this.SendPropertyChanging();
					this._Expires = value;
					this.SendPropertyChanged("Expires");
					this.OnExpiresChanged();
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

