<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PromotionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Promotion</title>
   <style type="text/css">
      #float_box { 
       position: absolute; 
       top: 0; right: 0;
       z-index: 1000; 
	   background:#ffc;
	   padding:5px;
       border:1px solid #CCCCCC;
       width:200px;
      }
      #close_float
      {
          float:right;
          cursor:pointer;
      }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $('#PromotionId').change(RefreshPage);
            $('#ScheduleId').change(RefreshPage);
            $('#FilterUnassigned').click(RefreshPage);
            $('#NormalMembersOnly').click(RefreshPage);
            $('input.check').click(UpdateTotals);
            $('#Promotions > thead a.sortable').click(function(ev) {
                var newsort = $(this).text();
                var oldsort = $("#Sort").val();
                $("#Sort").val(newsort);
                var dir = $("#Dir").val();
                if (oldsort == newsort && dir == 'asc')
                    $("#Dir").val('desc');
                else
                    $("#Dir").val('asc');
                RefreshList();
            });
            $(window).scroll(function() {
                $('#float_box').animate({ top: $(window).scrollTop() + "px" }, { queue: false, duration: 350 });
            });
            $('#close_float').click(function() {
                $('#float_box').animate({ top: "+=15px", opacity: 0 }, "slow");
            });
            var ttotal = $('.check').length;
            var tboys = $('.check[gender=M]').length;
            var tgirls = $('.check[gender=F]').length;
            var thigh = $('.check[attend=Hi]').length;
            var tmed = $('.check[attend=Med]').length;
            var tlow = $('.check[attend=Lo]').length;
            $('#ttotal').text(ttotal);
            $('#tboys').text(tboys);
            $('#tgirls').text(tgirls);
            $('#thigh').text(thigh);
            $('#tmed').text(tmed);
            $('#tlow').text(tlow);

            $('#tpboys').text((tboys / ttotal * 100).toFixed(0) + '%');
            $('#tpgirls').text((tgirls / ttotal * 100).toFixed(0) + '%');
            $('#tphigh').text((thigh / ttotal * 100).toFixed(0) + '%');
            $('#tpmed').text((tmed / ttotal * 100).toFixed(0) + '%');
            $('#tplow').text((tlow / ttotal * 100).toFixed(0) + '%');
            UpdateTotals();
        });
        function RefreshPage() {
            var q = $('#form').formSerialize();
            $.navigate("/Promotion/Index", q);
        }
        function RefreshList() {
            var q = $('#form').formSerialize();
            $.post('/Promotion/List/', q, function(ret) {
                $('#Promotions > tbody').html(ret);
                $('input.check').click(UpdateTotals);
            });
        }
        function UpdateTotals() {
            var total = $('.check:checked').length;
            var boys = $('.check:checked[gender=M]').length;
            var girls = $('.check:checked[gender=F]').length;
            var high = $('.check:checked[attend=Hi]').length;
            var med = $('.check:checked[attend=Med]').length;
            var low = $('.check:checked[attend=Lo]').length;

            $('#total').text(total);
            $('#boys').text(boys);
            $('#girls').text(girls);
            $('#high').text(high);
            $('#med').text(med);
            $('#low').text(low);

            $('#pboys').text((boys / total * 100).toFixed(0) + '%');
            $('#pgirls').text((girls / total * 100).toFixed(0) + '%');
            $('#phigh').text((high / total * 100).toFixed(0) + '%');
            $('#pmed').text((med / total * 100).toFixed(0) + '%');
            $('#plow').text((low / total * 100).toFixed(0) + '%');
        }
    </script>
    
    <div id="float_box">
        <img id="close_float" src="/Content/12-em-cross.png" />
        <table>
        <tr><td></td><th colspan="2">Checked</td><th colspan="2">Total</td></tr>
        <tr><td>Girls</td><td id="girls"></td><td id="pgirls"></td><td id="tgirls"></td><td id="tpgirls"></td></tr>
        <tr><td>Boys</td><td id="boys"></td><td id="pboys"></td><td id="tboys"></td><td id="tpboys"></td></tr>
        <tr><td colspan="5"><hr /></td></tr>
        <tr><td>High</td><td id="high"></td><td id="phigh"></td><td id="thigh"></td><td id="tphigh"></td></tr>
        <tr><td>Medium</td><td id="med"></td><td id="pmed"></td><td id="tmed"></td><td id="tpmed"></td></tr>
        <tr><td>Low</td><td id="low"></td><td id="plow"></td><td id="tlow"></td><td id="tplow"></td></tr>
        <tr><td colspan="5"><hr /></td></tr>
        <tr><td>Total</td><td id="total"></td><td></td><td id="ttotal"></td><td></td></tr>
        </table>
    </div>
    <form id="form" method="post" action="/Promotion/AssignPending">
    <div class="modalPopup">
       Promotion: <%=Html.DropDownList("PromotionId", Model.Promotions())%>
       Schedule: <%=Html.DropDownList("ScheduleId", Model.Schedules())%>&nbsp;&nbsp;&nbsp;
       <%=Html.CheckBox("FilterUnassigned") %> Unassigned Only
       <%=Html.CheckBox("NormalMembersOnly")%> Exclude Teachers<br />
       Target Class: <%=Html.DropDownList("TargetClassId", Model.TargetClasses()) %>
       <%=Html.SubmitButton("submit", "Assign to Class") %>
    <%=Html.Hidden("Sort", Model.Sort) %>
    <%=Html.Hidden("Dir", Model.Dir) %>
    <table id="Promotions">
        <thead>
        <tr>
            <th><a href="#" class="sortable">Mixed</a></th>
            <th><a href="#" class="sortable">Gender</a></th>
            <th colspan="2"><a href="#" class="sortable">Attendance</a></th>
            <th><a href="#" class="sortable">Name</a></th>
            <th><a href="#" class="sortable">CurrentClass</a></th>
            <th><a href="#" class="sortable">PendingClass</a></th>
            <th><a href="#" class="sortable">Birthday</a></th>
        </tr>
        </thead>
        <tbody>
        <% Html.RenderPartial("List", Model); %>
        </tbody>
    </table>
    </form>
    
    <form id="form1" method="post" action="/Promotion/EasyButton">
    <%=Html.Hidden("PromotionId")%>
    <input name="easybutton" type="image" align="top" src="/Content/Easy.gif" style="vertical-align:bottom;width: 41px; height: 41px" value="easybutton" /></div>Assigns 
    Promotions 
    </form>
</asp:Content>
