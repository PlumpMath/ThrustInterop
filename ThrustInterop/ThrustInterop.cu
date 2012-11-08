#include<cuda_runtime_api.h>
#include<thrust/device_vector.h>
#include<thrust/fill.h>
#include<thrust/copy.h>
#include<thrust/inner_product.h>

typedef thrust::device_vector<double> Vector;

extern "C"
{
	__declspec(dllexport) int _stdcall GetDeviceCount()
	{
		int count;
		::cudaGetDeviceCount(&count);

		return count;
	}

	__declspec(dllexport) Vector* _stdcall Create(int size, int deviceID)
	{
		::cudaSetDevice(deviceID);

		Vector* vec = new Vector(size);

		return vec;
	}
	
	__declspec(dllexport) void _stdcall Fill(Vector* vec, int size, double value, int deviceID)
	{
		::cudaSetDevice(deviceID);
		thrust::fill_n(vec->begin(), size, value);
	}
	
	__declspec(dllexport) double _stdcall Length2(Vector* vec, int size, int deviceID)
	{
		::cudaSetDevice(deviceID);
		return thrust::inner_product(vec->begin(),vec->begin() + size, vec->begin(), 0.0);
	}

	__declspec(dllexport) void _stdcall CopyTo(Vector* source, double destination[], int size, int deviceID)
	{
		::cudaSetDevice(deviceID);
		thrust::copy_n(source->begin(), size, destination);
	}
	
	__declspec(dllexport) void _stdcall Delete(Vector* vec, int deviceID)
	{
		::cudaSetDevice(deviceID);
		delete vec;
	}
}