using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsCheckin.Classes
{
	class LabelSet
	{
		public List<LabelPage> lPages = new List<LabelPage>();
		public List<LabelEntryBase> lFormats;

		public LabelSet() { }

		public int getCount() { return lPages.Count(); }

		public void addPages( string sLabelFormat, List<LabelInfo> lItems )
		{
			lFormats = new List<LabelEntryBase>();

			string[] sTextList = sLabelFormat.Split(new char[] { '~' });
			foreach( string sItem in sTextList )
			{
				if( sItem.Length > 0 ) lFormats.Add( LabelEntryBase.create(sItem) );
			}

			if (lItems == null)
			{
				LabelPage lpNew = new LabelPage();
				lpNew.populate(lFormats);
				lPages.Add(lpNew);
			}
			else
			{
				if (lItems.Count() == 0) return;

				LabelPage lpNew = new LabelPage();
				int iOffset = lpNew.populate(lFormats, lItems, 0);
				lPages.Add(lpNew);

				while( iOffset > 0 )
				{
					lpNew = new LabelPage();
					iOffset = lpNew.populate(lFormats, lItems, iOffset);
					lPages.Add(lpNew);
				}
			}
		}
	}
}
