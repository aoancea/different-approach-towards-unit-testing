using System;

namespace Ragnar.MockDriven.Interest.Repository
{
    public interface IBankRepository
    {
        Model.Bank Detail(Guid bankId, Guid userId);
    }
}