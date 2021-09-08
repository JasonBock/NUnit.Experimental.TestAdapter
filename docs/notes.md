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