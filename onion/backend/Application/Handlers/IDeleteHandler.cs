namespace Application.Handlers;

public interface IDeleteHandler<T>
{
    Task HandleDeleteRequest(T entityId);
}