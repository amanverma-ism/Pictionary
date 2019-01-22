#include "ImageProcessingFilter.h"
#include<functional>

namespace CppProjects
{
	ImageProcessingFilter::ImageProcessingFilter()
	{

	}

	inline bool ImageProcessingFilter::RGBPixelsToHSVPixels(byte R, byte G, byte B, float &H, float &S, float &V)
	{
		//Start- Initializing variables needed for conversion
		float r = ((float)R / 255.0f);
		float g = ((float)G / 255.0f);
		float b = ((float)B / 255.0f);
		float cMax = r > g ? (r > b ? r : b) : (g > b ? g : b);
		float cMin = r < g ? (r < b ? r : b) : (g < b ? g : b);
		float delta = cMax - cMin;
		//End- Initializing variables needed for conversion

		//Value Calculation
		V = cMax;
		//Start- Saturation Calculation
		if (cMax <= 0.0)
		{
			S = 0;
			H = -1.0;
			return false;
		}
		else
		{
			S = delta / cMax;
		}
		//End- Saturation Calculation

		//Start- Hue Calculation
		if (delta < 0.000000001)
		{
			H = 0;
		}
		else if (r >= cMax)
		{
			H = (60.0f * (std::fmod(((g - b) / delta), 6.0f)));
		}
		else if (g >= cMax)
		{
			H = (60.0f * (((b - r) / delta) + 2.0f));
		}
		else
		{
			H = (60.0f * (((r - g) / delta) + 4.0f));
		}

		if (H < 0)
		{
			H += 360;
		}
		//End- Hue Calculation
		return true;
	}

	inline bool ImageProcessingFilter::HSVPixelsToRGBPixels(float H, float S, float V, byte &R, byte &G, byte &B)
	{
		if (!(0 <= H && H < 360 && S >= 0 && S <= 1 && V >= 0 && V <= 1))
			return false;
		//Start- Initializing variables needed for conversion
		float C = V * S;
		float tmp = std::fmod((H / 60.0f), 2.0f) - 1.0f;
		if (tmp < 0)
			tmp = -tmp;
		float X = (C * (1.0f - tmp));
		float m = V - C;
		float r, g, b;
		//End- Initializing variables needed for conversion

		//Start- r, g, b calculation
		if (H >= 0 && H < 60)
		{
			r = C;
			g = X;
			b = 0;
		}
		else if (H >= 60 && H < 120)
		{
			r = X;
			g = C;
			b = 0;
		}
		else if (H >= 120 && H < 180)
		{
			r = 0;
			g = C;
			b = X;
		}
		else if (H >= 180 && H < 240)
		{
			r = 0;
			g = X;
			b = C;
		}
		else if (H >= 240 && H < 300)
		{
			r = X;
			g = 0;
			b = C;
		}
		else //when H>=300 && H<360
		{
			r = C;
			g = 0;
			b = X;
		}
		//End- r, g, b calculation

		//Start- Final R, G, B calculation
		R = (byte)((r + m) * 255.0f);
		G = (byte)((g + m) * 255.0f);
		B = (byte)((b + m) * 255.0f);
		//End- Final R, G, B calculation

		return true;
	}

	inline bool ImageProcessingFilter::RGBPixelsToHSLPixels(byte R, byte G, byte B, float &H, float &S, float &L)
	{
		//Start- Initializing variables needed for conversion
		float r = ((float)R / 255.0f);
		float g = ((float)G / 255.0f);
		float b = ((float)B / 255.0f);
		float cMax = r > g ? (r > b ? r : b) : (g > b ? g : b);
		float cMin = r < g ? (r < b ? r : b) : (g < b ? g : b);
		float delta = cMax - cMin;
		//End- Initializing variables needed for conversion

		//Luminance Calculation
		L = (cMax + cMin) / 2.0f;
		//Start- Saturation Calculation
		if (cMax == 0.0f)
		{
			S = 0;
			H = 0;
			return true;
		}
		else
		{
			float tmp = ((2.0f*(float)L) - 1.0f);
			if (tmp < 0.0f)
				tmp = -tmp;
			S = delta / (1 - tmp);
		}
		//End- Saturation Calculation

		//Start- Hue Calculation
		if (r >= cMax)
		{
			H = (60.0f * (std::fmod(((g - b) / delta), 6.0f)));
		}
		else if (g >= cMax)
		{
			H = (60.0f * (((b - r) / delta) + 2.0f));
		}
		else
		{
			H = (60.0f * (((r - g) / delta) + 4.0f));
		}

		if (H < 0)
		{
			H += 360;
		}
		//End- Hue Calculation
		return true;
	}

