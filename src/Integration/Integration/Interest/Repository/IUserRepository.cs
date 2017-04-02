using System;

namespace Ragnar.Integration.Interest.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}