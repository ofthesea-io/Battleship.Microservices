namespace Battleship.Microservices.Infrastructure.Messages
{
    using System.Threading.Tasks;

    public interface IMessagePublisher
    {
        string Host { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        string Exchange { get; set; }

        string Queue { get; set; }

        Task PublishMessageAsync(string message, string queue);
    }
}