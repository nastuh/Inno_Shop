using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Services
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly List<object> _publishedEvents = new();

        public Task PublishAsync<T>(T eventMessage) where T : class
        {
            _publishedEvents.Add(eventMessage);
            Console.WriteLine($"Event published: {typeof(T).Name}");
            return Task.CompletedTask;
        }

        public List<T> GetPublishedEvents<T>() where T : class
        {
            return _publishedEvents.OfType<T>().ToList();
        }
    }
}