using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Repo.Data;
using Tuldok.Bowling.Service.Interfaces;

namespace Tuldok.Bowling.Service
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _repository; 

        public GameService(IRepository<Game> repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteGame(Guid id)
        {
            var game = await _repository.Get(id);
            return await _repository.Delete(game);
        }

        public async Task<Game> GetGame(Guid id)
        {
            var game = await _repository.Get(id);
            return game;
        }

        public async Task<List<Game>> GetGames()
        {
            var games = await _repository.GetAll();
            return games;
        }

        public async Task<int> InsertGame(Game game)
        {
            return await _repository.Insert(game);
        }

        public Task<int> UpdateGame(Game game)
        {
            return _repository.Update(game);
        }
    }
}
