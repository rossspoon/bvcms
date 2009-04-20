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
	[Table(Name="CMS_ENROLL.PROMOTION_REF_TBL")]
	public partial class PromotionRef : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PromotionId;
		
		private int _PromotionControlId;
		
		private int _ScheduleId;
		
		private int? _ExistingOrgId;
		
		private int _NewOrgId;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnPromotionIdChanging(int value);
		partial void OnPromotionIdChanged();
		
		partial void OnPromotionControlIdChanging(int value);
		partial void OnPromotionControlIdChanged();
		
		partial void OnScheduleIdChanging(int value);
		partial void OnScheduleIdChanged();
		
		partial void OnExistingOrgIdChanging(int? value);
		partial void OnExistingOrgIdChanged();
		
		partial void OnNewOrgIdChanging(int value);
		partial void OnNewOrgIdChanged();
		
    #endregion
		public PromotionRef()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="PROMOTION_ID", UpdateCheck=UpdateCheck.Never, Storage="_PromotionId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int PromotionId
		{
			get { return this._PromotionId; }

			set
			{
				if (this._PromotionId != value)
				{
				
                    this.OnPromotionIdChanging(value);
					this.SendPropertyChanging();
					this._PromotionId = value;
					this.SendPropertyChanged("PromotionId");
					this.OnPromotionIdChanged();
				}

			}

		}

		
		[Column(Name="PROMOTION_CONTROL_ID", UpdateCheck=UpdateCheck.Never, Storage="_PromotionControlId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int PromotionControlId
		{
			get { return this._PromotionControlId; }

			set
			{
				if (this._PromotionControlId != value)
				{
				
                    this.OnPromotionControlIdChanging(value);
					this.SendPropertyChanging();
					this._PromotionControlId = value;
					this.SendPropertyChanged("PromotionControlId");
					this.OnPromotionControlIdChanged();
				}

			}

		}

		
		[Column(Name="SCHEDULE_ID", UpdateCheck=UpdateCheck.Never, Storage="_ScheduleId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int ScheduleId
		{
			get { return this._ScheduleId; }

			set
			{
				if (this._ScheduleId != value)
				{
				
                    this.OnScheduleIdChanging(value);
					this.SendPropertyChanging();
					this._ScheduleId = value;
					this.SendPropertyChanged("ScheduleId");
					this.OnScheduleIdChanged();
				}

			}

		}

		
		[Column(Name="EXISTING_ORG_ID", UpdateCheck=UpdateCheck.Never, Storage="_ExistingOrgId", DbType="int")]
		public int? ExistingOrgId
		{
			get { return this._ExistingOrgId; }

			set
			{
				if (this._ExistingOrgId != value)
				{
				
                    this.OnExistingOrgIdChanging(value);
					this.SendPropertyChanging();
					this._ExistingOrgId = value;
					this.SendPropertyChanged("ExistingOrgId");
					this.OnExistingOrgIdChanged();
				}

			}

		}

		
		[Column(Name="NEW_ORG_ID", UpdateCheck=UpdateCheck.Never, Storage="_NewOrgId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int NewOrgId
		{
			get { return this._NewOrgId; }

			set
			{
				if (this._NewOrgId != value)
				{
				
                    this.OnNewOrgIdChanging(value);
					this.SendPropertyChanging();
					this._NewOrgId = value;
					this.SendPropertyChanged("NewOrgId");
					this.OnNewOrgIdChanged();
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

