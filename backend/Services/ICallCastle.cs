using System.Net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using backend.Persistence;
using webbot.Models;

namespace backend.Services
{
    public interface ICallCastle
    {
        Player Player { get; set; }

        List<Player> AllPlayers { get; set; }

        Task<Player> DummyAsync();

        Task<bool> LoginAsync();

        Task LogoutAsync();

        Task<(ReturnCodeEnum, string)> CollectResourceAsync();

        Task<(ReturnCodeEnum, string)> ArchiveAsync();

        Task<Player> ColosseumAsync();
        Task ColosseumWaitForBattleAsync();
        Task ColosseumAttackOpponent(HttpResponseMessage response);

        Task<(ReturnCodeEnum, string)> CrystalPrayerAsync();

        Task<(ReturnCodeEnum, string)> DemiPowerAsync(string id);
        Task<(ReturnCodeEnum, string)> CustomRequestAsync(string uri, bool booster);

        //Task<(ReturnCodeEnum, string)> BoostAsync(string body, string uri);

        Task<HttpResponseMessage> PostRequestUrlEncodedAsync(string body, string uri);

        Task<HttpResponseMessage> GetRequestAsync(string uri);

        void SetPlayerCookie(IEnumerable<string> cookies);
    }
}