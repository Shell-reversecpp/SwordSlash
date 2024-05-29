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
    public class Button2D : TextureInit2D
    {
        public string text;
        public Color HoverColor;
        public bool isPressed;

        public Main main;

        public Vector2 textPosition;
        private UI ui; // Ссылка на класс UI

        // Конструктор класса Button2D
        public Button2D(Texture2D texture, Vector2 POS, Vector2 DIMS, string TEXT, SpriteFont font, Vector2 textPosition, UI ui) :
            base(texture, POS, DIMS)
        {
            text = TEXT;
            HoverColor = new Color(200, 230, 255);
            isPressed = false;
            this.textPosition = textPosition;
            this.ui = ui; // Присваиваем ссылку на UI
        }

        // Метод отрисовки кнопки
        public override void Draw(Vector2 OFFSET, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            base.Draw(OFFSET, spriteBatch, spriteFont);
            spriteBatch.DrawString(spriteFont, text, textPosition, Color.Black);
        }

        // Метод для обработки нажатия кнопки
        public bool OnClick()
        {
            isPressed = !isPressed;
            ui.ToggleGameStarted(isPressed); // Уведомляем класс UI о нажатии кнопки
            Console.WriteLine("Button clicked!"); // Отображаем сообщение в консоли
            return isPressed;
        }

        // Метод для обновления состояния кнопки
        public void Update(Vector2 OFFSET, Vector2 mousePosition, McMouseControl mouse)
        {
            if (Hover(mousePosition, OFFSET) && mouse.LeftClick())
            {
                if (!isPressed) // Проверяем, была ли кнопка уже нажата
                {
                    OnClick(); // Вызываем метод обработки нажатия кнопки
                    ui.RegisterObserver(main); // Регистрируем Main в качестве наблюдателя
                }
            }
        }
    }
}