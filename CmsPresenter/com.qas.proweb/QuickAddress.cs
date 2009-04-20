/// QuickAddress Pro Web > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > QuickAddress.cs
/// Main searching functionality


using System;
using System.Text;						// StringBuilder
using System.Configuration;			// ConfigurationSettings.AppSettings
using com.qas.proweb.soap;				// QuickAddress Pro Web wrapped SOAP-layer


namespace com.qas.proweb
{
	/// <summary>
	/// This class is a facade into Pro Web and provides the main functionality of the package.
	/// It uses the com.qas.proweb.soap package in a stateless manner, but some optional settings
	/// are maintained between construction and the main "business" call to the soap package.
	/// An instance of this class is not intended to be preserved across different pages.
	/// The intended usage idiom is:
	///   construct instance - set optional settings - call main method (e.g. search) - discard instance
	/// </summary>
	public class QuickAddress
	{
		// -- Public Constants --


		/// <summary>
		/// Enumeration of engine types
		/// </summary>
		public enum EngineTypes
		{
			Singleline = EngineEnumType.Singleline,
			Typedown = EngineEnumType.Typedown,
			Verification = EngineEnumType.Verification
		}

		/// <summary>
		/// Enumeration of engine searching intensity levels
		/// </summary>
		public enum SearchingIntesityLevels
		{
			Exact = EngineIntensityType.Exact,
			Close = EngineIntensityType.Close,
			Extensive = EngineIntensityType.Extensive
		}

		/// Line separator - determined by a configuration setting on the server
		private const char cLINE_SEPARATOR = '|';


		// -- Private Members --


		// QuickAddress Pro Web search service
		private ProWeb m_Service = null;
		// Engine searching configuration settings (optional to override server defaults)
		private EngineType m_Engine = null;
		// Configuration file settings (optional to override defaults)
		private QAConfigType m_Config = null;


		// -- Public Construction --


		/// <summary>
		/// Constructs the search service, using the URL of the QuickAddress Server
		/// </summary>
		/// <param name="sEndpointURL">The URL of the QuickAddress SOAP service, e.g. http://localhost:2021/</param>
		public QuickAddress(String sEndpointURL)
		{
			m_Service = new ProWeb();
			m_Service.Url = sEndpointURL;
			m_Engine = new EngineType();
			m_Config = new QAConfigType();
		}


		// -- Public Properties --


		/// <summary>
		/// Sets the current engine; if left unset, the search will use the default, SingleLine
		/// </summary>
		public EngineTypes Engine
		{
			get
			{
				return (EngineTypes)m_Engine.Value;
			}
			set
			{
				m_Engine.Value = (EngineEnumType)value;
			}
		}

		/// <summary>
		/// Sets the engine intensity; if left unset, the search will use the server default value
		/// </summary>
		public SearchingIntesityLevels SearchingIntesity
		{
			set
			{
				m_Engine.Intensity = (EngineIntensityType)value;
				m_Engine.IntensitySpecified = true;
			}
		}

		/// <summary>
		/// Sets the picklist threshold - the maximum number of entries in a picklist,
		/// returned by step-in and refinement; ignored by the initial search
		/// If left unset, the server default will be used
		/// </summary>
		public int Threshold
		{
			set
			{
				m_Engine.Threshold = System.Convert.ToString(value);
			}
		}

		/// <summary>
		/// Sets the timeout period (milliseconds); if left unset, the server default will be used
		/// </summary>
		public int Timeout
		{
			set
			{
				m_Engine.Timeout = System.Convert.ToString(value);
			}
		}

		/// <summary>
		/// Sets whether operations produce a flattened or hierarchical picklist;
		/// If left unset, the default value of false (hierarchical) will be used
		/// </summary>
		public bool Flatten
		{
			set
			{
				m_Engine.Flatten = value;
				m_Engine.FlattenSpecified = true;
			}
		}

		/// <summary>
		/// Sets the config file to read settings from; if left unset, the default "qawserve.ini" file will be used
		/// </summary>
		public string ConfigFile
		{
			set
			{
				m_Config.IniFile = value;
			}
		}

