using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using System.Net.Http;
namespace VpnChecker
{
    
    namespace VoiceCrasherProtect
    {
        public static class WebHook
        {
            public static void SendDiscordWebhook(string WebhookURL, string Text, string Title = null, string WebhookText = null, string AvatarURL = null, string ImageURL = null, int Color = 0xff0000)
            {
                try
                {
                    var message = new
                    {
                        content = Text,
                        avatar_url = AvatarURL,
                        embeds = new[]
        {
                    new
                    {
                        color = Color,
                        title = Title,
                        description = WebhookText,
                        image = new { url = ImageURL }
                    }
                }
                    };

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(message);

                    client.PostAsync(WebhookURL, new StringContent(json, Encoding.UTF8, "application/json"));
                }
                catch
                {
                    Log.Error("Webhook is wrong. Please check your configuration");
                }
            }
        }
    }

}
