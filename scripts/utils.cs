using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace scripts.LoadImages
{
    public static class ImageLoader
    {
        private static readonly string BaseImgPath = Path.Combine("Content", "data", "images");

        public static Texture2D LoadImage(GraphicsDevice graphicsDevice, string relativePath)
        {
            string fullPath = Path.Combine(BaseImgPath, relativePath);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"File '{fullPath}' not found.");
                return null;
            }

            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
                    return texture;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading image '{fullPath}': {e.Message}");
                return null;
            }
        }

        public static List<Texture2D> LoadImages(GraphicsDevice graphicsDevice, string subfolder)
        {
            List<Texture2D> images = new List<Texture2D>();
            string folderPath = Path.Combine(BaseImgPath, subfolder);

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder '{folderPath}' not found.");
                return images;
            }

            string[] supportedExtensions = new[] { ".png", ".jpg", ".bmp" };
            var files = Directory.GetFiles(folderPath);
            Array.Sort(files); // Sort alphabetically

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                if (Array.Exists(supportedExtensions, e => e == ext))
                {
                    string relativePath = Path.Combine(subfolder, Path.GetFileName(file));
                    Texture2D image = LoadImage(graphicsDevice, relativePath);
                    if (image != null)
                    {
                        images.Add(image);
                    }
                }
            }

            return images;
        }
    }
}
