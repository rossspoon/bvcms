$(function(){$(".bt").button(),$("#searchvalues select").css("width","100%"),$("#clear").click(function(n){return n.preventDefault(),$("input:text").val(""),$("#memberstatus,#campus").val(0),$("#gender,#marital").val(99),!1}),$("#Name").focus(),$("#search").click(function(n){return n.preventDefault(),$.getTable(),!1}),$("a.bt").bind("contextmenu",function(n){n.preventDefault()}),$("#targetpeople").click(function(n){return n.preventDefault(),$('a.target[target="people"]').length==0?$("a.target").attr("target","people"):$("a.target").removeAttr("target"),!1}),$("#convert").click(function(n){n.preventDefault();var t=$("#results").closest("form"),i=t.serialize();return $.post($("#convert").attr("href"),i,function(n){window.location=n}),!1}),$.gotoPage=function(n,t){return $("#Page").val(t),$.getTable(),!1},$.setPageSize=function(n){return $("#Page").val(1),$("#PageSize").val($(n).val()),$.getTable()},$.getTable=function(){var n=$("#results").closest("form"),t=n.serialize();return $.blockUI(),$.post($("#search").attr("href"),t,function(n){$("#results").replaceWith(n).ready(function(){$("#results > tbody > tr:even").addClass("alt"),$("#totalcount").text($("#totcnt").val()),$.unblockUI()})}),!1},$("#results > tbody > tr:even").addClass("alt"),$("#results > thead a.sortable").live("click",function(n){n.preventDefault();var i=$(this).text(),r=$("#Sort"),t=$("#Direction");return $(r).val()==i&&$(t).val()=="asc"?$(t).val("desc"):$(t).val("asc"),$(r).val(i),$.getTable(),!1}),$("form input").live("keypress",function(n){return n.which&&n.which==13||n.keyCode&&n.keyCode==13?($("#search").click(),!1):!0}),$("a.taguntag").live("click",function(n){n.preventDefault();var t=$(this);return $.post("/Tags/ToggleTag/"+$(this).attr("value"),null,function(t){$(n.target).text(t)}),!1})})