using System;

namespace Ragnar.IntegrationDriven.Interest.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}