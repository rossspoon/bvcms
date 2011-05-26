<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>

<script runat="server">
   void Page_Load() {
      byte[] delay = new byte[1];
      RandomNumberGenerator prng = new RNGCryptoServiceProvider();

      prng.GetBytes(delay);
      Thread.Sleep((int)delay[0]);
        
      IDisposable disposable = prng as IDisposable;
      if (disposable != null) { disposable.Dispose(); }
    }
</script>

<html>
<head runat="server">
    <title>Error</title>
    <style type="text/css">
        body
        {
            font-family: Arial;
        }
        H1
        {
            color: Red;
        }
    </style>
</head>
<body>
    <div>
    <h1>An error occurred while processing your request.</h1>
    <div>An email has been sent to the developer to report the problem.</div>
    <a href="/">Home</a>
    </div>
</body>
</html>
