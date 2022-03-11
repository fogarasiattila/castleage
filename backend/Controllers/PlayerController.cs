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
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public PlayerController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> New([FromBody()] Player player)
        {
            await uow.PlayerRepository.NewPlayer(player);
            if (!await uow.CompleteAsync()) return BadRequest("Nem sikerült a felhasználó mentése!");
            return Ok("Az új felhasználót mentettük!");
        }

        [HttpGet()]
        public async Task<IEnumerable<PlayerDto>> GetPlayers()
        {
            var players = await  uow.PlayerRepository.GetPlayersAsync();
            return mapper.Map<List<Player>, List<PlayerDto>>(players);
        }

        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> Modify([FromBody()] Player player)
        {
            if (!await uow.PlayerRepository.ModifyPlayer(player)) return BadRequest("Sikertelen felhasználó módosítás!");
            await uow.CompleteAsync();
            return Ok("Sikeres felhasználó módosítás!");
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await uow.PlayerRepository.Delete(id);
            await uow.CompleteAsync();

        }

    }
}