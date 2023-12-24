using BusinessLogic.Mapper;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class CardsService
{
    private const int DeckSize = 4;
    private readonly CardsRepository _cardsRepository = new CardsRepository();
    private readonly UserRepository _userRepository = new UserRepository();
    
    public List<CardDto> GetAllCards(string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            return CardsMapper.MapToDtoList(_cardsRepository.GetAllCards(user!.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return new List<CardDto>();
    }

    public List<CardDto> GetDeck(string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            return CardsMapper.MapToDtoList(_cardsRepository.GetDeck(user!.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
        return new List<CardDto>();
    }

    public void ConfigureDeck(string username, List<CardDto> cards)
    {
        try
        {
            if (cards.Count != 4  /*|| Comparator.Unique(cards)*/) 
                throw new ArgumentException("The deck must contain 4 different cards.");
            
            var user = _userRepository.GetUserByUsername(username)!;
            
            if(!_cardsRepository.ValidateDeckForConfiguration(user.Id, CardsMapper.MapToDaoList(cards), DeckSize))
                throw new ArgumentException("At least one of the provided cards does not belong to the user or is not available.");
            _cardsRepository.ResetDeck(user.Id);
            _cardsRepository.ConfigureDeck(user.Id, CardsMapper.MapToDaoList(cards));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}