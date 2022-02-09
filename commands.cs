using Discord;
using Discord.WebSocket;

namespace ChocolaBot {
    public class commands {
        static Emoji heart = new Emoji("\u2665");
        public static async Task ping(SocketUserMessage message) {
            var cb = new ComponentBuilder().WithButton("Click me!", "ping", ButtonStyle.Primary);
            await message.ReplyAsync("pong!", components: cb.Build(), allowedMentions: AllowedMentions.None);
        }

        public static async Task avatar(SocketUserMessage message) {
            var avatarEmbed = new EmbedBuilder {
                Title = message.Author.Username,
                ImageUrl = message.Author.GetAvatarUrl()
            };

            var msg = await message.ReplyAsync(embed: avatarEmbed.Build(), allowedMentions: AllowedMentions.None);

            await msg.AddReactionAsync(heart);
        }

        public static async Task button(SocketUserMessage message) {
            var content = message.Content.Split(' ');
            var cb = new ComponentBuilder().WithButton(content[1], content[2], ButtonStyle.Primary);
            await message.Channel.SendMessageAsync(content[3], components: cb.Build());
            await message.DeleteAsync();
        }
    }
}