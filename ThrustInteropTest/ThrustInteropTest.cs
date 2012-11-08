using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace LWisteria.ThrustInterop
{
	static class ThrustInteropTestMain
	{
		const string DLL_NAME = "ThrustInterop.dll";

		[DllImport(DLL_NAME)]
		static extern int GetDeviceCount();

		[DllImport(DLL_NAME)]
		static extern IntPtr Create(int size, int deviceID);

		[DllImport(DLL_NAME)]
		static extern void Fill(IntPtr vec, int size, double value, int deviceID);

		[DllImport(DLL_NAME)]
		static extern double Length2(IntPtr vec, int size, int deviceID);

		[DllImport(DLL_NAME)]
		static extern void Delete(IntPtr vec, int deviceID);

		const int N = 12345678 * 2;

		const double VALUE = 0.0956;

		const int ITRATION = 1000;

		static int Main()
		{
			// 1GPU
			{
				var vec = Create(N, 0);

				Fill(vec, N, VALUE, 0);

				double result = 0;
				var stopwatch = new System.Diagnostics.Stopwatch();

				stopwatch.Start();
				for(int i = 0; i < ITRATION; i++)
				{
					result += Length2(vec, N, 0);
				}
				stopwatch.Stop();

				Console.WriteLine("1GPU: {0}", stopwatch.ElapsedMilliseconds);
				Console.WriteLine("result = {0}", result);
				Console.WriteLine("answer = {0}", VALUE * VALUE * ITRATION * N);

				Delete(vec, 0);
			}

			// 複数GPU
			{
				int deviceCount = GetDeviceCount();

				var vecs = new IntPtr[deviceCount];

				for(int deviceID = 0; deviceID < deviceCount; deviceID++)
				{
					vecs[deviceID] = Create(N / deviceCount, deviceID);

					Fill(vecs[deviceID], N / deviceCount, VALUE, deviceID);
				}

				var results = new double[deviceCount];

				var stopwatch = new System.Diagnostics.Stopwatch();

				stopwatch.Start();
				System.Threading.Tasks.Parallel.For(0, deviceCount, deviceID =>
				{
					for(int i = 0; i < ITRATION; i++)
					{
						results[deviceID] += Length2(vecs[deviceID], N / deviceCount, deviceID);
					}
				});
				stopwatch.Stop();

				Console.WriteLine("1GPU: {0}", stopwatch.ElapsedMilliseconds);
				Console.WriteLine("result = {0}", results.Sum());
				Console.WriteLine("answer = {0}", VALUE * VALUE * ITRATION * N);

				for(int deviceID = 0; deviceID < deviceCount; deviceID++)
				{
					Delete(vecs[deviceID], deviceID);
				}
			}

			return Environment.ExitCode;
		}
	}
}