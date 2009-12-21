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
	[Table(Name="OrphanedImages")]
	public partial class OrphanedImage
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Id;
		
		private string _Mimetype;
		
		private int? _Length;
		
		
		public OrphanedImage()
		{
		}

		
		
		[Column(Name="Id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
					this._Id = value;
			}

		}

		
		[Column(Name="mimetype", Storage="_Mimetype", DbType="varchar(20)")]
		public string Mimetype
		{
			get
			{
				return this._Mimetype;
			}

			set
			{
				if (this._Mimetype != value)
					this._Mimetype = value;
			}

		}

		
		[Column(Name="length", Storage="_Length", DbType="int")]
		public int? Length
		{
			get
			{
				return this._Length;
			}

			set
			{
				if (this._Length != value)
					this._Length = value;
			}

		}

		
    }

}
