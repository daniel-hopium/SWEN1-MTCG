using BusinessLogic.Utils;
using DataAccess.Repository;

namespace BusinessLogic.Services;

public class GameService
{
    private BattleManager _battleManager = new BattleManager();
    private UserRepository _userRepository = new UserRepository();
    private CardsRepository _cardsRepository = new CardsRepository();
    
    public String AttemptStartBattle(string username)
    {
        var player = _userRepository.GetUserByUsername(username);
        player.Deck = _cardsRepository.GetDeck(player.Id);
        
        _battleManager.JoinBattle(player);
        return null;
    }
}