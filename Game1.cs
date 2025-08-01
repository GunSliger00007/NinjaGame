using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using scripts.PhysicsEntity;
using scripts.LoadImages;
using scripts.TileMap;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private PhysicsEntity _player;
        private float _playerSpeed = 1200f;

        private AssetManager assetManager;
        private TileMap tileMap;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            assetManager = new AssetManager(GraphicsDevice);

            tileMap = new TileMap();

            Texture2D playerTexture = assetManager.GetTexture("player");

            _player = new PhysicsEntity(
                assetManager.SingleTextures,
                "player",
                new Vector2(50, 50),
                new Point(playerTexture.Width, playerTexture.Height)
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                movement.X += _playerSpeed * delta;
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                movement.X -= _playerSpeed * delta;

            // Jump on Space key
            if (state.IsKeyDown(Keys.Space))
            {
                _player.Jump(600f); // jump velocity, tune as needed
            }

            // Get player tile position for collision detection
            Point playerTilePos = new Point(
                (int)(_player.Position.X / tileMap.TileSize),
                (int)(_player.Position.Y / tileMap.TileSize)
            );

            List<Rectangle> physicsRects = tileMap.PhysicsRectsAround(playerTilePos);

            _player.Update(gameTime, movement, physicsRects);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            tileMap.Render(_spriteBatch, assetManager.TextureLists);

            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
