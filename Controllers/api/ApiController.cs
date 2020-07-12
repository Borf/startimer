using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarTimer.Models;
using StarTimer.Models.db;
using StarTimer.Services;

namespace StarTimer.Controllers.api
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly TimerContext context;
        public ApiController(TimerContext context)
        {
            this.context = context;
        }

        public IActionResult Get()
        {
            return Ok(context.Spots.Select(s => new
            { 
                Channel = s.Channel.Name, 
                MonsterId = s.Monster.MonsterId, 
                LastSpot = Math.Floor(s.lastSpot == null ? -999999 : (60 * s.Monster.RespawnTime - (DateTime.Now - s.lastSpot.Value).TotalSeconds)),
                LastNoSpot = Math.Floor(s.lastNoSpot == null ? -1 : (DateTime.Now - s.lastNoSpot.Value).TotalSeconds)
            }).ToList());
        }

        [HttpPost("Clear")]
        public async Task<IActionResult> Clear(Mob mob)
        {
            context.Spots.RemoveRange(context.Spots.Where(s => s.Monster.MonsterId == mob.MonsterId));

            await context.SaveChangesAsync();
            return Ok("Ok");
        }

        [HttpPost("Kill")]
        public async Task<IActionResult> KillAsync(ChanMob data)
        {
            Spot spot = context.Spots.Where(s => s.Channel.Name == data.Channel && s.Monster.MonsterId == data.MonsterId).Include(s => s.Channel).Include(s => s.Monster).FirstOrDefault();
            if(spot == null)
            {
                spot = new Spot()
                {
                    Channel = context.Channels.Where(c => c.Name == data.Channel).FirstOrDefault(),
                    Monster = context.Monsters.Where(m => m.MonsterId == data.MonsterId).FirstOrDefault()
                };
                context.Spots.Add(spot);
            }

            spot.lastNoSpot = null;
            spot.lastSpot = DateTime.Now;

            await context.SaveChangesAsync();
            return Ok(new
            {
                Channel = spot.Channel.Name,
                MonsterId = spot.Monster.MonsterId,
                LastSpot = Math.Floor(spot.lastSpot == null ? -999999 : (60 * spot.Monster.RespawnTime - (DateTime.Now - spot.lastSpot.Value).TotalSeconds)),
                LastNoSpot = Math.Floor(spot.lastNoSpot == null ? -1 : (DateTime.Now - spot.lastNoSpot.Value).TotalSeconds)
            });
        }
        [HttpPost("NoSpot")]
        public async Task<IActionResult> NoSpot(ChanMob data)
        {
            Spot spot = context.Spots.Where(s => s.Channel.Name == data.Channel && s.Monster.MonsterId == data.MonsterId).Include(s => s.Channel).Include(s => s.Monster).FirstOrDefault();
            if (spot == null)
            {
                spot = new Spot()
                {
                    Channel = context.Channels.Where(c => c.Name == data.Channel).FirstOrDefault(),
                    Monster = context.Monsters.Where(m => m.MonsterId == data.MonsterId).FirstOrDefault()
                };
                context.Spots.Add(spot);
            }

            spot.lastNoSpot = DateTime.Now;
            spot.lastSpot = null;

            await context.SaveChangesAsync();
            return Ok(new
            {
                Channel = spot.Channel.Name,
                MonsterId = spot.Monster.MonsterId,
                LastSpot = Math.Floor(spot.lastSpot == null ? -999999 : (60 * spot.Monster.RespawnTime - (DateTime.Now - spot.lastSpot.Value).TotalSeconds)),
                LastNoSpot = Math.Floor(spot.lastNoSpot == null ? -1 : (DateTime.Now - spot.lastNoSpot.Value).TotalSeconds)
            });
        }


    }
}
