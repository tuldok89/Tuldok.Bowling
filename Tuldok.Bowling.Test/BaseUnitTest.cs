using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Repo.Data;
using Tuldok.Bowling.Service;
using Tuldok.Bowling.Service.Interfaces;

namespace Tuldok.Bowling.Test
{
    public abstract class BaseUnitTest : IDisposable
    {
        private const string _connString = "DataSource=file::memory:?cache=shared";
        protected DataContext Context;
        protected IRepository<Game> GameRepo;
        protected IRepository<Frame> FrameRepo;
        protected IRepository<Shot> ShotRepo;
        protected IGameService GameService;
        protected IFrameService FrameService;
        protected IShotService ShotService;
        protected IBowlingService BowlingService;
        protected IScoreService ScoreService;

        public BaseUnitTest()
        {
            var sqlopts = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connString)
                .Options;

            Context = new DataContext(sqlopts);
            Context.Database.EnsureCreated();

            GameRepo = new DataRepository<Game>(Context);
            FrameRepo = new DataRepository<Frame>(Context);
            ShotRepo = new DataRepository<Shot>(Context);
            GameService = new GameService(GameRepo);
            FrameService = new FrameService(FrameRepo);
            ShotService = new ShotService(ShotRepo);
            BowlingService = new BowlingService(GameService, FrameService, ShotService);
            ScoreService = new ScoreService(FrameService, ShotService);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
