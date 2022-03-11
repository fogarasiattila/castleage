using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async Task<IActionResult> StartColosseum()
        {
            if (await unitOfWork.SettingsRepository.GetStartColosseumBattle() == State.Off)
            {
                await unitOfWork.SettingsRepository.ToggleStartColosseumBattle();
                await unitOfWork.CompleteAsync();
            }

            colosseumBattleService.StartAsync(new System.Threading.CancellationToken());
            return Ok("Starting colosseum...");
        }

        [HttpGet()]
        public async Task<IActionResult> StopColosseum()
        {
            if (await unitOfWork.SettingsRepository.GetStartColosseumBattle() == State.On)
            {
                await unitOfWork.SettingsRepository.ToggleStartColosseumBattle();
                await unitOfWork.CompleteAsync();
            }

            colosseumBattleService.StopAsync(new System.Threading.CancellationToken());
            return Ok("Cancellation request has been sent...");
        }
    }
}
