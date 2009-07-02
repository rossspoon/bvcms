using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintActvx
{
    public interface PrintActvx
    {
        void PrintLabel(string line1, string line2, string line3);
    }
    public partial class UserControl1 : UserControl, PrintActvx
    {
        public UserControl1()
        {
            InitializeComponent();
        }


        #region PrintActvx Members

        public void PrintLabel(string line1, string line2, string line3)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    /*
<html>
 <body color=white>
  <hr>

 <font face=arial size=1>
  <OBJECT id="myControl1" name="myControl1" classid="ActiveXDotNet.dll#ActiveXDotNet.myControl" 
  width=288 height=72>
  </OBJECT>
 </font>

 <form name="frm" id="frm">
   <input type="text" name="txt" value="enter text here"><input type=button value="Click me" 
   onClick="doScript();">
 </form>

  <hr>
 </body>

<script language="javascript">
 function doScript()
  {
   myControl1.UserText = frm.txt.value;
  }
</script>

</html>
     */
    //http://msdn.microsoft.com/en-us/library/system.drawing.printing.printdocument(VS.71).aspx
    //http://stackoverflow.com/questions/123154/sending-raw-data-to-fedex-label-printer

}
