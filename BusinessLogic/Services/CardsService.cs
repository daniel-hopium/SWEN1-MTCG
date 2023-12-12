﻿using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using NUnit.Framework;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class CardsService
{
    private const int DECK_SIZE = 4;

    CardsRepository _cardsRepository = new CardsRepository();
    UserRepository _userRepository = new UserRepository();
    
    public List<CardsDto> GetAllCards(string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            return CardsMapper.MapToDtoList(_cardsRepository.GetAllCards(user.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return new List<CardsDto>();
    }

    public List<CardsDto> GetDeck(string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            return CardsMapper.MapToDtoList(_cardsRepository.GetDeck(user.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
        return new List<CardsDto>();
    }

    public void ConfigureDeck(string username, List<CardsDto> cards)
    {
        try
        {
            if (cards.Count != 4  || Comparator.Unique(cards))
                throw new ArgumentException("The deck must contain 4 different cards.");
            
            var user = _userRepository.GetUserByUsername(username);
            
            if(!_cardsRepository.ValidateDeckForConfiguration(user.Id, CardsMapper.MapToDaoList(cards), DECK_SIZE))
               throw new ArgumentException("At least one of the provided cards does not belong to the user or is not available.");
            _cardsRepository.ResetDeck(user.Id);
            _cardsRepository.ConfigureDeck(user.Id, CardsMapper.MapToDaoList(cards));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
    }
}