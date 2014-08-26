using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ESRGC.DLLR.EARN.Domain.Model;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ESRGC.DLLR.EARN.Helpers
{
  public static class Utility
  {
    public static List<string> SecurityQuestions {
      get {
        return new List<string> {
                    "What is your favorite pet's tagName?",
                    "What is your favorite beverage?",
                    "What is your best friend's tagName?",
                    "What is your favorite sport team?",
                    "What is the tagName of the first company you worked for?",
                    "Where did you go on your honeymoon?",
                    "What is your favorite movie?",
                    "What is your father's middle tagName?",
                    "In what city was your mother born?",
                    "In what city were you born?"
                };
      }
    }

    private static Random random = new Random((int)DateTime.Now.Ticks);
    public static string RandomString(int size) {
      StringBuilder builder = new StringBuilder();
      char ch;
      for (int i = 0; i < size; i++) {
        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
        builder.Append(ch);
      }

      return builder.ToString();
    }
    public static string toShorDescription(this string text, int l) {
      int length = l > text.Length ? text.Length : l;
      if (l > text.Length)
        return text;
      else
        return text.Substring(0, length) + "...";
    }
    public static string TimeSpan(this DateTime timeInPast) {
      if (timeInPast == null)
        return "unknown";
      var timeSpan = DateTime.Now - timeInPast;
      if (timeSpan.Days == 0) {
        if (timeSpan.Hours == 0) {
          if (timeSpan.Minutes == 0)
            return "just now";
          if (timeSpan.Minutes == 1)
            return timeSpan.Minutes + " minute ago";
          else
            return timeSpan.Minutes + " minutes ago";
        }
        if (timeSpan.Hours == 1)
          return timeSpan.Hours + " hour ago";
        else
          return timeSpan.Hours + " hours ago";
      }
      if (timeSpan.Days == 1)
        return timeSpan.Days + " day ago";
      else {
        if (timeSpan.Days < 30)
          return timeSpan.Days + " days ago";
        if (timeSpan.Days >= 30 && timeSpan.Days < 60)
          return (int)timeSpan.Days / 30 + " month ago";
        else if (timeSpan.Days > 60 && timeSpan.Days < 365)
          return (int)timeSpan.Days / 30 + " months ago";
        else if (timeSpan.Days >= 365 && timeSpan.Days < 730)
          return "over a year ago";
        else
          return (int)timeSpan.Days / 365 + " years ago";
      }
    }
  }

  /// <summary>
  /// Contains Utility functions to process tiff images
  /// </summary>
  public class ImageProcessor
  {

    /// <summary>
    /// crope image
    /// </summary>
    /// <param name="sourceImg">Image to be cropped</param>
    /// <param name="cropSize">rectangle represents crop size</param>
    /// <returns>cropped image</returns>
    public static Bitmap cropImage(Image sourceImg, Rectangle cropSize) {
      var sourceBitmap = new Bitmap(sourceImg);
      var croppedImage = sourceBitmap.Clone(
          new Rectangle(cropSize.X, cropSize.Y, cropSize.Width, cropSize.Height),
          sourceBitmap.PixelFormat);
      croppedImage.SetResolution(300, 300);
      //var croppedImage = new Bitmap(cropSize.Width, cropSize.Height);
      //using (Graphics g = Graphics.FromImage(croppedImage))
      //{
      //    g.DrawImage(sourceBitmap,
      //                new Rectangle(0, 0, croppedImage.Width, croppedImage.Height),
      //                cropSize,
      //                GraphicsUnit.Pixel);
      //}
      return croppedImage;
    }
    public static Image resizeImage(Image imgToResize, Size size) {
      int sourceWidth = imgToResize.Width;
      int sourceHeight = imgToResize.Height;

      float nPercent = 0;
      float nPercentW = 0;
      float nPercentH = 0;

      nPercentW = ((float)size.Width / (float)sourceWidth);
      nPercentH = ((float)size.Height / (float)sourceHeight);

      if (nPercentH < nPercentW)
        nPercent = nPercentH;
      else
        nPercent = nPercentW;

      int destWidth = (int)(sourceWidth * nPercent);
      int destHeight = (int)(sourceHeight * nPercent);

      Bitmap b = new Bitmap(destWidth, destHeight);
      Graphics g = Graphics.FromImage((Image)b);
      g.InterpolationMode = InterpolationMode.HighQualityBilinear;

      g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
      g.Dispose();

      return (Image)b;
    }
  }
}