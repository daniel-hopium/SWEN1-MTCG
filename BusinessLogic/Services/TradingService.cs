using BusinessLogic.Exceptions;
using BusinessLogic.Mapper;
using DataAccess.Repository;
using Transversal.Entities;
using static DataAccess.Repository.CardsRepository.Usage;

namespace BusinessLogic.Services;

public class TradingService
{
    private readonly TradeRepository _tradeRepository = new TradeRepository();
    private readonly UserRepository _userRepository = new UserRepository();
    private readonly CardsRepository _cardRepository = new CardsRepository();
    public List<TradeDto> GetTrades(string username)
    {
        try
        {
            return TradeMapper.MapToDtoList(_tradeRepository.GetTrades());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return new List<TradeDto>();
    }

    public void CreateTrade(string username, TradeDto trade)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username)!;
            //deal exists already 
            if(_tradeRepository.Exists(trade.Id))
                throw new InvalidTradeException("Trade already exists");
            
            //check card if card is available
            var userCard = _cardRepository.GetUserCardByUserAndCardId(user.Id, trade.CardToTrade, None); // change to get only id
            if (userCard == null)
                throw new InvalidCardException("The deal contains a card that is not owned by the user or locked in the deck.");
           
            var tradeDao = TradeMapper.MapToDao(trade);
            tradeDao.CardToTradeId = userCard.CardId; // userCardId!
            
            _tradeRepository.CreateTrade(tradeDao);
            _cardRepository.UpdateUsage(user.Id, userCard.CardId, Trade);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void DeleteTrade(string username, Guid tradeId)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username)!;
            var trade = _tradeRepository.GetTrade(tradeId);
            if (trade == null)
                throw new InvalidTradeException("Trade does not exist");
            
            var userCard = _cardRepository.GetUserCardByUserAndCardId(user.Id, trade.CardToTradeId, Trade);
            if (userCard == null || userCard.UserId != user.Id) // Improve the null check
                throw new InvalidOperationException("User is not the owner of the trade");
            
            _tradeRepository.DeleteTrade(tradeId);
            _cardRepository.UpdateUsage(user.Id, userCard.CardId, None);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void CarryOutTrade(string username, Guid tradeId, Guid cardToTrade)
    {
        try
        {
            var initiatingUser = _userRepository.GetUserByUsername(username)!;
            var existingTrade = _tradeRepository.GetTrade(tradeId);
            if (existingTrade == null)
                throw new InvalidTradeException("Trade does not exist");
            
            var receivingUserCard = _cardRepository.GetUserCardByCardId(existingTrade.CardToTradeId);
            if (receivingUserCard!.UserId == initiatingUser.Id) 
                throw new InvalidOperationException("User is the owner of the trade");
            
            var cardToTradeInitiatingUserCard = _cardRepository.GetUserCardByUserAndCardId(initiatingUser.Id, cardToTrade, None);
            if (cardToTradeInitiatingUserCard == null)
                throw new InvalidTradeException("Card does not exist or is currently in use");
            
            var initiatingTradeUserCard = _cardRepository.GetCardById(cardToTrade);
            if (initiatingTradeUserCard.Damage < existingTrade.MinimumDamage || initiatingTradeUserCard.CardType != existingTrade.Type)
                throw new InvalidTradeException("Requirements not met");
            
            _tradeRepository.UpdateCards(initiatingUser.Id, receivingUserCard.CardId); 
            _tradeRepository.UpdateCards(receivingUserCard.UserId, initiatingTradeUserCard.Id); 
            _tradeRepository.DeleteTrade(tradeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
