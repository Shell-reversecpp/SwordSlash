#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using static swordSlash.Button2D;
#endregion


namespace swordSlash;

public class Main : Game , IObserver
{
    private GraphicsDeviceManager _graphics;
    //private Texture2D rectangleTexture;
//List<Enemy> enemies;
  List<Tiles> tilesList;
    public int score;
    public bool paused = true;

    private Vector2 cameraPosition;

    private McMouseControl mouseControl;
  //  public bool gameStarted = false;
    public UI ui;
    public GameWindow gameWindow;
float enemySpawnTimer = 0f;
float enemySpawnInterval = 10f; 
    private Animation showAnimation;

    private Camera camera;
    private Random random;

   // private Animation showAnimation;
    private SpriteBatch _spriteBatch;
// A random number generator
    
    private InputHandler inputHandler;

    private float attackAnimationTimer; // Добавляем таймер для анимации атаки
    private float attackAnimationTime = 0.5f; 

    

  //  private Vector2 Position;
    
    private  Player player;
 //  private  Enemy  enemy;
 private List<Enemy> enemies;

 public bool isWaveStarted = true;
    
    Texture2D dojo , Salt1 , Salt2 , plant1 , plant2, waveCount , grass1 , grass2 , bat;
     ParallaxBackground bgLayer0;
    ParallaxBackground bgLayer1;
    ParallaxBackground bgLayer2;
    ParallaxBackground bgLayer3;
    ParallaxBackground bgLayer4;
    ParallaxBackground bgLayer5;
    ParallaxBackground bgLayer6;

    GraphicsDevice graphicsDevice;

public Main()
{ 
   _graphics = new GraphicsDeviceManager(this);
     graphicsDevice = _graphics.GraphicsDevice;
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
}

protected override void Initialize()
{
  
    inputHandler = new InputHandler(); 
    mouseControl = new McMouseControl();
    tilesList = new List<Tiles>(); 
    enemies = new List<Enemy>();
    random = new Random();
    Content = new ContentManager(Services, "Content");
    camera = new Camera(GraphicsDevice.Viewport, 200);

    Window.Title = "Sword Slash";
    Window.AllowUserResizing = true;
    Window.Position = new Point(100, 100);
    Window.IsBorderless = false;

    gameWindow = new GameWindow();
    gameWindow.InitWindow(_graphics, 1920, 1080); // Initialize window size

    ui = new UI(Content, mouseControl , this , camera); 

    score = 0;

 //  enemy = new Enemy(new Vector2(100, 400), 64, 64);
    player = new Player(inputHandler, new Vector2(270, 900), 64, 64);
   //  enemies = new List<Enemy>();

   

    bgLayer0 = new ParallaxBackground();
    bgLayer1 = new ParallaxBackground();
    bgLayer2 = new ParallaxBackground();
    bgLayer3 = new ParallaxBackground();
    bgLayer4 = new ParallaxBackground();
    bgLayer5 = new ParallaxBackground();
    bgLayer6 = new ParallaxBackground();

    base.Initialize();
}


protected override void Update(GameTime gameTime)
{
    mouseControl.Update();
    ui.Update(gameTime ,  player.CurrentHealth , player.HealthMax,  player.CurrentMana, player.ManaMax);
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && paused == true)
    {
        paused = true;
    }

    if (ui.gameStarted == true) {
        camera.Follow(player, gameWindow.WIDTH);
        //ui.Update(gameTime, player.CurrentHealth , player.HealthMax, player.CurrentMana, player.ManaMax);

        // Обновите смещение для DisplayBar, передав текущую позицию камеры
        ui.UpdateOffset(camera.GetPosition());
    }

    enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
   if (player.currentState == PlayerState.Death)
    {
        // Сбросить позицию и другие параметры игрока
        ResetPlayer();
        
        // Перезапустить игру
        RestartGame();
    }
 
