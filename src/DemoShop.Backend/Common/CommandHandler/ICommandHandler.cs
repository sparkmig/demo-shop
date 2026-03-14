namespace Common.CommandHandler
{
    public interface ICommandHandler<T>
    {
        Task HandleAsync(T command);
    }

    public interface IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        Task<TResponse> GetAsync(TQuery query);
    }

    public interface IQuery<TResult>;
}
