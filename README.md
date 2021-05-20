# SimpleTest

- Motivation: There is a pattern amongst .Net unit testing frameworks to utilize Reflection, which is very slow. The idea is to see if we can make something that is faster, simpler and just as powerful for most cases.

The reflection being discussed here is not that of Mocking libraries, but with regards to the attribute driven test frameworks. All of the most popular .Net unit testing libraries use this pattern: NUnit, XUnit, MSTest.

```c#

[TestFixture]
public class TestService
{
	[OneTimeSetup]
	public void Setup()
	{
		// ...
	}	

	[Test]
	public void Test1()
	{
		// ...
	}
}
```

The primary advantage to building unit tests with such libraries is that a single command line application can be built to run the unit tests by targeting any test dlls. This standardizes test output across unit test projects and allows you to invoke an abritrary number of projects to run tests. However, this is extremely slow because when loading a dll, the console application has to load the unit test project dll and search based on the appropriate attributes to setup the necessary test environment to run each test.

```
[> runner.exe testproj1.dll
[>		100 successful
[>		10 failed
[>		110 tests run
```

The idea of this SimpleTest project is to see how much performance we sacrifice for convenience. Unit tests in a large project can quickly go from a couple 100 unit tests to thousands. As a result, that extra time spent on reflection can add up quickly. Usually Unit tests are run as a step in the CI build process for an application as well, so it is ideal to keep that as small as possible so that developers can iterate quickly and get builds out much quicker.

SimpleTest therefore is a barebones unit testing library. The idea is that you create a basic console application, include this library as a dependency and program your tests and test groups using the built-in functions.

```cs

public class Program
{
	public static void Main(string [] args)
	{
		RunTestServiceTests();
	}
	
	static void RunTestServiceTests()
	{
		for (int i = 0; i < 10; ++i)
		{
			Test.Run($"Test if {i} is even", () => {
				RunIsEvenTest(i, i%2 == 0);
			});
		}
	}

	static void RunIsEvenTest(int i, bool expectedResultIsEven)
	{
		Assert.AreEqual(expectedResultIsEven, IntTester.IsEven(i));
	}
}
``` 

The real power that comes from doing it this way, is in the flexibility of parameterized tests. Now this could arguably get more unruly quicker, but with the appropriate discipline and code reviews, this shouldn't be much of an issue. The performance is also 10x better than what you get with one of the traditional .Net unit testing libraries.

The API's are relatively straightforward and utilize a similar callback structure to what you see in other ecosystems like Node.js/Lua/etc.


