#pragma once

#include <collection.h>

using namespace Windows::Storage::Streams;


namespace PhotoFilterLib_Win10
{
	public ref class ImageFilter sealed
	{
	public:
		ImageFilter();

		Platform::Array<unsigned char>^ ImageFilter::AntiqueImage(const Platform::Array<unsigned char>^ buffer);
	};
}