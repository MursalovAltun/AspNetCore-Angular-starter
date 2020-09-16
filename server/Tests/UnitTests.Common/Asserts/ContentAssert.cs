using KellermanSoftware.CompareNetObjects;
using Xunit.Sdk;

namespace UnitTests.Common.Asserts
{
    public static class ContentAssert
    {
        public static void AreEqual(object expected, object actual, string errorMessage = null)
        {
            var compareObjects = new CompareLogic(new ComparisonConfig());

            var comparisonResult = compareObjects.Compare(expected, actual);

            if (comparisonResult.AreEqual)
                return;

            throw new XunitException($"{comparisonResult.DifferencesString}.\n {errorMessage}");
        }

        public static bool IsEqual(object expected, object actual, string errorMessage = null)
        {
            var compareObjects = new CompareLogic(new ComparisonConfig());

            var comparisonResult = compareObjects.Compare(expected, actual);

            if (comparisonResult.AreEqual)
                return true;

            throw new XunitException($"{comparisonResult.DifferencesString}.\n {errorMessage}");
        }
    }
}