using System;
using System.Collections.Generic;

namespace Ragnar.Integration.UnitTesting.Tooling
{
    public static class UnitTestHelper
    {
        public static Guid guid1 = new Guid("c7ec5b53-e800-4bac-b87f-4f24694aeb9f");
        public static Guid guid2 = new Guid("b3dff433-c9af-43dd-a556-6c45185b484f");
        public static Guid guid3 = new Guid("2f9fcb8a-2b97-4964-81b3-de49d3cde3ef");

        public class GuidProvider : Integration.Tooling.Utilities.IGuidProvider
        {
            private readonly Queue<Guid> guidsCache;

            public GuidProvider(IEnumerable<Guid> guids)
            {
                guidsCache = new Queue<Guid>(guids);
            }

            public Guid NewGuid()
            {
                return guidsCache.Dequeue();
            }
        }
    }
}