using System;

namespace SimpleTest
{
	public static class Assert
	{
		public static void AreEqual<T>(T expectedValue, T actualValue) where T : struct
		{
			if (!expectedValue.Equals(actualValue))
				throw new Exception($"Expected value: '{expectedValue}' does not match the Actual value: '{actualValue}'");
		}

		public static void AreEqual(string expectedValue, string actualValue)
		{
			if (expectedValue != actualValue)
				throw new Exception($"Expected value: '{expectedValue}' does not match the Actual value: '{actualValue}'");
		}

		public static void AreNotEqual<T>(T expectedValue, T actualValue) where T : struct
		{
			if (expectedValue.Equals(actualValue))
				throw new Exception($"Expected value: '{expectedValue}' matches the Actual value: '{actualValue}'");
		}

		public static void AreNotEqual(string expectedValue, string actualValue)
		{
			if (expectedValue == actualValue)
				throw new Exception($"Expected value: '{expectedValue}' matches the Actual value: '{actualValue}'");
		}

		public static void AreSame<T>(T i1, T i2) where T : class
		{
			if (i1 != i2)
				throw new Exception($"Expected objects to be the same");
		}

		public static void AreNotSame<T>(T i1, T i2) where T : class
		{
			if (i1 == i2)
				throw new Exception($"Expected objects to be the same");
		}

		public static void IsTrue(bool condition)
		{
			if (!condition)
				throw new Exception($"Expected condition to be true but was false");
		}

		public static void IsFalse(bool condition)
		{
			if (condition)
				throw new Exception($"Expected condition to be false but was true");
		}

		public static void IsNull(object obj)
		{
			if (obj != null)
				throw new Exception($"Expected object to be null but was not null");
		}

		public static void IsNotNull(object obj)
		{
			if (obj == null)
				throw new Exception($"Expected object to be not null but was null");
		}

		public static void Throws<T>(Action fn) where T : Exception
		{
			try
			{
				fn.Invoke();
				throw new Exception($"Expected function to throw an exception of type: {typeof(T)}");
			}
			catch
			{
			}
		}

		public static void DoesNotThrow(Action fn)
		{
			try
			{
				fn.Invoke();
			}
			catch (Exception)
			{
				throw new Exception($"Expected function to not throw an exception");
			}
		}

		public static void Pass()
		{
			throw new SuccessException();
		}

		public static void Fail(string message)
		{
			throw new Exception(message);
		}

		#region Comparisons
		public static void IsGreaterThan(int left, int right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(int left, int right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(int left, int right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(int left, int right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}

		public static void IsGreaterThan(short left, short right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(short left, short right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(short left, short right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(short left, short right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}

		public static void IsGreaterThan(byte left, byte right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(byte left, byte right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(byte left, byte right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(byte left, byte right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}

		public static void IsGreaterThan(long left, long right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(long left, long right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(long left, long right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(long left, long right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}

		public static void IsGreaterThan(float left, float right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(float left, float right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(float left, float right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(float left, float right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}

		public static void IsGreaterThan(double left, double right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(double left, double right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(double left, double right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(double left, double right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}

		public static void IsGreaterThan(decimal left, decimal right)
		{
			if (left <= right)
				throw new Exception($"Expected {left} to be greater than {right}");
		}

		public static void IsGreaterThanOrEqual(decimal left, decimal right)
		{
			if (left < right)
				throw new Exception($"Expected {left} to be greater than or equal to {right}");
		}

		public static void IsLessThan(decimal left, decimal right)
		{
			if (left >= right)
				throw new Exception($"Expected {left} to be less than {right}");
		}

		public static void IsLessThanOrEqual(decimal left, decimal right)
		{
			if (left > right)
				throw new Exception($"Expected {left} to less than or equal to {right}");
		}
		#endregion
	}
}