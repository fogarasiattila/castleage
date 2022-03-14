using System.Net;
using System.Net.Http.Headers;
using System.IO;
using System.Web;
using System.Net.Http;
//using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System.Linq;
using backend.Persistence;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using webbot.Services;
using webbot.Models;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace backend.Services
{
    //TODO: player inicializálás konstruktorból
    public class CallCastle : ICallCastle
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IParseHtml parseHtml;
        private readonly ILogger logger;

        #region constants
        private const string ajax2x = "&ajax=1&ajax=1";
        private const string mediaTypeUrlEncoded = "application/x-www-form-urlencoded";
        private const string baseUrl = "https://web3.castleagegame.com";
        private const string mainPageUrl = baseUrl + "/castle_ws/index.php?";
        private const string demiPowerUrl = baseUrl + "/castle_ws/symbols.php";
        private const string territoryUrl = baseUrl + "/castle_ws/territory.php";
        private const string resultsMainWrapperXPath = "//div[@id=\"results_main_wrapper\"]";
        private const string playerNotLoggedInMessage = "Player not logged in";
        private const string redirectMessage = "HTTP Redirect jött válaszul...";
        private const string logoutUri = "/castle_ws/connect_login.php?platform_action=CA_web3_logout";
        private const string colosseumMatchingUrl = baseUrl + "/castle_ws/five_vs_five.php";
        private const string colosseumMatchingAjax = "matching=matching_request" + ajax2x;
        private const string colosseumBattlePageUrl = baseUrl + "/castle_ws/5v5_battle.php";
        private const string colosseumQueryBattleUrl = baseUrl + "/castle_ws/query_five_battle.php";
        private const string colosseumOpponentFormXPath = "//form[@onsubmit=\"ajaxFormSend('globalContainer', '5v5_battle.php', this);return false;\"]";
        private const string colosseumOpponentHealthXPath = "//*[@id=\"enemy_guild_member_list_1\"]/div[1]/div/div[4]/div[2]/div[1]/div[2]/div";
        private const string colosseumBattleResultMessageXPath = "//*[@id=\"results_main_wrapper\"]/div[1]/div[1]/div";
        private const string colosseumTokenXPath = "//*[@id=\"colosseum_token_current_value\"]";
        private const string colosseumTokenTimerXPath = "//*[@id=\"colosseum_token_time_value\"]";
        private const string collectResourceUrl = baseUrl + "/castle_ws/player_monster_list.php?";
        private const string collectResourceAjax = "action=conquestResourceCollectHeader" + ajax2x;
        private const string collectTerritory = "collect=1" + ajax2x;
        private const string dailySpin = "spin=1&ajax=1";
        private const string archiveAjax = "action=enableItemArchiveBonusHeader" + ajax2x;
        private const string conquestDemiCollectAjax = "action=conquestDemiCollectHeader" + ajax2x;
        private const string resultPopupMessageXpath = "//div[@class=\"result_popup_message\"]";
        private const string playerCodeXpath = "//*[@id=\"app_body\"]";
        #endregion

        public Player Player { get; set; }
        public List<Player> AllPlayers { get; set; }

        public CallCastle(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IParseHtml parseHtml, ILogger<CallCastle> logger)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.parseHtml = parseHtml;
            this.logger = logger;
        }

        public async Task<Player> DummyAsync()
        {
            await Task.Delay(10000);
            logger.LogInformation($"{Player.Username} collect ");
            await this.CollectResourceAsync();
            logger.LogInformation($"{Player.Username} várunk 10mp-et");
            return this.Player;
        }

        public async Task<HttpResponseMessage> GetRequestAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Cookie", this.Player.Cookie);
            request.Headers.Add(HeaderNames.AcceptEncoding, "gzip, deflate, br");
            var result = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return result;
        }

        public async Task<HttpResponseMessage> PostRequestUrlEncodedAsync(string body, string uri)
        {
            var content = new StringContent(body);
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = content;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaTypeUrlEncoded);
            request.Headers.Add("Cookie", this.Player.Cookie);
            request.Headers.Add(HeaderNames.AcceptEncoding, "gzip, deflate, br");
            //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            //request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17763");
            var result = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return result;
        }

        public async Task<bool> LoginAsync()
        {
            var encodedPass = HttpUtility.UrlEncode(Player.Password);
            var encodedUser = HttpUtility.UrlEncode(Player.Username);

            var result = await PostRequestUrlEncodedAsync($"platform_action=CA_web3_login&player_email={encodedUser}&player_password={encodedPass}&x=88&y=31", "/castle_ws/connect_login.php");
            if (result.Headers.TryGetValues("Set-Cookie", out var cookie))
            {
                SetPlayerCookie(cookie);

                if (string.IsNullOrEmpty(Player.PlayerCode)) await GetPlayerCode();

                return true;
            }
            return false;

            async Task GetPlayerCode()
            {
                var response = await GetRequestAsync(mainPageUrl);
                var content = await response.Content.ReadAsStringAsync();
                this.Player.PlayerCode = parseHtml.Regexp(content, playerCodeXpath, "[0-9]{10,15}");
            }
        }

        public async Task LogoutAsync()
        {
            await GetRequestAsync(logoutUri);
            Player.Cookie = null;
        }

        public async Task<(ReturnCodeEnum, string)> CollectResourceAsync()
        {
            var response = await PostRequestUrlEncodedAsync(collectResourceAjax, collectResourceUrl);

            if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.NotLoggedIn, playerNotLoggedInMessage);

            var content = await response.Content.ReadAsStringAsync();

            var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);

            return (ReturnCodeEnum.Ok, parsedPage);

        }

        public async Task<(ReturnCodeEnum, string)> ArchiveAsync()
        {
            var response = await PostRequestUrlEncodedAsync(archiveAjax, mainPageUrl);

            if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.NotLoggedIn, playerNotLoggedInMessage);

            var content = await response.Content.ReadAsStringAsync();

            var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);

            return (ReturnCodeEnum.Ok, parsedPage);

        }

        public async Task<(ReturnCodeEnum, string)> DemiPowerAsync(string id)
        {
            var response = await PostRequestUrlEncodedAsync($"symbol={id}&action=tribute{ajax2x}", demiPowerUrl);

            if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.NotLoggedIn, playerNotLoggedInMessage);

            var content = await response.Content.ReadAsStringAsync();

            var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);

            return (ReturnCodeEnum.Ok, parsedPage);

        }

        public async Task<(ReturnCodeEnum, string)> CrystalPrayerAsync()
        {
            var response = await PostRequestUrlEncodedAsync(conquestDemiCollectAjax, mainPageUrl);

            if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.NotLoggedIn, playerNotLoggedInMessage);

            var content = await response.Content.ReadAsStringAsync();

            var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);
            //var parsedPage = parseHtml.InnerText(content, resultPopupMessageXpath);

            return (ReturnCodeEnum.Ok, parsedPage);

        }

        public async Task<(ReturnCodeEnum, List<string>)> CustomRequestAsync(string uri, bool booster)
        {
            List<string> resultMessages = new();

            //https://www.michalbialecki.com/en/2018/04/19/how-to-send-many-requests-in-parallel-in-asp-net-core/
            if (booster)
            {
                //Parallel.For(0, 100, async (i) =>
                //{
                //    await GetRequestAsync(uri);
                //});

                var requests = Enumerable.Range(0, 50).Select(r =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, uri)
                    {
                        Headers =
                        {
                            { HeaderNames.Cookie, this.Player.Cookie },
                            { HeaderNames.XRequestedWith, "XMLHttpRequest" },
                            { HeaderNames.AcceptEncoding, "gzip, deflate, br" }

                        }, 
                        Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ajax", "1") })
                    };

                    return request;                
                });

                var tasks = requests.Select(req => httpClient.SendAsync(req));
                var responds = await Task.WhenAll(tasks);

                foreach (var task in responds)
                {
                    var content = await task.Content.ReadAsStringAsync();
                    var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath) + '\n';
                    resultMessages.Add(parsedPage);
                }

                return (ReturnCodeEnum.Ok, resultMessages);
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Headers =
                    {
                        { HeaderNames.Cookie, this.Player.Cookie },
                        { HeaderNames.XRequestedWith, "XMLHttpRequest" },
                        { HeaderNames.AcceptEncoding, "gzip, deflate, br" }
                    },
                    Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ajax", "1") })
                };

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    resultMessages.Add(await response.Content.ReadAsStringAsync());
                    return (ReturnCodeEnum.NotOk, resultMessages);

                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);
                resultMessages.Add(parsedPage);

                return (ReturnCodeEnum.Ok, resultMessages);
            }
        }

        public void SetPlayerCookie(IEnumerable<string> cookies)
        {
            var regex = new Regex("CA_[0-9]{1,20}");

            var cookie = cookies.Single(c =>
                        {
                            return (regex.Match(c)).Success;
                        });

            Player.Cookie = cookie;
        }

        public async Task<Player> ColosseumAsync()
        {
            var response = await GetRequestAsync(colosseumBattlePageUrl);

            if (response.StatusCode != HttpStatusCode.Redirect)
            {
                logger.LogInformation($"{Player.Username}: A játékos már csatában van!");
                await ColosseumAttackOpponent(response);
            }
            else
            {
                response = await PostRequestUrlEncodedAsync(colosseumMatchingAjax, colosseumMatchingUrl);

                if (response.StatusCode != HttpStatusCode.Redirect)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);

                    if (parsedPage.Contains("Matchmaking"))
                    {
                        logger.LogInformation($"{Player.Username}: {parsedPage}");
                        await ColosseumWaitForBattleAsync();
                    }
                    else
                    {
                        logger.LogInformation($"{Player.Username}: nem sikerült a match: {parsedPage}");
                        if (parsedPage == "parseHtmlNULL") logger.LogDebug(content);
                    }
                }
            }
            return Player;
        }

        public async Task ColosseumWaitForBattleAsync()
        {
            var i = 0;
            string content = null;

            while (content != "1" && i < 90)
            {
                await Task.Delay(1000);
                var response = await PostRequestUrlEncodedAsync($"player_id={Player.PlayerCode}", colosseumQueryBattleUrl);
                content = await response.Content.ReadAsStringAsync();
                ++i;
            }

            if (content == "1")
            {
                logger.LogInformation($"{Player.Username}: Kezdődik a csata!");
                await ColosseumAttackOpponent();
            }
            else
            {
                logger.LogInformation($"{Player.Username}: Nem indult el a csata 90mp-en belül, vagy a CastleAge nem jelezte!");
            }
        }

        public async Task ColosseumAttackOpponent(HttpResponseMessage response = null)
        {
            if (response == null)
            {
                response = await GetRequestAsync(colosseumBattlePageUrl);

                if (response.StatusCode == HttpStatusCode.Redirect)
                {
                    logger.LogInformation($"{Player.Username}: HIBA! Redirect a csata oldalra belépésnél!");
                    return;
                }

            }

            var content = await response.Content.ReadAsStringAsync();

            var opponentForm = parseHtml.Nodes(content, colosseumOpponentFormXPath);

            var x = opponentForm.Count / 5 - 1;

            var battle = new ColosseumBattleModel
            {

                BattleId = opponentForm[0].ChildNodes[11].Attributes[1].Value,
                Bqh = opponentForm[0].ChildNodes[13].Attributes[2].Value,
                Opponents = new List<OpponentModel>
                {
                    new OpponentModel
                    {
                        TargetId = opponentForm[0<<x].ChildNodes[9].Attributes[1].Value
                    },
                    new OpponentModel
                    {
                        TargetId = opponentForm[1<<x].ChildNodes[9].Attributes[1].Value
                    },
                    new OpponentModel
                    {
                        TargetId = opponentForm[2<<x].ChildNodes[9].Attributes[1].Value
                    },
                    new OpponentModel
                    {
                        TargetId = opponentForm[3<<x].ChildNodes[9].Attributes[1].Value
                    },
                    new OpponentModel
                    {
                        TargetId = opponentForm[4<<x].ChildNodes[9].Attributes[1].Value
                    },
                }
            };

            while (true)
            {
                var token = parseHtml.InnerText(content, colosseumTokenXPath);

                if (token == "0")
                {
                    var tokenTimer = parseHtml.InnerText(content, colosseumTokenTimerXPath);
                    logger.LogInformation($"{Player.Username}: Elfogyott a token, {tokenTimer} időt várunk!");
                    await Task.Delay(TimeSpan.ParseExact(tokenTimer, "m':'ss", null));
                    logger.LogInformation($"{Player.Username}: Várakozás vége!");
                    response = await GetRequestAsync(colosseumBattlePageUrl);

                    if (response.StatusCode == HttpStatusCode.Redirect) break;

                    content = await response.Content.ReadAsStringAsync();
                }

                for (var i = 0; i < battle.Opponents.Count; i++)
                {
                    battle.Opponents[i].HealthStatus = parseHtml.InnerText(content, $"//*[@id=\"enemy_guild_member_list_1\"]/div[{i + 1}]/div/div[4]/div[2]/div[1]/div[1]/div/span");
                }

                var opponent = battle.Opponents.FirstOrDefault(o => o.HealthStatus == "Healthy");
                if (opponent == null) opponent = battle.Opponents.FirstOrDefault(o => o.HealthStatus != "Stunned");

                if (opponent == null)
                {
                    logger.LogInformation($"{Player.Username}: minden ellenfél stunned, kilépek!");
                    break;
                }

                string attack = $"action=guild_attack&attack_type=normal&attack_key=1&target_id={opponent.TargetId}&battle_id={battle.BattleId}&bqh={battle.Bqh}" + ajax2x;

                response = await PostRequestUrlEncodedAsync(attack, colosseumBattlePageUrl);

                if (response.StatusCode == HttpStatusCode.Redirect)
                {
                    logger.LogInformation($"{Player.Username}: Redirect! Vége a csatának?");
                    break;
                }

                content = await response.Content.ReadAsStringAsync();

                var parseResult = parseHtml.InnerText(content, colosseumBattleResultMessageXPath);

                if (parseResult == "parseHtmlNULL")
                {
                    parseResult = parseHtml.InnerText(content, resultsMainWrapperXPath);
                    if (String.IsNullOrWhiteSpace(parseResult))
                    {
                        logger.LogInformation($"Csata ID: {battle.BattleId} {Player.Username}: Nem adott vissza semmilyen eredményt, kilépek a csatából!");
                        break;
                    }
                    if (parseResult.Contains("The battle is over")) break;
                    if (parseResult.Contains("not have any health"))
                    {
                        logger.LogInformation($"Csata ID: {battle.BattleId} {Player.Username}: {parseResult}");
                        continue;
                    }
                }

                if (String.IsNullOrWhiteSpace(parseResult))
                {
                    logger.LogInformation($"Csata ID: {battle.BattleId} {Player.Username}: Nem adott vissza semmilyen eredményt, kilépek a csatából!");
                    break;
                }

                logger.LogInformation($"Csata ID: {battle.BattleId} {Player.Username}: A támadás eredménye: {parseResult}!");
            }

            logger.LogInformation($"{Player.Username}: Kilépek a csata algoritmusból!");
        }

        public async Task<(ReturnCodeEnum, string)> DailySpinAsync()
        {
            var response = await PostRequestUrlEncodedAsync(dailySpin, mainPageUrl);

            if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.NotLoggedIn, playerNotLoggedInMessage);

            var content = await response.Content.ReadAsStringAsync();

            var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);
            //var parsedPage = parseHtml.InnerText(content, resultPopupMessageXpath);

            return (ReturnCodeEnum.Ok, parsedPage);
        }

        public async Task<(ReturnCodeEnum, string)> CollectTerritoryAsync()
        {
            var response = await PostRequestUrlEncodedAsync(collectTerritory, territoryUrl);

            if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.Redirect, redirectMessage);
            //if (response.StatusCode == HttpStatusCode.Redirect) return (ReturnCodeEnum.NotLoggedIn, playerNotLoggedInMessage);

            var content = await response.Content.ReadAsStringAsync();

            var parsedPage = parseHtml.InnerText(content, resultsMainWrapperXPath);
            //var parsedPage = parseHtml.InnerText(content, resultPopupMessageXpath);

            return (ReturnCodeEnum.Ok, parsedPage);
        }
    }
}