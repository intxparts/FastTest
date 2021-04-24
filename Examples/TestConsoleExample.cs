using System;
using SimpleTest;

namespace Examples
{
	internal static class TestConsoleExamples
	{
		public static void BasicExample()
		{
			TestConsole.RunTests(() => {
				Test.Run("Basic Example", () => {
					Assert.IsTrue(true);
				});
			});
		}
	}
}