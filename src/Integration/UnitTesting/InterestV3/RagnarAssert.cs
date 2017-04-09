using DeepEqual.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ragnar.Integration.UnitTesting.InterestV3
{
    public static class RagnarAssert
    {
        public static void AreEqual<E, A>(E expected, A actual)
        {
            AreEqual(expected, actual, new RagnarAssertOptions());
        }

        public static void AreEqual<E, A>(E expected, A actual, RagnarAssertOptions options)
        {
            CompareSyntax(expected, actual, options).Assert();
        }

        public static bool Match<E, A>(E expected, A actual)
        {
            return Match(expected, actual, new RagnarAssertOptions());
        }

        public static bool Match<E, A>(E expected, A actual, RagnarAssertOptions options)
        {
            return CompareSyntax(expected, actual, options).Compare();
        }


        private static CompareSyntax<A, E> CompareSyntax<E, A>(E expected, A actual, RagnarAssertOptions options)
        {
            CompareSyntax<A, E> compareSintax = actual.WithDeepEqual(expected);

            compareSintax = compareSintax.IgnoreProperty(pr =>
            {
                string[] propertyNames;
                return options.TypeExclusions.TryGetValue(pr.DeclaringType, out propertyNames) && propertyNames.Contains(pr.Name);
            });

            foreach (Expression expressionExclusion in options.ExpressionExclusions)
            {
                Expression<Func<E, object>> expectedPropertyAccessor = expressionExclusion as Expression<Func<E, object>>;

                if (expectedPropertyAccessor != null)
                    compareSintax = compareSintax.IgnoreDestinationProperty(expectedPropertyAccessor);

                Expression<Func<A, object>> actualPropertyAccessor = expressionExclusion as Expression<Func<A, object>>;

                if (actualPropertyAccessor != null)
                    compareSintax = compareSintax.IgnoreSourceProperty(actualPropertyAccessor);
            }

            return compareSintax;
        }
    }

    public class RagnarAssertOptions
    {
        public RagnarAssertOptions()
        {
            ExpressionExclusions = new List<Expression>();
            TypeExclusions = new Dictionary<Type, string[]>();
        }

        public List<Expression> ExpressionExclusions { get; set; }

        public Dictionary<Type, string[]> TypeExclusions { get; set; }
    }
}
