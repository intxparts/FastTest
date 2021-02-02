using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
	public class SuccessException : Exception {}

	public static class Test
	{
		private static ConcurrentDictionary<string, Exception> _failedTestData = new ConcurrentDictionary<string, Exception>();
		public static ConcurrentDictionary<string, Exception> FailedTestData => _failedTestData;

		private static List<Task> _tasks = new List<Task>();
		public static List<Task> Tasks => _tasks;

		private static int _testCount = 0;
		public static int TestCount => _testCount;

		private static int _passedTests = 0;
		public static int PassedTests => _passedTests;

		public static void Run<T>(string name, Action<T> fn, T input)
		{
			Interlocked.Add(ref _testCount, 1);
			try
			{
				fn.Invoke(input);
				Interlocked.Add(ref _passedTests, 1);
			}
			catch (SuccessException)
			{
				Interlocked.Add(ref _passedTests, 1);
			}
			catch (Exception ex)
			{
				_failedTestData.TryAdd(name, ex);
			}
		}

		public static void Run(string name, Action fn)
		{
			Interlocked.Add(ref _testCount, 1);
			try
			{
				fn.Invoke();
				Interlocked.Add(ref _passedTests, 1);
			}
			catch (SuccessException) 
			{
				Interlocked.Add(ref _passedTests, 1);
			}
			catch (Exception ex)
			{
				_failedTestData.TryAdd(name, ex);
			}
		}

		public static long Time(Action fn)
		{
			Stopwatch s = new Stopwatch();
			s.Start();

			try
			{
				fn.Invoke();
			}
			catch {}

			s.Stop();
			return s.ElapsedMilliseconds;
		}
	}
}
