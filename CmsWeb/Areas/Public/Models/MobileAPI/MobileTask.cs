using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.MobileAPI
{
	/*
	+ [Id] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[ListId] [int] NOT NULL,
	+ [CoOwnerId] [int] NULL,
	[CoListId] [int] NULL,
	+ [StatusId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SourceContactId] [int] NULL,
	[CompletedContactId] [int] NULL,
	+ [Notes] [varchar](max) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[Project] [varchar](50) NULL,
	[Archive] [bit] NOT NULL,
	+ [Priority] [int] NULL,
	+ [WhoId] [int] NULL,
	+ [Due] [datetime] NULL,
	[Location] [varchar](50) NULL,
	+ [Description] [varchar](100) NULL,
	[CompletedOn] [datetime] NULL,
	[ForceCompleteWContact] [bit] NULL,
	[OrginatorId] [int] NULL,
	*/

	public class MobileTask
	{
		public int id = 0;

        public int ownerID = 0;
        public int boxID = 0; // Connects to a list

        public int updateDue = 0;
		public DateTime due = DateTime.Now;
		public int priority = 0;
		
		public string description = "";

		public string status = "";
		public int statusID = 0;

		public string about = "";
		public int aboutID = 0;

		public string delegated = "";
		public int delegatedID = 0;

		public string notes = "";

		public MobileTask populate(CmsData.Task t)
		{
			id = t.Id;

            ownerID = t.OwnerId;
            boxID = t.ListId;

			due = t.Due ?? DateTime.Now;
			priority = t.Priority ?? 0;

			description = t.Description ?? "";

			status = t.TaskStatus.Description ?? "";
			statusID = t.StatusId ?? 0;

			about = t.AboutName ?? "";
			aboutID = t.WhoId ?? 0;

			if( t.CoOwner != null ) delegated = t.CoOwner.Name ?? "";
			delegatedID = t.CoOwnerId ?? 0;

			notes = t.Notes ?? "";

			return this;
		}

        public int addToDB()
        {
            Task t = new Task();

            t.OwnerId = ownerID;
            t.ListId = boxID;
            t.Due = due;
            t.Priority = priority;
            t.Description = description;
            t.StatusId = statusID;
            t.WhoId = aboutID;
            if(delegatedID > 0) t.CoOwnerId = delegatedID;
            t.Notes = notes;

            DbUtil.Db.Tasks.InsertOnSubmit(t);
            DbUtil.Db.SubmitChanges();

            return t.Id;
        }
	}
}