		/// <summary>
		/// Sets the config section to read settings from; if left unset, the default "[QADefault]" section will be used
		/// </summary>
		public string ConfigSection
		{
			set
			{
				m_Config.IniSection = value;
			}
		}


		// -- Public Methods - Searching Operations --


		/// <summary>
		/// Test whether a search can be performed using a data set/layout/engine combination
		/// </summary>
		/// <param name="sDataID">Three-letter data identifier</param>
		/// <param name="sLayout">Name of the layout; optional</param>
		/// <returns>Is the country and layout combination available</returns>
		/// <throws>SoapException</throws>
		public CanSearch CanSearch(string sDataID, string sLayout)
		{
			QACanSearch param = new QACanSearch();
			param.Country = sDataID;
			param.Engine = m_Engine;
			param.Layout = sLayout;
			param.QAConfig = m_Config;

			CanSearch tResult = null;
			try
			{
				// Make the call to the server
				QASearchOk cansearchResult = SearchService.DoCanSearch(param);

				tResult = new CanSearch(cansearchResult);
			}
			catch(Exception x)
			{
				MapException(x);
			}

			return tResult;
		}


		/// <summary>
		/// Method overload: provides the CanSearch function without the optional sLayout argument
		/// </summary>
		public CanSearch CanSearch(string sDataID)
		{
			return CanSearch(sDataID, null);
		}


		/// <summary>
		/// Perform an initial search for the search terms in the specified data set
		/// If using the verification engine, the result may include a formatted address and/or a picklist
		/// Other engines only produce a picklist
		/// </summary>
		/// <param name="sDataID">Three-letter identifier of the data to search</param>
		/// <param name="asSearch">Array of search terms</param>
		/// <param name="sPromptSet">Name of the search prompt set applied to the search terms</param>
		/// <param name="Layout">Name of the layout (verification engine only); optional</param>
		/// <returns>Search result, containing a picklist and/or formatted address</returns>
		/// <throws>SoapException</throws>
		public SearchResult Search(string sDataID, string[] asSearch, PromptSet.Types tPromptSet, string sLayout)
		{
			System.Diagnostics.Debug.Assert(asSearch != null && asSearch.GetLength(0) > 0);

			// Concatenate search terms
			StringBuilder sSearch = new StringBuilder(asSearch[0]);
			for (int i=1; i < asSearch.GetLength(0); i++)
			{
				sSearch.Append(cLINE_SEPARATOR);
				sSearch.Append(asSearch[i]);
			}

			return Search(sDataID, sSearch.ToString(), tPromptSet, sLayout);
		}


		/// <summary>
		/// Method overload: provides the Search function without the optional Layout argument
		/// </summary>
		/// <param name="sDataID">Three-letter identifier of the data to search</param>
		/// <param name="asSearch">Array of search terms</param>
		/// <param name="sPromptSet">Name of the search prompt set applied to the search terms</param>
		/// <returns>Search result, containing a picklist and/or formatted address</returns>
		/// <throws>SoapException</throws>
		public SearchResult Search(string sDataID, string[] asSearch, PromptSet.Types tPromptSet)
		{
			return Search(sDataID, asSearch, tPromptSet, null);
		}


		/// <summary>
		/// Method overload: the Search function with search terms as a single string
		/// </summary>
		/// <param name="sDataID">Three-letter identifier of the data to search</param>
		/// <param name="sSearch">Search terms</param>
		/// <param name="sPromptSet">Name of the search prompt set applied to the search terms</param>
		/// <param name="Layout">Name of the layout (verification engine only); optional</param>
		/// <returns>Search result, containing a picklist and/or formatted address</returns>
		/// <throws>SoapException</throws>
		public SearchResult Search(string sDataID, string sSearch, PromptSet.Types tPromptSet, string sLayout)
		{
			System.Diagnostics.Debug.Assert(sDataID != null);
			System.Diagnostics.Debug.Assert(sSearch != null);

			// Set up the parameter for the SOAP call
			QASearch param = new QASearch();
			param.Country = sDataID;
			param.Engine = m_Engine;
			param.Engine.PromptSet = (PromptSetType)tPromptSet;
			param.Engine.PromptSetSpecified = true;
			param.Layout = sLayout;
			param.QAConfig = m_Config;
			param.Search = sSearch;

			SearchResult result = null;
			try
			{
				// Make the call to the server
				QASearchResult searchResult = SearchService.DoSearch(param);

				result = new SearchResult(searchResult);
			}
			catch(Exception x)
			{
				MapException(x);
			}

			return result;
		}


