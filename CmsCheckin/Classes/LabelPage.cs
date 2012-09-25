using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CmsCheckin.Classes
{
	class LabelPage
	{
		public List<LabelEntry> leEntries = new List<LabelEntry>();

		public LabelPage() { }

		public int populate(List<LabelEntryBase> lList)
		{
			foreach(LabelEntryBase lebItem in lList)
			{
				for( int iX = 0; iX < lebItem.iRepeat; iX++ )
				{
					LabelEntry leNew = new LabelEntry();
					leNew.copy(lebItem.textToUpper());
					leNew.adjust(iX);
					leEntries.Add(leNew);
				}
			}

			return 0;
		}

		public int populate(List<LabelEntryBase> lebList, List<LabelInfo> liItem, int iOffset)
		{
			int iLast = 0;
			int iMaxRepeat = getMaxRepeat(lebList);

			foreach (LabelEntryBase lebItem in lebList)
			{
				if (lebItem.iRepeat == 1)
				{
					LabelEntry leNew = new LabelEntry();
					leNew.copy(lebItem);
					leNew.fill(liItem.ElementAt(iOffset));
					leEntries.Add(leNew);

                    iLast = (liItem.Count - 1 - iOffset);
				}
				else if ((iMaxRepeat + iOffset) >= liItem.Count())
				{
					for (int iX = iOffset; iX < liItem.Count(); iX++)
					{
						LabelEntry leNew = new LabelEntry();
						leNew.copy(lebItem);
						leNew.adjust(iX - iOffset);
						leNew.fill(liItem.ElementAt(iX));
						leEntries.Add(leNew);
					}
				}
				else
				{
					int iX;

					for (iX = iOffset; iX < (iMaxRepeat + iOffset); iX++)
					{
						LabelEntry leNew = new LabelEntry();
						leNew.copy(lebItem);
						leNew.adjust(iX - iOffset);
						leNew.fill(liItem.ElementAt(iX));
						leEntries.Add(leNew);
					}

					iLast = iX;
				}
			}

			return iLast;
		}

		public int getMaxRepeat(List<LabelEntryBase> lebList)
		{
			int iMax = 0;

			foreach (LabelEntryBase lebItem in lebList)
			{
				if (lebItem.iRepeat > iMax) iMax = lebItem.iRepeat;
			}

			return iMax;
		}
	}
}
