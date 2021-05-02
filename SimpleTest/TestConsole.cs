using System;

namespace SimpleTest
{
	public enum ExitCode : int 
	{
		Success = 0,
		FailedTest = 1,
	}

	public static class TestConsole
	{
		public static ExitCode RunTests(Action fn)
		{
			var timeMilliseconds = Test.Time(fn);
			var failedTestData = Test.FailedTestData;
			foreach (var f in failedTestData)
			{
				Console.WriteLine($"Test '{f.Key}' failed: {f.Value.Message}\n{f.Value.StackTrace}");
			}

			Console.WriteLine($"Test Count: {Test.TestCount}");
			Console.WriteLine($"Tests Passed: {Test.PassedTestCount}");
			Console.WriteLine($"Tests Failed: {(Test.TestCount - Test.PassedTestCount)}");
			Console.WriteLine($"elapsed milliseconds: {timeMilliseconds}");
			Console.WriteLine($"elapsed seconds: {(timeMilliseconds / 1000.0)}");

			return failedTestData.Count > 0 ? ExitCode.FailedTest : ExitCode.Success;
		}
	}
}