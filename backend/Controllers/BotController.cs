using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using backend.Services;
using botservice;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using webbot.Enums;
using webbot.Models;

namespace webbot.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]/[action]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ColosseumBattleService colosseumBattleService;
        private readonly IUnitOfWork unitOfWork;

        public BotController(ColosseumBattleService colosseumBattleService, IUnitOfWork unitOfWork)
        {
            this.colosseumBattleService = colosseumBattleService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("/")]
        [HttpGet("/api/status")]
        [HttpGet("/api/version")]
        public string Index()
        {
            return "CastleAge WebBot version 0.2";
        }

        [HttpGet()]
        public async Task<IActionResult> StartColosseumBattle(CancellationToken cancellationToken)
        {
            //await unitOfWork.SettingsRepository.ToggleColosseumBattle();
            //await unitOfWork.CompleteAsync();

            await colosseumBattleService.StartAsync(cancellationToken);
            return Ok("Starting colosseum...");
        }

        [HttpGet()]
        public async Task<IActionResult> StopColosseumBattle(CancellationToken cancellationToken)
        {
            //await unitOfWork.SettingsRepository.ToggleColosseumBattle();
            //await unitOfWork.CompleteAsync();

            //lefagy ez a thread, ha await-elem
            colosseumBattleService.StopAsync(new System.Threading.CancellationToken());
            return Ok("Cancellation request has been sent...");
        }

        [HttpGet()]
        public async Task<IActionResult> GetColosseumBattleStartStopState(CancellationToken cancellationToken)
        {
            var state = await unitOfWork.SettingsRepository.GetStartColosseumBattle();
            return Ok(state);
        }

        [HttpGet()]
        public async Task<IActionResult> ToggleColosseumBattle(CancellationToken cancellationToken)
        {
            var state = await unitOfWork.SettingsRepository.ToggleColosseumBattle();
            await unitOfWork.CompleteAsync();

            if (state == State.On) { await StartColosseumBattle(cancellationToken); } else { await StopColosseumBattle(cancellationToken); }

            return Ok(state);
        }
    }
}
