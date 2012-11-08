using System;
using System.Runtime.InteropServices;

namespace LWisteria.ThrustInterop
{
	static class ThrustInteropTestMain
	{
		const string DLL_NAME = "ThrustInterop.dll";

		[DllImport(DLL_NAME)]
		static extern IntPtr Create(int size);

		[DllImport(DLL_NAME)]
		static extern void Fill(IntPtr vec, int size, double value);

		[DllImport(DLL_NAME)]
		static extern double Length2(IntPtr vec, int size);

		[DllImport(DLL_NAME)]
		static extern void CopyTo(IntPtr vec, double[] array, int size);

		[DllImport(DLL_NAME)]
		static extern void Delete(IntPtr vec);

		const int N = 1300;
		const double VALUE = 9.56;

		static int Main()
		{
			IntPtr vec = Create(N);

			Fill(vec, N, VALUE);

			double result = Length2(vec, N);
			Console.WriteLine("result = {0}, answer = {1}", result, N * VALUE * VALUE);

			var array = new double[N];
			CopyTo(vec, array, N);

			Delete(vec);

			return Environment.ExitCode;
		}
	}
}