    if (enemySpawnTimer >= enemySpawnInterval)
    {
        SpawnEnemies(); 
        enemySpawnTimer = 0f; 
    }
    if (paused == false)
    {
        bgLayer1.Update(gameTime);
        bgLayer2.Update(gameTime);
        bgLayer3.Update(gameTime);
        bgLayer4.Update(gameTime);
        bgLayer5.Update(gameTime);
        bgLayer6.Update(gameTime);
        
        // Проверяем коллизии игрока с тайлами
        bool PlayerHasCollision = CollisionManager.CheckPlayerTileCollisions(player, tilesList);

        // Обновляем игрока, передавая информацию о коллизиях
        player.Update(GraphicsDevice, gameTime, PlayerHasCollision);

        // Проверяем коллизии врагов с тайлами
        CollisionManager.CheckEnemyTileCollisions(enemies, tilesList);


        // Проверяем коллизии игрока с врагами
        CollisionManager.CheckPlayerEnemyCollisions(player, enemies);

        // Проверяем атаку игрока на коллизии с врагами
        if(player.isAttacking == true)
        {
            CollisionManager.CheckPlayerAttackCollision(player, enemies);
        }



    
    
        // Обновляем тайлы
        foreach (Tiles tile in tilesList)
        {
            tile.UpdateCollision();
        }
        
        // Обновляем врагов


for (int i = enemies.Count - 1; i >= 0; i--)
{
    Enemy enemy = enemies[i];
    enemy.Update(graphicsDevice, gameTime);

    if(enemy.isAttacking == true)
    {
        CollisionManager.CheckEnemyAttackCollision(player, enemies);
    }

    if(isWaveStarted == true ){
        showAnimation.Update(gameTime);
        enemy.followPlayerX(player.GetPosition());
    }

    // Проверяем, завершилась ли анимация смерти и только после этого удаляем врага из списка
    if (enemy.Health <= 0 && enemy.isDeathAnimationCompleted && enemy.deathAnimationTimer <= 0)
    {
        ui.score +=1;
        enemies.RemoveAt(i);
    }
}

        // Обновляем ввод
        inputHandler.Update();
    }

    base.Update(gameTime);
}
private void ResetPlayer()
{
    // Сбросить позицию игрока на начальные значения
    player.SetPosition(new Vector2(270, 900)); 
    player.CurrentHealth = 1000;
    
}

private void RestartGame()
{
    
    enemies.Clear();
    
   
}

public void Update(bool isPressed)
{
    paused = !isPressed; // Обновляем состояние paused на основе isPressed
    
}


public void SpawnEnemies(){
     for (int i = 0; i < 8; i++)
{
    //int x = random.Next(200, GraphicsDevice.Viewport.Width - 100);
    int x = random.Next(2300, 3500);
    int y = 900;

    Enemy newEnemy = new Enemy(new Vector2(x, y), 64, 64);
    
    // Инициализация анимаций для каждого врага
    Texture2D idleEnemyTexture = Content.Load<Texture2D>("game/Sprite/Idle\\enemyIdle");
    Animation idleEnemyAnimation = new Animation();
    idleEnemyAnimation.Initialize(idleEnemyTexture, Vector2.Zero, 140, 70, 8, 60, Color.White, 1f, true);
    newEnemy.AddAnimation(EnemyState.Idle, idleEnemyAnimation);

    Texture2D baseAttackEnemyTexture = Content.Load<Texture2D>("game/Sprite/Attack\\attackEnemy");
    Animation baseAttackEnemyAnimation = new Animation();
    baseAttackEnemyAnimation.Initialize(baseAttackEnemyTexture, Vector2.Zero, 140, 93, 8, 60, Color.White, 1f, true);
    newEnemy.AddAnimation(EnemyState.Attack1, baseAttackEnemyAnimation);

    Texture2D enemyGetHurtTexture = Content.Load<Texture2D>("game/Sprite/Hit\\enemyHit");
    Animation enemyGetHurtAnimation = new Animation();
    enemyGetHurtAnimation.Initialize(enemyGetHurtTexture, Vector2.Zero, 140, 70, 3, 60, Color.White, 1f, true);
    newEnemy.AddAnimation(EnemyState.TakeHit, enemyGetHurtAnimation);

    Texture2D runEnemyTexture = Content.Load<Texture2D>("game/Sprite/Walk\\WalkEnemy");
    Animation runEnemyAnimation = new Animation();
    runEnemyAnimation.Initialize(runEnemyTexture, Vector2.Zero, 140, 70, 8, 60, Color.White, 1f, true);
    newEnemy.AddAnimation(EnemyState.Run, runEnemyAnimation);

    Texture2D deathEnemyTexture = Content.Load<Texture2D>("game/Sprite/Death\\enemyDeath");
    Animation deathEnemyAnimation = new Animation();
    deathEnemyAnimation.Initialize(deathEnemyTexture, Vector2.Zero, 170, 70, 10, 60, Color.White, 1f, true);
    newEnemy.AddAnimation(EnemyState.Death, deathEnemyAnimation);

    // Инициализация врага и добавление его в список
    newEnemy.Initialize(GraphicsDevice, idleEnemyAnimation); // Используйте idleEnemyAnimation в качестве стартовой анимации
    enemies.Add(newEnemy);
}
}
protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);

