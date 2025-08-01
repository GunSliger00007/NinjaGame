using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using scripts.LoadImages;

namespace MyGame
{
    public class AssetManager
    {
        private GraphicsDevice graphicsDevice;

        private Dictionary<string, Texture2D> singleTextures;
        private Dictionary<string, List<Texture2D>> textureLists;

        public AssetManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            singleTextures = new Dictionary<string, Texture2D>();
            textureLists = new Dictionary<string, List<Texture2D>>();

            LoadAssets();
        }

        private void LoadAssets()
        {
            // Load single textures
            singleTextures["player"] = ImageLoader.LoadImage(graphicsDevice, "entities/player.png");

            // Load texture lists
            textureLists["decor"] = ImageLoader.LoadImages(graphicsDevice, "tiles/decor");
            textureLists["grass"] = ImageLoader.LoadImages(graphicsDevice, "tiles/grass");
            textureLists["large_decor"] = ImageLoader.LoadImages(graphicsDevice, "tiles/large_decor");
            textureLists["stone"] = ImageLoader.LoadImages(graphicsDevice, "tiles/stone");

            Console.WriteLine("Loaded single textures:");
            foreach (var key in singleTextures.Keys)
                Console.WriteLine("- " + key);

            Console.WriteLine("Loaded texture lists:");
            foreach (var key in textureLists.Keys)
                Console.WriteLine($"- {key} ({textureLists[key].Count} textures)");
        }

        // Expose single textures dictionary
        public Dictionary<string, Texture2D> SingleTextures => singleTextures;

        // Expose texture lists dictionary
        public Dictionary<string, List<Texture2D>> TextureLists => textureLists;

        public Texture2D GetTexture(string key)
        {
            if (singleTextures.TryGetValue(key, out var texture))
                return texture;
            throw new Exception($"Texture '{key}' not found.");
        }

        public List<Texture2D> GetTextureList(string key)
        {
            if (textureLists.TryGetValue(key, out var list))
                return list;
            throw new Exception($"Texture list '{key}' not found.");
        }
    }
}
