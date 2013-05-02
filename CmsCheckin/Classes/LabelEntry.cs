namespace CmsCheckin.Classes
{
	class LabelEntry : LabelEntryBase
	{
		public LabelEntry() { }

		public void fill(LabelInfo liItem)
		{
			switch (iType)
			{
				case TYPE_STRING:
				{
					sText = liItem.GetType().GetProperty( sText ).GetValue(liItem, null).ToString();
					break;
				}

				case TYPE_BARCODE:
				{
					sText = liItem.GetType().GetProperty( sText ).GetValue(liItem, null).ToString();
					break;
				}
			}
		}
	}
}