/////////////////////////////////////////////////////////////////////////
    bgLayer0.Initialize(Content, "game/Sprite/Layers\\2", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 0);

    bgLayer1.Initialize(Content, "game/Sprite/Layers\\3", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 60);

    bgLayer2.Initialize(Content, "game/Sprite/Layers\\4", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 40);

    bgLayer3.Initialize(Content, "game/Sprite/Layers\\5", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 0);

    bgLayer4.Initialize(Content, "game/Sprite/Layers\\6", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 0);

    bgLayer5.Initialize(Content, "game/Sprite/Layers\\7", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 0);

    bgLayer6.Initialize(Content, "game/Sprite/Layers\\8", GraphicsDevice.Viewport.Width,
    GraphicsDevice.Viewport.Height, 0);
///////////////////////////////////////////////////////////////////////////
    dojo = Content.Load<Texture2D>("game/Sprite\\dojo");
    Salt1 = Content.Load<Texture2D>("game/Sprite\\Salt");
    Salt2 = Content.Load<Texture2D>("game/Sprite\\Salt");
    plant1 = Content.Load<Texture2D>("game/Sprite\\pla1");
    plant2 = Content.Load<Texture2D>("game/Sprite\\plant2");
    grass1 = Content.Load<Texture2D>("game/Sprite\\grass1");
    grass2 = Content.Load<Texture2D>("game/Sprite\\grass2");
    bat = Content.Load<Texture2D>("game/Sprite\\bat");
//////////////////////////////////////////////////////////////////////////

    waveCount = Content.Load<Texture2D>("game/Sprite\\waveCount");
    
//////////////////////////////////////////////////////////////////////////
  

    Texture2D idleTexture = Content.Load<Texture2D>("game/Sprite\\Idle");
    Animation idleAnimation = new Animation();
    idleAnimation.Initialize(idleTexture, Vector2.Zero, 200, 200, 8, 30, Color.White, 1f, true);

    player.AddAnimation(PlayerState.Idle, idleAnimation);
    

    Texture2D runTexture = Content.Load<Texture2D>("game/Sprite\\Run1");
    Animation runAnimation = new Animation();
    runAnimation.Initialize(runTexture, Vector2.Zero, 200, 200, 8, 30, Color.White, 1f, true);

    player.AddAnimation(PlayerState.Run, runAnimation);


    Texture2D attackTexture = Content.Load<Texture2D>("game/Sprite\\Attack1");
    Animation attackAnimation = new Animation();
    attackAnimation.Initialize(attackTexture, Vector2.Zero, 200, 200, 6, 60, Color.White, 1f, true);

    player.AddAnimation(PlayerState.Attack1, attackAnimation);

    Texture2D deathTexture = Content.Load<Texture2D>("game/Sprite\\Death");
    Animation deathAnimation = new Animation();
    deathAnimation.Initialize(deathTexture, Vector2.Zero, 200, 200, 6 , 60 , Color.White , 1f, true);

    player.AddAnimation(PlayerState.Death, deathAnimation);


    Texture2D attack2Texture = Content.Load<Texture2D>("game/Sprite\\Attack2");
    Animation attack2Animation = new Animation();
    attack2Animation.Initialize(attack2Texture, Vector2.Zero, 200, 200, 6, 60 , Color.White, 1f, true);

    player.AddAnimation(PlayerState.Attack2, attack2Animation);

    Texture2D jumpTexture = Content.Load<Texture2D>("game/Sprite\\Jump");
    Animation jumpAnimation = new Animation();
    jumpAnimation.Initialize(jumpTexture, Vector2.Zero, 200, 200 , 4 , 60, Color.White, 1f, true);

    player.AddAnimation(PlayerState.Jump, jumpAnimation);

    Texture2D takeHitTexture = Content.Load<Texture2D>("game/Sprite\\Take Hit - white silhouette");
    Animation takeHitAnimation = new Animation();
    takeHitAnimation.Initialize(takeHitTexture, Vector2.Zero, 200,200,  4 , 120 , Color.White, 1f, true);

    player.AddAnimation(PlayerState.TakeHit, takeHitAnimation);

    Texture2D fallTexture = Content.Load<Texture2D>("game/Sprite\\Fall");
    Animation fallAnimation = new Animation();
    fallAnimation.Initialize(fallTexture, Vector2.Zero, 200, 200 , 4 , 60, Color.White, 1f, true);

    player.AddAnimation(PlayerState.Fall, fallAnimation);

    Texture2D dashTexture = Content.Load<Texture2D>("game/Sprite\\dash");
    Animation dashAnimation = new Animation();
    dashAnimation.Initialize(dashTexture, Vector2.Zero, 213, 200 , 9 , 60, Color.White, 0.23f, true);

    player.AddAnimation(PlayerState.Dash, dashAnimation);



