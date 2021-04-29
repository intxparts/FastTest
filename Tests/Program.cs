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
				
				Test.Run("Test.Time(Action fn) estimates the elapsed amount of time (ms) to run fn.", () => {
					var timeMilliseconds = Test.Time(() => {
						Thread.Sleep(25);
					});

					Assert.IsGreaterThanOrEqual(timeMilliseconds, 25);
				});
				
				RunAssertTests();
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

		static void RunAssertTests()
		{
		
		#region Assert.DoesNotThrow
		
			Test.Run("Assert.DoesNotThrow(Action fn) raises an Exception when an Exception is thrown in fn", () => {
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
			
			Test.Run("Assert.DoesNotThrow(Action fn) does not throw an Exception when an Exception is not thrown in fn", () => {
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
			Test.Run("Assert.Throws<Exception>(Action fn) raises an Exception when Exception is not thrown in fn", () => {
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
			
			Test.Run("Assert.Throws<Exception>(Action fn) does not raise an Exception when fn throws an Exception", () => {
				Exception exception = null;
				try {
					exception = Assert.Throws<Exception>(() => { throw new FakeException(); });
				}
				catch
				{
				}
				Assert.AreEqual("FakeException", exception.Message);
			});
			
			Test.Run("Assert.Throws<FakeException>(Action fn) raises an Exception when a FakeException is not thrown in fn", () => {
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
			
			Test.Run("Assert.Throws<FakeException>(Action fn) raises an Exception when an Exception not derived from FakeException is thrown in fn", () => {
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
			
			Test.Run("Assert.Throws<Exception>(Action fn) does not raise an Exception when fn throws an Exception", () => {
				Exception exception = null;
				try {
					exception = Assert.Throws<Exception>(() => { throw new Exception("custom"); });
				}
				catch
				{
				}
				Assert.AreEqual("custom", exception.Message);
			});
			
			Test.Run("Assert.Throws<FakeException>(Action fn) does not raise an Exception when fn throws a FakeException", () => {
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
			
		#endregion

		#region Assert.Pass/Fail
			Test.Run("Assert.Pass(string message) raises a SuccessException with message", () => {
				var exception = Assert.Throws<SuccessException>(() => {
					Assert.Pass("Condition met.");
				});
				Assert.AreEqual("Condition met.", exception.Message);
			});
			
			Test.Run("Assert.Pass() raises a SuccessException", () => {
				var exception = Assert.Throws<SuccessException>(() => {
					Assert.Pass();
				});
				Assert.AreEqual("", exception.Message);
			});

			Test.Run("Assert.Fail(string message) raises an Exception with message", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.Fail("Fail now.");
				});
				Assert.AreEqual("Fail now.", exception.Message);
			});
			
			Test.Run("Assert.Fail() raises an Exception", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.Fail();
				});
				Assert.AreEqual("", exception.Message);
			});
		#endregion
		
		#region Assert.IsNull/NotNull
		
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
		#endregion

		#region AreEqual/NotEqual string
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

		#region AreEqual/NotEqual int (struct)
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

		#region Assert.AreSame
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
		#endregion
		
	#region Numerical Comparisons
		
		#region Assert.IsGreaterThan (int)
		
			Test.Run("Assert.IsGreaterThan(int left, int right) throws an Exception when right is greater than or left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsGreaterThan(2, 10);
				});
				Assert.AreEqual(exception.Message, "Expected 2 to be greater than 10");
			});
			
			Test.Run("Assert.IsGreaterThan(int left, int right) throws an Exception when right is equal to left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsGreaterThan(5, 5);
				});
				Assert.AreEqual(exception.Message, "Expected 5 to be greater than 5");
			});
			
			Test.Run("Assert.IsGreaterThan(int left, int right) does not raise an Exception when left is greater than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsGreaterThan(10, 3);
				});
			});
		
		#endregion
		
		#region Assert.IsGreaterThanOrEqual (int)
		
			Test.Run("Assert.IsGreaterThanOrEqual(int left, int right) raises an Exception when right is greater than left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsGreaterThanOrEqual(2, 5);
				});
				Assert.AreEqual(exception.Message, "Expected 2 to be greater than or equal to 5");
			});
		
			Test.Run("Assert.IsGreaterThanOrEqual(int left, int right) does not raise an Exception when left is greater than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsGreaterThanOrEqual(3, -1);
				});
			});
		
		
			Test.Run("Assert.IsGreaterThanOrEqual(int left, int right) does not raise an Exception when left is equal to the right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsGreaterThanOrEqual(-1, -1);
				});
			});
		
		#endregion
		
		#region Assert.IsLessThan (int)
			Test.Run("Assert.IsLessThan(int left, int right) raises an exception when right is less than left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsLessThan(5, 2);
				});
				Assert.AreEqual("Expected 5 to be less than 2", exception.Message);
			});
			
			Test.Run("Assert.IsLessThan(int left, int right) does not raise exception when left is less than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsLessThan(2, 4);
				});
			});
			
			Test.Run("Assert.IsLessThan(int left, int right) raises an exception when right is equal to left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsLessThan(5, 5);
				});
				Assert.AreEqual("Expected 5 to be less than 5", exception.Message);
			});
		
		#endregion
		
		#region Assert.IsLessThanOrEqual (int)
			Test.Run("Assert.IsLessThanOrEqual(int left, int right) raises an exception when right is less than left", () => {
				var exception = Assert.Throws<Exception>(() => {
					Assert.IsLessThanOrEqual(5, 2);
				});
				Assert.AreEqual("Expected 5 to less than or equal to 2", exception.Message);
			});
			
			Test.Run("Assert.IsLessThanOrEqual(int left, int right) does not raise exception when left is less than right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsLessThanOrEqual(2, 4);
				});
			});
			
			Test.Run("Assert.IsLessThanOrEqual(int left, int right) does not raise exception when left is equal to right", () => {
				Assert.DoesNotThrow(() => {
					Assert.IsLessThanOrEqual(3, 3);
				});
			});
		
		#endregion
	
	#endregion
		}
	}
}
