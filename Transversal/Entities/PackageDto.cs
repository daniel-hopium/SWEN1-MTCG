namespace Transversal.Entities;

public class PackageDto
{
    public Guid Id { get; set; }
    public List<CardDto> Cards { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}