using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;

namespace Tuldok.Bowling.Service.Interfaces
{
    public interface IBowlingService
    {
        Task<Frame> CreateFrame(Guid gameId, int? frameNumber = null);
        Task<Game> CreateGame(string name);
        Task<Shot> CreateShot(Guid frameId, int pinFalls, int? sequenceNumber = null);
        Task DeleteFrame(Guid frameId);
        Task DeleteGame(Guid gameId);
        Task DeleteShot(Guid shotId);
        Task<List<Frame>> GetAllFrames(Guid gameId);
        Task<List<Game>> GetAllGames();
        Task<List<Shot>> GetAllShots(Guid frameId);
        Task<Frame> GetFrame(Guid frameId);
        Task<Game> GetGame(Guid id);
        Task<Shot> GetShot(Guid shotId);
        Task<Shot> GetShot(Guid frameId, int sequenceNumber);
        Task UpdateFrame(Frame frame);
        Task UpdateGame(Game game);
        Task DeleteAllShots(Guid frameId);
    }
}