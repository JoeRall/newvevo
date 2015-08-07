using NewVevo.Entity.Models;
using RestSharp;
using System.Collections.Generic;

namespace NewVevo.Entity.Services
{
    public class VideoService
    {
        private readonly string _key;
        public VideoService(string key = @"_TMw_fGgJHvzr84MqwK1eWhBgbdebZhAm_y3W1ou-sU1.1439085600.xrqkd87wbBX66Jh0rdWF_bDvOl6CfmhH_vc1-THLJjnmOfVeGM1dK14xiHsiZTSP7-jakA2")
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
