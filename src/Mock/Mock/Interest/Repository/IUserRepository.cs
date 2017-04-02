using System;

namespace Ragnar.Mock.Interest.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}