using System;

namespace Ragnar.Integration.Tooling.Utilities
{
    public static class RagnarGuid
    {
        public static IGuidProvider GuidProvider { get; set; }

        public static Guid NewGuid()
        {
            return GuidProvider.NewGuid();
        }
    }
}