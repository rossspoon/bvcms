using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData.View
{
	[Table(Name="TableColumns")]
	public partial class TableColumn
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Database;
		
		private string _Owner;
		
		private string _TableName;
		
		private string _ColumnName;
		
		private int? _OrdinalPosition;
		
		private string _DefaultSetting;
		
		private string _IsNullable;
		
		private string _DataType;
		
		private int? _MaxLength;
		
		private short? _DatePrecision;
		
		private int? _IsIdentity;
		
		private int? _IsComputed;
		
		private int? _Scale;
		
		private byte? _Precision;
		
		private bool? _IsPrimaryKey;
		
		
		public TableColumn()
		{
		}

		
		
		[Column(Name="Database", Storage="_Database", DbType="nvarchar(128)")]
		public string Database
		{
			get
			{
				return this._Database;
			}

			set
			{
				if (this._Database != value)
					this._Database = value;
			}

		}

		
		[Column(Name="Owner", Storage="_Owner", DbType="nvarchar(128)")]
		public string Owner
		{
			get
			{
				return this._Owner;
			}

			set
			{
				if (this._Owner != value)
					this._Owner = value;
			}

		}

		
		[Column(Name="TableName", Storage="_TableName", DbType="nvarchar(128) NOT NULL")]
		public string TableName
		{
			get
			{
				return this._TableName;
			}

			set
			{
				if (this._TableName != value)
					this._TableName = value;
			}

		}

		
		[Column(Name="ColumnName", Storage="_ColumnName", DbType="nvarchar(128)")]
		public string ColumnName
		{
			get
			{
				return this._ColumnName;
			}

			set
			{
				if (this._ColumnName != value)
					this._ColumnName = value;
			}

		}

		
		[Column(Name="OrdinalPosition", Storage="_OrdinalPosition", DbType="int")]
		public int? OrdinalPosition
		{
			get
			{
				return this._OrdinalPosition;
			}

			set
			{
				if (this._OrdinalPosition != value)
					this._OrdinalPosition = value;
			}

		}

		
		[Column(Name="DefaultSetting", Storage="_DefaultSetting", DbType="nvarchar(4000)")]
		public string DefaultSetting
		{
			get
			{
				return this._DefaultSetting;
			}

			set
			{
				if (this._DefaultSetting != value)
					this._DefaultSetting = value;
			}

		}

		
		[Column(Name="IsNullable", Storage="_IsNullable", DbType="varchar(3)")]
		public string IsNullable
		{
			get
			{
				return this._IsNullable;
			}

			set
			{
				if (this._IsNullable != value)
					this._IsNullable = value;
			}

		}

		
		[Column(Name="DataType", Storage="_DataType", DbType="nvarchar(128)")]
		public string DataType
		{
			get
			{
				return this._DataType;
			}

			set
			{
				if (this._DataType != value)
					this._DataType = value;
			}

		}

		
		[Column(Name="MaxLength", Storage="_MaxLength", DbType="int")]
		public int? MaxLength
		{
			get
			{
				return this._MaxLength;
			}

			set
			{
				if (this._MaxLength != value)
					this._MaxLength = value;
			}

		}

		
		[Column(Name="DatePrecision", Storage="_DatePrecision", DbType="smallint")]
		public short? DatePrecision
		{
			get
			{
				return this._DatePrecision;
			}

			set
			{
				if (this._DatePrecision != value)
					this._DatePrecision = value;
			}

		}

		
		[Column(Name="IsIdentity", Storage="_IsIdentity", DbType="int")]
		public int? IsIdentity
		{
			get
			{
				return this._IsIdentity;
			}

			set
			{
				if (this._IsIdentity != value)
					this._IsIdentity = value;
			}

		}

		
		[Column(Name="IsComputed", Storage="_IsComputed", DbType="int")]
		public int? IsComputed
		{
			get
			{
				return this._IsComputed;
			}

			set
			{
				if (this._IsComputed != value)
					this._IsComputed = value;
			}

		}

		
		[Column(Name="scale", Storage="_Scale", DbType="int")]
		public int? Scale
		{
			get
			{
				return this._Scale;
			}

			set
			{
				if (this._Scale != value)
					this._Scale = value;
			}

		}

		
		[Column(Name="precision", Storage="_Precision", DbType="tinyint")]
		public byte? Precision
		{
			get
			{
				return this._Precision;
			}

			set
			{
				if (this._Precision != value)
					this._Precision = value;
			}

		}

		
		[Column(Name="IsPrimaryKey", Storage="_IsPrimaryKey", DbType="bit")]
		public bool? IsPrimaryKey
		{
			get
			{
				return this._IsPrimaryKey;
			}

			set
			{
				if (this._IsPrimaryKey != value)
					this._IsPrimaryKey = value;
			}

		}

		
    }

}
