(function($) {
    var o;
    $.fn.SearchPeople = function(ev, select, options) {
        o = $.extend({
            multi: false
        }, options || {});

        o.Target = ev.target;
        o.Select = select;
        o.SearchClicked = function() {
            o.qs = $('#searchform', o.$this).formSerialize2();
            $.post('/SearchPeople/Rows/0', o.qs, o.LoadRows);
            return false;
        };
        o.SortClicked = function(ev) {
            $("#people #Sort", o.$this).val($(ev.target).text());
            return o.SearchClicked();
        };
        o.LoadRows = function(ret) {
            $('#people > tbody', o.$this).html(ret).ready(function() {
                $("#people", o.$this).SearchPeoplePager();
                o.BindSelect();
            });
        };
        o.LoadRowsPage = function(ret) {
            $('#people > tbody', o.$this).html(ret).ready(function() {
                o.BindSelect();
            });
        };
        o.AddNew = function(ev) {
            if (!confirm("Are you sure you want to add a new person?"))
                return false;
            o.qs = $("#searchform", o.$this).formSerialize2();
            if (ev.target.id != "AddNew")
                o.qs = o.qs.appendQuery("ExistingFamilyMember=" + ev.target.id.substring(1));
            $.post('/SearchPeople/AddNew', o.qs, function(ret) {
                if (ret.err)
                    alert(ret.err);
                else {
                    o.Select(o.Target.id, ret.PeopleId);
                    o.$this.dialog("close");
                }
            }, "json");
            return false;
        };
        o.ClearForm = function(ev) {
            $('#searchform .clearable').clearFields();
            return false;
        };
        o.DisplaySelect = function() {
            if ($("#AddToExisting").attr("checked")) {
                $(".select", o.$this).hide();
                $(".add", o.$this).show();
            }
            else {
                $(".add", o.$this).hide();
                $(".select", o.$this).show();
            }
        };
        o.BindSelect = function() {
            $(".select", o.$this).click(function(ui) {
                o.Select(o.Target.id, ui.target.id.substring(1));
                o.$this.dialog("close");
            });
            $(".add", o.$this).click(o.AddNew);
            o.DisplaySelect();
        };
        o.$this = $(this[0]);
        o.qs = "";
        if (o.entrypoint)
            o.qs = o.qs.appendQuery("entrypoint=" + o.entrypoint);
        if (o.origin)
            o.qs = o.qs.appendQuery("origin=" + o.origin);
        if (o.multi)
            o.qs = o.qs.appendQuery("select=2");
        this.load("/SearchPeople/", o.qs, function() {
            o.qs = $('#searchform', o.$this).formSerialize2();
            $("#people", o.$this).SearchPeoplePager();
            o.BindSelect();
            $("#AddNew", o.$this).click(o.AddNew);
            $("#ClearForm", o.$this).click(o.ClearForm);
            $("#AddToExisting", o.$this).click(o.DisplaySelect);
            $('a.sortable', o.$this).click(o.SortClicked);
            $('#Search', o.$this).click(o.SearchClicked);
        });
        this.dialog("open");
        return this;
    };
    $.fn.SearchPeoplePager = function() {
        return this.each(function() {
            $("#PageSize", o.$this).change(o.SearchClicked);
            $(".pagination", o.$this).pagination($("#Count", o.$this).val(), {
                items_per_page: $("#PageSize", o.$this).val(),
                num_display_entries: 5,
                num_edge_entries: 1,
                current_page: 0,
                callback: function(page, pager) {
                    $.post('/SearchPeople/Rows/' + page, o.qs, o.LoadRowsPage);
                    return false;
                }
            });
            $('#NumItems', this).text($('#Count', o.$this).val().addCommas() + " items");
        });
    };
    $.fn.SearchPeopleInit = function(options) {
        var settings = $.extend({
            overlay: { background: "#000", opacity: 0.8 },
            bgiframe: true,
            modal: true,
            autoOpen: false,
            title: 'Search People',
            closeOnEscape: true,
            width: 700,
            height: 525,
            position: 'top',
            close: function(event, ui) {
                $(this).empty();
            }
        }, options || {});
        return this.each(function() {
            $(this).dialog(settings);
        });
    };
})(jQuery);
