using BusinessLogic.Exceptions;
using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class CardsService
{
    public static readonly int DeckSize = 4;
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
            throw;
            
        }
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
            if (cards.Count != DeckSize  || !Comparator.Unique(cards)) 
                throw new InvalidDeckException("The provided deck did not include the required amount of cards");
            
            var user = _userRepository.GetUserByUsername(username)!;
            
            if(!_cardsRepository.ValidateDeckForConfiguration(user.Id, CardsMapper.MapToDaoList(cards), DeckSize))
                throw new InvalidCardException("At least one of the provided cards does not belong to the user or is not available.");
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