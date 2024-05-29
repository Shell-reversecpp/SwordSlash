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

namespace swordSlash
{
      public class TextureInit2D
    {
        public Vector2 pos, dims;
        public Texture2D mymmodel;
        public Color color; // Add color property

        public TextureInit2D(Texture2D texture, Vector2 POS, Vector2 DIMS)
        {
            mymmodel = texture;
            pos = POS;
            dims = DIMS;
            color = Color.White; // Default color is white
        }

public virtual bool Hover(Vector2 mousePosition, Vector2 OFFSET)
{
    if (mousePosition.X >= (pos.X + OFFSET.X) - dims.X / 2 && mousePosition.X <= (pos.X + OFFSET.X) + dims.X / 2 &&
        (mousePosition.Y >= (pos.Y + OFFSET.Y) - dims.Y / 2 && mousePosition.Y <= (pos.Y + OFFSET.Y) + dims.Y / 2))
    {
        return true;
    }
    return false;
}

        public virtual void Draw(Vector2 OFFSET, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (mymmodel != null)
            {
                spriteBatch.Draw(mymmodel, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y),
                                 null, Color.White, 0f, new Vector2(mymmodel.Bounds.Width / 2, mymmodel.Bounds.Height / 2), SpriteEffects.None, 0);
            }
        }

        public virtual void Draw(Vector2 OFFSET, Vector2 ORIGIN, Color color, SpriteBatch spriteBatch)
        {
            if (mymmodel != null)
            {
                spriteBatch.Draw(mymmodel, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y),
                                 null, color, 0f, ORIGIN, SpriteEffects.None, 0);
            }
        }
    }
}