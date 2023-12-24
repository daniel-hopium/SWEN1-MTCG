using DataAccess.Daos;
using Transversal.Entities;

namespace BusinessLogic.Mapper;

public class UserScoreboardMapper
{
    public static List<UserScoreboardDto> MapToDtoList(List<UserScoreboardDao> userScoreboardDao)
    {
        return userScoreboardDao.Select(MapToDto).ToList();
    }
    
    public static UserScoreboardDto MapToDto(UserScoreboardDao userScoreboardDao)
    {
        return new()
        {
            Username = userScoreboardDao.Username,
            Elo = userScoreboardDao.Elo,
            Wins = userScoreboardDao.Wins,
            Losses = userScoreboardDao.Losses
        };
    }
}