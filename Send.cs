using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

class Send
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

        string message = "Hello RabbitMQ!";
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties(); // ✅ new in v7, strongly typed

        await channel.BasicPublishAsync<BasicProperties>(
            exchange: "",
            routingKey: "hello",
            mandatory: false,
            props,
            body);

        Console.WriteLine($" [x] Sent: {message}");
    }
}
