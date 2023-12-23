namespace BusinessLogicLayer.Entity.Stats;
[Serializable]
public class Mark
{
    public const string timeFormat = "dddd, dd.MM.yyyy, H:mm";

    #region Ctors
    public Mark()
    {

    }
    public Mark(double value, DateTime time)
    {
        Value = value;
        Time = time;
    }
    #endregion

    #region Properties
    public double Value { get; set; }
    public DateTime Time { get; set; }
    #endregion

    #region Object
    public override string ToString()
    {
        return $"{Value:f2}/{100:f2} : {Time.ToString(timeFormat)};";
    }
    public override bool Equals(object? obj)
    {
        Mark? mark = obj as Mark;
        if(mark is not null)
        {
            return Value == mark.Value && Time == mark.Time;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode() * Time.GetHashCode();
    }
    #endregion
}
