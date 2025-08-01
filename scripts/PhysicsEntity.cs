using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace scripts.PhysicsEntity
{
    public class PhysicsEntity
    {
        protected Dictionary<string, Texture2D> assets;

        protected string e_type;
        public Vector2 Position;
        public Point Size;
        public Vector2 Velocity = Vector2.Zero;

        private float Gravity = 1200f;  // gravity pixels/sec^2 (adjust)
        private bool isOnGround = false;

        public bool IsOnGround => isOnGround;

        public PhysicsEntity(Dictionary<string, Texture2D> assets, string e_type, Vector2 pos, Point size)
        {
            this.assets = assets;
            this.e_type = e_type;
            this.Position = pos;
            this.Size = size;
        }

        // Update entity with movement input and tile collisions
        public void Update(GameTime gameTime, Vector2 movement, List<Rectangle> physicsRects)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Apply horizontal movement (from input)
            Velocity.X = movement.X;

            // Apply gravity
            Velocity.Y += Gravity * deltaTime;

            Vector2 newPosition = Position + Velocity * deltaTime;

            Rectangle newBounds = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                Size.X,
                Size.Y
            );

            isOnGround = false; // reset onGround before collision checks

            foreach (var rect in physicsRects)
            {
                if (newBounds.Intersects(rect))
                {
                    // Simple collision response: snap to top of tile and stop falling
                    Velocity.Y = 0;
                    newPosition.Y = rect.Top - Size.Y;
                    isOnGround = true;
                    break;
                }
            }

            Position = newPosition;
        }

        public void Jump(float jumpVelocity)
        {
            if (isOnGround)
            {
                Velocity.Y = -jumpVelocity;
                isOnGround = false;
            }
        }

        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Size.X, Size.Y);

        public void Draw(SpriteBatch spriteBatch)
        {
            if (assets.TryGetValue(e_type, out Texture2D texture))
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }
    }
}
