
using System;

namespace SimpleTest
{
	public static class TestConsole
	{
		public static void RunTests(Action fn)
		{
			var timeMilliseconds = Test.Time(fn);
			var failedTestData = Test.FailedTestData;
			foreach (var f in failedTestData)
			{
				Console.WriteLine($"Test '{f.Key}' failed: {f.Value.Message}\n{f.Value.StackTrace}");
			}

			Console.WriteLine($"Test Count: {Test.TestCount}");
			Console.WriteLine($"Tests Passed: {Test.PassedTests}");
			Console.WriteLine($"Tests Failed: {(Test.TestCount - Test.PassedTests)}");
			Console.WriteLine($"elapsed milliseconds: {timeMilliseconds}");
			Console.WriteLine($"elapsed seconds: {(timeMilliseconds / 1000.0)}");
		}
	}
}