	inline bool ImageProcessingFilter::HSLPixelsToRGBPixels(float H, float S, float L, byte &R, byte &G, byte &B)
	{
		if (!(0 <= H && H < 360 && S >= 0 && S <= 1 && L >= 0 && L <= 1))
			return false;
		//Start- Initializing variables needed for conversion
		float tmp = ((2.0f*(float)L) - 1.0f);
		if (tmp < 0.0f)
			tmp = -tmp;
		float C = (1.0f - tmp) * S;
		tmp = std::fmod((H / 60.0f), 2.0f) - 1.0f;
		if (tmp < 0)
			tmp = -tmp;
		float X = (C * (1.0f - tmp));
		float m = L - (C / 2.0f);
		float r, g, b;
		//End- Initializing variables needed for conversion

		//Start- r, g, b calculation
		if (H >= 0 && H < 60)
		{
			r = C;
			g = X;
			b = 0;
		}
		else if (H >= 60 && H < 120)
		{
			r = X;
			g = C;
			b = 0;
		}
		else if (H >= 120 && H < 180)
		{
			r = 0;
			g = C;
			b = X;
		}
		else if (H >= 180 && H < 240)
		{
			r = 0;
			g = X;
			b = C;
		}
		else if (H >= 240 && H < 300)
		{
			r = X;
			g = 0;
			b = C;
		}
		else //when H>=300 && H<360
		{
			r = C;
			g = 0;
			b = X;
		}
		//End- r, g, b calculation

		//Start- Final R, G, B calculation
		R = (byte)((r + m) * 255.0f);
		G = (byte)((g + m) * 255.0f);
		B = (byte)((b + m) * 255.0f);
		//End- Final R, G, B calculation

		return true;
	}

