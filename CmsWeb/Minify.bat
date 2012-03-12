
path=%path%;c:\Program Files (x86)\Microsoft\Microsoft Ajax Minifier

ajaxmin ^
Content/js/jquery-1.7.1.js ^
Content/js/jquery-ui-1.8.18.custom.js ^
Content/js/jquery.bgiframe-2.1.2.js ^
Content/js/jquery.hoverIntent.js ^
Content/js/jquery.cookie.js ^
Content/js/jquery.blockUI.js ^
Content/js/jquery.tooltip.js ^
Content/js/jquery.mousewheel.js ^
Content/js/jquery.jscrollpane.js ^
Content/js/jQuery.autocomplete.js ^
Content/js/jquery.jeditable.js ^
Content/js/jquery.validate.js ^
Scripts/ExportToolBar.js ^
Scripts/menu.js ^
Scripts/Pager.js ^
Scripts/headermenu.js ^
-o Min/Content/js/combined.js -clobber:true || pause

ajaxmin ^
Content/js/jquery.multiSelect.js ^
Content/js/jquery-ui-timepicker-addon.js ^
Content/js/jquery.textarea.js ^
Content/js/jquery-ui-dialog-patch.js ^
Scripts/Organization.js ^
Scripts/RegSetting.js ^
-o Min/Content/js/combined-org.js -clobber:true || pause

ajaxmin ^
Content/js/jquery.pagination.js ^
Scripts/SearchPeople.js ^
Scripts/Person.js ^
-o Min/Content/js/combined-person.js -clobber:true || pause

ajaxmin ^
Content/js/jquery-1.7.1.js ^
Content/js/jquery-ui-1.8.18.custom.js ^
Content/js/jquery.validate.js ^
Content/js/jquery.idle-timer.js ^
Content/js/jquery.blockUI.js ^
Content/js/jquery.tooltip.js ^
Content/js/jquery.showpassword.js ^
-o Min/Content/js/combined-onlinereg.js -clobber:true || pause

ajaxmin ^
Content/jquery-ui-1.8.18.custom.css ^
Content/site.css ^
Content/style2.css ^
Content/cmenu.css ^
Content/pager.css ^
Content/jquery.jscrollpane.css ^
Content/jquery.tooltip.css ^
Content/jquery.autocomplete.css ^
-o Content/combined.css -clobber:true || pause

ajaxmin Content/js/jquery-1.7.1.js -o Min\Content/js/jquery-1.7.1.js -clobber:true || pause
ajaxmin Content/js/jquery-ui-1.8.18.custom.js -o Min\Content/js/jquery-ui-1.8.18.custom.js -clobber:true || pause
ajaxmin Content/js/jquery.contextMenu.js -o Min\Content/js/jquery.contextMenu.js -clobber:true || pause
ajaxmin Content/js/jquery.form.js -o Min\Content/js/jquery.form.js -clobber:true || pause
ajaxmin Content/js/jquery.form2.js -o Min\Content/js/jquery.form2.js -clobber:true || pause
ajaxmin Content/js/jquery.idle-timer.js -o Min\Content/js/jquery.idle-timer.js -clobber:true || pause
ajaxmin Content/js/jquery.multiSelect.js -o Min\Content/js/jquery.multiSelect.js -clobber:true || pause
ajaxmin Content/js/jquery.pagination.js -o Min\Content/js/jquery.pagination.js -clobber:true || pause
ajaxmin Content/js/jquery.showpassword.js -o Min\Content/js/jquery.showpassword.js -clobber:true || pause
ajaxmin Content/js/jquery.sortElements.js -o Min\Content/js/jquery.sortElements.js -clobber:true || pause
ajaxmin Content/js/jquery.textarea.js -o Min\Content/js/jquery.textarea.js -clobber:true || pause
ajaxmin Content/js/jquery.transpose.js -o Min\Content/js/jquery.transpose.js -clobber:true || pause
ajaxmin Content/js/jquery-ui-timepicker-addon.js -o Min\Content/js/jquery-ui-timepicker-addon.js -clobber:true || pause

