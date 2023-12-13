using BusinessLogic.Mapper;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class TradingService
{
    private readonly  TradeMapper _tradeMapper = new TradeMapper();
    private readonly TradeRepository _tradeRepository = new TradeRepository();
    private readonly UserRepository _userRepository = new UserRepository();
    public List<TradeDto> GetTradings(string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            return TradeMapper.MapToDtoList(_tradeRepository.GetTradings());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
        return new List<TradeDto>();
    }

    public void CreateTrading(string username, TradeDto trade)
    {
        throw new NotImplementedException();
    }
}
