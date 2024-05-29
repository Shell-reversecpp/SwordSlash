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

namespace  swordSlash;
 

public class Base2D : CollisionBox
{
    protected Animation base_animation;
     public Base2D(Vector2 position, int width, int height) : base( width, height)
    {
        base_position = position;
        //this.Initialize(position.X,position.Y);
        collisionWidth = width;
        collisionHeight = height;
        //rectangle = new Rectangle(x, y, width, height);
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Animation animation)
    {
        pixelTexture = CreatePixelTexture(graphicsDevice);
        base_animation = animation;
        base.Initialize(graphicsDevice, animation);

    }
     public override void UpdateCollision()
    {
    }

    public override void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
    {
        base.Update(graphicsDevice, gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
    {
        base_animation.Draw(spriteBatch, spriteEffects);
        base.Draw(spriteBatch, spriteEffects);
    
    }
}