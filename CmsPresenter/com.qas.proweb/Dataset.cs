/// QuickAddress Pro Web > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > DataSet.cs
/// Data set details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate a Dataset - a searchable 'country'
	/// </summary>
	public class Dataset
	{
		// -- Private Members --


		private string m_sID = null;
		private string m_sName = null;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="d"></param>
		public Dataset(QADataSet d)
		{
			m_sID = d.ID;
			m_sName = d.Name;
		}

		/// <summary>
		/// Create array from SOAP-layer array
		/// </summary>
		/// <param name="aDatasets"></param>
		/// <returns></returns>
		public static Dataset[] CreateArray(QADataSet[] aDatasets)
		{
			Dataset[] aResults = null;
			if (aDatasets != null)
			{
				int iSize = aDatasets.GetLength(0);
				if (iSize > 0)
				{
					aResults = new Dataset[iSize];
					for (int i=0; i < iSize; i++)
					{
						aResults[i] = new Dataset(aDatasets[i]);
					}
				}
			}
			return aResults;
		}

		/// <summary>
		/// Returns the Dataset which matches the data ID, otherwise null
		/// </summary>
		/// <param name="aDatasets">Dataset array to search</param>
		/// <param name="sID">Data identifier to search for</param>
		/// <returns></returns>
		public static Dataset FindByID(Dataset[] aDatasets, string sDataID)
		{
			for (int i=0; i < aDatasets.GetLength(0); i++)
			{
				if (aDatasets[i].ID.Equals(sDataID))
				{
					return aDatasets[i];
				}
			}
			return null;
		}

		
		// -- Read-only Properties --


		/// <summary>
		/// Returns the name of the data set
		/// </summary>
		public string Name
		{
			get
			{
				return m_sName;
			}
		}

		/// <summary>
		/// Returns the ID of the data set (DataId)
		/// </summary>
		public string ID
		{
			get
			{
				return m_sID;
			}
		}
	}
}
