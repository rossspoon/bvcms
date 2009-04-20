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
	[Table(Name="dbo.PromotionControl")]
	public partial class PromotionControl : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PromotionId;
		
		private int _PromotionControlId;
		
		private int _ScheduleId;
		
		private int _CreatedBy;
		
		private DateTime _CreatedDate;
		
		private bool _RecordStatus;
		
		private int _DivisionId;
		
		private int _GenderControlId;
		
		private bool _TeacherControl;
		
		private bool _MixControl;
		
		private int _NbrOfClasses;
		
		private int _NbrOfFemaleClasses;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
		private int? _OldDiisionId;
		
   		
    	
		private EntityRef< Promotion> _Promotion;
		
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
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		
		partial void OnRecordStatusChanging(bool value);
		partial void OnRecordStatusChanged();
		
		partial void OnDivisionIdChanging(int value);
		partial void OnDivisionIdChanged();
		
		partial void OnGenderControlIdChanging(int value);
		partial void OnGenderControlIdChanged();
		
		partial void OnTeacherControlChanging(bool value);
		partial void OnTeacherControlChanged();
		
		partial void OnMixControlChanging(bool value);
		partial void OnMixControlChanged();
		
		partial void OnNbrOfClassesChanging(int value);
		partial void OnNbrOfClassesChanged();
		
		partial void OnNbrOfFemaleClassesChanging(int value);
		partial void OnNbrOfFemaleClassesChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
		partial void OnOldDiisionIdChanging(int? value);
		partial void OnOldDiisionIdChanged();
		
    #endregion
		public PromotionControl()
		{
			
			
			this._Promotion = default(EntityRef< Promotion>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="PromotionId", UpdateCheck=UpdateCheck.Never, Storage="_PromotionId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int PromotionId
		{
			get { return this._PromotionId; }

			set
			{
				if (this._PromotionId != value)
				{
				
					if (this._Promotion.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPromotionIdChanging(value);
					this.SendPropertyChanging();
					this._PromotionId = value;
					this.SendPropertyChanged("PromotionId");
					this.OnPromotionIdChanged();
				}

			}

		}

		
		[Column(Name="PromotionControlId", UpdateCheck=UpdateCheck.Never, Storage="_PromotionControlId", DbType="int NOT NULL", IsPrimaryKey=true)]
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

		
		[Column(Name="ScheduleId", UpdateCheck=UpdateCheck.Never, Storage="_ScheduleId", DbType="int NOT NULL", IsPrimaryKey=true)]
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

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int NOT NULL")]
		public int CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
		[Column(Name="CreatedDate", UpdateCheck=UpdateCheck.Never, Storage="_CreatedDate", DbType="datetime NOT NULL")]
		public DateTime CreatedDate
		{
			get { return this._CreatedDate; }

			set
			{
				if (this._CreatedDate != value)
				{
				
                    this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._CreatedDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}

			}

		}

		
		[Column(Name="RecordStatus", UpdateCheck=UpdateCheck.Never, Storage="_RecordStatus", DbType="bit NOT NULL")]
		public bool RecordStatus
		{
			get { return this._RecordStatus; }

			set
			{
				if (this._RecordStatus != value)
				{
				
                    this.OnRecordStatusChanging(value);
					this.SendPropertyChanging();
					this._RecordStatus = value;
					this.SendPropertyChanged("RecordStatus");
					this.OnRecordStatusChanged();
				}

			}

		}

		
		[Column(Name="DivisionId", UpdateCheck=UpdateCheck.Never, Storage="_DivisionId", DbType="int NOT NULL")]
		public int DivisionId
		{
			get { return this._DivisionId; }

			set
			{
				if (this._DivisionId != value)
				{
				
                    this.OnDivisionIdChanging(value);
					this.SendPropertyChanging();
					this._DivisionId = value;
					this.SendPropertyChanged("DivisionId");
					this.OnDivisionIdChanged();
				}

			}

		}

		
		[Column(Name="GenderControlId", UpdateCheck=UpdateCheck.Never, Storage="_GenderControlId", DbType="int NOT NULL")]
		public int GenderControlId
		{
			get { return this._GenderControlId; }

			set
			{
				if (this._GenderControlId != value)
				{
				
                    this.OnGenderControlIdChanging(value);
					this.SendPropertyChanging();
					this._GenderControlId = value;
					this.SendPropertyChanged("GenderControlId");
					this.OnGenderControlIdChanged();
				}

			}

		}

		
		[Column(Name="TeacherControl", UpdateCheck=UpdateCheck.Never, Storage="_TeacherControl", DbType="bit NOT NULL")]
		public bool TeacherControl
		{
			get { return this._TeacherControl; }

			set
			{
				if (this._TeacherControl != value)
				{
				
                    this.OnTeacherControlChanging(value);
					this.SendPropertyChanging();
					this._TeacherControl = value;
					this.SendPropertyChanged("TeacherControl");
					this.OnTeacherControlChanged();
				}

			}

		}

		
		[Column(Name="MixControl", UpdateCheck=UpdateCheck.Never, Storage="_MixControl", DbType="bit NOT NULL")]
		public bool MixControl
		{
			get { return this._MixControl; }

			set
			{
				if (this._MixControl != value)
				{
				
                    this.OnMixControlChanging(value);
					this.SendPropertyChanging();
					this._MixControl = value;
					this.SendPropertyChanged("MixControl");
					this.OnMixControlChanged();
				}

			}

		}

		
		[Column(Name="NbrOfClasses", UpdateCheck=UpdateCheck.Never, Storage="_NbrOfClasses", DbType="int NOT NULL")]
		public int NbrOfClasses
		{
			get { return this._NbrOfClasses; }

			set
			{
				if (this._NbrOfClasses != value)
				{
				
                    this.OnNbrOfClassesChanging(value);
					this.SendPropertyChanging();
					this._NbrOfClasses = value;
					this.SendPropertyChanged("NbrOfClasses");
					this.OnNbrOfClassesChanged();
				}

			}

		}

		
		[Column(Name="NbrOfFemaleClasses", UpdateCheck=UpdateCheck.Never, Storage="_NbrOfFemaleClasses", DbType="int NOT NULL")]
		public int NbrOfFemaleClasses
		{
			get { return this._NbrOfFemaleClasses; }

			set
			{
				if (this._NbrOfFemaleClasses != value)
				{
				
                    this.OnNbrOfFemaleClassesChanging(value);
					this.SendPropertyChanging();
					this._NbrOfFemaleClasses = value;
					this.SendPropertyChanged("NbrOfFemaleClasses");
					this.OnNbrOfFemaleClassesChanged();
				}

			}

		}

		
		[Column(Name="ModifiedBy", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedBy", DbType="int")]
		public int? ModifiedBy
		{
			get { return this._ModifiedBy; }

			set
			{
				if (this._ModifiedBy != value)
				{
				
                    this.OnModifiedByChanging(value);
					this.SendPropertyChanging();
					this._ModifiedBy = value;
					this.SendPropertyChanged("ModifiedBy");
					this.OnModifiedByChanged();
				}

			}

		}

		
		[Column(Name="ModifiedDate", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedDate", DbType="datetime")]
		public DateTime? ModifiedDate
		{
			get { return this._ModifiedDate; }

			set
			{
				if (this._ModifiedDate != value)
				{
				
                    this.OnModifiedDateChanging(value);
					this.SendPropertyChanging();
					this._ModifiedDate = value;
					this.SendPropertyChanged("ModifiedDate");
					this.OnModifiedDateChanged();
				}

			}

		}

		
		[Column(Name="OldDiisionId", UpdateCheck=UpdateCheck.Never, Storage="_OldDiisionId", DbType="int")]
		public int? OldDiisionId
		{
			get { return this._OldDiisionId; }

			set
			{
				if (this._OldDiisionId != value)
				{
				
                    this.OnOldDiisionIdChanging(value);
					this.SendPropertyChanging();
					this._OldDiisionId = value;
					this.SendPropertyChanged("OldDiisionId");
					this.OnOldDiisionIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="PROMOTION_CONTROL_PROMO_FK", Storage="_Promotion", ThisKey="PromotionId", IsForeignKey=true)]
		public Promotion Promotion
		{
			get { return this._Promotion.Entity; }

			set
			{
				Promotion previousValue = this._Promotion.Entity;
				if (((previousValue != value) 
							|| (this._Promotion.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Promotion.Entity = null;
						previousValue.PromotionControls.Remove(this);
					}

					this._Promotion.Entity = value;
					if (value != null)
					{
						value.PromotionControls.Add(this);
						
						this._PromotionId = value.PromotionId;
						
					}

					else
					{
						
						this._PromotionId = default(int);
						
					}

					this.SendPropertyChanged("Promotion");
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

