using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ChocolaBot {
    class Program {
        private readonly DiscordSocketClient _client = new DiscordSocketClient();
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public Program() {
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
            _client.InteractionCreated += InteractionCreatedAsync;
            _client.ButtonExecuted += ButtonHandler;
        }

        public async Task MainAsync() {
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
            var message = messageParam as SocketUserMessage;
            if(message == null) return;

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