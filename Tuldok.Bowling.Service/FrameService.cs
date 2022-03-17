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
    public class FrameService : IFrameService
    {
        private readonly IRepository<Frame> _repository;

        public FrameService(IRepository<Frame> repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteFrame(Guid id)
        {
            var frame = await _repository.Get(id);
            return await _repository.Delete(frame);
        }

        public async Task<Frame> GetFrame(Guid id)
        {
            return await _repository.Get(id);
        }

        public async Task<List<Frame>> GetFrames(Guid gameGuid)
        {
            var frames = await _repository
                .Query()
                .Where(x => x.Game.Id == gameGuid)
                .ToListAsync();

            return frames;
        }

        public async Task<int> InsertFrame(Frame frame)
        {
            return await _repository.Insert(frame);
        }

        public Task<int> UpdateFrame(Frame frame)
        {
            return _repository.Update(frame);
        }

        public async Task<int> TotalFrames(Guid gameId)
        {
            var frameCount = await _repository.Query().Where(x => x.Game.Id == gameId).CountAsync();

            return frameCount;
        }
    }
}
