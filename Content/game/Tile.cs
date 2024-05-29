using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace swordSlash
{
    public class Tiles : CollisionBox
    {
        protected Texture2D texture;
        protected Vector2 position;

        public Tiles(int width, int height, Texture2D texture, Vector2 position) : base(width, height)
        {
            this.texture = texture;
            this.position = position;
            this.collisionWidth = texture.Width; // Установим ширину коллизии равной ширине текстуры
            this.collisionHeight = texture.Height; // Установим высоту коллизии равной высоте текстуры
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            base.Initialize(graphicsDevice, null);
            // Нет необходимости в инициализации текстуры здесь
            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            // Отрисовка текстуры и обводки коллизий
            spriteBatch.Draw(texture, position, Color.White);
            base.Draw(spriteBatch, spriteEffects);
        }

        public bool CheckCollisionWithObjects(Rectangle collisionRect, Player player, List<Enemy> enemies)
        {
            bool collisionDetected = false;
            if (collisionRect.Intersects(player.GetCurrentCollision()))
            {
                player.SetPosition(player.GetPreviousPosition());
                collisionDetected = true;
            }

            foreach (Enemy enemy in enemies)
            {
                if (collisionRect.Intersects(enemy.GetCurrentCollision()))
                {
                    enemy.SetPosition(enemy.GetPreviousPosition());
                    collisionDetected = true;
                }
            }
            return collisionDetected;
        }

        public Rectangle GetCurrentCollision()
        {
            // Возвращает текущий прямоугольник коллизии тайла
            return collisionRectangles.Count > 0 ? collisionRectangles[0] : Rectangle.Empty;
        }

public void UpdateCollision()
{
    // Обновляем прямоугольник коллизии в соответствии с текущим положением тайла
    collisionRectangles.Clear();
    Rectangle collisionRect = new Rectangle((int)position.X, (int)position.Y, collisionWidth, collisionHeight);
    collisionRectangles.Add(collisionRect);
}

        public override void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
             UpdateCollision();
        }
    }
}