using System;

namespace Ragnar.Mock.InterestV2.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}