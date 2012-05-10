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
    			return Approved == true 
    			       && TransactionGateway == "Sage"
    			       && Voided != true
    			       && Credited != true
    			       && (Coupon ?? false) == false
    			       && TransactionId.HasValue()
					   && Amt > 0;
    		}
    	}
		public int FirstTransactionPeopleId()
		{
			return OriginalTransaction.TransactionPeople.Select(pp => pp.PeopleId).First();
		}
    }
}