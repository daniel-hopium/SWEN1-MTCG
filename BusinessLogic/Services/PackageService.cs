using System.Text.RegularExpressions;
using BusinessLogic.Mapper;
using DataAccess.Daos;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class PackageService
{
    private PackageRepository _packageRepository = new PackageRepository();
    private CardsRepository _cardsRepository = new CardsRepository();
    private UserRepository _userRepository = new UserRepository();
    
    public void CreatePackage(List<CardDto> cards)
    {
        var packageId = Guid.NewGuid();
        
        try
        {
            var parsedCards = ParseCards(cards);
            _cardsRepository.CreateCards(CardsMapper.MapToDaoList(parsedCards));
            _packageRepository.CreatePackage(CardsMapper.MapToDaoList(cards), packageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    public void BuyCardPackage(PackageDto dtoPackage, string username)
    {
        var user = _userRepository.GetUserByUsername(username);
        // Check if packages exist, check user money, transaction
        Console.WriteLine(user.Coins);
        var daoPackage = new PackageDao();
        try
        {
            if (dtoPackage == null || dtoPackage.Id == Guid.Empty)
                daoPackage = _packageRepository.FindLastestPackage();
            // package if empty take the first or last?
            else
                daoPackage = _packageRepository.FindPackage(dtoPackage.Id);
            
            if (daoPackage == null)
                throw new Exception("Package does not exist.");
            if (user.Coins < daoPackage.Price)
                throw new Exception("User does not have enough money to buy this package.");
            _userRepository.UpdateUserMoney(user, daoPackage.Price);
            _packageRepository.AddPackageToUser(user, daoPackage);
            _packageRepository.DeletePackage(daoPackage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private List<CardDto> ParseCards(List<CardDto> cards)
    {
        foreach (var card in cards)
        {
            string[] splitStrings = Regex.Split(card.Name, @"(?<!^)(?=[A-Z])");
            
            if (splitStrings.Length == 2)
            {
                card.ElementType = splitStrings[0];
                card.CardType = splitStrings[1];
            }
            else
            {
                card.CardType = card.Name;
                card.ElementType = "None";
            }
        }
        return cards;
    }
}