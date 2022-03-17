using System;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Interfaces
{
    public interface IScoreService
    {
        Task<int> GameScore(Guid gameGuid);
    }
}