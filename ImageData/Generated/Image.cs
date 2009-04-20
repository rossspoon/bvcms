using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace ImageData
{
	[Table(Name="dbo.Image")]
	public partial class Image : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private byte[] _Bits;
		
		private int? _Length;
		
		private string _Mimetype;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnBitsChanging(byte[] value);
		partial void OnBitsChanged();
		
		partial void OnLengthChanging(int? value);
		partial void OnLengthChanged();
		
		partial void OnMimetypeChanging(string value);
		partial void OnMimetypeChanged();
		
    #endregion
		public Image()
		{
			
			
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

		
		[Column(Name="bits", UpdateCheck=UpdateCheck.Never, Storage="_Bits", DbType="varbinary")]
		public byte[] Bits
		{
			get { return this._Bits; }

			set
			{
				if (this._Bits != value)
				{
				
                    this.OnBitsChanging(value);
					this.SendPropertyChanging();
					this._Bits = value;
					this.SendPropertyChanged("Bits");
					this.OnBitsChanged();
				}

			}

		}

		
		[Column(Name="length", UpdateCheck=UpdateCheck.Never, Storage="_Length", DbType="int")]
		public int? Length
		{
			get { return this._Length; }

			set
			{
				if (this._Length != value)
				{
				
                    this.OnLengthChanging(value);
					this.SendPropertyChanging();
					this._Length = value;
					this.SendPropertyChanged("Length");
					this.OnLengthChanged();
				}

			}

		}

		
		[Column(Name="mimetype", UpdateCheck=UpdateCheck.Never, Storage="_Mimetype", DbType="varchar(20)")]
		public string Mimetype
		{
			get { return this._Mimetype; }

			set
			{
				if (this._Mimetype != value)
				{
				
                    this.OnMimetypeChanging(value);
					this.SendPropertyChanging();
					this._Mimetype = value;
					this.SendPropertyChanged("Mimetype");
					this.OnMimetypeChanged();
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

