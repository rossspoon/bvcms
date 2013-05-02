class RegisterEvent:

  def AddToSmallGroup(self, smallgroup, orgmember):
    id = GetCampusId(smallgroup)
    if id < 99:
      orgmember.Person.CampusId = id
      # model gets passed in to the script for your use
      model.CreateTask(819918, orgmember.Person, "Please Contact about " + smallgroup)

def GetCampusId(campus):
  if campus == 'East Side':
    return 3
  elif campus == 'West Side': 
    return 2
  elif campus == 'Downtown':
    return 4    
  else:
    # default if campus not found
    return 99