/////////////////////////////////////////////////////////////////////////////////////
Texture2D showTexture = Content.Load<Texture2D>("game/Sprite\\smoke");    
showAnimation = new Animation();
//showAnimation.Initialize(showTexture, new Vector2(3000, 500), 252 , 61, 30, 60, Color.White, 10f, true);
showAnimation.Initialize(showTexture, new Vector2(3000, 590), 101, 62, 30, 60, Color.White, 10f, true);

if( isWaveStarted == true)
{
    SpawnEnemies();
}
/////////////////////////////////////////////////////////////////////////////////////
//enemies = new List<Enemy>();
List<(Texture2D texture, Vector2 position)> tileInfo = new List<(Texture2D texture, Vector2 position)>()
{
    (Content.Load<Texture2D>("game/Sprite\\tilegr"), new Vector2(0, 932)),
    (Content.Load<Texture2D>("game/Sprite\\wallLeft"), new Vector2(190, 690)),
    (Content.Load<Texture2D>("game/Sprite\\wallRight"), new Vector2(3610, 690)),
    // Добавьте любые другие позиции плиток и соответствующие текстуры, которые вам нужны
};

int numberOfTilegr = 17; // Например, сгенерировать 10 плиток tilegr
int numberOfWallLeft = 4; 
int numberOfWallRIght = 4;

// Проходим по каждому кортежу в списке и создаем плитки
foreach (var tile in tileInfo)
{
    // Создаем новую плитку
    Tiles newTile = new Tiles(tile.texture.Width, tile.texture.Height, tile.texture, tile.position);
    newTile.Initialize(GraphicsDevice, null);

    // Добавляем новую плитку в список
    tilesList.Add(newTile);

    // Если текущая текстура является текстурой tilegr и количество сгенерированных плиток tilegr меньше заданного числа
    if (tile.texture == Content.Load<Texture2D>("game/Sprite\\tilegr"))
    {
        for (int j = 1; j <= numberOfTilegr; j++)
        {
            // Генерируем новую плитку tilegr со смещением
            Vector2 newTilePosition = new Vector2(tile.position.X + 207 * j, tile.position.Y);
            Tiles newTileGr = new Tiles(tile.texture.Width, tile.texture.Height, tile.texture, newTilePosition);
            newTileGr.Initialize(GraphicsDevice, null);
            tilesList.Add(newTileGr);
        }
    }
    if (tile.texture == Content.Load<Texture2D>("game/Sprite\\wallLeft"))
    {
       // Например, сгенерировать 10 плиток wallLeft
        for (int j = 1; j <= numberOfWallLeft; j++)
        {
            // Генерируем новую плитку wallLeft со смещением по оси Y
            Vector2 newTilePosition = new Vector2(tile.position.X, tile.position.Y - 242 * j);
            Tiles newWallLeft = new Tiles(tile.texture.Width, tile.texture.Height, tile.texture, newTilePosition);
            newWallLeft.Initialize(GraphicsDevice, null);
            tilesList.Add(newWallLeft);
        }
    }
    if (tile.texture == Content.Load<Texture2D>("game/Sprite\\wallRight"))
    {
       // Например, сгенерировать 10 плиток wallLeft
        for (int j = 1; j <= numberOfWallRIght; j++)
        {
            // Генерируем новую плитку wallLeft со смещением по оси Y
            Vector2 newTilePosition = new Vector2(tile.position.X, tile.position.Y - 242 * j);
            Tiles newWallLeft = new Tiles(tile.texture.Width, tile.texture.Height, tile.texture, newTilePosition);
            newWallLeft.Initialize(GraphicsDevice, null);
            tilesList.Add(newWallLeft);
        }
    }
}
////////////////////////////////////////////////////////////////////////////

