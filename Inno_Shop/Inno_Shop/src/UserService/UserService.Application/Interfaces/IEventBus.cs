namespace UserService.Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T eventMessage) where T : class;
    }
}