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

        public static async Task level(SocketUserMessage message, int userLevel, int userExp, int userTotalExp, Int32 userTime, Int32 currentTime) {
            int secs = (180 - (currentTime - userTime));
            int min = secs / 60;
            String time = $"You can gain exp again in {secs.ToString()} seconds";
            if(min > 0 && secs <= 0) {
                time = $"You can gain exp again in {min.ToString()}:00";
            }
            else if(min > 0 && secs < 10) {
                time = $"You can gain exp again in {min.ToString()}:0{(secs - (min * 60)).ToString()}";
            }
            else if(min > 0 && secs > 10) {
                time = $"You can gain exp again in {min.ToString()}:{(secs - (min * 60)).ToString()}";
            }
            else {
                time = "You just gained exp";
            }
            await message.ReplyAsync($"You are currently level {userLevel}!\nYou need {100 - userExp} more exp to level up! | You have a total of {userTotalExp} acquired already!\n{time}!");
            /*cmd.CommandText = $"SELECT lvl FROM users WHERE userId = {message.Author.Id}";
            using(var reader = cmd.ExecuteReader()) {
                while(reader.Read()) {
                    var userLevel = reader.GetInt16(0);

                    await message.ReplyAsync($"Your level is: {userLevel}");
                }
            }*/
        }
    }
}