// ProjectArchive.NativeLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

extern "C" PROJECTARCHIVE_NATIVE_API void DoWork() {
	std::vector<int> vect;

	for (int x = 0; x < 10; x++) {
		vect.push_back(x);
	}

}


