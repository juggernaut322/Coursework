namespace BusinessLogicLayer.Entity.Stats;
[Serializable]
public class User
{
    #region Ctors
    public User()
    {

    }
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    #endregion

    #region Properties
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    #endregion

    #region Object
    public override string ToString()
    {
        return FirstName + " " + LastName;
    }
    public override bool Equals(object? obj)
    {
        User? user = obj as User;
        if(user is not null)
        {
            return FirstName == user.FirstName && LastName == user.LastName;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return FirstName.GetHashCode() * LastName.GetHashCode();
    }
    #endregion
}
