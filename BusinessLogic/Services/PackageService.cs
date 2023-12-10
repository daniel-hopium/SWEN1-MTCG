using Transversal.Entities;

namespace BusinessLogic.Services;

public class PackageService
{
    public void CreatePackage(List<CardsDto> cards)
    {
        foreach (var card in cards)
        {
            Console.WriteLine(card.Name);
        }
    }
}