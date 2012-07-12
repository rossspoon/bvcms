﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CmsCheckin
{
	[XmlRoot("BuildingChecking")]
	public class BaseBuildingInfo
	{
		[XmlAttribute]
		public bool membersonly { get; set; }

		[XmlAttribute]
		public int maxguests { get; set; }

		[XmlArray]
		public List<Activity> Activities { get; set; }

		public BaseBuildingInfo()
		{
			membersonly = false;
			maxguests = -1;
		}
	}

	[Serializable]
	public class Activity
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public int org { get; set; }

		[XmlText]
		public string display { get; set; }

		public override string ToString()
		{
			return display;
		}
	}
}