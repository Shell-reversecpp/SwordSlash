using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace swordSlash
{
    public class CollisionBox
    {
        protected List<Rectangle> collisionRectangles;
        protected Texture2D pixelTexture;
       
        protected Animation animation;
        protected int collisionWidth;
        protected int collisionHeight;
        protected Vector2 base_position;
        protected Color collisionColor = Color.Red;


        public CollisionBox(int width, int height)
        {
            collisionRectangles = new List<Rectangle>();
            collisionWidth = width;
            collisionHeight = height;
        }

        public virtual void Initialize(GraphicsDevice graphicsDevice, Animation animation)
        {
            pixelTexture = CreatePixelTexture(graphicsDevice);
          
        }

public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
{
    foreach (Rectangle rect in collisionRectangles)
    {
        // Отрисовка верхней и нижней границ
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), collisionColor);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom - 1, rect.Width, 1), collisionColor);
        // Отрисовка левой и правой границ
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), collisionColor);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right - 1, rect.Top, 1, rect.Height), collisionColor);
    }
    
}

        public virtual void UpdateCollision()
        {
            // Обновляем список прямоугольников коллизии в соответствии с текущим положением объекта
            collisionRectangles.Clear();
            // Создаем прямоугольник коллизии и добавляем его в список
            Rectangle collisionRect = new Rectangle((int)base_position.X, (int)base_position.Y, collisionWidth, collisionHeight);
            collisionRectangles.Add(collisionRect);
        }

        public virtual bool CheckCollision(Rectangle other)
        {
            foreach (Rectangle rect in collisionRectangles)
            {
                if (rect.Intersects(other))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            // Логика обновления состояния объекта
        }

        public Texture2D CreatePixelTexture(GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });
            return texture;
        }
    }
}

/*
public virtual bool CheckCollision(Rectangle other, Rectangle main)
{
    Rectangle rectA = CreateCollisionRectangle(main);
    Rectangle rectB = other;

    return rectA.Intersects(rectB);
}

protected virtual Rectangle CreateCollisionRectangle(Rectangle main)
{
    return new Rectangle((int)(main.X - collisionWidth / 2), (int)(main.Y - collisionHeight / 2), collisionWidth, collisionHeight);
}
*/