using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MC_CommandControlFrontEnd.Controllers
{
    public class CommandController : ApiController
    {
        [HttpGet]
        [Route("api/CommandControl/{deviceId}")]
        public HttpResponseMessage Get(string deviceId)
        {
            if (String.IsNullOrEmpty(deviceId))
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            ConfigurationOptions configurationOptions = new ConfigurationOptions();
            configurationOptions.EndPoints.Add("<< add your Redis cache name here >>.redis.cache.windows.net");
            configurationOptions.Ssl = true;
            configurationOptions.Password = "<< Add Access Key Here >>";

            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            IDatabase commandQueue = connectionMultiplexer.GetDatabase();

            RedisValue redisValue = commandQueue.ListRightPop(deviceId);
            if (redisValue.HasValue)
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(redisValue.ToString()),
                };
            else
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Content = new StringContent(""),
                };
        }

        [HttpPost]
        [Route("api/CommandControl/{deviceId}/{command}")]
        public async Task<HttpResponseMessage> Post(string deviceId, string command)
        {
            if (String.IsNullOrEmpty(deviceId) || String.IsNullOrEmpty(command))
                return new HttpResponseMessage(HttpStatusCode.PartialContent);

            ConfigurationOptions configurationOptions = new ConfigurationOptions();
            configurationOptions.EndPoints.Add("<< add your Redis cache name here >>.redis.cache.windows.net");
            configurationOptions.Ssl = true;
            configurationOptions.Password = "<< Add Access Key Here >>";

            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            IDatabase commandQueue = connectionMultiplexer.GetDatabase();

            await commandQueue.ListRightPushAsync(deviceId, command);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpDelete]
        [Route("api/CommandControl/{deviceId}")]
        public async Task<HttpResponseMessage> Delete(string deviceId)
        {
            if (String.IsNullOrEmpty(deviceId))
                return new HttpResponseMessage(HttpStatusCode.PartialContent);

            ConfigurationOptions configurationOptions = new ConfigurationOptions();
            configurationOptions.EndPoints.Add("<< add your Redis cache name here >>.redis.cache.windows.net");
            configurationOptions.Ssl = true;
            configurationOptions.Password = "<< Add Access Key Here >>";

            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            IDatabase commandQueue = connectionMultiplexer.GetDatabase();

            await commandQueue.KeyDeleteAsync(deviceId);
            return new HttpResponseMessage(HttpStatusCode.OK);

        }

    }
}
