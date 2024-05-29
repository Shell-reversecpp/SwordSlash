#region Includes
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace swordSlash
{
    public class DisplayBar
    {
        public int boarder;
        public TextureInit2D bar, barBKG, barFace, barMana, barHealthBl, barManaBl;
        private Vector2 offset;

        public Camera camera;
        public Texture2D barImg, barBKGimage, barFaceImage, barManaImage, barHpBlImage, barManaBlImage;
        private Texture2D pixelTexture;

        public DisplayBar(Vector2 DIMS, int Border, ContentManager contentManager, Camera cam)
        {
            boarder = Border;
            camera = cam;
            offset = Vector2.Zero;

            barImg = contentManager.Load<Texture2D>("game/Misc\\hud_bg");
            barBKGimage = contentManager.Load<Texture2D>("game/Misc\\hp_bar");
            barFaceImage = contentManager.Load<Texture2D>("game/Sprite\\face");
            barManaImage = contentManager.Load<Texture2D>("game/Misc\\mp_bar");
            barHpBlImage = contentManager.Load<Texture2D>("game/Misc\\hp_barBl");
            barManaBlImage = contentManager.Load<Texture2D>("game/Misc\\mp_barBl");

            // Double the dimensions of barImg
            Vector2 doubledDims = new Vector2(barImg.Width, barImg.Height);

            bar = new TextureInit2D(barImg, new Vector2(0, 0), doubledDims);
            barBKG = new TextureInit2D(barBKGimage, new Vector2(83, 18), new Vector2(DIMS.X * 3.6f, DIMS.Y * 2.3f));
            barFace = new TextureInit2D(barFaceImage, new Vector2(10, 11), new Vector2(DIMS.X * 1.5f, DIMS.Y * 4.2f));
            barMana = new TextureInit2D(barManaImage, new Vector2(80, 45), new Vector2(DIMS.X * 2.1f, DIMS.Y * 1.4f));
            barHealthBl = new TextureInit2D(barHpBlImage, new Vector2(86, 20), new Vector2(DIMS.X * 3.4f, DIMS.Y * 2.1f));

            // Создаем пиксельную текстуру
        }

public virtual void Update(float currentHealth, float maxHealth, float currentMana, float maxMana)
{
    // Calculate the health percentage
    float healthPercentage = currentHealth / maxHealth;

    // Calculate the mana percentage
    float manaPercentage = currentMana / maxMana;

    // Use health percentage to determine red color value
    byte healthRed = (byte)(255 * healthPercentage); // Scale red value between 0 and 255
    byte healthGreen = 0;
    byte healthBlue = 0;

    // Use mana percentage to determine blue color value
    byte manaRed = 0;
    byte manaGreen = 0;
    byte manaBlue = (byte)(255 * manaPercentage); // Scale blue value between 0 and 255

    // Update the color of the bars
    Color healthGradientColor = new Color(healthRed, healthGreen, healthBlue);
    Color manaGradientColor = new Color(manaRed, manaGreen, manaBlue);

    // Print health and mana info for debugging
  //  Console.WriteLine($"Current Health: {currentHealth}, Max Health: {maxHealth}, Health Red: {healthRed}");
  //  Console.WriteLine($"Current Mana: {currentMana}, Max Mana: {maxMana}, Mana Blue: {manaBlue}");

    bar.color = healthGradientColor;
    barBKG.color = healthGradientColor; // Background color is the same as health bar color for consistency
    barMana.color = manaGradientColor; // Mana bar color is determined by mana percentage
}


        public virtual void Draw(Vector2 OFFSET, SpriteBatch spriteBatch, Camera camera)
        {
            // Calculate the positions of barBKG and bar relative to the camera
            Vector2 barBKGPosition = OFFSET - camera.GetPosition() + offset;
            Vector2 barPosition = new Vector2(OFFSET.X + boarder, OFFSET.Y + boarder) - camera.GetPosition() + offset;
            Vector2 facePosition = new Vector2(OFFSET.X + boarder, OFFSET.Y + boarder) - camera.GetPosition() + offset;
            Vector2 manaPosition = new Vector2(OFFSET.X + boarder, OFFSET.Y + boarder) - camera.GetPosition() + offset;

            // Draw the background bar
            barFace.Draw(facePosition, Vector2.Zero, Color.White, spriteBatch);
            barBKG.Draw(barBKGPosition, Vector2.Zero, barBKG.color, spriteBatch);

            // Draw the mana bar
            barMana.Draw(manaPosition, Vector2.Zero, barMana.color, spriteBatch);

            // Draw the foreground bar
            bar.Draw(barPosition, Vector2.Zero, Color.Silver, spriteBatch);
        }
    }
}

/*
public virtual void Draw(Vector2 OFFSET, SpriteBatch spriteBatch, Camera camera)
{
    // Вычисляем позиции элементов относительно камеры
    Vector2 barBKGPosition = OFFSET - camera.GetPosition() + offset;
    Vector2 barPosition = new Vector2(OFFSET.X + boarder, OFFSET.Y + boarder) - camera.GetPosition() + offset;
    Vector2 manaPosition = new Vector2(OFFSET.X + boarder, OFFSET.Y + boarder) - camera.GetPosition() + offset;
    Vector2 facePosition = new Vector2(OFFSET.X + boarder, OFFSET.Y + boarder) - camera.GetPosition() + offset;
    barFace.Draw(facePosition, Vector2.Zero, Color.Silver, spriteBatch);
    // Отрисовываем фоновые полоски
    barBKG.Draw(barBKGPosition, Vector2.Zero, color, spriteBatch);
    
    // Здоровье меняет цвет в зависимости от процента потерянного здоровья
    float healthPercentage = Player.GetCurrentHealth() / Player.HealthMax;
    Color healthColor = new Color(1 - healthPercentage, healthPercentage, 0);
    bar.Draw(barPosition, Vector2.Zero, healthColor, spriteBatch);
    
    // Фон маны
    barMana.Draw(manaPosition, Vector2.Zero,Color.Silver, spriteBatch);
    
    // Передаем информацию о здоровье и максимальном здоровье для расчета изменения цвета
    float lostHealth = Player.HealthMax - Player.GetCurrentHealth();
    float healthLostPercentage = lostHealth / Player.HealthMax;
    Color manaColor = new Color(0, 0, 1 - healthLostPercentage, 0.5f); // Пример цвета: синий с прозрачностью
    barMana.Draw(manaPosition, Vector2.Zero, color, spriteBatch);
}
*/
 