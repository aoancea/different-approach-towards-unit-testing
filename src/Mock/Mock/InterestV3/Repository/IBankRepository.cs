using System;

namespace Ragnar.Mock.InterestV3.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}