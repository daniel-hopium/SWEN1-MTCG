﻿using System.Text.RegularExpressions;
using BusinessLogic.Exceptions;
using BusinessLogic.Mapper;
using DataAccess.Daos;
using DataAccess.Repository;
using Transversal.Entities;
using Transversal.Utils;

namespace BusinessLogic.Services;

public class PackageService
{
    private readonly PackageRepository _packageRepository = new PackageRepository();
    private readonly CardsRepository _cardsRepository = new CardsRepository();
    private readonly UserRepository _userRepository = new UserRepository();
    
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
            Log.Error($"An error occured while creating a package with ID {packageId}", e);
            throw;
        }
    }


    public void BuyPackage(PackageDto? dtoPackage, string username)
    {
        var user = _userRepository.GetUserByUsername(username)!;
        try
        {
            PackageDao daoPackage;
            if (dtoPackage == null || dtoPackage.Id == Guid.Empty)
                daoPackage = _packageRepository.FindLastestPackage();
            else
                daoPackage = _packageRepository.FindPackage(dtoPackage.Id);
            
            if (daoPackage == null)
                throw new InvalidOperationException("Package does not exist.");
            if (user.Coins < daoPackage.Price)
                throw new InsufficientFundsException("User does not have enough money to buy this package.");
            _userRepository.UpdateUserMoney(user, daoPackage.Price);
            _packageRepository.AddPackageToUser(user, daoPackage);
            _packageRepository.DeletePackage(daoPackage);
        }
        catch (Exception e)
        {
            Log.Error("An error occured while buying a package", e);
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
                card.ElementType = splitStrings[0].ToLower();
                card.CardType = splitStrings[1].ToLower();
            }
            else
            {
                card.CardType = card.Name.ToLower();
                card.ElementType = "regular";
            }
        }
        return cards;
    }
}