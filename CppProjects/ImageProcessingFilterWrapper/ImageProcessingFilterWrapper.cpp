// This is the main DLL file.

#include "ImageProcessingFilterWrapper.h"

namespace ImageProcessingCppWrapper
{
	ImageProcessingFilterWrapper::ImageProcessingFilterWrapper() :WrapperManagedObject(new CppProjects::ImageProcessingFilter())
	{
		
	}

	ImageProcessingFilterWrapper::ImageProcessingFilterWrapper(array<byte>^ imageData) : WrapperManagedObject(new CppProjects::ImageProcessingFilter())
	{
		localImageDataPointer = imageData;
	}

	void ImageProcessingFilterWrapper::AdjustBrightness(array<byte>^ imagePixelArray, int brightnessFactor)
	{
		pin_ptr<byte> pnPixelData = &imagePixelArray[0];
		m_Instance->AdjustBrightness(pnPixelData, brightnessFactor, imagePixelArray->Length);
		pnPixelData = nullptr;
	}

	void ImageProcessingFilterWrapper::AdjustHSVParameters(array<byte>^ imagePixelArray, int hueFactor, int saturationFactor, int brightnessFactor)
	{
		pin_ptr<byte> pnPixelData = &imagePixelArray[0];
		m_Instance->AdjustHSVParameters(pnPixelData, hueFactor, saturationFactor, brightnessFactor, imagePixelArray->Length);
		pnPixelData = nullptr;
	}

	void ImageProcessingFilterWrapper::AdjustHue(array<byte>^ imagePixelArray, int hueFactor)
	{
		pin_ptr<byte> pnPixelData = &imagePixelArray[0];
		m_Instance->AdjustHue(pnPixelData, hueFactor, imagePixelArray->Length);
		pnPixelData = nullptr;
	}

	void ImageProcessingFilterWrapper::AdjustSaturation(array<byte>^ imagePixelArray, int saturationFactor)
	{
		pin_ptr<byte> pnPixelData = &imagePixelArray[0];
		m_Instance->AdjustSaturation(pnPixelData, saturationFactor, imagePixelArray->Length);
		pnPixelData = nullptr;
	}

	void ImageProcessingFilterWrapper::AdjustLuminance(array<byte>^ imagePixelArray, int saturationFactor)
	{
		pin_ptr<byte> pnPixelData = &imagePixelArray[0];
		m_Instance->AdjustLuminance(pnPixelData, saturationFactor, imagePixelArray->Length);
		pnPixelData = nullptr;
	}

	void ImageProcessingFilterWrapper::ApplyFilters(array<byte>^ TargetImagePixelArray, int hueValue, int saturationFactor, int brightnessFactor, int blurFactor, int luminanceFactor, int contrastFactor, bool _isPerwittEdgeDetectionChecked, int ImageHeight, int ImageWidth)
	{
		pin_ptr<byte> targetArray = &TargetImagePixelArray[0];
		m_Instance->ApplyFilters(targetArray, hueValue, saturationFactor, brightnessFactor, blurFactor, luminanceFactor, contrastFactor, _isPerwittEdgeDetectionChecked, ImageHeight, ImageWidth, (TargetImagePixelArray->Length));
		targetArray = nullptr;
	}

}

