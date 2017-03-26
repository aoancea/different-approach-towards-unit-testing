using System;

namespace Ragnar.IntegrationDriven.Interest.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}