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
    
    public class GameWindow
    {
       
       
        public int HEIGHT;
        public int WIDTH;

        public GameWindow ()
        {
            
        }

        public void InitWindow(GraphicsDeviceManager graphics ,  int width , int height)
        {
            HEIGHT = graphics.PreferredBackBufferHeight = height;
            WIDTH = graphics.PreferredBackBufferWidth = width;
            graphics.ApplyChanges();
        }
    }
}