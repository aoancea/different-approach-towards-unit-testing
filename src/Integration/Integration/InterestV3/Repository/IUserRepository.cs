using System;

namespace Ragnar.Integration.InterestV3.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}