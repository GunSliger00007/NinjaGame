using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace scripts.TileMap
{
    public class TileMap
    {
        public static readonly Point[] NEIGHBOR_OFFSETS = new Point[]
        {
            new Point(-1, 0),  // left
            new Point(-1, -1), // top-left
            new Point(0, -1),  // up
            new Point(1, -1),  // top-right
            new Point(1, 0),   // right
            new Point(0, 0),   // center (the cell itself)
            new Point(-1, 1),  // bottom-left
            new Point(0, 1),   // down
            new Point(1, 1)    // bottom-right
        };

        public static readonly HashSet<string> PHYSICS_TILES = new HashSet<string> { "grass", "stone" };

        private int tile_size;
        public Dictionary<string, Tile> tilemap;
        public List<Tile> offgridTiles;

        public int TileSize => tile_size;

        public class Tile
        {
            public string Type;
            public int variant;
            public Point pos;

            public Tile(string type, int variant, Point pos)
            {
                this.Type = type;
                this.variant = variant;
                this.pos = pos;
            }
        }

        public TileMap(int tile_size = 16)
        {
            this.tile_size = tile_size;
            tilemap = new Dictionary<string, Tile>();
            offgridTiles = new List<Tile>();

            // Example tiles
            for (int i = 0; i < 10; i++)
            {
                tilemap[$"{3 + i};10"] = new Tile("grass", 0, new Point(3 + i, 10));
                tilemap[$"10;{5 + i}"] = new Tile("stone", 0, new Point(10, 5 + i));
            }
        }

        // Get tiles around a given grid position, including the tile itself
        public List<Tile> TilesAround(Point pos)
        {
            List<Tile> neighbors = new List<Tile>();

            foreach (var offset in NEIGHBOR_OFFSETS)
            {
                Point neighborPos = new Point(pos.X + offset.X, pos.Y + offset.Y);
                string key = $"{neighborPos.X};{neighborPos.Y}";

                if (tilemap.TryGetValue(key, out Tile tile))
                {
                    neighbors.Add(tile);
                }
            }

            return neighbors;
        }

        // Return bounding rectangles of physics tiles around a position
        public List<Rectangle> PhysicsRectsAround(Point pos)
        {
            List<Rectangle> rects = new List<Rectangle>();

            foreach (var offset in NEIGHBOR_OFFSETS)
            {
                Point neighborPos = new Point(pos.X + offset.X, pos.Y + offset.Y);
                string key = $"{neighborPos.X};{neighborPos.Y}";

                if (tilemap.TryGetValue(key, out Tile tile))
                {
                    if (PHYSICS_TILES.Contains(tile.Type))
                    {
                        Rectangle rect = new Rectangle(
                            tile.pos.X * tile_size,
                            tile.pos.Y * tile_size,
                            tile_size,
                            tile_size
                        );
                        rects.Add(rect);
                    }
                }
            }

            return rects;
        }

        public void Render(SpriteBatch spriteBatch, Dictionary<string, List<Texture2D>> textureLists)
        {
            foreach (var tileEntry in tilemap)
            {
                Tile tile = tileEntry.Value;

                if (textureLists.TryGetValue(tile.Type, out List<Texture2D> textures))
                {
                    int variant = MathHelper.Clamp(tile.variant, 0, textures.Count - 1);
                    spriteBatch.Draw(
                        textures[variant],
                        new Vector2(tile.pos.X * tile_size, tile.pos.Y * tile_size),
                        Color.White
                    );
                }
            }

            foreach (var tile in offgridTiles)
            {
                if (textureLists.TryGetValue(tile.Type, out List<Texture2D> textures))
                {
                    int variant = MathHelper.Clamp(tile.variant, 0, textures.Count - 1);
                    spriteBatch.Draw(
                        textures[variant],
                        new Vector2(tile.pos.X, tile.pos.Y), // Absolute position for offgrid
                        Color.White
                    );
                }
            }
        }
    }
}
