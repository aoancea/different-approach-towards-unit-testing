using System;

namespace Ragnar.Integration.Interest.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}