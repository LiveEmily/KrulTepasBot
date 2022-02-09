using Discord;
using Discord.WebSocket;

namespace ChocolaBot {
    public class commands {
        public static Task ping(ref SocketUserMessage message) {
            var cb = new ComponentBuilder().WithButton("Click me!", "ping", ButtonStyle.Primary);
            message.Channel.SendMessageAsync("pong!", components: cb.Build());

            return Task.CompletedTask;
        }
    }
}