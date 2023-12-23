using System.Xml.Serialization;
namespace DataAccess;
public class XMLProvider : DataProvider
{
    readonly Type[]? _param;
    public XMLProvider(Type type)
        : base(type)
    {
    }
    public XMLProvider(Type type, Type[] param)
        : base(type)
    {
        _param = param;
    }
    public override void Serialize(object graph, string filePath)
    {
        using(FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            XmlSerializer xs;
            xs = _param is null ? new XmlSerializer(_type) : new XmlSerializer(_type, _param);
            xs.Serialize(fileStream, graph);
        }
    }
    public override object? Deserialize(string filePath)
    {
        object? graph;
        using(FileStream fileStream = File.OpenRead(filePath))
        {
            XmlSerializer xs;
            try
            {
                xs = _param is null ? new XmlSerializer(_type) : new XmlSerializer(_type, _param);

                graph = xs.Deserialize(fileStream);
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
