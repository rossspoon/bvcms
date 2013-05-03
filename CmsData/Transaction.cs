using System;
using UtilityExtensions;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CmsData
{
    public partial class Transaction
    {
    	public bool CanCredit
    	{
    		get
    		{
				if (!Util.IsSage.HasValue)
					Util.IsSage = DbUtil.Db.Setting("TransactionGateway", "").ToLower() == "sage";
    			return Approved == true 
    			       && Util.IsSage.Value
    			       && Voided != true
    			       && Credited != true
    			       && (Coupon ?? false) == false
    			       && TransactionId.HasValue()
					   && Batchtyp == "eft" || Batchtyp == "bankcard"
					   && Amt > 0;
    		}
    	}
    	public bool CanVoid
    	{
    		get
    		{
				if (!Util.IsSage.HasValue)
					Util.IsSage = DbUtil.Db.Setting("TransactionGateway", "").ToLower() == "sage";
    			return Approved == true 
					 && !CanCredit
    			       && Util.IsSage.Value
    			       && Voided != true
    			       && Credited != true
    			       && (Coupon ?? false) == false
    			       && TransactionId.HasValue()
					   && Amt > 0;
    		}
    	}
		public int FirstTransactionPeopleId()
		{
			return OriginalTransaction.TransactionPeople.Select(pp => pp.PeopleId).FirstOrDefault();
		}
        public string FullName
        {
            get
            {
                var s = "";
                if (Last.HasValue())
                {
                    if (MiddleInitial.HasValue())
                        s = "{0} {1} {2}".Fmt(First, MiddleInitial, Last);
                    else
                        s = "{0} {1}".Fmt(First, Last);
                    if (Suffix.HasValue())
                        s = s + ", " + Suffix;
                    return s;
                }
                return Name;
            }
        }
    }
}