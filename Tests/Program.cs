using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SimpleTest;

namespace Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			var exitCode = TestConsole.RunTests(() => {
								
				RunTestTests();
				
				RunAssertTests();
				
				RunThreadedTests();
				
				if (args.Any(a => a == "--includeFailTests"))
					RunFailTests();
			});

			Environment.Exit((int) exitCode);
		}

		public class Vector
		{
			public double X;
			public double Y;
			public double Z;
		}
		
		public class FakeException : Exception 
		{
			public FakeException() : base("FakeException"){}
		}

		static void Test_Run<T>(string name, Action<T> fn, T input)
		{
			int prevTestCount = Test.TestCount;
			int prevPassedTestCount = Test.PassedTestCount;
			
			Test.Run<T>(name, fn, input);
			Test.Run($"++ Counting test ++ {name}", () => {
				Assert.AreEqual(prevTestCount + 1, Test.TestCount);
				Assert.AreEqual(prevPassedTestCount + 1, Test.PassedTestCount);
			});
		}

		static void Test_Run(string name, Action fn)
		{
			int prevTestCount = Test.TestCount;
			int prevPassedTestCount = Test.PassedTestCount;
			
			Test.Run(name, fn);
			Test.Run($"++ Counting test ++ {name}", () => {
				Assert.AreEqual(prevTestCount + 1, Test.TestCount);
				Assert.AreEqual(prevPassedTestCount + 1, Test.PassedTestCount);
			});
		}

		static void RunTestTests()
		{
			Test_Run("Test.Time(Action fn) estimates the elapsed amount of time (ms) to run fn.", () => {
				var timeMilliseconds = Test.Time(() => {
					Thread.Sleep(25);
				});

				Assert.IsGreaterThanOrEqual(timeMilliseconds, 25);
			});
			
			// parameterized tests
			{
				string [] animals = { "pig", "cow", "dog" };
				foreach (string s in animals)
				{
					Test_Run($"Test.Run<string>(string name, Action<string> fn, string input): input = {s}", (string animal) => {
						Assert.AreEqual(s, animal);
					}, s);
				}
			}
		}

		static void RunAssertTests()
		{
		
		#region Assert.DoesNotThrow
		
			Test_Run("Assert.DoesNotThrow(Action fn) raises an Exception when an Exception is thrown in fn", () => {
				string message = "";
				
				try {
					Assert.DoesNotThrow(() => { throw new Exception(); });
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
				Assert.AreEqual("Expected function to not throw an Exception", message);
			});
			
			Test_Run("Assert.DoesNotThrow(Action fn) does not throw an Exception when an Exception is not thrown in fn", () => {
				bool success = true;
				try {
					Assert.DoesNotThrow(() => {});
				}
				catch
				{
					success = false;
				}
				Assert.IsTrue(success);
			});
		
		#endregion
		
		#region Assert.Throws<T>
			Test_Run("Assert.Throws<Exception>(Action fn) raises an Exception when Exception is not thrown in fn", () => {
				string message = "";
				try {
					Assert.Throws<Exception>(() => {});
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
				Assert.AreEqual("Expected fn to throw an exception of type: System.Exception", message);
			});
			
			Test_Run("Assert.Throws<Exception>(Action fn) does not raise an Exception when fn throws an Exception", () => {
				Exception exception = null;
				try {
					exception = Assert.Throws<Exception>(() => { throw new FakeException(); });
				}
				catch
				{
				}
				Assert.AreEqual("FakeException", exception.Message);
			});
			
			Test_Run("Assert.Throws<FakeException>(Action fn) raises an Exception when a FakeException is not thrown in fn", () => {
				string message = "";
				try {
					Assert.Throws<FakeException>(() => {});
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
				Assert.AreEqual("Expected fn to throw an exception of type: Tests.Program+FakeException", message);
			});
			
			Test_Run("Assert.Throws<FakeException>(Action fn) raises an Exception when an Exception not derived from FakeException is thrown in fn", () => {
				string message = "";
				try {
					Assert.Throws<FakeException>(() => { throw new Exception("wrong exception to throw"); });
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
				Assert.AreEqual("Expected fn to throw an exception of type: Tests.Program+FakeException", message);
			});
			
			Test_Run("Assert.Throws<Exception>(Action fn) does not raise an Exception when fn throws an Exception", () => {
				Exception exception = null;
				try {
					exception = Assert.Throws<Exception>(() => { throw new Exception("custom"); });
				}
				catch
				{
				}
				Assert.AreEqual("custom", exception.Message);
			});
			
			Test_Run("Assert.Throws<FakeException>(Action fn) does not raise an Exception when fn throws a FakeException", () => {
				Exception exception = null;
				try {
					exception = Assert.Throws<FakeException>(() => { throw new FakeException(); });
				}
				catch
				{
				}
				Assert.AreEqual("FakeException", exception.Message);
			});
		#endregion

		#region Assert.IsTrue/False

			Test_Run("Assert.IsTrue(bool cond) raises an exception when cond is false", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsTrue(false);
				});

				Assert.AreEqual(exception.Message, "Expected condition to be true but was false");
			});

			Test_Run("Assert.IsTrue(bool cond) does not raise an exception when cond is true", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsTrue(true);
				});
			});

			Test_Run("Assert.IsFalse(bool cond) raises an exception when cond is true", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsFalse(true);
				});
				Assert.AreEqual(exception.Message, "Expected condition to be false but was true");
			});

			Test_Run("Assert.IsFalse(bool cond) does not raise an exception when cond is false", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsFalse(false);
				});
			});
			
		#endregion

		#region Assert.Pass/Fail
			Test_Run("Assert.Pass(string message) raises a SuccessException with message", () => {
				var exception = Assert.Throws<SuccessException>(() => {
					Assert.Pass("Condition met.");
				});
				Assert.AreEqual("Condition met.", exception.Message);
			});
			
			Test_Run("Assert.Pass() raises a SuccessException", () => {
				var exception = Assert.Throws<SuccessException>(() => {
					Assert.Pass();
				});
				Assert.AreEqual("", exception.Message);
			});

			Test_Run("Assert.Fail(string message) raises an Exception with message", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.Fail("Fail now.");
				});
				Assert.AreEqual("Fail now.", exception.Message);
			});
			
			Test_Run("Assert.Fail() raises an Exception", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.Fail();
				});
				Assert.AreEqual("", exception.Message);
			});
		#endregion
		
		#region Assert.IsNull/NotNull
		
			Test_Run("Assert.IsNull(object obj) raises an Exception when obj is not null", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsNull("");
				});
				Assert.AreEqual(exception.Message, "Expected object to be null but was not null");
			});

			Test_Run("Assert.IsNull(object obj) does not raise an Exception when obj is null", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsNull(null);
				});
			});

			Test_Run("Assert.IsNotNull(object obj) raises an Exception when obj is null", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsNotNull(null);
				});
				Assert.AreEqual(exception.Message, "Expected object to be not null but was null");
			});

			Test_Run("Assert.IsNotNull(object obj) does not raise an Exception when obj is not null", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsNotNull("");
				});
			});
		#endregion

		#region AreEqual/NotEqual string
			Test_Run("Assert.AreEqual(string expected, string actual) raises an Exception when actual is not equal to expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreEqual("expected", "actual");
				});
				Assert.IsTrue(exception.Message == "Expected value: 'expected' does not match the Actual value: 'actual'");
			});

			Test_Run("Assert.AreEqual(string expected, string actual) does not raise an exception when expected equals actual", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreEqual("same", "same");
				});
			});

			Test_Run("Assert.AreNotEqual(string expected, string actual) raises an Exception when actual equals expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreNotEqual("same", "same");
				});
				Assert.IsTrue(exception.Message == "Expected value: 'same' matches the Actual value: 'same'");
			});

			Test_Run("Assert.AreNotEqual(string expected, string actual) does not raise an Exception when actual does not equal expected", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreNotEqual("expected", "actual");
				});
			});
		#endregion

		#region AreEqual/NotEqual int (struct)
			Test_Run("Assert.AreEqual(int expected, int actual) raises an Exception when actual is not equal to expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreEqual(10, 5);
				});
				Assert.IsTrue(exception.Message == "Expected value: '10' does not match the Actual value: '5'");
			});

			Test_Run("Assert.AreEqual(int expected, int actual) does not raise an exception when expected equals actual", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreEqual(2, 2);
				});
			});

			Test_Run("Assert.AreNotEqual(int expected, int actual) raises an Exception when actual equals expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreNotEqual(3, 3);
				});
				Assert.IsTrue(exception.Message == "Expected value: '3' matches the Actual value: '3'");
			});

			Test_Run("Assert.AreNotEqual(int expected, int actual) does not raise an Exception when actual does not equal expected", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreNotEqual(7, 11);
				});
			});
		#endregion

		#region Assert.AreSame
			Test_Run("Assert.AreSame(class expected, class actual) raises an Exception when actual is not the same object as expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = new Vector() { X = 0, Y = 1, Z = 0 };
					Assert.AreSame(v1, v2);
				});
				Assert.AreEqual(exception.Message, "Expected objects to be the same");
			});

			Test_Run("Assert.AreSame(class expected, class actual) does not raise an Exception when actual is the same object as expected", () => {
				Assert.DoesNotThrow(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = v1;
					Assert.AreSame(v1, v2);
				});
			});

			Test_Run("Assert.AreNotSame(class expected, class actual) raises an Exception when actual is the same object as expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = v1;
					Assert.AreNotSame(v1, v2);
				});
				Assert.AreEqual(exception.Message, "Expected objects to be the same");
			});

			Test_Run("Assert.AreNotSame(class expected, class actual) does not raise an Exception when actual is not the same object as expected", () => {
				Assert.DoesNotThrow(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = new Vector() { X = 0, Y = 1, Z = 0 };
					Assert.AreNotSame(v1, v2);
				});
			});
		#endregion
		
	#region Numerical Comparisons
		
		#region Assert.IsGreaterThan (int)
		
			Test_Run("Assert.IsGreaterThan(int left, int right) throws an Exception when right is greater than or left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsGreaterThan(2, 10);
				});
				Assert.AreEqual(exception.Message, "Expected 2 to be greater than 10");
			});
			
			Test_Run("Assert.IsGreaterThan(int left, int right) throws an Exception when right is equal to left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsGreaterThan(5, 5);
				});
				Assert.AreEqual(exception.Message, "Expected 5 to be greater than 5");
			});
			
			Test_Run("Assert.IsGreaterThan(int left, int right) does not raise an Exception when left is greater than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsGreaterThan(10, 3);
				});
			});
		
		#endregion
		
		#region Assert.IsGreaterThanOrEqual (int)
		
			Test_Run("Assert.IsGreaterThanOrEqual(int left, int right) raises an Exception when right is greater than left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsGreaterThanOrEqual(2, 5);
				});
				Assert.AreEqual(exception.Message, "Expected 2 to be greater than or equal to 5");
			});
		
			Test_Run("Assert.IsGreaterThanOrEqual(int left, int right) does not raise an Exception when left is greater than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsGreaterThanOrEqual(3, -1);
				});
			});
		
		
			Test_Run("Assert.IsGreaterThanOrEqual(int left, int right) does not raise an Exception when left is equal to the right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsGreaterThanOrEqual(-1, -1);
				});
			});
		
		#endregion
		
		#region Assert.IsLessThan (int)
			Test_Run("Assert.IsLessThan(int left, int right) raises an exception when right is less than left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsLessThan(5, 2);
				});
				Assert.AreEqual("Expected 5 to be less than 2", exception.Message);
			});
			
			Test_Run("Assert.IsLessThan(int left, int right) does not raise exception when left is less than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsLessThan(2, 4);
				});
			});
			
			Test_Run("Assert.IsLessThan(int left, int right) raises an exception when right is equal to left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsLessThan(5, 5);
				});
				Assert.AreEqual("Expected 5 to be less than 5", exception.Message);
			});
		
		#endregion
		
		#region Assert.IsLessThanOrEqual (int)
			Test_Run("Assert.IsLessThanOrEqual(int left, int right) raises an exception when right is less than left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsLessThanOrEqual(5, 2);
				});
				Assert.AreEqual("Expected 5 to less than or equal to 2", exception.Message);
			});
			
			Test_Run("Assert.IsLessThanOrEqual(int left, int right) does not raise exception when left is less than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsLessThanOrEqual(2, 4);
				});
			});
			
			Test_Run("Assert.IsLessThanOrEqual(int left, int right) does not raise exception when left is equal to right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsLessThanOrEqual(3, 3);
				});
			});
		
		#endregion
	
	#endregion
		}
		
		static void RunFailTests()
		{
			string failTest = "Fail Test";
			Test.Run(failTest, () => {
				Assert.Fail("The fail message");
			});
			Test.Run("Exception information on FailedTests are stored in Test.FailedTestData by the testName", () => {
				Assert.AreEqual("The fail message", Test.FailedTestData[failTest].Message);
			});
		}

		static void RunThreadedTests()
		{
			var prevTestCount = Test.TestCount;
			var prevPassedTestCount = Test.PassedTestCount;
			
			// each group represents a group of tests that can be parallelized
			Action group4 = () => { Thread.Sleep(50); };
			Action group3 = () => { Thread.Sleep(25); };
			Action group2 = () => { Thread.Sleep(10); };
			Action group1 = () => { Thread.Sleep(5); };
			
			var parallelizedTimeMilliseconds = Test.Time(() => {
				var taskPool = new List<Task>();
				taskPool.Add(Task.Run(() => {
					Test.Run("|| fourth group", group4);
				}));

				taskPool.Add(Task.Run(() => {
					Test.Run("|| third group", group3);
				}));

				taskPool.Add(Task.Run(() => {
					Test.Run("|| first group", group1);
				}));
				
				taskPool.Add(Task.Run(() => {
					Test.Run("|| second group", group2);
				}));
				
				Task.WaitAll(taskPool.ToArray());
				
				Test.Run("Test uses atomics on its test counters so users can parallelize groups of tests", () => {
					Assert.AreEqual(prevTestCount + 4, Test.TestCount);
					Assert.AreEqual(prevPassedTestCount + 4, Test.PassedTestCount);
				});
			});

			var timeMilliseconds = Test.Time(() => {
				Test.Run("fourth group", group4);
				Test.Run("third group", group3);
				Test.Run("first group", group1);
				Test.Run("second group", group2);
			});

			// Console.WriteLine($"|| {parallelizedTimeMilliseconds} vs {timeMilliseconds}");

			Test.Run("Test.Run() can be parallelized: parallelized group is much faster", () => {
				Assert.IsGreaterThan(timeMilliseconds, parallelizedTimeMilliseconds);
			});
		}
	}
}
