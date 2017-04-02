using System;

namespace Ragnar.Mock.InterestV2.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}