using System;

namespace Ragnar.Integration.Tooling.Utilities
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }

    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}