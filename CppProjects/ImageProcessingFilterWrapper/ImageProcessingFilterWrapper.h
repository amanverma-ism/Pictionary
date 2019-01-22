// ImageProcessingFilterWrapper.h

#pragma once
#include "ManagedWrapperObject.h"
#include "../ImageProcessingFilter/ImageProcessingFilter.h"
using namespace System;

namespace ImageProcessingCppWrapper {

	public ref class ImageProcessingFilterWrapper : public WrapperManagedObject<CppProjects::ImageProcessingFilter>
	{
	private:
		array<byte>^ localImageDataPointer;
	public:
		ImageProcessingFilterWrapper();
		ImageProcessingFilterWrapper(array<byte>^ imageData);
		void AdjustBrightness(array<byte>^ imagePixelArray, int brightnessFactor);
		void AdjustHSVParameters(array<byte>^ imagePixelArray, int hueFactor, int saturationFactor, int brightnessFactor);
		void AdjustHue(array<byte>^ imagePixelArray, int hueFactor);
		void AdjustSaturation(array<byte>^ imagePixelArray, int saturationFactor);
		void AdjustLuminance(array<byte>^ imagePixelArray, int luminanceFactor);
		void ApplyFilters(array<byte>^ TargetImagePixelArray, int hueValue, int saturationFactor, int brightnessFactor, int blurFactor, int luminanceFactor, int contrastFactor, bool _isPerwittEdgeDetectionChecked, int ImageHeight, int ImageWidth);
	};
}
