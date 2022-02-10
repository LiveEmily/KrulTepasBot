using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Data.SQLite;

namespace ChocolaBot {
    class Program {
        private readonly DiscordSocketClient _client = new DiscordSocketClient();
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private static string db = @"URI=file:chocola";
        private static SQLiteConnection con = new SQLiteConnection(db);

        public Program() {
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
            _client.InteractionCreated += InteractionCreatedAsync;
            _client.ButtonExecuted += ButtonHandler;

            con.Open();
        }

        public async Task MainAsync() {
            SQLiteCommand cmd = new SQLiteCommand(con);

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(userId INT PRIMARY KEY, lvl INT NOT NULL, exp INT NOT NULL, totalExp INT NOT NULL, time INT NOT NULL)";
            await cmd.ExecuteNonQueryAsync();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("chocolatoken"));
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log) {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync() {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage messageParam) {
            int userLevel = 1;
            int userExp = 0;
            int userTotalExp = 0;
            String hour = DateTime.Now.ToString("HH");
            String min = DateTime.Now.ToString("mm");
            String sec = DateTime.Now.ToString("ss");
            Int32 currentTime = (Int32.Parse(hour) * 3600) + (Int32.Parse(min) * 60) + Int32.Parse(sec);
            Int32 userTime = 0;
            var message = messageParam as SocketUserMessage;
            if(message == null || message.Author.IsBot) return;
            SQLiteCommand cmd = new SQLiteCommand(con);

            cmd.CommandText = $"INSERT OR IGNORE INTO users(userId, lvl, exp, totalExp, time) VALUES({message.Author.Id}, {userLevel}, {userExp}, {userTotalExp}, {userTime})";
            await cmd.ExecuteNonQueryAsync();

            var rnd = new Random();
            int exp = rnd.Next(5, 10);

            cmd.CommandText = $"SELECT * FROM users WHERE userId = {message.Author.Id}";
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                userLevel = rdr.GetInt16(1);
                userExp = rdr.GetInt16(2);
                userTotalExp = rdr.GetInt16(3);
                userTime = rdr.GetInt32(4);
            }
            await rdr.CloseAsync();

            if(currentTime - 180 >= userTime) {
                userExp = userExp + exp;

                if(userExp >= 100) {
                    userLevel++;
                    userTotalExp = userTotalExp + userExp;
                    userExp = userTotalExp - ((userLevel - 1) * 100);
                }
                else {
                    userTotalExp = userTotalExp + userExp;
                }

                cmd.CommandText = $"UPDATE users SET lvl = {userLevel}, exp = {userExp}, totalExp = {userTotalExp}, time = {currentTime} WHERE userId = {message.Author.Id}";
                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            int argPos = 0;
            var msg = message.Content.TrimStart('!').Split(' ');

            if(!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot) return;

            if(message.Author.Id == _client.CurrentUser.Id) return;


            /*if(msg == "ping") {
                var cb = new ComponentBuilder().WithButton("Click me!", "ping", ButtonStyle.Primary);

                await message.Channel.SendMessageAsync("pong!", components: cb.Build());
            }*/

            switch(msg[0]) {
                case "ping":
                    await commands.ping(message);
                    break;
                case "avatar":
                    await commands.avatar(message);
                    break;
                case "button":
                    await commands.button(message);
                    break;
                case "level":
                    await commands.level(message, userLevel, userExp, userTotalExp, userTime, currentTime);
                    break;
                default:
                    break;
            }
        }

        private async Task ButtonHandler(SocketMessageComponent component) {
            switch(component.Data.CustomId) {
                case "ping":
                    await component.UpdateAsync(x => {
                        x.Content = $"Thank you {component.User.Mention} for clicking my button!";
                        x.Components = new ComponentBuilder().WithButton("Thank you!", "success", ButtonStyle.Success, disabled: true).Build();
                    });
                    break;
                case "button":
                    await component.UpdateAsync(x => {
                        x.Content = $"Thank you {component.User.Mention} for clicking my button!";
                        x.Components = new ComponentBuilder().WithButton("Thank you!", "success", ButtonStyle.Success, disabled: true).Build();
                    });
                    break;
                default:
                    await component.UpdateAsync(x => {
                        x.Content = $"Thank you {component.User.Mention} for clicking my button!";
                        x.Components = new ComponentBuilder().WithButton("Thank you!", "success", ButtonStyle.Success, disabled: true).Build();
                    });
                    break;
            }
        }

        private Task InteractionCreatedAsync(SocketInteraction interaction) {
            //(TODO) Fill this in
            return Task.CompletedTask;
        }
    }
}