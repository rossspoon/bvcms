# this is an IronPython script for MembershipAutomation in BVCMS
# the variable p has been passed in and is the person that we are saving Member Profile information for

from constants import *

# define all functions first, codes starts running below functions

# don't allow empty join date
def SetJoinDate(name, dt):
    if dt == None:
        p.errorReturn = 'need a ' + name + ' date'
    p.JoinDate = dt;

# this controls the membership status, makes them a member if they have completed the two requirements
def CheckJoinStatus():

    if p.DecisionTypeId == DecisionCodeProfessionForMembership:
        if p.DiscClassStatusCompletedCodes.Contains(p.NewMemberClassStatusId) and \
        p.BaptismStatusId == BaptismStatusCodeCompleted:
            if p.NewMemberClassDate != None and p.BaptismDate != None:
                if p.NewMemberClassDate > p.BaptismDate:
                    SetJoinDate("NewMemberClass", p.NewMemberClassDate)
                else:
                    SetJoinDate("Baptism", p.BaptismDate)
            p.MemberStatusId = MemberStatusCodeMember
            if p.BaptismTypeId == BaptismTypeCodeBiological:
                p.JoinCodeId = JoinTypeCodeBaptismBIO
            else:
                p.JoinCodeId = JoinTypeCodeBaptismPOF

    elif p.DecisionTypeId == DecisionCodeLetter:
        if p.NewMemberClassStatusIdChanged:
            if p.DiscClassStatusCompletedCodes.Contains(p.NewMemberClassStatusId) \
            or p.NewMemberClassStatusId == NewMemberClassStatusCodeAdminApproval:
                p.MemberStatusId = MemberStatusCodeMember
                p.JoinCodeId = JoinTypeCodeLetter
                if p.NewMemberClassDate != None:
                    SetJoinDate("NewMember", p.NewMemberClassDate)
                else:
                    SetJoinDate("Decision", p.DecisionDate)

    elif p.DecisionTypeId == DecisionCodeStatement:
        if p.NewMemberClassStatusIdChanged:
            if p.DiscClassStatusCompletedCodes.Contains(p.NewMemberClassStatusId):
                p.MemberStatusId = MemberStatusCodeMember
                p.JoinCodeId = JoinTypeCodeStatement
                if p.NewMemberClassDate != None:
                    SetJoinDate("NewMember", p.NewMemberClassDate)
                else:
                    SetJoinDate("Decision", p.DecisionDate)

    elif p.DecisionTypeId == DecisionCodeStatementReqBaptism:
        if p.DiscClassStatusCompletedCodes.Contains(p.NewMemberClassStatusId) \
        and p.BaptismStatusId == BaptismStatusCodeCompleted:
            p.MemberStatusId = MemberStatusCodeMember
            p.JoinCodeId = JoinTypeCodeBaptismSRB
            if p.NewMemberClassDate != None:
                 if p.NewMemberClassDate > p.BaptismDate:
                    SetJoinDate("NewMember", p.NewMemberClassDate)
                 else: 
                    SetJoinDate("Baptism", p.BaptismDate)
            else:
                 SetJoinDate("Baptism", p.BaptismDate)

