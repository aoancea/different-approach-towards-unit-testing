using System;

namespace Ragnar.Integration.InterestV2.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}