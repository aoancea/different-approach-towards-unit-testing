using System;

namespace Ragnar.Mock.InterestV3.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}