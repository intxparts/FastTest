using System;
using System.Threading;
using SimpleTest;

namespace Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			var exitCode = TestConsole.RunTests(() => {
				RunAssertTests();

				Test.Run("Test failure", () => {
					Assert.Fail("Testing failure with github action integration");
				});
			});

			Environment.Exit((int) exitCode);
		}

		public class Vector
		{
			public double X;
			public double Y;
			public double Z;
		}

		static void RunAssertTests()
		{
			Test.Run("Test.Time(Action fn) estimates the elapsed amount of time (ms) to run fn.", () => {
				var timeMilliseconds = Test.Time(() => {
					Thread.Sleep(25);
				});

				Assert.IsGreaterThanOrEqual(timeMilliseconds, 25);
			});

			Test.Run("Assert.IsTrue(bool cond) raises an exception when cond is false", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsTrue(false);
				});

				Assert.AreEqual(exception.Message, "Expected condition to be true but was false");
			});

			Test.Run("Assert.IsTrue(bool cond) does not raise an exception when cond is true", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsTrue(true);
				});
			});

			Test.Run("Assert.IsFalse(bool cond) raises an exception when cond is true", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsFalse(true);
				});
				Assert.AreEqual(exception.Message, "Expected condition to be false but was true");
			});

			Test.Run("Assert.IsFalse(bool cond) does not raise an exception when cond is false", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsFalse(false);
				});
			});

			Test.Run("Assert.Pass(string message) raises a SuccessException with message", () => {
				var exception = Assert.Throws<SuccessException>(() => {
					Assert.Pass("Condition met.");
				});
				Assert.AreEqual(exception.Message, "Condition met.");
			});

			Test.Run("Assert.Fail(string message) raises an Exception with message", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.Fail("Fail now.");
				});
				Assert.AreEqual(exception.Message, "Fail now.");
			});
			
			Test.Run("Assert.IsNull(object obj) raises an Exception when obj is not null", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsNull("");
				});
				Assert.AreEqual(exception.Message, "Expected object to be null but was not null");
			});

			Test.Run("Assert.IsNull(object obj) does not raise an Exception when obj is null", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsNull(null);
				});
			});

			Test.Run("Assert.IsNotNull(object obj) raises an Exception when obj is null", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsNotNull(null);
				});
				Assert.AreEqual(exception.Message, "Expected object to be not null but was null");
			});

			Test.Run("Assert.IsNotNull(object obj) does not raise an Exception when obj is not null", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsNotNull("");
				});
			});

		#region AreEqual string
			Test.Run("Assert.AreEqual(string expected, string actual) raises an Exception when actual is not equal to expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreEqual("expected", "actual");
				});
				Assert.IsTrue(exception.Message == "Expected value: 'expected' does not match the Actual value: 'actual'");
			});

			Test.Run("Assert.AreEqual(string expected, string actual) does not raise an exception when expected equals actual", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreEqual("same", "same");
				});
			});

			Test.Run("Assert.AreNotEqual(string expected, string actual) raises an Exception when actual equals expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreNotEqual("same", "same");
				});
				Assert.IsTrue(exception.Message == "Expected value: 'same' matches the Actual value: 'same'");
			});

			Test.Run("Assert.AreNotEqual(string expected, string actual) does not raise an Exception when actual does not equal expected", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreNotEqual("expected", "actual");
				});
			});
		#endregion

		#region AreEqual int
			Test.Run("Assert.AreEqual(int expected, int actual) raises an Exception when actual is not equal to expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreEqual(10, 5);
				});
				Assert.IsTrue(exception.Message == "Expected value: '10' does not match the Actual value: '5'");
			});

			Test.Run("Assert.AreEqual(int expected, int actual) does not raise an exception when expected equals actual", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreEqual(2, 2);
				});
			});

			Test.Run("Assert.AreNotEqual(int expected, int actual) raises an Exception when actual equals expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.AreNotEqual(3, 3);
				});
				Assert.IsTrue(exception.Message == "Expected value: '3' matches the Actual value: '3'");
			});

			Test.Run("Assert.AreNotEqual(int expected, int actual) does not raise an Exception when actual does not equal expected", () => {
				Assert.DoesNotThrow(() => {
					Assert.AreNotEqual(7, 11);
				});
			});
		#endregion

			Test.Run("Assert.AreSame(class expected, class actual) raises an Exception when actual is not the same object as expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = new Vector() { X = 0, Y = 1, Z = 0 };
					Assert.AreSame(v1, v2);
				});
				Assert.AreEqual(exception.Message, "Expected objects to be the same");
			});

			Test.Run("Assert.AreSame(class expected, class actual) does not raise an Exception when actual is the same object as expected", () => {
				Assert.DoesNotThrow(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = v1;
					Assert.AreSame(v1, v2);
				});
			});
		
			Test.Run("Assert.AreNotSame(class expected, class actual) raises an Exception when actual is the same object as expected", () => {
				var exception = Assert.Throws<Exception>(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = v1;
					Assert.AreNotSame(v1, v2);
				});
				Assert.AreEqual(exception.Message, "Expected objects to be the same");
			});

			Test.Run("Assert.AreNotSame(class expected, class actual) does not raise an Exception when actual is not the same object as expected", () => {
				Assert.DoesNotThrow(() => {
					var v1 = new Vector() { X = 0, Y = 1, Z = 0 };
					var v2 = new Vector() { X = 0, Y = 1, Z = 0 };
					Assert.AreNotSame(v1, v2);
				});
			});

		}
	}
}
