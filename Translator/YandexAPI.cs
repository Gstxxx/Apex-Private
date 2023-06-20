using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Private_Apex.Translator
{
    public static class YandexAPI
    {
        public static string TranslationRequest(string text, string payload)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (payload is null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(payload),
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "text", text },
                    { "options", "4" },
                }),
            };

            var response = client.Send(request);
            response.EnsureSuccessStatusCode();
            var body = JsonConvert.DeserializeObject<YandexStructure>(response.Content.ReadAsStringAsync().Result);
            return body.Text[0];
        }
    }

    public struct YandexStructure
    {
        [JsonProperty("align")]
        public string[] Align { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("text")]
        public string[] Text { get; set; }
    }
}
