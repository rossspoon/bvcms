// code modified from from Luis Ramirez 2008

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using UtilityExtensions;

namespace CustomControls
{
    public class PagerField : DataPagerField
    {
        private int startRowIndex;
        private int maximumRows;
        private int totalRowCount;

        public event EventHandler GoNextPrev;

        public PagerField()
        {
        }

        private string _NextPageText = "Next";
        public string NextPageText
        {
            get { return _NextPageText; }
            set { _NextPageText = value; }
        }
        private string _PreviousPageText = "Prev";
        public string PreviousPageText
        {
            get { return _PreviousPageText; }
            set { _PreviousPageText = value; }
        }
        public string NextPageImageUrl { get; set; }
        public string PreviousPageImageUrl { get; set; }


        [CssClassProperty]
        public string ButtonCssClass { get; set; }

        private bool EnablePreviousPage
        {
            get { return (this.startRowIndex > 0); }
        }

        private bool EnableNextPage
        {
            get { return ((this.startRowIndex + this.maximumRows) < this.totalRowCount); }
        }

        public override void CreateDataPagers(DataPagerFieldItem container, int startrow, int rowsperpage, int totalrows, int fieldIndex)
        {
            startRowIndex = startrow;
            maximumRows = rowsperpage;
            totalRowCount = totalrows;

            if (string.IsNullOrEmpty(DataPager.QueryStringField))
                CreateDataPagersForCommand(container, fieldIndex);
            else
                CreateDataPagersForQueryString(container, fieldIndex);
        }

        protected override DataPagerField CreateField()
        {
            return new PagerField();
        }

        public override void HandleEvent(CommandEventArgs e)
        {
            if (e.CommandName == "UpdatePageSize")
            {
                int pgsz = int.Parse(e.CommandArgument.ToString());
                Util.SetPageSizeCookie(pgsz);
                DataPager.PageSize = pgsz;
                DataPager.SetPageProperties(startRowIndex, pgsz, true);
                return;
            }

            if (e.CommandName == "GoToItem")
            {
                int newStartRowIndex;
                if (int.TryParse(e.CommandArgument.ToString(), out newStartRowIndex))
                    DataPager.SetPageProperties(newStartRowIndex - 1, DataPager.PageSize, true);
                return;
            }

            if (!DataPager.QueryStringField.HasValue())
            {
                if (e.CommandName == "Prev")
                {
                    if (GoNextPrev != null)
                        GoNextPrev(this, e);
                    int startrow = startRowIndex - DataPager.PageSize;
                    if (startrow < 0)
                        startrow = 0;
                    DataPager.SetPageProperties(startrow, DataPager.PageSize, true);
                }
                else if (e.CommandName == "Next")
                {
                    if (GoNextPrev != null)
                        GoNextPrev(this, e);
                    int nextstart = startRowIndex + DataPager.PageSize;

                    if (nextstart > totalRowCount)
                        nextstart = totalRowCount - DataPager.PageSize;

                    if (nextstart < 0)
                        nextstart = 0;

                    DataPager.SetPageProperties(nextstart, DataPager.PageSize, true);
                }
            }
        }

        private void CreateDataPagersForCommand(DataPagerFieldItem container, int fieldIndex)
        {
            CreateLabelRecordControl(container);
            CreateGoToTexBox(container);
            CreatePageSizeControl(container);
            container.Controls.Add(CreateControl("Prev", PreviousPageText, fieldIndex, PreviousPageImageUrl));
            AddNonBreakingSpace(container);
            container.Controls.Add(CreateControl("Next", NextPageText, fieldIndex, NextPageImageUrl));
        }

        private Control CreateControl(string commandName, string buttonText, int fieldIndex, string imageUrl)
        {
            var ib = new ImageButton();
            ib.ImageUrl = imageUrl;
            ib.AlternateText = HttpUtility.HtmlDecode(buttonText);

            ((IButtonControl)ib).Text = buttonText;
            ib.CommandName = commandName;
            ib.CommandArgument = fieldIndex.ToString(CultureInfo.InvariantCulture);
            if (ib != null && !ButtonCssClass.HasValue())
                ib.CssClass = ButtonCssClass;
            return ib;
        }

