#include<thrust/device_vector.h>
#include<thrust/fill.h>
#include<thrust/copy.h>
#include<thrust/inner_product.h>

typedef thrust::device_vector<double> Vector;

extern "C"
{
	__declspec(dllexport) Vector* _stdcall Create(int size)
	{
		Vector* vec = new Vector(size);

		return vec;
	}
	
	__declspec(dllexport) void _stdcall Fill(Vector* vec, int size, double value)
	{
		thrust::fill_n(vec->begin(), size, value);
	}
	
	__declspec(dllexport) double _stdcall Length2(Vector* vec, int size)
	{
		return thrust::inner_product(vec->begin(),vec->begin() + size, vec->begin(), 0.0);
	}

	__declspec(dllexport) void _stdcall CopyTo(Vector* source, double destination[], int size)
	{
		thrust::copy_n(source->begin(), size, destination);
	}
	
	__declspec(dllexport) void _stdcall Delete(Vector* vec)
	{
		delete vec;
	}
}