using NUnit.Framework;

namespace NUnit.Experimental.Hardcoded.TestAdapter.Tests;

public static class TimedTests
{
	[Test]
	public static void TestOf50ms() => Thread.Sleep(TimeSpan.FromMilliseconds(50.0));

	[Test]
	public static void TestOf100ms() => Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

	[Test]
	public static void TestOf200ms() => Thread.Sleep(TimeSpan.FromMilliseconds(200.0));

	[Test]
	public static void TestOf500ms() => Thread.Sleep(TimeSpan.FromMilliseconds(500.0));

	[Test]
	public static void TestOf1000ms() => Thread.Sleep(TimeSpan.FromMilliseconds(1000.0));
}