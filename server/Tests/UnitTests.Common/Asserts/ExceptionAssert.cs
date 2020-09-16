using System;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Xunit;
using Xunit.Sdk;

namespace UnitTests.Common.Asserts
{
    public static class ExceptionAssert
    {
        public static T Throws<T>(Action testAction) where T : Exception
        {
            var actualException = GetException<T>(testAction);
            Assert.NotNull(actualException);
            Assert.IsType<T>(actualException);
            return actualException;
        }

        public static T ThrowsAsync<T>(Func<Task> testAction) where T : Exception
        {
            var actualException = GetAsyncException<T>(testAction);
            Assert.NotNull(actualException);
            Assert.IsType<T>(actualException);
            return actualException;
        }

        public static void ThrowsAsync<T>(T expected, Func<Task> testAction)
            where T : Exception
        {
            var actualException = GetAsyncException<Exception>(testAction);
            var comparisonConfig = new ComparisonConfig
            {
                MembersToIgnore =
                {
                    nameof(Exception.StackTrace),
                    nameof(Exception.TargetSite),
                    nameof(Exception.Source)
                }
            };
            var compareObjects = new CompareLogic(comparisonConfig);
            var comparisonResult = compareObjects.Compare(expected, actualException);

            if (comparisonResult.AreEqual)
                return;

            throw new XunitException($"{comparisonResult.DifferencesString}.");
        }

        private static T GetException<T>(Action testAction) where T : Exception
        {
            try
            {
                testAction();
            }
            catch (Exception ex)
            {
                return ex as T;
            }

            throw new XunitException("Action did not raise any exceptions");
        }

        private static T GetAsyncException<T>(Func<Task> testAction) where T : Exception
        {
            try
            {
                testAction().Wait();
            }
            catch (AggregateException ex)
            {
                return ex.InnerException as T;
            }

            throw new XunitException("Action did not raise any exceptions");
        }

        public static void NoAsyncExceptionThrown<T>(Func<Task> testAction) where T : Exception
        {
            try
            {
                testAction().Wait();
            }
            catch (T)
            {
                throw new XunitException($"Expected no {typeof(T).Name} to be thrown");
            }
        }
    }
}