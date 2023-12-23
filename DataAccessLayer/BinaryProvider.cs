using System.Runtime.Serialization.Formatters.Binary;

namespace DataAccess;
public class BinaryProvider : DataProvider
{
    public BinaryProvider(Type type)
        : base(type)
    {
    }
    public override void Serialize(object graph, string filePath)
    {
        using(FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new();
            bf.Serialize(fileStream, graph);
        }
    }
    public override object? Deserialize(string filePath)
    {
        object? graph;
        using(FileStream fileStream = File.OpenRead(filePath))
        {
            BinaryFormatter bf = new();
            try
            {
                graph = bf.Deserialize(fileStream);
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
