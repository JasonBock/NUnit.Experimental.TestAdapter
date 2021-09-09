using NUnit.Framework;

namespace NUnit.Experimental.Hardcoded.TestAdapter.Tests;

public static class TimedTests
{
	[Test]
	public static void TestOf500ms() => Thread.Sleep(TimeSpan.FromMilliseconds(500.0));

	[Test]
	public static void TestOf1000ms() => Thread.Sleep(TimeSpan.FromMilliseconds(1000.0));

	[Test]
	public static void TestOf1500ms() => Thread.Sleep(TimeSpan.FromMilliseconds(1500.0));

	[Test]
	public static void TestOf2000ms() => Thread.Sleep(TimeSpan.FromMilliseconds(2000.0));
}