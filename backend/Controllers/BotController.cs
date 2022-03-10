using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using backend.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using webbot.Models;

namespace webbot.Controllers
{
    /// <summary>
    /// Teszt célokra, direkt lekérdezésekre
    /// </summary>
    [Route("")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IPlayerRepository playerRepository;
        private readonly ICallCastle callCastle;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private Player player;

        public BotController(IPlayerRepository playerRepository, ICallCastle callCastle, IUnitOfWork uow, IMapper mapper)
        {
            this.playerRepository = playerRepository;
            this.callCastle = callCastle;
            this.uow = uow;
            this.mapper = mapper;
        }


        [HttpGet("/query")]
        public async Task<object> QueryParameters([FromQuery] string user, [FromQuery] string action)
        {
            this.player = await playerRepository.GetPlayerAsync(user);

            if (this.player == null)
                return NotFound();

            this.callCastle.Player = this.player;

            switch (action)
            {
                case "login":
                    await Login();
                    break;
                case "logout":
                    await Logout();
                    break;
                //case "collectresource":
                //    return await CollectResource();
                    //break;
                //case "demipower":
                //    await DemiPower();
                //    break;
            }

            return Ok();
        }

        [EnableCors("MyAllowedDomains")]
        [HttpGet("/login")]
        public async Task<IActionResult> Login()
        {
            if (await callCastle.LoginAsync())
            {
                await uow.CompleteAsync();
                return Ok();
            }
            return StatusCode(403);
        }


        [HttpGet("/logout")]
        public async Task Logout()
        {
            await callCastle.LogoutAsync();
            await uow.CompleteAsync();
        }

        //[HttpGet("/demipower")]
        //public async Task DemiPower()
        //{
        //    await callCastle.DemiPowerAsync();
        //}

        //[HttpGet("/collectresource")]
        //public async Task<string> CollectResource()
        //{
        //    return await callCastle.CollectResourceAsync();
        //}

        //[HttpGet("/getplayers")]
        //public async Task<IEnumerable<PlayerDto>> GetPlayers()
        //{
        //    var players = await this.playerRepository.GetPlayersAsync();
        //    return mapper.Map<List<Player>, List<PlayerDto>>(players);
        //}

        [HttpGet()]
        public string Index()
        {
            return "CastleAge WebBot version 0.2";
        }

        [HttpPost("/test")]
        public IActionResult Test([FromBody] string user)
        {
            return Ok($"ezt a faszom user-t küldted ide: ${user}");
        }
    }
}
