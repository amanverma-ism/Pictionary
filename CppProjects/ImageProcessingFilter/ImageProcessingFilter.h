#pragma once
#include<Windows.h>
namespace CppProjects
{
	class ImageProcessingFilter
	{
	private:
	public:
		ImageProcessingFilter();
		void AdjustBrightness(byte imagePixelArray[], int brightnessFactor, int ImagePixelArrlength);
		void AdjustHSVParameters(byte imagePixelArray[], int hueValue, int saturationFactor, int brightnessFactor, int ImagePixelArrlength);
		void AdjustLuminance(byte imagePixelArray[], int luminanceFactor, int ImagePixelArrlength);
		void AdjustHue(byte imagePixelArray[], int hueFactor, int ImagePixelArrlength);
		void AdjustSaturation(byte imagePixelArray[], int saturationFactor, int ImagePixelArrlength);
		void AdjustContrast(byte ImagePixelArray[], int luminanceFactor, int ImagePixelArrlength);
		inline bool RGBPixelsToHSVPixels(byte R, byte G, byte B, float &H, float &S, float &V);
		inline bool HSVPixelsToRGBPixels(float H, float S, float V, byte &R, byte &G, byte &B);
		inline bool RGBPixelsToHSLPixels(byte R, byte G, byte B, float &H, float &S, float &L);
		inline bool HSLPixelsToRGBPixels(float H, float S, float L, byte &R, byte &G, byte &B);
		void AdjustSmoothening(byte TargetImagePixelArray[], int blurFactor, int ImageHeight, int ImageWidth);
		void CalculatePerwittEdges(byte ImagePixelArray[], int _imageHeight, int _imageWidth);
		void ApplyFilters(byte TargetImagePixelArray[], int hueValue, int saturationFactor, int brightnessFactor, int blurFactor, int luminanceFactor, int contrastFactor, bool _isPerwittEdgeDetectionChecked, int ImageHeight, int ImageWidth, int ImagePixelArrlength);
	};
}
