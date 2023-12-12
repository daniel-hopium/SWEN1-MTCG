using BusinessLogic.Mapper;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class PackageService
{
    private PackageRepository _packageRepository = new PackageRepository();
    private CardsRepository _cardsRepository = new CardsRepository();
    private UserRepository _userRepository = new UserRepository();
    
    
    public void CreatePackage(List<CardsDto> cards)
    {
        var packageId = Guid.NewGuid();
        
        try
        {
            _cardsRepository.CreateCards(CardsMapper.MapToDaoList(cards));
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
        try
        {
            var daoPackage = _packageRepository.FindPackage(dtoPackage.Id);
            if (daoPackage == null)
                throw new Exception("Package does not exist.");
            if (user.Coins < daoPackage.Price)
                throw new Exception("User does not have enough money to buy this package.");
            
            _userRepository.UpdateUserMoney(user, daoPackage.Price);
            _packageRepository.AddPackageToUser(user, daoPackage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}