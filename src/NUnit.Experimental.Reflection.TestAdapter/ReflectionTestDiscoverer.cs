using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NUnit.Experimental.Reflection.TestAdapter;

[DefaultExecutorUri(Shared.ExecutorUri)]
public sealed class ReflectionTestDiscoverer
	: ITestDiscoverer
{
	public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
		IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
	{
		foreach (var source in sources)
		{
			// TODO: This would need LOTS of exception handling to ensure this was
			// a loadable assembly file.
			var assembly = Assembly.LoadFile(source);

			foreach (var type in assembly.GetTypes())
			{
				foreach (var method in type.GetMethods())
				{
					if (method.GetCustomAttribute<TestAttribute>() is not null)
					{
						var fullyQualifiedName = $"{type.FullName}::{method.Name}";
						discoverySink.SendTestCase(new TestCase(fullyQualifiedName, new Uri(Shared.ExecutorUri), source));
					}
				}
			}
		}
	}
}
