using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NUnit.Experimental.Hardcoded.TestAdapter;

[ExtensionUri(Shared.ExecutorUri)]
public sealed class ReflectionTestExecutor
	: ITestExecutor
{
	public void Cancel() => throw new NotImplementedException();

	public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
	{
		// Find all the tests with a given source.
		var testCaseGroups = tests.GroupBy(_ => _.Source);

		foreach (var testCaseGroup in testCaseGroups)
		{
			var assembly = Assembly.LoadFile(testCaseGroup.Key);

			foreach (var testCase in testCaseGroup)
			{
				var name = testCase.FullyQualifiedName.Split("::");

				// TODO: LOTS of assumptions here.
				// I'm assuming I'll only find one static method with the
				// given qualified name.
				var testMethod = (from type in assembly.GetTypes()
										where type.FullName == name[0]
										from method in type.GetMethods()
										where method.Name == name[1]
										select method).Single();

				testMethod.Invoke(null, null);
				frameworkHandle.RecordResult(new TestResult(testCase) { Outcome = TestOutcome.Passed });
			}
		}
	}

	public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
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
						method.Invoke(null, null);
						var fullyQualifiedName = $"{type.FullName}::{method.Name}";
						var testCase = new TestCase(fullyQualifiedName, new Uri(Shared.ExecutorUri), source);
						frameworkHandle.RecordResult(new TestResult(testCase) { Outcome = TestOutcome.Passed });
					}
				}
			}
		}
	}
}