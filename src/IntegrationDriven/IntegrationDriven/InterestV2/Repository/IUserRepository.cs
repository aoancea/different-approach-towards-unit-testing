using System;

namespace Ragnar.IntegrationDriven.InterestV2.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}