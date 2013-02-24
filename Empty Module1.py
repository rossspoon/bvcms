CAMPUSES = {
    "East Side": [731, 3],
    "West Side": [733, 2],
    "Downtown": [158, 4],
    "Do Not Attend": [None, 6]
}

class RegisterEvent:

    def AddToSmallGroup(self, smallgroup, orgmember):
        campus = CAMPUSES.get(smallgroup, None)
        if campus:
            if campus[0]:
                model.JoinOrg(campus[0], orgmember.Person)
            model.UpdateField(orgmember.Person, "CampusId", campus[1])
 