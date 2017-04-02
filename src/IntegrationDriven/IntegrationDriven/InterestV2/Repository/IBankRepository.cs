using System;

namespace Ragnar.IntegrationDriven.InterestV2.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}