	void ImageProcessingFilter::AdjustBrightness(byte ImagePixelArray[], int brightnessFactor, int ImagePixelArrlength)
	{
		float H, S, V; byte R, G, B;
		if (brightnessFactor >= 0)
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; V = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSVPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, V))
				{
					V += ((float)brightnessFactor / 100.0f);
					V = (1 < V ? 1 : V);
					if (HSVPixelsToRGBPixels(H, S, V, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; V = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSVPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, V))
				{
					V += ((float)brightnessFactor / 100.0f);
					V = (0 > V ? 0 : V);
					if (HSVPixelsToRGBPixels(H, S, V, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
	}

	void ImageProcessingFilter::AdjustLuminance(byte ImagePixelArray[], int luminanceFactor, int ImagePixelArrlength)
	{
		register float H, S, L; 
		register byte R, G, B;
		if (luminanceFactor >= 0)
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; L = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSLPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, L))
				{
					L += ((float)luminanceFactor / 100.0f);
					L = (1 < L ? 1 : L);
					if (HSLPixelsToRGBPixels(H, S, L, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; L = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSLPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, L))
				{
					L += ((float)luminanceFactor / 100.0f);
					L = (0 > L ? 0 : L);
					if (HSLPixelsToRGBPixels(H, S, L, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
	}

	void ImageProcessingFilter::AdjustContrast(byte ImagePixelArray[], int contrastFactor, int ImagePixelArrlength)
	{
		register float factor;
		register int R, G, B;

		for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
		{
			factor = (259.0f * ((float)contrastFactor + 255.0f)) / (255.0f * (259.0f - (float)contrastFactor));
			R = (int)(factor * (ImagePixelArray[i] - 128.0f) + 128.0f);
			G = (int)(factor * (ImagePixelArray[i + 1] - 128.0f) + 128.0f);
			B = (int)(factor * (ImagePixelArray[i + 2] - 128.0f) + 128.0f);
			R = R < 0 ? 0 : (R > 255 ? 255 : R);
			G = G < 0 ? 0 : (G > 255 ? 255 : G);
			B = B < 0 ? 0 : (B > 255 ? 255 : B);
			
			ImagePixelArray[i] = R;
			ImagePixelArray[i + 1] = G;
			ImagePixelArray[i + 2] = B;
		}
	}

	void ImageProcessingFilter::AdjustHue(byte ImagePixelArray[], int hueFactor, int ImagePixelArrlength)
	{
		float H, S, L; byte R, G, B;
		if (hueFactor >= 0)
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; L = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSLPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, L))
				{
					H += ((float)hueFactor);
					H = (360 < H ? 360 - H : H);
					if (HSLPixelsToRGBPixels(H, S, L, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; L = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSLPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, L))
				{
					H += ((float)hueFactor);
					H = (0 > H ? H + 360 : H);
					if (HSLPixelsToRGBPixels(H, S, L, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
	}

	void ImageProcessingFilter::AdjustSaturation(byte ImagePixelArray[], int saturationFactor, int ImagePixelArrlength)
	{
		float H, S, L; byte R, G, B;
		if (saturationFactor >= 0)
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; L = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSLPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, L))
				{
					S += ((float)saturationFactor / 100.0f);
					S = (1 < S ? 1 : S);
					if (HSLPixelsToRGBPixels(H, S, L, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < ImagePixelArrlength - 4; i += 4)
			{
				H = 0; S = 0; L = 0; R = 0; G = 0; B = 0;
				if (RGBPixelsToHSLPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, L))
				{
					S += ((float)saturationFactor / 100.0f);
					S = (0 > S ? 0 : S);
					if (HSLPixelsToRGBPixels(H, S, L, R, G, B))
					{
						ImagePixelArray[i] = R;
						ImagePixelArray[i + 1] = G;
						ImagePixelArray[i + 2] = B;
					}
				}
			}
		}
	}

	void ImageProcessingFilter::AdjustHSVParameters(byte ImagePixelArray[], int hueFactor, int saturationFactor, int brightnessFactor, int ImagePixelArrlength)
	{
		register float H, S, V; 
		register byte R, G, B;
		for (register int i = 0; i < ImagePixelArrlength - 4; i += 4)
		{
			H = 0; S = 0; V = 0; R = 0; G = 0; B = 0;
			if (RGBPixelsToHSVPixels(ImagePixelArray[i], ImagePixelArray[i + 1], ImagePixelArray[i + 2], H, S, V))
			{
				H += ((float)hueFactor);
				H = (H > 360 ? 360 - H : H);
				H = (H < 0 ? H + 360 : H);

				S += ((float)saturationFactor / 100.0f);
				S = (S > 1 ? 1 : S);
				S = (S < 0 ? 0 : S);

				V += ((float)brightnessFactor / 100.0f);
				V = (V > 1 ? 1 : V);
				V = (V < 0 ? 0 : V);

				if (HSVPixelsToRGBPixels(H, S, V, R, G, B))
				{
					ImagePixelArray[i] = R;
					ImagePixelArray[i + 1] = G;
					ImagePixelArray[i + 2] = B;
				}
			}
		}
	}

	void ImageProcessingFilter::AdjustSmoothening(byte TargetImagePixelArray[], int blurFactor, int ImageHeight, int ImageWidth)
	{
		if (blurFactor <= 1)
			return;

		register short rTotal = 0, gTotal = 0, bTotal = 0, rAverage = 0, gAverage = 0, bAverage = 0;
		register short rTotalFirst = 0, gTotalFirst = 0, bTotalFirst = 0;
		register bool calculate = true;
		register int i = 0, j = 0, k = 0, l = 0, index = 0;
		register const short halfFactor = blurFactor / 2, count = blurFactor * blurFactor, _imageHeight = ImageHeight, _imageWidth = ImageWidth;

		register int len = _imageHeight*_imageWidth * 4;
		byte *OriginalImagePixelArray = new byte[len];
		for (register int iI = 0; iI < len; iI++)
		{
			OriginalImagePixelArray[iI] = TargetImagePixelArray[iI];
		}

		for (i = halfFactor; i < _imageHeight - halfFactor; i++)
		{
			for (j = halfFactor; j < _imageWidth - halfFactor; j++)
			{
				rAverage = gAverage = bAverage = 0;
				if (!calculate)
				{
					if (j != halfFactor)  //Traversing through the width row.     
					{
						for (k = (i - halfFactor); k <= (i + halfFactor); k++)
						{
							index = ((k * _imageWidth) + ((j - 1) - halfFactor)) * 4;
							//Drop the previous pixels which are not in the new position of box.
							rTotal -= OriginalImagePixelArray[index];
							gTotal -= OriginalImagePixelArray[index + 1];
							bTotal -= OriginalImagePixelArray[index + 2];
						}
						for (k = (i - halfFactor); k <= (i + halfFactor); k++)
						{
							index = (((k * _imageWidth) + (j + halfFactor)) * 4);
							//Add the new pixels which becomes the part after moving box to new position.
							rTotal += OriginalImagePixelArray[index];
							gTotal += OriginalImagePixelArray[index + 1];
							bTotal += OriginalImagePixelArray[index + 2];
						}
					}
					else			//Every new row for new height index.
					{
						for (l = (j - halfFactor); l <= (j + halfFactor); l++)
						{
							index = (((i - 1 - halfFactor) * _imageWidth) + l) * 4;
							//Drop the previous pixels which are not in the new position of box.
							rTotalFirst -= OriginalImagePixelArray[index];
							gTotalFirst -= OriginalImagePixelArray[index + 1];
							bTotalFirst -= OriginalImagePixelArray[index + 2];
						}
						for (l = (j - halfFactor); l <= (j + halfFactor); l++)
						{
							index = (((i + halfFactor) * _imageWidth) + l) * 4;
							//Add the new pixels which becomes the part after moving box to new position.
							rTotalFirst += OriginalImagePixelArray[index];
							gTotalFirst += OriginalImagePixelArray[index + 1];
							bTotalFirst += OriginalImagePixelArray[index + 2];
						}
						rTotal = rTotalFirst;
						gTotal = gTotalFirst;
						bTotal = bTotalFirst;
					}
				}
				else      //Only for the first time.
				{
					rTotal = gTotal = bTotal = 0;

					for (k = (i - halfFactor); k <= (i + halfFactor); k++)
					{
						for (l = (j - halfFactor); l <= (j + halfFactor); l++)
						{
							index = (k * _imageWidth + l) * 4;
							rTotal += OriginalImagePixelArray[index];
							gTotal += OriginalImagePixelArray[index + 1];
							bTotal += OriginalImagePixelArray[index + 2];
						}
					}
					calculate = false;
					//Save the first box values for next iteration.
					rTotalFirst = rTotal;
					gTotalFirst = gTotal;
					bTotalFirst = bTotal;
				}

				//Calculate the average.
				rAverage = rTotal / count;
				gAverage = gTotal / count;
				bAverage = bTotal / count;
				//Fill the pixel values.
				TargetImagePixelArray[(i * _imageWidth + j) * 4] = (byte)rAverage;
				TargetImagePixelArray[(i * _imageWidth + j) * 4 + 1] = (byte)gAverage;
				TargetImagePixelArray[(i * _imageWidth + j) * 4 + 2] = (byte)bAverage;
			}
		}
		delete[] OriginalImagePixelArray;
	}

	void ImageProcessingFilter::CalculatePerwittEdges(byte ImagePixelArray[], int _imageHeight, int _imageWidth)
	{

		//Using perwitt algorithm for edge detection. 
		register const int imgHeight = _imageHeight;
		register const int imgWidth = _imageWidth;
		register short i = 0, j = 0;
		register short Rx = 0, Gx = 0, Bx = 0, Ry = 0, Gy = 0, By = 0, R = 0, G = 0, B = 0;
		register int index1, index2, index3, index4, index5, index6, index7, index8, index9;
		
		register int len = _imageHeight*_imageWidth * 4;
		byte *OriginalImagePixelArray = new byte[len];
		for (register int iI = 0; iI < len; iI++)
		{
			OriginalImagePixelArray[iI] = ImagePixelArray[iI];
		}

		for (i = 1; i< imgHeight - 1; i++)
		{
			for (j = 1; j< imgWidth - 1; j++)
			{
				//Start- index calculation of the convolution matrix.
				index1 = ((i - 1)*imgWidth + (j - 1)) * 4;
				index2 = index1 + 4;
				index3 = index2 + 4;
				index4 = (i*imgWidth + (j - 1)) * 4;
				index5 = index4 + 4;
				index6 = index5 + 4;
				index7 = ((i + 1)*imgWidth + (j - 1)) * 4;
				index8 = index7 + 4;
				index9 = index8 + 4;
				//End- index calculation of the convolution matrix.

				//X-direction gradient calculation.
				Rx = OriginalImagePixelArray[index3] + OriginalImagePixelArray[index6] + OriginalImagePixelArray[index9] - OriginalImagePixelArray[index1] - OriginalImagePixelArray[index4] - OriginalImagePixelArray[index7];
				Gx = OriginalImagePixelArray[index3 + 1] + OriginalImagePixelArray[index6 + 1] + OriginalImagePixelArray[index9 + 1] - OriginalImagePixelArray[index1 + 1] - OriginalImagePixelArray[index4 + 1] - OriginalImagePixelArray[index7 + 1];
				Bx = OriginalImagePixelArray[index3 + 2] + OriginalImagePixelArray[index6 + 2] + OriginalImagePixelArray[index9 + 2] - OriginalImagePixelArray[index1 + 2] - OriginalImagePixelArray[index4 + 2] - OriginalImagePixelArray[index7 + 2];
				
				//Y-direction gradient calculation.
				Ry = OriginalImagePixelArray[index7] + OriginalImagePixelArray[index8] + OriginalImagePixelArray[index9] - OriginalImagePixelArray[index1] - OriginalImagePixelArray[index2] - OriginalImagePixelArray[index3];
				Gy = OriginalImagePixelArray[index7 + 1] + OriginalImagePixelArray[index8 + 1] + OriginalImagePixelArray[index9 + 1] - OriginalImagePixelArray[index1 + 1] - OriginalImagePixelArray[index2 + 1] - OriginalImagePixelArray[index3 + 1];
				By = OriginalImagePixelArray[index7 + 2] + OriginalImagePixelArray[index8 + 2] + OriginalImagePixelArray[index9 + 2] - OriginalImagePixelArray[index1 + 2] - OriginalImagePixelArray[index2 + 2] - OriginalImagePixelArray[index3 + 2];
				
				//Magnitude calculation.
				R = (short)std::sqrt((Rx*Rx) + (Ry*Ry));
				G = (short)std::sqrt((Gx*Gx) + (Gy*Gy));
				B = (short)std::sqrt((Bx*Bx) + (By*By));

				//Start- Boundary check
				R = R < 0 ? 0 : R;
				R = R > 255 ? 255 : R;

				G = G < 0 ? 0 : G;
				G = G > 255 ? 255 : G;

				B = B < 0 ? 0 : B;
				B = B > 255 ? 255 : B;
				//End- Boundary check

				//Fill the calculated pixel values.
				ImagePixelArray[index5] = (byte)R;
				ImagePixelArray[index5 + 1] = (byte)G;
				ImagePixelArray[index5 + 2] = (byte)B;
			}
		}
		delete[] OriginalImagePixelArray;
	}

	void ImageProcessingFilter::ApplyFilters(byte TargetImagePixelArray[], int hueValue, int saturationFactor, int brightnessFactor, int blurFactor, int luminanceFactor, int contrastFactor, bool _isPerwittEdgeDetectionChecked, int ImageHeight, int ImageWidth, int ImagePixelArrlength)
	{
		if (_isPerwittEdgeDetectionChecked)
		{
			CalculatePerwittEdges(TargetImagePixelArray, ImageHeight, ImageWidth);
		}
		if (hueValue != 0 || saturationFactor != 0 || brightnessFactor != 0)
		{
			AdjustHSVParameters(TargetImagePixelArray, hueValue, saturationFactor, brightnessFactor, ImagePixelArrlength);
		}
		if (luminanceFactor != 0)
		{
			AdjustLuminance(TargetImagePixelArray, luminanceFactor, ImagePixelArrlength);
		}
		if (contrastFactor != 0)
		{
			AdjustContrast(TargetImagePixelArray, contrastFactor, ImagePixelArrlength);
		}
		if (blurFactor > 1)
		{
			AdjustSmoothening(TargetImagePixelArray, blurFactor, ImageHeight, ImageWidth);
		}
	}
}
