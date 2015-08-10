using NewVevo.Entity.Models;
using RestSharp;
using System.Collections.Generic;

namespace NewVevo.Entity.Services
{
    public class VideoService
    {
        private readonly string _key;
        public VideoService(string key = @"_TMw_fGgJHvzr84MqwK1eWhBgbdebZhAm_y3W1ou-sU1.1439830800.f-MhU1MNRDGU2IxXn0LjvOByguOfXLSa5mbBrvqLx5KFfXnxQ72KR7EyyF-V-S0AQcs9Xw2")
        {
            _key = key;
        }

        public List<Stream> GetVideoUrls(string isrc)
        {
            var client = new RestClient(@"https://apiv2.vevo.com/");
            client.AddDefaultHeader(@"Authorization", "Bearer " + _key);

            var request = new RestRequest("video/{isrc}/streams/mp4", Method.GET);
            request.AddUrlSegment("isrc", isrc);

            var result = client.Execute<List<Stream>>(request);
            return result.Data;
        }
    }
}
