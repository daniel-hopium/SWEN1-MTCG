
using Transversal.Entities;
using TradeDao = DataAccess.Daos.TradeDao;

namespace BusinessLogic.Mapper;

public class TradeMapper
{
    public static TradeDto MapToDto(TradeDao tradeDao)
    {
        if (tradeDao == null)
        {
            return null;
        }

        return new TradeDto
        {
            Id = tradeDao.Id,
            CardToTrade = tradeDao.CardToTrade,
            Type = tradeDao.Type,
            Damage = tradeDao.Damage
        };
    }

    public static TradeDao MapToDao(TradeDto tradeDto)
    {
        if (tradeDto == null)
        {
            return null;
        }

        return new TradeDao
        {
            Id = tradeDto.Id,
            CardToTrade = tradeDto.CardToTrade,
            Type = tradeDto.Type,
            Damage = tradeDto.Damage
        };
    }

    public static List<TradeDto> MapToDtoList(List<TradeDao> tradeDaoList)
    {
        if (tradeDaoList == null)
        {
            return null;
        }

        List<TradeDto> tradeDtoList = new List<TradeDto>();
        foreach (var tradeDao in tradeDaoList)
        {
            tradeDtoList.Add(MapToDto(tradeDao));
        }

        return tradeDtoList;
    }

    public static List<TradeDao> MapToDaoList(List<TradeDto> tradeDtoList)
    {
        if (tradeDtoList == null)
        {
            return null;
        }

        List<TradeDao> tradeDaoList = new List<TradeDao>();
        foreach (var tradeDto in tradeDtoList)
        {
            tradeDaoList.Add(MapToDao(tradeDto));
        }

        return tradeDaoList;
    }
}