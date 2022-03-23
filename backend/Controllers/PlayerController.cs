using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webbot.Enums;
using webbot.Models;
using webbot.Persistence;

namespace webbot.Controllers
{
    [Route("[controller]/[action]")]
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly BotContext botContext;

        public PlayerController(IUnitOfWork uow, IMapper mapper, BotContext botContext)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.botContext = botContext;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> New([FromBody()] Player player, CancellationToken cancellationToken)
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
        public async Task<IActionResult> Modify([FromBody()] Player player, CancellationToken cancellationToken)
        {
            if (!await uow.PlayerRepository.ModifyPlayer(player)) return BadRequest("Sikertelen felhasználó módosítás!");
            await uow.CompleteAsync();
            return Ok("Sikeres felhasználó módosítás!");
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await uow.PlayerRepository.DeletePlayerAsync(id);
            await uow.CompleteAsync();

        }

        [HttpGet()]
        public async Task<List<Group>> GetGroups(CancellationToken cancellationToken)
        {
            return await uow.PlayerRepository.GetGroupsAsync();
        }

        [HttpGet()]
        public async Task<List<PlayerDto>> GetPlayersByGroupIdAsync(int id, CancellationToken cancellationToken)
        {
            var players = await uow.PlayerRepository.GetPlayersByGroupIdAsync(id);
            return mapper.Map<List<Player>, List<PlayerDto>>(players);
        }

        [HttpGet()]
        public async Task<object> GetPlayersByGroupNameAsync(string name, CancellationToken cancellationToken)
        {
            var players = await uow.PlayerRepository.GetPlayersByGroupNameAsync(name);
            return mapper.Map<List<Player>, List<PlayerDto>>(players);
        }

        [HttpGet()]
        public async Task<IActionResult> MovePlayerToGroup(string playerName, string srcGroupName, string dstGroupName, CancellationToken cancellationToken)
        {
            var srcGroup = uow.PlayerRepository.GetGroupByName(srcGroupName);
            var dstGroup = uow.PlayerRepository.GetGroupByName(dstGroupName);

            //if (srcGroup.Id == dstGroup.Id) return;

            var player = uow.PlayerRepository.GetPlayerByName(playerName);

            if (player.Groups.Contains(dstGroup)) return Ok("already member of the group");
            
            uow.PlayerRepository.MovePlayerToGroup(player, srcGroup, dstGroup);

            await uow.CompleteAsync();
            
            return Ok("Moved to group.");
        }

        [HttpPost()]
        public async Task<Group> AddOrModifyGroup([FromBody]Group groupDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return null;
            if (groupDto.Name is null) return null;

            if (groupDto.Id == (int)Groups.Mindenki || groupDto.Name == Groups.Mindenki.ToString()) return null;

            if (groupDto.Id == (int)Groups.UjCsoport) { NewGroup(groupDto); } else ModifyGroup(groupDto);
            
            await uow.CompleteAsync();
            return groupDto;

        }

        private void ModifyGroup(Group groupDto)
        {
            var group = uow.PlayerRepository.GetGroupById(groupDto.Id);
            group.Name = groupDto.Name;
            uow.PlayerRepository.ModifyGroup(group);
        }

        private void NewGroup(Group groupDto)
        {
            uow.PlayerRepository.NewGroup(groupDto);
        }

        private int ModifyGroup()
        {
            throw new NotImplementedException();
        }

        [HttpGet()]
        public async Task DeleteGroup(string name, CancellationToken cancellationToken)
        {
            if (name == Groups.Mindenki.ToString()) return;

            var group = uow.PlayerRepository.GetGroupByName(name);
            uow.PlayerRepository.DeleteGroup(group);
            await uow.CompleteAsync();
        }

        [HttpPost()]
        public async Task Players([FromBody] PlayerDto[] playerDtos, CancellationToken cancellationToken)
        {
            //validációk! pl. nem lehet 0-ás memberOf
            //...


            var allGroups = botContext.Groups.ToList();
            var mindenkiGroup = allGroups.Find(g => g.Id == (int)Groups.Mindenki);

            foreach (var playerDto in playerDtos)
            {
                //var player = mapper.Map<PlayerDto, Player>(playerDto);
                var player = botContext.Players.Include(p => p.Groups).FirstOrDefault<Player>(p => p.Id == playerDto.Id);
                //var groups = player
                player.Groups.Clear();

                foreach (var member in playerDto.MemberOf)
                {
                    var group = allGroups.FirstOrDefault(g => g.Id == member);
                    player.Groups.Add(group);
                }

                if (!player.Groups.Contains(mindenkiGroup)) player.Groups.Add(mindenkiGroup);
            }

            await uow.CompleteAsync();
        }

    }
}