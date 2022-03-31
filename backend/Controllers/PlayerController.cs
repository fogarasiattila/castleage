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
            var players = await uow.PlayerRepository.GetPlayersAsync();
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

            var player = uow.PlayerRepository.GetPlayerByName(playerName);

            if (player.Groups.Contains(dstGroup)) return Ok("already member of the group");

            uow.PlayerRepository.MovePlayerToGroup(player, srcGroup, dstGroup);

            await uow.CompleteAsync();

            return Ok("Moved to group.");
        }

        [HttpGet()]
        public void DeleteGroup(Group group)
        {
            if (group.Id == (int)Groups.Mindenki) return;
            uow.PlayerRepository.DeleteGroup(group);
        }

        [HttpPost()]
        public async Task<IActionResult> Players([FromBody] GroupsAndPlayersDto groupsAndPlayersDto, CancellationToken cancellationToken)
        {
            //validációk! pl. nem lehet 0-ás memberOf
            //...

            if (!ModelState.IsValid) return BadRequest(ModelState);

            List<string> errors = new();

            var allGroups = botContext.Groups.ToList();
            var mindenkiGroup = allGroups.Find(g => g.Id == (int)Groups.Mindenki);
            Dictionary<int, Group> mapNewGroups = new();

            foreach (var groupDto in groupsAndPlayersDto.Groups)
            {
                var group = allGroups.FirstOrDefault(g => g.Id == groupDto.Id);

                if (group is null)
                {
                    if (groupDto.Deleted) continue;

                    var newGroup = new Group { Name = groupDto.Name };

                    NewGroup(newGroup);

                    mapNewGroups.Add(groupDto.Id, newGroup);
                }
                else if (groupDto.Deleted) DeleteGroup(group);
                else if (groupDto.Name != group.Name) group.Name = groupDto.Name;
            }

            await uow.CompleteAsync();

            foreach (var playerDto in groupsAndPlayersDto.Players)
            {
                var player = botContext.Players.Include(p => p.Groups).FirstOrDefault<Player>(p => p.Id == playerDto.Id);
                player.Groups.Clear();

                foreach (var groupMemberId in playerDto.MemberOf)
                {
                    var group = groupMemberId < (int)Groups.UjCsoport 
                        ? botContext.Groups.ToList().FirstOrDefault(g => g.Id == (mapNewGroups[groupMemberId]).Id) 
                        : botContext.Groups.ToList().FirstOrDefault(g => g.Id == groupMemberId);

                    player.Groups.Add(group);
                }

                if (!player.Groups.Contains(mindenkiGroup)) player.Groups.Add(mindenkiGroup);

                await uow.CompleteAsync();
            }

            if (errors.Count > 0) return BadRequest(errors);

            return Ok();
        }

        private void NewGroup(Group newGroup)
        {
            uow.PlayerRepository.NewGroup(newGroup);
        }
    }
}