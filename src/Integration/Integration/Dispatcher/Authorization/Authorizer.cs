namespace Ragnar.Integration.Dispatcher.Authorization
{
    public interface IAuthorizer
    {
        bool CheckAccess();
    }
}