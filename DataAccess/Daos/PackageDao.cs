namespace DataAccess.Daos;

public class PackageDao
{
    public Guid Id { get; set; }
    public List<CardsDao> Cards { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Price: {Price}, Cards: [{string.Join(", ", Cards)}]";
    }
}