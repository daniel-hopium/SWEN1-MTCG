using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Daos;
using DataAccess.Repository;
using Transversal.Entities;
using Transversal.Utils;
using static BusinessLogic.Services.CardsService;

namespace BusinessLogic.Services;

public class GameService
{
    private readonly BattleManager _battleManager = new BattleManager();
    private readonly UserRepository _userRepository = new UserRepository();
    private readonly CardsRepository _cardsRepository = new CardsRepository();
    private readonly GameRepository _gameRepository = new GameRepository();
    
    public string StartBattle(string username)
    {
        try
        {
            var player = _userRepository.GetUserWithScoreByUsername(username);
            player!.Deck = _cardsRepository.GetDeck(player.Id);
            if (player.Deck.Count != DeckSize)
                throw new Exception("Deck to play must have 4 cards");
        
            _battleManager.JoinBattle(player);
            var finishedBattle = _gameRepository.FindLastBattle();
        
            return finishedBattle.Log;
        }
        catch (Exception e)
        {
            Log.Error("An error occured while starting a battle", e);
            throw;
        }
    }

    public List<UserScoreboardDto> GetScoreboard()
    {
        return UserScoreboardMapper.MapToDtoList(_gameRepository.GetScoreboard());
    }

    public UserScoreboardDto GetUserStats(string username)
    {
        return UserScoreboardMapper.MapToDto(_gameRepository.GetScoreboardEntry(username)!);
    }
}