using Microsoft.EntityFrameworkCore;
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
    public class ShotService : IShotService
    {
        private readonly IRepository<Shot> _repository;

        public ShotService(IRepository<Shot> repository)
        {
            _repository = repository;
        }
        
        public async Task<int> DeleteShot(Guid id)
        {
            var shot = await _repository.Get(id);
            return await _repository.Delete(shot);
        }

        public async Task<Shot> GetShot(Guid id)
        {
            var shot = await _repository.Get(id);
            return shot;
        }

        public async Task<List<Shot>> GetShots(Guid frameGuid)
        {
            var shots = await _repository
                .Query()
                .Where(x => x.Frame.Id == frameGuid)
                .ToListAsync();

            return shots;
        }

        public async Task<int> InsertShot(Shot shot)
        {
            return await _repository.Insert(shot);
        }

        public Task<int> UpdateShot(Shot shot)
        {
            return _repository.Update(shot);
        }

        public async Task<int> TotalShots(Guid frameId)
        {
            return await _repository.Query().Where(x => x.Frame.Id == frameId).CountAsync();
        }
    }
}
