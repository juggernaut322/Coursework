using System.Text.Json;
namespace DataAccess;
public class JSONProvider : DataProvider
{
    public JSONProvider(Type type)
        : base(type)
    {

    }
    public override void Serialize(object graph, string filePath)
    {
        using(FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            JsonSerializer.Serialize(fileStream, graph, _type);
        }
    }
    public override object? Deserialize(string filePath)
    {
        object? graph;
        using(var fileStream = File.OpenRead(filePath))
        {
            try
            {
                graph = JsonSerializer.Deserialize(fileStream, _type);
                return graph;
            }
            catch(FileNotFoundException)
            {
                throw;
            }
            catch
            {
                return null;
            }
        }
    }
}
