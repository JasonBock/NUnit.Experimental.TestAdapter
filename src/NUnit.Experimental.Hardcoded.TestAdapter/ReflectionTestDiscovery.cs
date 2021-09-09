//using Microsoft.VisualStudio.TestPlatform.ObjectModel;
//using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
//using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
//using NUnit.Framework;
//using System.Reflection;

//namespace NUnit.Experimental.Hardcoded.TestAdapter;

//[DefaultExecutorUri(Shared.ExecutorUri)]
//public sealed class ReflectionTestDiscovery
//	: ITestDiscoverer
//{
//	public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
//		IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
//	{
//		logger.SendMessage(TestMessageLevel.Informational, $"{nameof(ReflectionTestDiscovery)} - discovery started...");

//		foreach (var source in sources)
//		{
//			logger.SendMessage(TestMessageLevel.Informational, $"{nameof(source)} is {source}");

//			// TODO: This would need LOTS of exception handling to ensure this was
//			// a loadable assembly file.
//			var assembly = Assembly.LoadFile(source);

//			foreach(var type in assembly.GetTypes())
//			{
//				foreach(var method in type.GetMethods())
//				{
//					if(method.GetCustomAttribute<TestAttribute>() is not null)
//					{
//						var fullyQualifiedName = $"{type.FullName}::{method.Name}";
//						logger.SendMessage(TestMessageLevel.Informational, 
//							$"{nameof(ReflectionTestDiscovery)} - found {fullyQualifiedName}");
//						discoverySink.SendTestCase(new TestCase(fullyQualifiedName, new Uri(Shared.ExecutorUri), source));
//					}
//				}
//			}
//		}

//		logger.SendMessage(TestMessageLevel.Informational, $"{nameof(ReflectionTestDiscovery)} - discovery finished.");
//	}
//}
