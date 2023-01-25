

using Shared.Models.Football_Player_Models;
using Shared.Models.FootballPlayer;

namespace SquadManager.Builder
{
    internal sealed class SquadBuilderService
    {
        private SquadBuilder _squadBuilder;

        public SquadBuilderService(SquadBuilder squadBuilder)
        {
            _squadBuilder = squadBuilder;
        }

        public bool AddPlayer(Player player)
        {
            if(player != null)
            {
                _squadBuilder.Players.Add(player);
                return true;
            }
            return false;   
        }

        public bool RemovePlayer(Player player)
        {
            int index = 0;
            bool found = false;
            if(player != null)
            {
                foreach(var p in _squadBuilder.Players)
                {
                    if(p.ID == player.ID)
                    {
                        found = true;
                        break;
                    }
                    index++;
                }
                if (found)
                {
                    _squadBuilder.Players.RemoveAt(index);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void IncreaseBalance(int balance)
        {
            _squadBuilder.Team.Balance += balance;
        }

        public void DeacreseBalance(int balance)
        {
            _squadBuilder.Team.Balance -= balance;
        }

    }
}
