using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services;

public class GameService
{
    private readonly BattleManager _battleManager = new BattleManager();
    private readonly UserRepository _userRepository = new UserRepository();
    private readonly CardsRepository _cardsRepository = new CardsRepository();
    private readonly GameRepository _gameRepository = new GameRepository();
    
    public string AttemptStartBattle(string username)
    {
        var player = _userRepository.GetUserByUsername(username);
        player!.Deck = _cardsRepository.GetDeck(player.Id);
        if (player.Deck.Count != 4)
            throw new Exception("Deck to play must have 4 cards");
        
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