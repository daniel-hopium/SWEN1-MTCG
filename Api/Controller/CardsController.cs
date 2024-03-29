﻿using API.HttpServer;
using Api.Utils;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;
using Transversal.Utils;

namespace API.Controller;
public class CardsController
{
    private readonly CardsService _cardsService;
    
    public CardsController(CardsService cardsService)
    {
        _cardsService = cardsService;
    }
    
    public void ProcessRequest(object sender, HttpSvrEventArgs e)
    {
        //Authorization
        if (!Authorization.UserIsAuthorized(e))
            return;
        
        if (e.Path.Equals("/cards") && e.Method.Equals("GET"))
        {
            GetAllCards(e);
        }
        else if (e.Path.StartsWith("/deck") && e.Method.Equals("GET"))
        {
            GetDeck(e);
        }
        else if (e.Path.Equals("/deck") && e.Method.Equals("PUT"))
        {
            ConfigureDeck(e);
        }
    }
    
    private void GetAllCards(HttpSvrEventArgs e)
    {
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);

        try
        {
            var cards = _cardsService.GetAllCards(username);
            
            if (cards.Count == 0)
            {
                e.Reply(204, "");
                return;
            }
            e.Reply(200, JsonConvert.SerializeObject(cards));
        }
        catch 
        {
            e.Reply(500, "Error getting cards");
        }
    }

    private void GetDeck(HttpSvrEventArgs e)
    {
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        
        try
        {
            var cards = _cardsService.GetDeck(username);
            if (cards.Count == 0)
            {
                e.Reply(204, "");
                return;
            }

            if (e.Query.Get("format") == "plain")
                e.Reply(200, string.Join("\n", cards.Select(card => "Id: " + card.Id+ " Name: " + card.Name + " Damage: " + card.Damage)));
            else
                e.Reply(200, JsonConvert.SerializeObject(cards));
        }
        catch (Exception exception)
        {
            Log.Error("An error occured while getting a deck", exception);
            e.Reply(500, "Error getting deck");
        }
        
    }
    
    private void ConfigureDeck(HttpSvrEventArgs e)
    {
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        var cardIds = JsonConvert.DeserializeObject<List<Guid>>(e.Payload);
        var cards = cardIds!.Select(cardId => new CardDto { Id = cardId }).ToList();
        try
        {
            _cardsService.ConfigureDeck(username, cards);
            e.Reply(200, "Successfully configured deck");
        }
        catch (InvalidDeckException exception)
        {
            e.Reply(400, exception.Message);
        }
        catch (InvalidCardException exception)
        {
            e.Reply(403, exception.Message);
        }
        catch 
        {
            e.Reply(500, "Error configuring deck");
        }
    }
}