		/// <summary>
		/// Method overload: the Search function with search terms as a single string, without Layout
		/// </summary>
		/// <param name="sDataID">Three-letter identifier of the data to search</param>
		/// <param name="sSearch">Search terms</param>
		/// <param name="sPromptSet">Name of the search prompt set applied to the search terms</param>
		/// <returns>Search result, containing a picklist and/or formatted address</returns>
		/// <throws>SoapException</throws>
		public SearchResult Search(string sDataID, string sSearch, PromptSet.Types tPromptSet)
		{
			return Search(sDataID, sSearch, tPromptSet, null);
		}


		/// <summary>
		/// Perform a refinement, filtering the specified picklist using the supplied text
		/// NB: Stepin delegates to this function with blank refinement text
		/// </summary>
		/// <param name="sMoniker">The search point moniker of the picklist to refine</param>
		/// <param name="sRefinementText">The refinement text</param>
		/// <returns>Picklist result</returns>
		public Picklist Refine(String sMoniker, String sRefinementText)
		{
			System.Diagnostics.Debug.Assert(sMoniker != null && sRefinementText != null);

			// Set up the parameter for the SOAP call
			QARefine param = new QARefine();
			param.Moniker = sMoniker;
			param.Refinement = sRefinementText;
			param.QAConfig = m_Config;
			param.Threshold = m_Engine.Threshold;
			param.Timeout = m_Engine.Timeout;

			Picklist result = null;
			try
			{
				// Make the call to the server
				QAPicklistType picklist = SearchService.DoRefine(param).QAPicklist;

				result = new Picklist(picklist);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return result;
		}


		/// <summary>
		/// Perform a step-in: return the picklist for a particular moniker
		/// NB: delegates to the Refine function with blank refinement text
		/// </summary>
		/// <param name="sMoniker">The search point moniker of the picklist being displayed</param>
		/// <returns>Picklist result</returns>
		public Picklist StepIn(String sMoniker)
		{
			return Refine(sMoniker, "");
		}


		/// <summary>
		/// Retrieve the final address specifed by the moniker, formatted using the requested layout
		/// </summary>
		/// <param name="sMoniker">Search point moniker of the address item</param>
		/// <param name="sLayout">Name of the layout name (specifies how the address should be formatted)</param>
		/// <returns>Formatted address result</returns>
		public FormattedAddress GetFormattedAddress(string sMoniker, string sLayout)
		{
			System.Diagnostics.Debug.Assert(sMoniker != null && sLayout != null);

			// Set up the parameter for the SOAP call
			QAGetAddress param = new QAGetAddress();
			param.Layout = sLayout;
			param.Moniker = sMoniker;
			param.QAConfig = m_Config;

			FormattedAddress result = null;
			try
			{
				// Make the call to the server
				QAAddressType address = SearchService.DoGetAddress(param).QAAddress;

				result = new FormattedAddress(address);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return result;
		}


		// -- Public Methods - Status Operations --


		/// <summary>
		/// Retrieve all the available data sets
		/// </summary>
		/// <returns>Array of available data sets</returns>
		public Dataset[] GetAllDatasets()
		{
			Dataset[] aResults = null;
			try
			{
				// Make the call to the server
				QADataSet[] aDatasets = SearchService.DoGetData();
				aResults = Dataset.CreateArray(aDatasets);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return aResults;
		}


		/// <summary>
		/// Retrieve an array of all the layouts available for the specified data set
		/// </summary>
		/// <param name="sDataID">3-letter identifier of the data set of interest</param>
		/// <returns>Array of layouts within this data set</returns>
		public Layout[] GetAllLayouts(string sDataID)
		{
			System.Diagnostics.Debug.Assert(sDataID != null);

			// Set up the parameter for the SOAP call
			QAGetLayouts param = new QAGetLayouts();
			param.Country = sDataID;
			param.QAConfig = m_Config;

			Layout[] aResults = null;
			try
			{
				// Make the call to the server
				QALayout[] aLayouts = SearchService.DoGetLayouts(param);
				aResults = Layout.CreateArray(aLayouts);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return aResults;
		}


		/// <summary>
		/// Retrieve an array of example addresses for this data set in the specified layouit
		/// </summary>
		/// <param name="sDataID">data set of interest, 3-letter identifier</param>
		/// <param name="sLayout">Layout to apply</param>
		/// <returns>Array of example addresses</returns>
		public ExampleAddress[] GetExampleAddresses(String sDataID, String sLayout)
		{
			// Set up the parameter for the SOAP call
			QAGetExampleAddresses param = new QAGetExampleAddresses();
			param.Country = sDataID;
			param.Layout = sLayout;
			param.QAConfig = m_Config;

			ExampleAddress[] aResults = null;
			try
			{
				// Make the call to the server
				QAExampleAddress[] aAddresses = SearchService.DoGetExampleAddresses(param);
				aResults = ExampleAddress.createArray(aAddresses);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return aResults;
		}


		/// <summary>
		/// Retrieve detailed licensing information about all the data sets and DataPlus sets installed
		/// </summary>
		/// <returns> Array of licencing information, one per data set</returns>
		public LicensedSet[] GetLicenceInfo()
		{
			LicensedSet[] aResults = null;
			try
			{
				// Make the call to the server
				QALicenceInfo info = SearchService.DoGetLicenseInfo();
				aResults = LicensedSet.createArray(info);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return aResults;
		}


		/// <summary>
		/// Retrieve the search prompt set for a particular data set
		/// </summary>
		/// <param name="sDataID">data set of interest, 3-letter identifier</param>
		/// <param name="eTemplateName">Input template of interest</param>
		/// <returns>Input template, array of template lines</returns>
		public PromptSet GetPromptSet(string sDataID, PromptSet.Types tType)
		{
			System.Diagnostics.Debug.Assert(sDataID != null);

			// Set up the parameter for the SOAP call
			QAGetPromptSet param = new QAGetPromptSet();
			param.Country = sDataID;
			param.Engine = m_Engine;
			param.PromptSet = (PromptSetType)tType;
			param.QAConfig = m_Config;

			PromptSet result = null;
			try
			{
				// Make the call to the server
				QAPromptSet tPromptSet = SearchService.DoGetPromptSet(param);
				result = new PromptSet(tPromptSet);
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return result;
		}


		/// <summary>
		/// Retrieve system (diagnostic) information from the server
		/// <returns>Array of strings, tab-separated key/value pairs of system info</returns>
		public String[] GetSystemInfo()
		{
			String[] aResults = null;
			try
			{
				// Make the call to the server
				aResults = SearchService.DoGetSystemInfo();
			}
			catch (Exception x)
			{
				MapException(x);
			}

			return aResults;
		}


		// -- Private Methods - Helpers --


		/// <summary>
		/// Return the QuickAddress Pro Web SOAP service
		/// </summary>
		private ProWeb SearchService
		{
			get
			{
				return m_Service;
			}
		}


		/// <summary>
		/// Rethrow a remote SoapException exception, with details parsed and exposed
		/// </summary>
		/// <param name="e"></param>
		private void MapException(Exception x)
		{
			System.Diagnostics.Debugger.Log(0, "Error", x.ToString() + "\n");

			if (x is System.Web.Services.Protocols.SoapHeaderException)
			{
				System.Web.Services.Protocols.SoapHeaderException xHeader = x as System.Web.Services.Protocols.SoapHeaderException;
				throw x;
			}
			else if (x is System.Web.Services.Protocols.SoapException)
			{
				// Parse out qas:QAFault string
				System.Web.Services.Protocols.SoapException xSoap = x as System.Web.Services.Protocols.SoapException;
				System.Xml.XmlNode xmlDetail = xSoap.Detail;

				string sDetail = xmlDetail.InnerText.Trim();
				string[] asDetail = sDetail.Split('\n');

				string sMessage = asDetail[1].Trim() + " [" + asDetail[0].Trim() + "]";
				Exception xThrow = new Exception(sMessage, xSoap);
				throw xThrow;
			}
			else
			{
				throw x;
			}
		}
	}
}
