/// <summary>
/// Requires NuGet package: MSTest.TestFramework
/// (Added automatically if generated from Unit Test project template)
/// May require an additonal NuGet package if Test Explorer won't run tests
/// NuGet package: MSTest.TestAdapter
/// (This issue might be caused by another test runner on system from current or previous VS version, like Resharper or NUnit.)
/// </summary>
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void ExpectedPassTestMethod()
    {
        Assert.AreEqual(1, 1);
    }

    [TestMethod]
    public void ExpectedFailTestMethod()
    {
        Assert.AreEqual(1, 2, "As expected, tested failed because 1 != 2");
    }

    /// <summary>
    /// Async Tests not supported. If absolutely required, see this article for ugly technique required
    /// https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/november/async-programming-unit-testing-asynchronous-code
    /// This test method might appear in Test Explorer, but yields a "Not Run" result when attempted.
    /// </summary>
    [TestMethod]
    public async void AsyncTestMethod1()
    {
        Assert.AreEqual(1, 1);
    }
}