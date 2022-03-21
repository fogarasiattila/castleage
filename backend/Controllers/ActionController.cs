using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using backend.Services;
using botservice;
using Microsoft.AspNetCore.Mvc;
using webbot.Models;
using webbot.Persistence;

namespace webbot.Controllers
{
    [Route("[controller]/[action]")]
    [Route("api/[controller]/[action]")]

    public class ActionController : Controller
    {
        private readonly IPlayerRepository playerRepository;
        private readonly ICallCastle callCastle;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ColosseumBattleService hostedService;

        public ActionController(IPlayerRepository playerRepository, ICallCastle callCastle, IUnitOfWork uow, IMapper mapper)
        {
            this.playerRepository = playerRepository;
            this.callCastle = callCastle;
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpPost()]
        public async Task<IActionResult> CollectResource([FromBody] Player player, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(player.Username);
            var (returncode, returnmessage) = await callCastle.CollectResourceAsync();

            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }

        [HttpPost()]
        public async Task<IActionResult> Archive([FromBody] Player player, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(player.Username);
            var (returncode, returnmessage) = await callCastle.ArchiveAsync();

            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }

        [HttpPost()]
        public async Task<IActionResult> DemiPower([FromBody] DemigodRequest demigodRequest, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(demigodRequest.Username);
            var (returncode, returnmessage) = await callCastle.DemiPowerAsync(demigodRequest.DemigodId);

            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }

        [HttpPost()]
        public async Task<IActionResult> CrystalPrayer([FromBody] Player player, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(player.Username);
            var (returncode, returnmessage) = await callCastle.CrystalPrayerAsync();

            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }

        [HttpPost()]
        public async Task<IActionResult> DailySpin([FromBody] Player player, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(player.Username);
            var (returncode, returnmessage) = await callCastle.DailySpinAsync();

            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }
        
        [HttpPost()]
        public async Task<IActionResult> CollectTerritory([FromBody] Player player, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(player.Username);
            var (returncode, returnmessage) = await callCastle.CollectTerritoryAsync();

            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }

        [HttpPost()]
        public async Task<IActionResult> CustomUri([FromBody] CustomRequestModel customRequest, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(customRequest.Username);
            var (returncode, returnmessage) = await callCastle.CustomRequestAsync(customRequest.Uri, customRequest.Booster);
            if (returncode == ReturnCodeEnum.NotLoggedIn) return Accepted(returnmessage);

            return Ok(returnmessage);
        }

        [HttpPost()]
        public async Task<IActionResult> Colosseum([FromBody] Player player, CancellationToken cancellationToken)
        {
            this.callCastle.Player = await playerRepository.GetPlayerByNameAsync(player.Username);
            //var (returncode, returnmessage) = await 
                callCastle.ColosseumAsync();

            //sif (returncode == HttpStatusCode.Redirect) return Ok(returnmessage);

            //if (returnmessage.Contains("Matchmaking"))
            //{
            //    if (!await callCastle.ColosseumWaitForBattleAsync()) return Ok("Battle has not started for some reason...");
            //}
            //else if (!returnmessage.Contains("Ongoing")) return Ok("No \"Matchmaking...\"");

            //returnmessage = await callCastle.ColosseumAttackOpponent();

            //return Ok(returnmessage);
            return Ok("initializing battle...");
        }
    }
}