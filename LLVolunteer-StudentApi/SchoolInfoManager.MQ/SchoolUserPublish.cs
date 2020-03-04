using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace SchoolInfoManager.MQ.Publish
{
    /// <summary>
    /// 推送消息
    /// </summary>
    public class SchoolUserPublish
    {
        private readonly IModel _channel;

        private readonly ILogger _logger;


        public SchoolUserPublish(ILogger<SchoolUserPublish> logger)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost"
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQClient init fail");
            }
            _logger = logger;
        }

        public virtual void PushMessage(string routingKey, object message)
        {
            _logger.LogInformation($"PushMessage,routingKey:{routingKey}");
            _channel.QueueDeclare(
                queue: "schoolUserQueue",
            durable: false,
            exclusive: false,
            autoDelete: true,
            arguments: null);
            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            _channel.BasicPublish(
                exchange: "schoolUserExchange",
            routingKey: routingKey,
            basicProperties: null,
            body: body);
        }
    }
}