// Создаем определенное количество врагов и добавляем их в список


player.Initialize(GraphicsDevice, idleAnimation);
/////////////////////////////////////////////////////////////////////////////////////

}

protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.WhiteSmoke);

    _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformMatrix: camera.TransformMatrix);

    SpriteEffects _spriteEffects = SpriteEffects.None;

    // Рассчитываем смещение для фона
   // Vector2 backgroundOffset = new Vector2(cameraPosition.X * 0.2f, cameraPosition.Y * 0.2f);

    // Отрисовка фона с учетом смещения
    
    
    bgLayer0.Draw(_spriteBatch);
    bgLayer1.Draw(_spriteBatch);
    bgLayer2.Draw(_spriteBatch);
    bgLayer3.Draw(_spriteBatch);
    bgLayer4.Draw(_spriteBatch);
    bgLayer5.Draw(_spriteBatch);
    bgLayer6.Draw(_spriteBatch);

    Vector2 dojoPosition = new Vector2(500, 650);
    Vector2 dojoScale = new Vector2(1.8f); 


    showAnimation.Draw(_spriteBatch,_spriteEffects);
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    _spriteBatch.Draw(plant1, new Vector2(700,860),  null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(plant2, new Vector2(1240,845), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);

    _spriteBatch.Draw(grass1, new Vector2(2223,630), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(grass2, new Vector2(2540,630), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(grass1, new Vector2(2856,630), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(grass2, new Vector2(3170,630), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(bat, new Vector2(3394,730), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(bat, new Vector2(2120,730), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

    _spriteBatch.Draw(waveCount, new Vector2(1640,750), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


    _spriteBatch.Draw(dojo, dojoPosition, null, Color.White, 0f, Vector2.Zero, dojoScale, SpriteEffects.None, 0f);

    _spriteBatch.Draw(Salt1, new Vector2(660,845),  null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
    _spriteBatch.Draw(Salt2, new Vector2(1400,845), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    player.Draw(_spriteBatch, _spriteEffects);

    // Отрисовка тайлов и интерфейса
    foreach (Tiles tile in tilesList)
    {
        tile.Draw(_spriteBatch, SpriteEffects.None); // Поdзиция тайла уже должна быть с учетом камеры
    }
    foreach (Enemy enemy in enemies)
    {
        enemy.Draw(_spriteBatch, _spriteEffects);
    }

    ui.Draw(_spriteBatch, camera);

    _spriteBatch.End();
}


}

/*
private void DrawRectangle(Rectangle rect, Color color)
{
    // Вертикальные линии
    _spriteBatch.Draw(rectangleTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), color); // Левая
    _spriteBatch.Draw(rectangleTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), color); // Правая

    // Горизонтальные линии
   _spriteBatch.Draw(rectangleTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), color); // Верхняя
  _spriteBatch.Draw(rectangleTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), color); // Нижняя
}



private void CheckPlayerAttackCollision()
{
    if (player.isPlayerAttackingThisFrame && !player.isAttacking)
    {
        int dir = 1;
        if (player.GetCurrentState() == PlayerState.Attack1)
        {
            Rectangle playerAttack1Collision = player.CreateAttack1Collision(player.isFacingRight);
            if (playerAttack1Collision.Intersects(enemy.GetCurrentCollision()))
            {
                Console.WriteLine("Player attacked enemy!");
                enemy.SetCurrentState(EnemyState.TakeHit);
                enemy.isDamageTaken = true; // Устанавливаем флаг получения урона
                if (player.isFacingRight)
                {
                    Vector2 previousEnemyPosition = new Vector2(enemy.GetPosition().X + dir, enemy.GetPosition().Y);
                    enemy.SetPosition(previousEnemyPosition);
                    enemy.Health -= 10;
                }
                else
                {
                    Vector2 previousEnemyPosition = new Vector2(enemy.GetPosition().X - dir, enemy.GetPosition().Y);
                    enemy.SetPosition(previousEnemyPosition);
                    enemy.Health -= 10;
                }
                // Сброс флага атаки
                player.isAttacking = true;
            }
        }
        else if (player.GetCurrentState() == PlayerState.Attack2)
        {
            Rectangle playerAttack2Collision = player.CreateAttack2Collision(player.isFacingRight);
            if (playerAttack2Collision.Intersects(enemy.GetCurrentCollision()))
            {
                Console.WriteLine("Player attacked enemy!");
                enemy.SetCurrentState(EnemyState.TakeHit);
                enemy.isDamageTaken = true; // Устанавливаем флаг получения урона
                if (player.isFacingRight)
                {
                    Vector2 previousEnemyPosition = new Vector2(enemy.GetPosition().X + dir, enemy.GetPosition().Y);
                    enemy.SetPosition(previousEnemyPosition);
                    enemy.Health -= 10;
                }
                else
                {
                    Vector2 previousEnemyPosition = new Vector2(enemy.GetPosition().X - dir, enemy.GetPosition().Y);
                    enemy.SetPosition(previousEnemyPosition);
                    enemy.Health -= 10;
                }
              
                player.isAttacking = true;
            }
        }


    
        player.isPlayerAttackingThisFrame = false;
    }
}
*/

/*
private Texture2D CreateRectangleTexture(GraphicsDevice graphicsDevice, int width, int height, Color color)
{
    Texture2D texture = new Texture2D(graphicsDevice, width, height);

    Color[] data = new Color[width * height];
    for (int i = 0; i < data.Length; ++i)
    {
        data[i] = color; // Установите цвет, переданный в качестве параметра
    }
    
    texture.SetData(data);
    return texture;
}

*/
     /*

    foreach (Enemy newEnemy in enemies)
    {
       // Texture2D idleEnemyTexture = Content.Load<Texture2D>("game/Sprite/Idle/enemyIdle");
        Animation newEnemyIdleAnimation = new Animation();
        newEnemyIdleAnimation.Initialize(idleEnemyTexture, Vector2.Zero, 140, 70, 8, 60, Color.White, 1f, true);

        newEnemy.Initialize(GraphicsDevice, newEnemyIdleAnimation); // Инициализация с уникальной анимацией
        newEnemy.AddAnimation(EnemyState.Idle, newEnemyIdleAnimation);
        newEnemy.AddAnimation(EnemyState.Attack1, baseAttackEnemyAnimation);
        newEnemy.AddAnimation(EnemyState.TakeHit, enemyGetHurtAnimation);
        newEnemy.AddAnimation(EnemyState.Run, runEnemyAnimation);
        newEnemy.AddAnimation(EnemyState.Death, deathEnemyAnimation);
    }

enemy.Initialize(GraphicsDevice, idleEnemyAnimation);
*/
/*
private void RemoveDeadEnemies()
{
    for (int i = enemies.Count - 1; i >= 0; i--)
    {
        if (enemies[i].Health <= 0)
        {
            // Удаляем врага из списка
            enemies.RemoveAt(i);
        }
    }
}
*/
 /*
        if (player.CheckCollision(enemy.GetCurrentCollision()))
        {
            player.SetPosition(previousPlayerPosition);
        }
          foreach (Enemy currentEnemy in enemies)
        {
            currentEnemy.Update(GraphicsDevice, gameTime);
        }
        

        if (enemy.CheckCollision(player.GetCurrentCollision()))
        {
            enemy.SetPosition(previousEnemyPosition);
        }
        */
        /*
private void SpawnEnemies(GameTime gameTime)
{
    // Проверяем, есть ли враг в списке
    if (enemies.Count == 0)
    {
        // Генерируем нового врага только если список врагов пуст
        int x = 100; // Задаем координаты для появления врага
        int y = 200;

        // Создаем нового врага и добавляем его в список
        Enemy newEnemy = new Enemy(new Vector2(x, y), 64, 64);
        //enemies.Add(newEnemy);
    }
}
*/