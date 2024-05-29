#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
#endregion

namespace swordSlash {
    public class ParallaxBackground {
        Texture2D texture;
        Vector2[] positions;
        int speed;


        int bgHeight;
        int bgWidth;

         private void WrapTextureToLeft(int index){

            int prevTexture = index - 1;
            if (prevTexture < 0)
            prevTexture = positions.Length - 1;
            positions[index].X = positions[prevTexture].X + texture.Width;
            
            }

            private void WrapTextureToRight(int index){

           int nextTexture = index + 1;
           if (nextTexture == positions.Length)
               nextTexture = 0;
               positions[index].X = positions[nextTexture].X - texture.Width + 20;
            }

       public void Initialize(ContentManager content, string texturePath, int screenWidth, int screenHeight, int speed)
        {
            texture = content.Load<Texture2D>(texturePath);
            this.speed = speed;

            bgHeight = screenHeight;
            bgWidth = screenWidth;

            int numOfTiles = (int)(Math.Ceiling(screenWidth + 1950 / (float)texture.Width) + 1);
            positions = new Vector2[numOfTiles];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }
        
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (speed <= 0)
                {
                    if (positions[i].X <= -texture.Width)
                    {
                        WrapTextureToLeft(i);
                    }
                }
                else
                {
                    if (positions[i].X >= bgWidth)
                    {
                        WrapTextureToRight(i);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var position in positions)
            {
                Rectangle rectBg = new Rectangle((int)position.X, (int)position.Y, texture.Width, bgHeight);
                spriteBatch.Draw(texture, rectBg, Color.White);
            }
        }
    }   
}