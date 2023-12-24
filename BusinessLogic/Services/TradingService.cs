using BusinessLogic.Mapper;
using DataAccess.Repository;
using Transversal.Entities;

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
            //check card if card is available
            var userCard = _cardRepository.GetUserCardByUserAndCardId(user.Id, trade.CardId); // change to get only id
            if (userCard == null)
                throw new Exception("Card does not exist");
           
            //deal exists already 
            if(_tradeRepository.Exists(trade.Id))
                throw new Exception("Trade already exists");

            var tradeDao = TradeMapper.MapToDao(trade);
            tradeDao.UserCardToTradeId = userCard.Id; // userCardId!
            
            _tradeRepository.CreateTrade(tradeDao);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void DeleteTrade(string username, Guid tradeId)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username)!;
            var trade = _tradeRepository.GetTrade(tradeId);
            if (trade == null)
                throw new Exception("Trade does not exist");
            
            var userCard = _cardRepository.GetUserCardById(trade.UserCardToTradeId);
            if (userCard == null || userCard.UserId != user.Id) // Improve the null check
                throw new Exception("User is not the owner of the trade");
            
            _tradeRepository.DeleteTrade(tradeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void CarryOutTrade(string username, Guid tradeId)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            var trade = _tradeRepository.GetTrade(tradeId);
            
            if (trade == null)
                throw new Exception("Trade does not exist");
    
            // var userCard = _cardRepository.GetUserCardByUserAndCardId(user.Id, trade.UserCardToTradeId);
            // if (userCard == null)
            //     throw new Exception("Card does not exist");
            // if (userCard.UserId == user.Id)
            //     throw new Exception("User is the owner of the trade");
            // if (userCard.Damage < trade.MinimumDamage)
            //     throw new Exception("Card does not meet the minimum damage requirement");
            //
            // _cardRepository.DeleteUserCard(userCard.Id);
            // _cardRepository.DeleteUserCard(trade.UserCardToTradeId);
            // _cardRepository.CreateUserCard(user.Id, trade.UserCardToTradeId);
            // _cardRepository.CreateUserCard(trade.UserId, userCard.Id);
            // _tradeRepository.DeleteTrade(tradeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
