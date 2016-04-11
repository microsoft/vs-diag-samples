#pragma once
#include "stdafx.h"

#define PROJECTARCHIVE_NATIVE_API __declspec(dllexport)

extern "C" { 
	PROJECTARCHIVE_NATIVE_API void DoWork();
}
