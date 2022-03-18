using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;

namespace Tuldok.Bowling.Service.Interfaces
{
    public interface IFrameService
    {
        Task<List<Frame>> GetFrames(Guid gameGuid);
        Task<Frame> GetFrame(Guid id);
        Task<int> InsertFrame(Frame game);
        Task<int> UpdateFrame(Frame game);
        Task<int> DeleteFrame(Guid id);
        Task<int> TotalFrames(Guid id);
        Task<bool> HasDuplicateSequence(Guid gameId, Guid frameId, int frameNumber);
    }
}
