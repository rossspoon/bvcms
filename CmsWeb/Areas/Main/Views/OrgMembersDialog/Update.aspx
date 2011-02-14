<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<html>
    <script src="/Content/js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script>
        $(function() {
            self.parent.RebindMemberGrids();
        });
    </script>
</html>
