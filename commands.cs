using Discord;
using Discord.WebSocket;

namespace KrulTepasBot {
    public class commands {
        static Emoji heart = new Emoji("\u2665");
        public static async Task ping(SocketUserMessage message) {
            var cb = new ComponentBuilder().WithButton("Click me!", "ping", ButtonStyle.Primary);
            await message.ReplyAsync("pong!", components: cb.Build(), allowedMentions: AllowedMentions.None);
        }

        public static async Task avatar(SocketUserMessage message) {
            EmbedBuilder avatarEmbed = new EmbedBuilder();
            if(message.MentionedUsers.Count > 0) {
                avatarEmbed.Title = message.MentionedUsers.First().Username;
                avatarEmbed.ImageUrl = message.MentionedUsers.First().GetAvatarUrl();
            }
            else {
                avatarEmbed.Title = message.Author.Username;
                avatarEmbed.ImageUrl = message.Author.GetAvatarUrl();
            }

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
            String time;

            if(message.MentionedUsers.Count > 0) {
                if(min > 0 && (secs - (min * 60)) <= 0) {
                    time = $"{message.MentionedUsers.First().Username} can gain exp again in {min.ToString()}:00";
                }
                else if(min > 0 && (secs - (min * 60)) < 10) {
                    time = $"{message.MentionedUsers.First().Username} can gain exp again in {min.ToString()}:0{(secs - (min * 60)).ToString()}";
                }
                else if(min > 0 && (secs - (min * 60)) > 10) {
                    time = $"{message.MentionedUsers.First().Username} can gain exp again in {min.ToString()}:{(secs - (min * 60)).ToString()}";
                }
                else if(min <= 0 && (secs - (min * 60)) > 0) {
                    time = $"{message.MentionedUsers.First().Username} can gain exp again in {secs.ToString()} seconds";
                }
                else {
                    time = $"{message.MentionedUsers.First().Username} can gain exp!";
                }
                await message.ReplyAsync($"{message.MentionedUsers.First().Username} is currently level {userLevel}!\n{message.MentionedUsers.First().Username} needs {100 - userExp} more exp to level up! | {message.MentionedUsers.First().Username} has a total of {userTotalExp} acquired already!\n{time}!", allowedMentions: AllowedMentions.None);
            }
            else {
                if(min > 0 && (secs - (min * 60)) <= 0) {
                    time = $"You can gain exp again in {min.ToString()}:00";
                }
                else if(min > 0 && (secs - (min * 60)) < 10) {
                    time = $"You can gain exp again in {min.ToString()}:0{(secs - (min * 60)).ToString()}";
                }
                else if(min > 0 && (secs - (min * 60)) > 10) {
                    time = $"You can gain exp again in {min.ToString()}:{(secs - (min * 60)).ToString()}";
                }
                else if(min <= 0 && (secs - (min * 60)) > 0) {
                    time = $"You can gain exp again in {secs.ToString()} seconds";
                }
                else {
                    time = "You just gained exp";
                }
                await message.ReplyAsync($"You are currently level {userLevel}!\nYou need {100 - userExp} more exp to level up! | You have a total of {userTotalExp} acquired already!\n{time}!", allowedMentions: AllowedMentions.None);
            }
        }
    }
}