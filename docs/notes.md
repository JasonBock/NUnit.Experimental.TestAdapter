# Notes

Random notes on getting this to work (I hope).

## Plan

* Step 1 - Run `dotnet test` against a project with tests. Shouldn't do anything.
* Step 2 - See if the tests can be discovered, but not run. Hardcoded is fine.
* Then see if the tests can be discovered and executed. Hardcoded is fine.
* Then create two different test adapters. One that uses Reflection and one that is a source generator.

### Step 1

I created a project that references NUnit but it doesn't reference the adapter. I added 5 tests, and I told VS Test Explorer (or VSTE) to "run tests". Here's the output:
```
Building Test Projects
Starting test discovery for requested test run
Test project NUnit.Experimental.TestAdapter.Tests does not reference any .NET NuGet adapter. Test discovery or execution might not work for this project.
It's recommended to reference NuGet test adapters in each test project in the solution.
========== Starting test discovery ==========
Microsoft.VisualStudio.TestPlatform.ObjectModel.TestPlatformException: Unable to find C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.TestAdapter.Tests\bin\Debug\net6.0\testhost.dll. Please publish your test project and retry.
   at Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting.DotnetTestHostManager.GetTestHostPath(String runtimeConfigDevPath, String depsFilePath, String sourceDirectory)
   at Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting.DotnetTestHostManager.GetTestHostProcessStartInfo(IEnumerable`1 sources, IDictionary`2 environmentVariables, TestRunnerConnectionInfo connectionInfo)
   at Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.ProxyOperationManager.SetupChannel(IEnumerable`1 sources, String runSettings)
   at Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.ProxyDiscoveryManager.DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler)
