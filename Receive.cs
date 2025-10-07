using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Receive
{
    public static async Task RunAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "hello",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received: {message}");

            // Acknowledge message (since autoAck is false)
            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        // ✅ BasicConsumeAsync is available in v7.x
        string consumerTag = await channel.BasicConsumeAsync(
            queue: "hello",
            autoAck: false,
            consumer: consumer);

        Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
        Console.ReadLine();

        // Optional: cancel consumption when exiting
        await channel.BasicCancelAsync(consumerTag);
    }
}
