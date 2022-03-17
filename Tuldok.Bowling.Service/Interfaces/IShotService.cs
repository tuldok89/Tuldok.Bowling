﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;

namespace Tuldok.Bowling.Service.Interfaces
{
    public interface IShotService
    {
        Task<List<Shot>> GetShots(Guid frameGuid);
        Task<Shot> GetShot(Guid id);
        Task<int> InsertShot(Shot shot);
        Task<int> UpdateShot(Shot shot);
        Task<int> DeleteShot(Guid id);
        Task<int> TotalShots(Guid frameId);
    }
}