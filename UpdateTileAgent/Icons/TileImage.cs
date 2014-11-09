using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Png;


namespace UpdateTileAgent.Icons
{
    //http://habrahabr.ru/company/infopulse/blog/228619/
    public static class TileImage
    {
        public static Uri RenderSmallImage(string text)
        {
            var bitmap = new WriteableBitmap(70, 110);

            var canvas = new Grid
            {
                Width = bitmap.PixelWidth,
                Height = bitmap.PixelHeight
            };
            var background = new Canvas
            {
                Height = bitmap.PixelHeight,
                Width = bitmap.PixelWidth,
                Background = new SolidColorBrush(Colors.Transparent)
            };

            var titleBlock = new TextBlock
            {
                Text = text,
                //FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 19, 0, 19),
                TextWrapping = TextWrapping.NoWrap,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 30,
                Width = bitmap.PixelWidth
            };

            var comment = new TextBlock
            {
                Text = "CurrentSprint",
                //FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 5, 0, 0),
                TextWrapping = TextWrapping.NoWrap,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 10,
                Width = bitmap.PixelWidth
            };


            canvas.Children.Add(titleBlock);
            canvas.Children.Add(comment);

            bitmap.Render(background, null);
            bitmap.Render(canvas, null);
            bitmap.Invalidate();

            string fileName = string.Format("/Shared/ShellContent/small_icon_{0}.png", text);
            using (var imageStream = new IsolatedStorageFileStream(fileName, FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication()))
            {
                ExtendedImage myImage = ImageExtensions.ToImage(bitmap);
                myImage.WriteToStream(imageStream, fileName);

            }
            return new Uri("isostore:" + fileName, UriKind.Absolute);
        }
        /*
        public static Uri Render(
            String title,
            string row1,
            bool row1Bold,
            bool row1Italic,
            string row2,
            bool row2Bold,
            bool row2Italic)
        {
            var bitmap = new WriteableBitmap(Constants.TileWidth, Constants.TileHeight);
            var canvas = new Grid
            {
                Width = bitmap.PixelWidth,
                Height = bitmap.PixelHeight
            };
            var background = new Canvas
            {
                Height = bitmap.PixelHeight,
                Width = bitmap.PixelWidth,
                Background = new SolidColorBrush(Constants.TileBackgroundColor)
            };

            #region title
            var titleBlock = new TextBlock
            {
                Text = title,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(Constants.TilePadding),
                TextWrapping = TextWrapping.NoWrap,
                Foreground = new SolidColorBrush(Constants.TileForegroundColor),
                FontSize = Constants.TileTitleFontSize,
                Width = bitmap.PixelWidth - Constants.TilePadding * 2
            };
            #endregion

            #region first row
            var firstRowBlock = new TextBlock
            {
                Text = row1,
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(Constants.TilePadding, Constants.TilePadding * 2 + Constants.TileTitleFontSize, Constants.TilePadding, Constants.TilePadding),
                TextWrapping = TextWrapping.NoWrap,
                Foreground = new SolidColorBrush(Constants.TileForegroundColor),
                FontSize = Constants.TileTextFontSize,
                Width = bitmap.PixelWidth - Constants.TilePadding * 2
            };
            if (row1Bold)
            {
                firstRowBlock.FontWeight = FontWeights.Bold;
            }
            if (row1Italic)
            {
                firstRowBlock.FontStyle = FontStyles.Italic;
            }
            #endregion

            #region second row
            var secondRowBlock = new TextBlock
            {
                Text = row2,
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(Constants.TilePadding, Constants.TilePadding * 3 + Constants.TileTitleFontSize + Constants.TileTextFontSize, Constants.TilePadding, Constants.TilePadding),
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Constants.TileForegroundColor),
                FontSize = Constants.TileTextFontSize,
                Width = bitmap.PixelWidth - Constants.TilePadding * 2
            };
            if (row2Bold)
            {
                secondRowBlock.FontWeight = FontWeights.Bold;
            }
            if (row2Italic)
            {
                secondRowBlock.FontStyle = FontStyles.Italic;
            }
            #endregion

            canvas.Children.Add(titleBlock);
            canvas.Children.Add(firstRowBlock);
            canvas.Children.Add(secondRowBlock);

            bitmap.Render(background, null);
            bitmap.Render(canvas, null);
            bitmap.Invalidate();

            using (var imageStream = new IsolatedStorageFileStream(Constants.TilePath, FileMode.Create, Constants.AppStorage))
            {
                ExtendedImage myImage = ImageExtensions.ToImage(bitmap);
                myImage.WriteToStream(imageStream, Constants.TilePath);
                //bitmap.SaveJpeg(imageStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 90);

            }
            return new Uri("isostore:" + Constants.TilePath, UriKind.Absolute);
        }
        */

    }
}
