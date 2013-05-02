using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData.Classes.QuickBooks
{
    public class QBJournalEntryLine
    {
        public string sDescrition { get; set; }
        public string sAccountID { get; set; }

        public decimal dAmount { get; set; }

        public bool bCredit { get; set; }

        public QBJournalEntryLine() { }

        public QBJournalEntryLine(QBJournalEntryLine copyFrom)
        {
            sDescrition = string.Copy(copyFrom.sDescrition);
            sAccountID = string.Copy(copyFrom.sAccountID);

            dAmount = copyFrom.dAmount;

            bCredit = copyFrom.bCredit;
        }
    }
}