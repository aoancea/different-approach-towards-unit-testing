using System;

namespace Ragnar.Integration.InterestV3.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}