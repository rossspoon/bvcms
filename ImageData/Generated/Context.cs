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
	[DatabaseAttribute(Name="CMSImage")]
	public partial class CMSImageDataContext : DataContext
	{
		private static MappingSource mappingSource = new AttributeMappingSource();
		
#region Extensibility Method Definitions
        partial void OnCreated();
		
        partial void InsertImage(Image instance);
        partial void UpdateImage(Image instance);
        partial void DeleteImage(Image instance);
        
#endregion
		
		public CMSImageDataContext() : 
				base(System.Configuration.ConfigurationManager.ConnectionStrings["CMSImage"].ConnectionString, mappingSource)
		{
			OnCreated();
		}

		
		public CMSImageDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public CMSImageDataContext(IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public CMSImageDataContext(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public CMSImageDataContext(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

    #region Tables
		
		public Table< Image> Images
		{
			get	{ return this.GetTable< Image>(); }

		}

	#endregion
	#region Views
		
    #endregion
	#region Table Functions
		
    #endregion
	#region Scalar Functions
		
    #endregion
	#region Stored Procedures
		
    #endregion
   }

}

