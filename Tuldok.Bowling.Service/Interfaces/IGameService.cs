using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;

namespace Tuldok.Bowling.Service.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetGames();
        Task<Game> GetGame(Guid id);
        Task<int> InsertGame(Game game);
        Task<int> UpdateGame(Game game);
        Task<int> DeleteGame(Guid id);
    }
}
