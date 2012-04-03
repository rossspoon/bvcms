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
    			       && Voided == null
    			       && Credited == null
    			       && (Coupon ?? false) == false
    			       && TransactionId.HasValue();
    		}
    	}
    }
}