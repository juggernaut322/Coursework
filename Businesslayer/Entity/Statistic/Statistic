namespace BusinessLogicLayer.Entity.Stats;
[Serializable]
public class Statistic : List<string>
{
    public void Add(User user, Mark mark)
    {
        string res = user.ToString() + " : " + mark.ToString() + "\n";
        Add(res);
    }
    public void AddRange(IDictionary<User, Mark> dict)
    {
        foreach(var item in dict)
        {
            Add(item.Key, item.Value);
        }
    }

    public Statistic GetStatsByUser(User user)
    {
        string userString = user.ToString();

        var listStats = from s in this
                        where s.StartsWith(userString)
                        select s;

        Statistic userStats = new();
        userStats.AddRange(listStats);

        //if we didnt find any entry throw exception
        return userStats.Count != 0 ? userStats : throw new Exception("No entry for this user!");
    }
    public Statistic GetStatsByDate(DateTime time)
    {
        string dateString = time.ToString("dd.MM.yyyy");

        var listStats = from s in this
                        where s.Contains(dateString)
                        select s;

        Statistic dateStats = new();
        dateStats.AddRange(listStats);

        //if we didnt find any entry throw exception
        return dateStats.Count != 0 ? dateStats : throw new Exception("No entry for this date!");
    }

    public override string ToString()
    {
        string res = "";
        foreach(var s in this)
        {
            res += s;
        }
        return res;
    }
}
