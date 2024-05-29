#region Includes
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using static swordSlash.Button2D;
#endregion

namespace swordSlash
{
public interface IObserver
{
    void Update(bool isPressed);
}

public interface IObservable
{
    void RegisterObserver(IObserver observer);
    void UnregisterObserver(IObserver observer);
    void NotifyObservers();
}
    public class UI :  IObservable
    {
    public bool gameStarted = false;
    private List<IObserver> observers = new List<IObserver>();


        
        public DisplayBar healthBar;

        public int score;
        public int waveCount;
        public Button2D btn_start;
        public SpriteFont spriteFont;

        public bool waveStarted = true;
        private bool previousWaveStarted = false;

        public Main main;

        private McMouseControl mouse;

        public Vector2 offset;
        private bool drawBackground = true;
        private bool drawButtonSetup = true;
        public GameWindow gameWindow;

        public Texture2D btn_start_image, menu_backgroundImg, menu_buttonSetup_Img;
        public int slayed = 0;

   
        public UI(ContentManager content, McMouseControl mouse ,  Main main , Camera camera)
        {
            gameWindow = new GameWindow();

            this.mouse = mouse;
            this.main = main;

            menu_backgroundImg = content.Load<Texture2D>("game/Misc\\back1");
            menu_buttonSetup_Img = content.Load<Texture2D>("game/Misc\\back2");

            spriteFont = content.Load<SpriteFont>("game/fonts\\gameFont");
            btn_start_image = content.Load<Texture2D>("game/Misc\\button1");

            btn_start = new Button2D(btn_start_image, new Vector2(940, 300), new Vector2(327, 105), "Start", spriteFont, new Vector2(890, 300), this);

            healthBar = new DisplayBar(new Vector2(50, 16), 2,  content, camera);

              RegisterObserver(main);
            
        }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    // Удаление наблюдателя
    public void UnregisterObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Уведомление всех наблюдателей об изменении состояния
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(gameStarted);
        }
    }

    // Метод для изменения состояния и уведомления наблюдателей
   public void ToggleGameStarted(bool isPressed)
{
    gameStarted = isPressed;
    NotifyObservers();
}

public void Update(GameTime gameTime , float currentHealth, float maxHealth , float currentMana , float MaxMana)
{
    mouse.Update();
    btn_start.Update(offset, new Vector2(mouse.newMouse.Position.X, mouse.newMouse.Position.Y), mouse);
    healthBar.Update(currentHealth, maxHealth , currentMana , MaxMana);
    // Добавьте вызов метода Update(bool isPressed) класса Main
    main.Update(gameStarted);
}

public void UpdateOffset(Vector2 cameraPosition) 
{
    offset = cameraPosition;
}
public void Draw(SpriteBatch spriteBatch, Camera camera)
{
     if (gameStarted == true) 
    {
        // Calculate the position of the UI elements relative to the camera
        Vector2 offsetPosition = offset + camera.GetPosition();

        // Draw the background and other elements that should appear only after pressing the "Start" button
        string scoreText = "Slayed - " + score.ToString(); // Преобразуем int в строку
        Vector2 dimScore = spriteFont.MeasureString(scoreText);
        Vector2 scorePosition = new Vector2(1750, 70) + offsetPosition * 0.5f; // Apply offset


        spriteBatch.DrawString(spriteFont, scoreText, scorePosition, Color.Black);

        healthBar.Draw(new Vector2(10, 10) + offsetPosition, spriteBatch, camera); // Apply offset and camera

        if (waveStarted != previousWaveStarted)
        {
            waveCount = 1;
            previousWaveStarted = waveStarted;
        }
    

if (waveStarted)
{
    Vector2 waveCountPos = new Vector2(1710, 830);
    float scale = 3.2f; 
    spriteBatch.DrawString(spriteFont, waveCount.ToString(), waveCountPos, Color.Red, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
}
}
    else 
    {
        if (drawBackground && menu_backgroundImg != null)
        {
            spriteBatch.Draw(menu_backgroundImg, Vector2.Zero, Color.White);
        }

    
Vector2 swordSlashPosition = new Vector2(850, 50); // Установите позицию надписи "Sword Slash"
spriteBatch.DrawString(spriteFont, "Sword Slash", swordSlashPosition, Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

        btn_start.Draw(new Vector2(0, 0), spriteBatch, spriteFont);

        if (drawButtonSetup && menu_buttonSetup_Img != null)
        {
            float scale = 0.5f;
            spriteBatch.Draw(menu_buttonSetup_Img, new Vector2(460, 50), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
        
    }

}

    }
}
    

