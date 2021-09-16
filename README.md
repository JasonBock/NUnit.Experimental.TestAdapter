# NUnit.Experimental.TestAdapter

An experimental idea to use a [source generator](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) to handle test discover and execution in [NUnit](https://nunit.org/).

## Overview

If test adapters use Reflection to discover and execute tests...wouldn't source generators be a better thing? That's what this experiment is all about: generate the needed types to find and run NUnit tests.

My first goal was simple: just find static methods that are attributed with `[Test]`, and run them. I'm not trying to support **all** the features NUnit provides, like [parameterization](https://docs.nunit.org/articles/nunit/technical-notes/usage/Parameterized-Tests.html), [assertions](https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertions.html), asynchronous execution, etc. Rather, the focus is on the source generation part. If that works, then I may look into supporting more scenarios. To be clear, though, my intent is **not** to replace the [current test adapter for NUnit](https://github.com/nunit/nunit3-vs-adapter).

If you're curious about what it takes to write a test adapter, [this link](https://github.com/microsoft/vstest) has all the details. It may seem complicated at first, but what I've found is that a test adapter needs two things:

* A **discoverer** that informs the testing infrastructure on the tests it found. This type implements `ITestDiscoverer` and is attributed with `[DefaultExecutorUri]`.
* An **executor** that runs the tests. This type implements `ITestExecutor` and is attributed with `[ExtensionUri]`.

These types can be in a separate assembly, or they can exist within the assembly that contains the tests. It appears that in either case, the assembly that contains the adapter types must end with the name `.TestAdapter`. I'm not sure if the dot is necessary, but from what I've found, the naming convention is required; otherwise, the testing infrastructure won't find the adapter types.

So, what I did was create two simple test adapter projects: one that uses Reflection to find and execute tests, and one that generates the types via a source generator into the assembly that contains the tests. Long story short, I got this to work, and in the next section, I'll describe the project layout so you can find where everything is.

## Projects

There are a number of projects within this solution - here's a description of them and what they contain:

* `NUnit.Experimental.Hardcoded.TestAdapter` - This was the first experiment, where there are two discoveres and executors. One uses Reflection, and one that has everything hardcoded.
* `NUnit.Experimental.Hardcoded.TestAdapter.Performance` - This runs [Benchmark.NET](https://benchmarkdotnet.org/) tests against these two approaches. The results (stored in `Results.md`) were promising, but note that this doesn't include any effort to create the hardcoded versions that a source generator would need to do.
* `NUnit.Experimental.Reflection.TestAdapter` - This is a test adapter project that uses the Reflection approach to discover and execute tests.
* `NUnit.Experimental.SourceGenerator.TestAdapter` - This is a test adapter project that uses a source generator to create the discoverer and executor types.
* `NUnit.Experimental.SourceGenerator.TestAdapter.Performance` - This runs Benchmark.NET tests to see how much time and memory a source generator approach would take. The results seem somewhat mixed, in that the total cost of time would be slightly higher, but the memory usage would be less.
* `NUnit.Experimental.SourceGenerator.TestAdapter.Tests` - This contains tests to confirm that the source generator is working as expected.
* `NUnit.Experimental.Tests` - This is a shared project that contains 5 simple tests.
* `NUnit.Experimental.Tests.Reflection` - This is a project that import `NUnit.Experimental.Tests` and `NUnit.Experimental.Reflection.TestAdapter` to run these tests.
* `NUnit.Experimental.Tests.SourceGenerator` - This is a project that import `NUnit.Experimental.Tests` and `NUnit.Experimental.SourceGenerator.TestAdapter` to run these tests. Note that this project has the `AssemblyName` property set to `NUnit.Experimental.Tests.SourceGenerator.TestAdapter` - this follows the naming convention mentioned before.

## Execution

You can execute the tests using either Visual Studio Test Explorer (VSTE) or `dotnet test`. Either approach should work. Here's what I see when I use VSTE:

![image](https://user-images.githubusercontent.com/904213/133625505-93a59484-fca1-4353-85ef-c7a8b3436be3.png)

Note that the test list seems to be duplicated. I'm not sure why this is happening, but I only get 5 tests running which is what I expect.

Here's what I get when I run `dotnet test`:

![image](https://user-images.githubusercontent.com/904213/133625647-650c6f8d-fcbf-481c-aa2b-18abd7f58a16.png)

In this case, the test list isn't duplicated. I should note that when you use `dotnet test`, it appears that the discoverer is never run. Also, when you use `dotnet test`, the `RunTests()` overload that takes a `IEnumerable<string>` parameter is called. In the case of VSTE, `RunTests()` that takes a `IEnumerable<TestCase>` parameter is called. This is why I generate a private `RunTests()` method that is called from both public `RunTests()` overloads.

## Future Work

For this MVP/proof-of-concept, I'm considering this "complete". To make this a full-fidelity test adapter for NUnit, a significant amount of effort would be needed to reach that level. I may see what it takes to support things like `[TestCase]` and other features, but that's not the primary goal. I'm hoping that the NUnit team considers this approach and, if they deem it worthwhile, to change their official test adapter to use a source generator.

## Conclusion

This has been a fun ride to use source generators in another area of software development: finding and running tests in .NET. If you have any questions or comments, please file an issue, though, again, keep in mind that the intent is **not** to create another test adapter for NUnit. I really don't have the time or the desire to do that :).
