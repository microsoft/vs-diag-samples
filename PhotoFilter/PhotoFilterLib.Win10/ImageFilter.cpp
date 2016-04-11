// Class1.cpp
#include "pch.h"
#include "ImageFilter.h"
#include <robuffer.h>
#include <wrl.h>


using namespace PhotoFilterLib_Win10;
using namespace concurrency;
using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation::Collections;
using namespace Windows::Foundation;
using namespace Windows::UI::Core;
using namespace Windows::UI::Xaml::Media::Imaging;
using namespace Windows::Storage::Pickers;
using namespace Windows::Storage;
using namespace Windows::Storage::Streams;



ImageFilter::ImageFilter()
{
}


Platform::Array<unsigned char>^ ImageFilter::AntiqueImage(const Platform::Array<unsigned char>^ buffer)
{
	auto pixels = ref new Platform::Array<unsigned char>(buffer->Length);
	byte* pixelBuffer = new byte[buffer->Length];

	// make workload bigger so that it is easier to see the effects of parallelization
	for (unsigned int x = 0; x < buffer->Length; x += 4)
	{
		int rgincrease = 100;
		int red = buffer[x] + rgincrease;
		int green = buffer[x + 1] + rgincrease;
		int blue = buffer[x + 2];
		int alpha = buffer[x + 3];

		if (red > 255)
			red = 255;
		if (green > 255)
			green = 255;

		pixels[x] = red;
		pixels[x + 1] = green;
		pixels[x + 2] = blue;
		pixels[x + 3] = alpha;
	}
	return pixels;
}