        private void AddNonBreakingSpace(DataPagerFieldItem container)
        {
            container.Controls.Add(new LiteralControl("&nbsp;"));
        }

        private void CreateLabelRecordControl(DataPagerFieldItem container)
        {
            int endRowIndex = startRowIndex + DataPager.PageSize;

            if (endRowIndex > totalRowCount)
                endRowIndex = totalRowCount;

            container.Controls.Add(new LiteralControl("{0:N0}-{1:N0} of <b>{2:N0}</b>".Fmt(startRowIndex + 1, endRowIndex, totalRowCount)));

            AddNonBreakingSpace(container);
            AddNonBreakingSpace(container);
            AddNonBreakingSpace(container);
        }

        private void CreatePageSizeControl(DataPagerFieldItem container)
        {
            container.Controls.Add(new LiteralControl("Show rows: "));

            var dd = new CommandDropDownList();

            dd.CommandName = "UpdatePageSize";

            dd.Items.Add(new ListItem("10"));
            dd.Items.Add(new ListItem("25"));
            dd.Items.Add(new ListItem("50"));
            dd.Items.Add(new ListItem("75"));
            dd.Items.Add(new ListItem("100"));
            dd.Items.Add(new ListItem("200"));

            var pageSizeItem = dd.Items.FindByValue(base.DataPager.PageSize.ToString());

            if (pageSizeItem == null)
            {
                pageSizeItem = new ListItem(DataPager.PageSize.ToString());
                dd.Items.Insert(0, pageSizeItem);
            }

            pageSizeItem.Selected = true;
            container.Controls.Add(dd);

            AddNonBreakingSpace(container);
            AddNonBreakingSpace(container);
        }

        private void CreateGoToTexBox(DataPagerFieldItem container)
        {
            var label = new Label();
            label.Text = "Go to row: ";
            container.Controls.Add(label);

            var goToTextBox = new CommandTextBox();
            goToTextBox.CommandName = "GoToItem";
            goToTextBox.Width = new Unit("20px");
            container.Controls.Add(goToTextBox);

            AddNonBreakingSpace(container);
            AddNonBreakingSpace(container);
        }

        private void CreateDataPagersForQueryString(DataPagerFieldItem container, int fieldIndex)
        {
            bool validPageIndex = false;
            if (!QueryStringHandled)
            {
                int num;
                QueryStringHandled = true;
                if (int.TryParse(base.QueryStringValue, out num))
                {
                    num--;
                    int currentPageIndex = startRowIndex / maximumRows;
                    int maxPageIndex = (totalRowCount - 1) / maximumRows;
                    if ((num >= 0) && (num <= maxPageIndex))
                    {
                        startRowIndex = num * maximumRows;
                        validPageIndex = true;
                    }
                }
            }

            CreateGoToTexBox(container);
            CreatePageSizeControl(container);
            CreateLabelRecordControl(container);

            int pageIndex = (startRowIndex / maximumRows) - 1;
            container.Controls.Add(CreateLink(PreviousPageText, pageIndex, PreviousPageImageUrl, EnablePreviousPage));
            AddNonBreakingSpace(container);
            int pagenum = (startRowIndex + maximumRows) / maximumRows;
            container.Controls.Add(CreateLink(NextPageText, pagenum, NextPageImageUrl, EnableNextPage));
            AddNonBreakingSpace(container);
            if (validPageIndex)
                DataPager.SetPageProperties(startRowIndex, maximumRows, true);
        }

        private HyperLink CreateLink(string buttonText, int pageIndex, string imageUrl, bool enabled)
        {
            int pageNumber = pageIndex + 1;
            var link = new HyperLink();
            link.Text = buttonText;
            link.NavigateUrl = GetQueryStringNavigateUrl(pageNumber);
            link.ImageUrl = imageUrl;
            link.Enabled = enabled;
            if (!string.IsNullOrEmpty(ButtonCssClass))
                link.CssClass = ButtonCssClass;
            return link;
        }
    }
}