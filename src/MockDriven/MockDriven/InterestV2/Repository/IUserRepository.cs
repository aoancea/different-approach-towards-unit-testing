using System;

namespace Ragnar.MockDriven.InterestV2.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}