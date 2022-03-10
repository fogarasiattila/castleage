using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using webbot.Models;

namespace webbot.Controllers
{
    [Route("[controller]/[action]")]
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public PlayerController(IPlayerRepository playerRepository, IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.playerRepository = playerRepository;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Update([FromBody()] Player player)
        {

            return Ok(player.Username);
        }

        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> New([FromBody()] Player player)
        {
            await this.playerRepository.NewPlayer(player);
            return Ok();
        }

        [HttpGet()]
        public async Task<IEnumerable<PlayerDto>> GetPlayers()
        {
            var players = await this.playerRepository.GetPlayersAsync();
            return mapper.Map<List<Player>, List<PlayerDto>>(players);
        }

        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> Modify([FromBody()] Player player)
        {
            if (await this.playerRepository.ModifyPlayer(player)) return Ok();
            else return BadRequest();

        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await this.playerRepository.Delete(id);

        }

    }
}