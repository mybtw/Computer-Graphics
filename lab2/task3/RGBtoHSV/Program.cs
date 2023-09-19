using System.Drawing;
using System.Drawing.Imaging;

class Program
{
    static void Main()
    {
        // Загрузка изображения
        Bitmap originalImage = new Bitmap("input.jpg");

        // Преобразование изображения в HSV
        Bitmap hsvImage = ConvertToHSV(originalImage);

        // Изменение оттенка, насыщенности и яркости
        ModifyHSV(hsvImage, deltaHue: 30, deltaSaturation: 0.5f, deltaBrightness: 0.2f);

        // Преобразование изображения обратно в RGB
        Bitmap resultImage = ConvertToRGB(hsvImage);

        // Сохранение результата
        resultImage.Save("output.jpg", ImageFormat.Jpeg);
    }

    static Bitmap ConvertToHSV(Bitmap inputImage)
    {
        Bitmap hsvImage = new Bitmap(inputImage.Width, inputImage.Height);

        for (int y = 0; y < inputImage.Height; y++)
        {
            for (int x = 0; x < inputImage.Width; x++)
            {
                Color pixel = inputImage.GetPixel(x, y);
                float hue, saturation, brightness;
                ColorToHSV(pixel, out hue, out saturation, out brightness);
                hsvImage.SetPixel(x, y, Color.FromArgb((int)hue, (int)(saturation * 255), (int)(brightness * 255)));
            }
        }

        return hsvImage;
    }

    static void ColorToHSV(Color color, out float hue, out float saturation, out float brightness)
    {
        float r = color.R / 255.0f;
        float g = color.G / 255.0f;
        float b = color.B / 255.0f;
        float max = Math.Max(r, Math.Max(g, b));
        float min = Math.Min(r, Math.Min(g, b));

        hue = 0;
        saturation = 0;
        brightness = max;

        float delta = max - min;

        if (max > 0)
        {
            saturation = delta / max;
            if (max == r)
                hue = (g - b) / delta + (g < b ? 6 : 0);
            else if (max == g)
                hue = (b - r) / delta + 2;
            else
                hue = (r - g) / delta + 4;

            hue *= 60;
        }
    }

    static Bitmap ConvertToRGB(Bitmap hsvImage)
    {
        Bitmap rgbImage = new Bitmap(hsvImage.Width, hsvImage.Height);

        for (int y = 0; y < hsvImage.Height; y++)
        {
            for (int x = 0; x < hsvImage.Width; x++)
            {
                Color hsvPixel = hsvImage.GetPixel(x, y);
                Color rgbPixel = HSVToColor(hsvPixel.R, hsvPixel.G / 255.0f, hsvPixel.B / 255.0f);
                rgbImage.SetPixel(x, y, rgbPixel);
            }
        }

        return rgbImage;
    }

    static Color HSVToColor(float hue, float saturation, float brightness)
    {
        int hi = (int)(hue / 60) % 6;
        float f = hue / 60 - (int)(hue / 60);
        float p = brightness * (1 - saturation);
        float q = brightness * (1 - f * saturation);
        float t = brightness * (1 - (1 - f) * saturation);

        switch (hi)
        {
            case 0:
                return Color.FromArgb((int)(brightness * 255), (int)(t * 255), (int)(p * 255));
            case 1:
                return Color.FromArgb((int)(q * 255), (int)(brightness * 255), (int)(p * 255));
            case 2:
                return Color.FromArgb((int)(p * 255), (int)(brightness * 255), (int)(t * 255));
            case 3:
                return Color.FromArgb((int)(p * 255), (int)(q * 255), (int)(brightness * 255));
            case 4:
                return Color.FromArgb((int)(t * 255), (int)(p * 255), (int)(brightness * 255));
            default:
                return Color.FromArgb((int)(brightness * 255), (int)(p * 255), (int)(q * 255));

        } 
    }

    static void ModifyHSV(Bitmap hsvImage, float deltaHue, float deltaSaturation, float deltaBrightness)
    {
        for (int y = 0; y < hsvImage.Height; y++)
        {
            for (int x = 0; x < hsvImage.Width; x++)
            {
                Color hsvPixel = hsvImage.GetPixel(x, y);
                float hue, saturation, brightness;
                ColorToHSV(hsvPixel, out hue, out saturation, out brightness);

                // Изменение оттенка, насыщенности и яркости
                hue += deltaHue;
                saturation = Math.Max(0, Math.Min(1, saturation + deltaSaturation));
                brightness = Math.Max(0, Math.Min(1, brightness + deltaBrightness));

                Color newHsvPixel = HSVToColor(hue, saturation, brightness);
                hsvImage.SetPixel(x, y, newHsvPixel);
            }
        }
    }
}
