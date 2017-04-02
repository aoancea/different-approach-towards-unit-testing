using System;

namespace Ragnar.MockDriven.InterestV2.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}