ajaxmin Scripts/CheckIn.js -o Min/Scripts/CheckIn.js -clobber:true || pause
ajaxmin Scripts/ContactSearch.js -o Min\Scripts/ContactSearch.js -clobber:true || pause
ajaxmin Scripts/Contributions.js -o Min\Scripts/Contributions.js -clobber:true || pause
ajaxmin Scripts/Divisions.js -o Min\Scripts/Divisions.js -clobber:true || pause
ajaxmin Scripts/Funds.js -o Min\Scripts/Funds.js -clobber:true || pause
ajaxmin Scripts/Meetings.js -o Min\Scripts/Meetings.js -clobber:true || pause
ajaxmin Scripts/OnlineReg.js -o Min\Scripts/OnlineReg.js -clobber:true || pause
ajaxmin Scripts/OnlineRegPayment.js -o Min\Scripts/OnlineRegPayment.js -clobber:true || pause
ajaxmin Scripts/OrgChildren.js -o Min\Scripts/OrgChildren.js -clobber:true || pause
ajaxmin Scripts/OrgGroups.js -o Min\Scripts/OrgGroups.js -clobber:true || pause
ajaxmin Scripts/OrgMemberDialog.js -o Min\Scripts/OrgMemberDialog.js -clobber:true || pause
ajaxmin Scripts/OrgMembers.js -o Min\Scripts/OrgMembers.js -clobber:true || pause
ajaxmin Scripts/OrgMembersDialog.js -o Min\Scripts/OrgMembersDialog.js -clobber:true || pause
ajaxmin Scripts/OrgSearch.js -o Min\Scripts/OrgSearch.js -clobber:true || pause
ajaxmin Scripts/Organization.js -o Min\Scripts/Organization.js -clobber:true || pause
ajaxmin Scripts/Person.js -o Min\Scripts/Person.js -clobber:true || pause
ajaxmin Scripts/PostBundle.js -o Min\Scripts/PostBundle.js -clobber:true || pause
ajaxmin Scripts/QueryBuilder.js -o Min\Scripts/QueryBuilder.js -clobber:true || pause
ajaxmin Scripts/QuickSearch.js -o Min\Scripts/QuickSearch.js -clobber:true || pause
ajaxmin Scripts/RegSetting.js -o Min\Scripts/RegSetting.js -clobber:true || pause
ajaxmin Scripts/SavedQuery.js -o Min\Scripts/SavedQuery.js -clobber:true || pause
ajaxmin Scripts/SearchAdd.js -o Min\Scripts/SearchAdd.js -clobber:true || pause
ajaxmin Scripts/SearchDivisions.js -o Min\Scripts/SearchDivisions.js -clobber:true || pause
ajaxmin Scripts/SearchOrgs.js -o Min\Scripts/SearchOrgs.js -clobber:true || pause
ajaxmin Scripts/SearchPeople.js -o Min\Scripts/SearchPeople.js -clobber:true || pause
ajaxmin Scripts/SearchUsers.js -o Min\Scripts/SearchUsers.js -clobber:true || pause
ajaxmin Scripts/Tags.js -o Min\Scripts/Tags.js -clobber:true || pause
ajaxmin Scripts/Task.js -o Min\Scripts/Task.js -clobber:true || pause
ajaxmin Scripts/TestAPI.js -o Min\Scripts/TestAPI.js -clobber:true || pause
ajaxmin Scripts/TransactionHistory.js -o Min\Scripts/TransactionHistory.js -clobber:true || pause
ajaxmin Scripts/UserDialog.js -o Min\Scripts/UserDialog.js -clobber:true || pause
ajaxmin Scripts/meeting.js -o Min\Scripts/meeting.js -clobber:true || pause
ajaxmin Scripts/ManageVolunteer.js -o Min\Scripts/ManageVolunteer.js -clobber:true || pause
ajaxmin Scripts/TotalsByFund.js -o Min\Scripts/TotalsByFund.js -clobber:true || pause
ajaxmin Scripts/merge.js -o Min\Scripts/merge.js -clobber:true || pause
ajaxmin Scripts/pickslots2.js -o Min\Scripts/pickslots2.js -clobber:true || pause
ajaxmin Content/js/jquery.checkboxtree.js -o Min/content/js/jquery.checkboxtree.js -clobber:true || pause
ajaxmin Scripts/meeting-ipad.js -o Min/scripts/meeting-ipad.js -clobber:true || pause
ajaxmin Scripts/email.js -o Min/scripts/email.js -clobber:true || pause
ajaxmin Scripts/edit.js -o Min/scripts/edit.js -clobber:true || pause
