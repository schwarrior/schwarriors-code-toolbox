using System.Collections.Specialized;
using System.Drawing;

public class ImageUtilities
{
    /// <summary>
    /// Resize an image propotionally to fit inside a rectangle
    /// with dimensions prescribed by minWidth and minHeight
    /// </summary>
    /// <param name="minWidth"></param>
    /// <param name="minHeight"></param>
    /// <param name="origImage"></param>
    /// <returns></returns>
    public static Image ZoomImage(int minWidth, int minHeight, Image origImage)
    {
        var scaledImageDimensions = ScaleInnerRectangleProportionallyToFitInsideOuterRectangle(
            new Point(minWidth, minHeight), new Point(origImage.Width, origImage.Height));
        return ResizeImage(origImage, scaledImageDimensions.X, scaledImageDimensions.Y);
    }

    /// <summary>
    /// Resize a rectangle proportionally to fit inside another rectangle
    /// </summary>
    /// <param name="outerRectDimensions"></param>
    /// <param name="innerRectDimensions"></param>
    /// <returns></returns>
    public static Point ScaleInnerRectangleProportionallyToFitInsideOuterRectangle(
        Point outerRectDimensions, Point innerRectDimensions)
    {
        var outerRectAspect = (double)outerRectDimensions.X / (double)outerRectDimensions.Y;
        var innerRectAspect = (double)innerRectDimensions.X / (double)innerRectDimensions.Y;
        if (innerRectAspect >= outerRectAspect)
        {
            var widthScale = (double)outerRectDimensions.X / (double)innerRectDimensions.X;
            var newInY = (int)(innerRectDimensions.Y * widthScale);
            return new Point(outerRectDimensions.X, newInY);
        }
        var heightScale = (double)outerRectDimensions.Y / (double)innerRectDimensions.Y;
        var newInX = (int)(innerRectDimensions.X * heightScale);
        return new Point(newInX, outerRectDimensions.Y);
    }

    public static Image ResizeImage(Image img, int width, int height)
    {
        Bitmap b = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage((Image)b))
        {
            g.DrawImage(img, 0, 0, width, height);
        }

        return (Image)b;
    }
}
