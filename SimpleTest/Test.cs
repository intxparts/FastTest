using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTest
{
	public static class Test
	{
		private static ConcurrentDictionary<string, Exception> _failedTestData = new ConcurrentDictionary<string, Exception>();
		public static ConcurrentDictionary<string, Exception> FailedTestData => _failedTestData;

		private static int _testCount = 0;
		public static int TestCount => _testCount;

		private static int _passedTestCount = 0;
		public static int PassedTestCount => _passedTestCount;

		public static void Run<T>(string name, Action<T> fn, T input)
		{
			bool success = false;
			try
			{
				fn.Invoke(input);
				success = true;
			}
			catch (SuccessException)
			{
				success = true;
			}
			catch (Exception ex)
			{
				_failedTestData.TryAdd(name, ex);
			}
			finally
			{
				Interlocked.Add(ref _testCount, 1);
				if (success)
					Interlocked.Add(ref _passedTestCount, 1);
			}
		}

		public static void Run(string name, Action fn)
		{
			bool success = false;
			try
			{
				fn.Invoke();
				success = true;
			}
			catch (SuccessException)
			{
				success = true;
			}
			catch (Exception ex)
			{
				_failedTestData.TryAdd(name, ex);
			}
			finally
			{
				Interlocked.Add(ref _testCount, 1);
				if (success)
					Interlocked.Add(ref _passedTestCount, 1);
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