========== Test discovery aborted: 0 Tests found in 3.5 ms ==========
========== Starting test run ==========
Unable to find C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.TestAdapter.Tests\bin\Debug\net6.0\testhost.dll. Please publish your test project and retry.
Unable to find C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.TestAdapter.Tests\bin\Debug\net6.0\testhost.dll. Please publish your test project and retry.
========== Test run aborted: 0 Tests (0 Passed, 0 Failed, 0 Skipped) run in < 1 ms ==========
```
As expected, there's errors related to not finding a test adapter so discovery and execution "might not work", and by that it means it won't.

Running `dotnet test` gives this:
```
Determining projects to restore...
All projects are up-to-date for restore.
```
It doesn't give any error messages, it just doesn't do anything.

## Step 2

OK, how do I get tests to be discovered? Let's start by look at [NUnit's adapter](https://github.com/nunit/nunit3-vs-adapter). It looks like we want to reference `Microsoft.TestPlatform.ObjectModel`. OK, referencing it.

Next, there's a type called [NUnit3TestDiscoverer](https://github.com/nunit/nunit3-vs-adapter/blob/master/src/NUnitTestAdapter/NUnit3TestDiscoverer.cs) to look at. It implements `ITestDiscoverer` ... I'm not sure why it derives from `NUnitTestAdapter`, I'll keep this in mind.

There's only one member to implement: `DiscoverTests`. I tried a simple iterator over `sources`, but...that didn't do anything.

Hold up. I need to also reference the `Microsoft.NET.Test.Sdk` NuGet package. `dotnet test` gives an error message about "compatiable framework versions" but VSTE logged this:
```
An exception occurred while test discoverer 'HardCodedTestDiscovery' was loading tests. Exception: DefaultExecutorUri is null, did you decorate the discoverer class with [DefaultExecutorUri()] attribute? For example [DefaultExecutorUri("executor://example.testadapter")].
```
I see that attribute on NUnit's discoverer, so I'll add that too.

Now VSTE says this:
```
========== Starting test discovery ==========
...
HardCodedTestDiscovery - discovery started...
source is C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.Hardcoded.TestAdapter\bin\Debug\net6.0\NUnit.Experimental.Hardcoded.TestAdapter.dll
========== Starting test run ==========
Could not find test executor with URI 'executor://nunitexperimentaltestadapter/'.  Make sure that the test executor is installed and supports .net runtime version  .
```
So the discoverer **is** firing. That's progress. The executor needs that URI to "match" to something I guess. I'll get there eventually.

I tried sending a hard-coded discovered test to the sink:
```
discoverySink.SendTestCase(new TestCase("TimedTests.TestOf50ms", new Uri(Shared.ExecutorUri), @"C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.Hardcoded.TestAdapter\bin\Debug\net6.0\NUnit.Experimental.Hardcoded.TestAdapter.dll"));
```
This had more promise:
```
HardCodedTestDiscovery - discovery started...
========== Test discovery finished: 1 Tests found in 1.1 sec ==========
========== Starting test run ==========
Could not find test executor with URI 'executor://nunitexperimentaltestadapter/'.  Make sure that the test executor is installed and supports .net runtime version  .
No test matches the given testcase filter `FullyQualifiedName=TimedTests.TestOf50ms|FullyQualifiedName=NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf1000ms|FullyQualifiedName=NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf100ms|FullyQualifiedName=NUnit.Experimenta...` in C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.Hardcoded.TestAdapter\bin\Debug\net6.0\NUnit.Experimental.Hardcoded.TestAdapter.dll
========== Test run finished: 0 Tests (0 Passed, 0 Failed, 0 Skipped) run in 7 ms ==========
```
It now thinks I "found" one test, though it can't execute it. So it seems like what I need to do is load all the sources, probably with `Assembly.LoadFile()`, and then find all the classes with methods marked with `[Test]`. This is basically changing the "hard-coded" nature of this experiment, so I'm going to change the discoverer (and subsequent executor) to `ReflectionTestDiscovery` (and probably `ReflectionTestExecutor`).

With `Assembly.LoadFile()` in place, we now get this:
```
ReflectionTestDiscovery - discovery started...
source is C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.Hardcoded.TestAdapter\bin\Debug\net6.0\NUnit.Experimental.Hardcoded.TestAdapter.dll
ReflectionTestDiscovery - found NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf50ms
ReflectionTestDiscovery - found NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf100ms
ReflectionTestDiscovery - found NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf200ms
ReflectionTestDiscovery - found NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf500ms
ReflectionTestDiscovery - found NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests.TestOf1000ms
ReflectionTestDiscovery - discovery finished.
========== Test discovery finished: 5 Tests found in 466.3 ms ==========
```
That's progress.

One "catch" I just realized is that the source is a full file path where the assembly eventually lands. If I source generate this, how would I know what the assembly file name is **and** where it's going to be located after it's built? This may be problematic. I'm not sure if/how the `source` in `TestCase` is used in the executor, and if things are hard-coded anyway, maybe it's not necessary. I'll see.

I think it's time to start working on the executor and see how that works. Seems like it's a class that inherits from `ITestExecutor`. Maybe `IExecutionContext` as well? This has three methods: `Cancel()` and two overloads of `RunTests()`. For now, I'll ignore `Cancel()` and assume I never cancel anything. I put a log method from `frameworkHandle` into each `RunTests()` and I ran from both VSTE and `dotnet test`.

In VSTE, I see:
```
========== Starting test run ==========
RunTests from tests called.
```

`dotnet test` is now complaining all the time about:
```
It was not possible to find any compatible framework version
The framework 'Microsoft.NETCore.App', version '6.0.0-preview.7.21377.19' was not found.
```
Which really sucks, because at some point I'm going to need to do performance testing comparing the two approaches, and if I can't launch `dotnet test`, that's a problem.

```
Testhost process exited with error: It was not possible to find any compatible framework version
The framework 'Microsoft.NETCore.App', version '5.0.0' was not found.
  - The following frameworks were found:
      6.0.0-preview.6.21352.12 at [C:\Program Files\Microsoft Visual Studio\2022\Preview\dotnet\runtime\shared\Microsoft.NETCore.App]
You can resolve the problem by installing the specified framework and/or SDK.
The specified framework can be found at:
  - https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=5.0.0&arch=x64&rid=win10-x64
. Please check the diagnostic logs for more information.

Test Run Aborted.
```

I'm going to visit some Discord channels and see if there's a "simple" workaround other than installing a previous .NET SDK version.

Well, on a whim, I used Terminal, and that has more success. Running `dotnet test ` gives this:
```
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
C:\Users\jason\source\repos\NUnit.Experimental.TestAdapter\src\NUnit.Experimental.Hardcoded.TestAdapter\bin\Debug\net6.0\NUnit.Experimental.Hardcoded.TestAdapter.dll
RunTests from sources called.
```
It's odd that the logs from the discoverer side don't show up, but it shows which test method is being run, and it's from `sources`. Which is different that VS. Of **course** they're different :|.

However, in either case, if I know the tests to run, I can create a generic `RunTests` that can be called from either method (probably).

Side note: I think I just realized that VSTE will call the discoverer to get that list, and then pass that to the executor. Whereas `dotnet test` doesn't do a "discovery" phase, it just does execution.