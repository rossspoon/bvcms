using UtilityExtensions;

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
    }
}