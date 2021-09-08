using NUnit.Framework;

namespace NUnit.Experimental.TestAdapter.Tests;

public static class TimedTests
{
	[Test]
	public static void TestOf50ms() => Task.Delay(TimeSpan.FromMilliseconds(50.0));

	[Test]
	public static void TestOf100ms() => Task.Delay(TimeSpan.FromMilliseconds(100.0));

	[Test]
	public static void TestOf200ms() => Task.Delay(TimeSpan.FromMilliseconds(200.0));

	[Test]
	public static void TestOf500ms() => Task.Delay(TimeSpan.FromMilliseconds(500.0));

	[Test]
	public static void TestOf1000ms() => Task.Delay(TimeSpan.FromMilliseconds(1000.0));
}