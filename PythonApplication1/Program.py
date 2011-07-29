# Tests
import System
import clr
clr.AddReferenceByName("CmsData")
from CmsData import *
from System import *
from MembershipAutomation2 import *
from constants import *

def ShowReturn(p):
    System.Console.WriteLine("{0}", p.errorReturn);
def ShowMessage(o):
    System.Console.WriteLine("{0}", o);

def ResetDb():
    return CMSDataContext('Data Source=.;Initial Catalog=CMS_bellevue;Integrated Security=True')

#def TestInit(Db):
#    per = Db.LoadPersonById(828612)
#    per.DecisionTypeId = None
#    per.DecisionDate = None
#    per.JoinCodeId = JoinTypeCode.Unknown
#    per.MemberStatusId = MemberStatusCode.NotMember
#    per.BaptismTypeId = None
#    per.BaptismStatusId = None
#    per.DropCodeId = DropTypeCode.NotDropped
#    per.NewMemberClassStatusId = None
#    per.NewMemberClassDate = None
#    per.JoinDate = None
#    per.BaptismDate = None
#    per.BaptismSchedDate = None
#    per.DropDate = None
#    Db.SubmitChanges()
#    return per

m = MembershipAutomation()
Db = ResetDb()
p = Db.LoadPersonById(25791)
p.DropCodeId = Codes.DropTypeCode.LetteredOut
p.DropDate = DateTime.Parse("7/20/11")
m.Run(Db, p)

#def Test1():
#    Db = ResetDb()
#    p = TestInit(Db)
#    p.DecisionTypeId = DecisionCode.ProfessionForMembership
#    m.Run(Db, p)
#    ShowMessage("After Test1 Automation--------------")
#    ShowReturn(p)
#    ShowMessage(p.MemberStatus.Description)
#    ShowMessage(p.NewMemberClassStatus.Description)

#def Test2():
#    Db = ResetDb()
#    p = TestInit(Db) # clean slate
#    # now pretend we are at the following state
#    p.NewMemberClassStatusId = Codes.NewMemberClassStatusCode.ExemptedChild
#    dt = DateTime.Parse("6/26/11")
#    p.NewMemberClassDate = dt
#    p.DecisionDate = dt
#    p.DecisionTypeId = DecisionCode.ProfessionForMembership
#    Db.SubmitChanges()
#    Db.Dispose()
#    Db = ResetDb()
#    # now pretend we are on the tab entering this info
#    p = Db.LoadPersonById(828612)
#    p.BaptismStatusId = BaptismStatusCode.Completed
#    m.Run(Db, p)
#    ShowMessage("After Test2 Automation--------------")
#    ShowReturn(p)
#    ShowMessage(p.MemberStatus.Description)

#def Test3():
#    Db = ResetDb()
#    p = TestInit(Db) # clean slate
#    p.DecisionTypeId = DecisionCode.Statement
#    dt = DateTime.Parse("6/26/11")
#    p.DecisionDate = dt
#    m.Run(Db, p)
#    ShowMessage("After Test3 Automation--------------")
#    ShowReturn(p)
#    ShowMessage(p.MemberStatus.Description)
#    ShowMessage(p.NewMemberClassStatus.Description)

#def Test4():
#    Db = ResetDb()
#    p = TestInit(Db) # clean slate
#    p.DecisionTypeId = DecisionCode.ProfessionForMembership
#    dt = DateTime.Parse("6/19/11")
#    p.DecisionDate = dt
#    m.Run(Db, p)
#    ShowMessage("After Test4 Automation setup--------------")
#    ShowReturn(p)
#    ShowMessage(p.DecisionType.Description)
#    ShowMessage(p.MemberStatus.Description)
#    ShowMessage(p.NewMemberClassStatus.Description)

#    Db.SubmitChanges()
#    Db.Dispose()
#    Db = ResetDb()
#    p = Db.LoadPersonById(828612)

#    p.BaptismStatusId = BaptismStatusCode.Completed
#    m.Run(Db, p)
#    ShowMessage("After Test4 Automation b--------------")
#    ShowReturn(p)
#    ShowMessage(p.BaptismStatus.Description)
#    ShowMessage(p.DecisionType.Description)
#    ShowMessage(p.MemberStatus.Description)
#    ShowMessage(p.NewMemberClassStatus.Description)

#    Db.SubmitChanges()
#    Db.Dispose()
#    Db = ResetDb()
#    p = Db.LoadPersonById(828612)

#    p.BaptismDate = dt.AddDays(1)
#    m.Run(Db, p)
#    ShowMessage("After Test4 Automation c--------------")
#    ShowReturn(p)
#    ShowMessage(p.BaptismStatus.Description)
#    ShowMessage(p.DecisionType.Description)
#    ShowMessage(p.MemberStatus.Description)
#    ShowMessage(p.NewMemberClassStatus.Description)

#    Db.SubmitChanges()
#    Db.Dispose()
#    Db = ResetDb()
#    p = Db.LoadPersonById(828612)

#    p.NewMemberClassStatusId = NewMemberClassStatusCode.Attended
#    p.NewMemberClassDate = dt.AddDays(2)
#    m.Run(Db, p)
#    ShowMessage("After Test4 Automation c--------------")
#    ShowReturn(p)
#    ShowMessage(p.BaptismStatus.Description)
#    ShowMessage(p.DecisionType.Description)
#    ShowMessage(p.MemberStatus.Description)
#    ShowMessage(p.NewMemberClassStatus.Description)
#    ShowMessage(p.JoinDate)

#Test1()
#Test2()
#Test3()
#Test4()

System.Console.Write("press any key")
System.Console.ReadKey(True)
