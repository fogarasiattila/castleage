using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace webbot.Controllers
{
    [Route("[controller]/[action]")]
    [Route("api/[controller]/[action]")]
    public class SessionController : Controller
    {
        private readonly IPlayerRepository playerRepository;
        private readonly ICallCastle callCastle;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public SessionController(IPlayerRepository playerRepository, ICallCastle callCastle, IUnitOfWork uow, IMapper mapper)
        {
            this.playerRepository = playerRepository;
            this.callCastle = callCastle;
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] Player player)
        {
            //this.playerRepository.ResetCache();

            this.callCastle.Player = await playerRepository.GetPlayerAsync(player.Username);

            if (await callCastle.LoginAsync())
            {
                await uow.CompleteAsync();

                return Ok($"{player.Username} logged in.");
            }
            return NotFound($"no cookie received for {player.Username}, maybe wrong password.");

        }

        [HttpPost()]
        public async Task<IActionResult> Logout([FromBody] Player player)
        {
            //this.playerRepository.ResetCache();
    
            this.callCastle.Player = await playerRepository.GetPlayerAsync(player.Username);

            await callCastle.LogoutAsync();
            await uow.CompleteAsync();
            return Ok($"{player.Username} has logged out.");
        }

    }
}