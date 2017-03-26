using System;

namespace Ragnar.MockDriven.Interest.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}