using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class GameService
{
    private BattleManager _battleManager = new BattleManager();
    private UserRepository _userRepository = new UserRepository();
    private CardsRepository _cardsRepository = new CardsRepository();
    private GameRepository _gameRepository = new GameRepository();
    
    public String AttemptStartBattle(string username)
    {
        var player = _userRepository.GetUserByUsername(username);
        player.Deck = _cardsRepository.GetDeck(player.Id);
        
        _battleManager.JoinBattle(player);
        var finishedBattle = _gameRepository.FindLastBattle();
        
        
        return finishedBattle.Log;
    }

    public List<UserDto> GetScoreboard()
    {
        var scoreboard = _userRepository.GetScoreboard();
        return UserMapper.MapToDto(scoreboard);
    }

    public UserDto GetUserStats(string username)
    {
        return UserMapper.MapToDto(_userRepository.GetUserByUsername(username)!);
    }
}