# this moves the membership process along, setting various codes based on decision
def CheckDecisionStatus():

    if p.DecisionTypeId == DecisionCodeProfessionForMembership:
        p.MemberStatusId = MemberStatusCodePending
        if p.NewMemberClassStatusId != NewMemberClassStatusCodeAttended:
            p.NewMemberClassStatusId = NewMemberClassStatusCodePending
        if p.Age <= 12 and p.FamilyHasPrimaryMemberForMoreThanDays(365):
            p.BaptismTypeId = BaptismTypeCodeBiological
        else:
            p.BaptismTypeId = BaptismTypeCodeOriginal
        BaptismStatusId = BaptismStatusCodeNotScheduled

    elif p.DecisionTypeId == DecisionCodeProfessionNotForMembership:
        p.MemberStatusId = MemberStatusCodeNotMember
        if p.NewMemberClassStatusId != NewMemberClassStatusCodeAttended:
            NewMemberClassStatusId = NewMemberClassStatusCodeNotSpecified
        if p.BaptismStatusId != BaptismStatusCodeCompleted:
            p.BaptismTypeId = BaptismTypeCodeNonMember
            p.BaptismStatusId = BaptismStatusCodeNotScheduled

    elif p.DecisionTypeId == DecisionCodeLetter:
        p.MemberStatusId = MemberStatusCodePending
        if p.NewMemberClassStatusId != NewMemberClassStatusCodeAttended:
            p.NewMemberClassStatusId = NewMemberClassStatusCodePending
            if p.BaptismStatusId != BaptismStatusCodeCompleted:
                p.BaptismTypeId = BaptismTypeCodeNotSpecified
                p.BaptismStatusId = BaptismStatusCodeNotSpecified

    elif p.DecisionTypeId == DecisionCodeStatement:
        p.MemberStatusId = MemberStatusCodePending
        if p.NewMemberClassStatusId != NewMemberClassStatusCodeAttended:
            NewMemberClassStatusId = NewMemberClassStatusCodePending
            if p.BaptismStatusId != BaptismStatusCodeCompleted:
                p.BaptismTypeId = BaptismTypeCodeNotSpecified
                p.BaptismStatusId = BaptismStatusCodeNotSpecified

    elif p.DecisionTypeId == DecisionCodeStatementReqBaptism:
        p.MemberStatusId = MemberStatusCodePending
        if p.NewMemberClassStatusId != NewMemberClassStatusCodeAttended:
            p.NewMemberClassStatusId = NewMemberClassStatusCodePending
        if p.BaptismStatusId != BaptismStatusCodeCompleted:
            p.BaptismTypeId = BaptismTypeCodeRequired
            p.BaptismStatusId = BaptismStatusCodeNotScheduled

    elif p.DecisionTypeId == DecisionCodeCancelled:
        p.MemberStatusId = MemberStatusCodeNotMember
        if p.NewMemberClassStatusId != NewMemberClassStatusCodeAttended:
            NewMemberClassStatusId = NewMemberClassStatusCodeNotSpecified
        if p.BaptismStatusId != BaptismStatusCodeCompleted:
            if p.BaptismStatusId != BaptismStatusCodeCompleted:
                p.BaptismTypeId = BaptismTypeCodeNotSpecified
                p.BaptismStatusId = BaptismStatusCodeCanceled
        p.EnvelopeOptionsId = EnvelopeOptionCodeNone

# cleanup for deceased and for dropped memberships
def DropMembership():

    if p.MemberStatusId == MemberStatusCodeMember:
        if p.Deceased:
            p.DropCodeId = DropTypeCodeDeceased
        p.MemberStatusId = MemberStatusCodePrevious
        p.DropDate = p.Now().Date

    if p.Deceased:
        p.EmailAddress = None
        p.DoNotCallFlag = True
        p.DoNotMailFlag = True
        p.DoNotVisitFlag = True

    if p.SpouseId != None:
        spouse = p.LoadSpouse()

        if p.Deceased:
            spouse.MaritalStatusId = MaritalStatusCodeWidowed
            if spouse.EnvelopeOptionsId != None: # not null
                if spouse.EnvelopeOptionsId != EnvelopeOptionCodeNone:
                    spouse.EnvelopeOptionsId = EnvelopeOptionCodeIndividual
            spouse.ContributionOptionsId = EnvelopeOptionCodeIndividual

        if spouse.MemberStatusId == MemberStatusCodeMember:
            if spouse.EnvelopeOptionsId == EnvelopeOptionCodeJoint:
                spouse.EnvelopeOptionsId = EnvelopeOptionCodeIndividual

    p.EnvelopeOptionsId = EnvelopeOptionCodeNone
    p.DropAllMemberships();

#-------------------------------------
# code starts running here
def MembershipAutomation():
    if p.DecisionTypeIdChanged:
        CheckDecisionStatus()

    if p.NewMemberClassStatusIdChanged and p.NewMemberClassDate == None:
        p.errorReturn = "need a NewMemberClass date"

    if p.NewMemberClassStatusIdChanged or p.BaptismStatusIdChanged:
        CheckJoinStatus()

    if p.DeceasedDateChanged:
        if p.DeceasedDate != None:
            DropMembership()

    # when people leave the church, lots of cleanup to do
    if p.DropCodeIdChanged:
        if p.DropCodesThatDrop.Contains(p.DropCodeId):
            DropMembership()

    # this does new member class completed
    if p.NewMemberClassStatusIdChanged \
    and p.NewMemberClassStatusId == NewMemberClassStatusCodeAttended:
        om = Db.LoadOrgMember(p.PeopleId, "Step 1", True) # must exist
        if om != None:
            om.Drop(True) # drops and records drop in history
