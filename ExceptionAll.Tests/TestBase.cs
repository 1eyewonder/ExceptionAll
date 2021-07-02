using System;
using Xunit.Abstractions;

namespace ExceptionAll.Tests
{
    public class TestBase
    {
        protected readonly ITestOutputHelper TestOutputHelper;

        public TestBase(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        }
    }
}