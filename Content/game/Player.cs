#region Includes
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Fall,
    Death,
    Attack1,
    Attack2,
    TakeHit,
    Dash
}
  public class Player : Base2D
    {
       private Dictionary<PlayerState, Animation> animations = new Dictionary<PlayerState, Animation>();
    public Rectangle attackCollisionRect;
    private float damageAnimationTimer = 0f;
    private bool isDamageAnimationCompleted = false;
    private float damageAnimationDuration = 1.5f;

    // Таймеры для управления анимациями
  private const float JumpSpeed = 10.0f; // Скорость начала прыжка
    private const float Gravity = 0.5f; // Сила гравитации

    // Таймеры для отдельных фаз прыжка
    private float jumpTimer = 0.0f;
    private float fallTimer = 0.0f;

    // Длительности анимаций прыжка вверх и падения
    private const float JumpAnimationDuration = 0.5f;
    private const float FallAnimationDuration = 0.5f;

    // Текущая вертикальная скорость
    private float currentJumpSpeed = 0.0f;
    public bool isPlayerAttackingThisFrame;
    public bool isDamageTaken = false;
    private const float comboWindow = 1f; // Окно комбинации в секундах
    private float comboTimer = 0f; // Таймер для отслеживания времени комбинации
    public bool IsAttacking { get; set; }
    public bool IsPlayerAttackingThisFrame { get; set; }
    private float attackDurationTimer = 1f;
    private const float additionalJumpDelay = 1.5f; // Задержка для возможности дополнительного прыжка

    public bool isDamageTakenThisFrame;
    private bool canAdditionalJump = true; // Флаг, разрешающий дополнительный прыжок
    private float additionalJumpTimer = 0f;
    private int jumpCount = 0; // Счетчик прыжков в комбинации

    public Vector2 PreviousPosition { get; set; }
    public bool isAttacking = false;

    private float groundLevel = 0f;

    public float DamageAnimationDuration = 1.5f;

    public float damageReceivedTimer = 0f;
    private float jumpAnimationTimer;
    private float jumpAnimationTime = 0.8f;
    public  float playerSpeed = 2.0f;
    private float jumpSpeed;
    private float initialJumpSpeed = 10.0f; // Увеличена начальная скорость прыжка
    private float gravity = 0.5f; // Увеличена гравитация
    private float attackAnimationTimer;
    private float attackAnimationTime = 1f;

    public PlayerState CurrentAttackState { get; private set; }

    private float fallSpeed = 4.0f;
    public PlayerState currentState = PlayerState.Idle;
    public bool isFacingRight = true;
    private InputHandler inputHandler;
    public bool Active;
    public float CurrentHealth = 1000f;
    public float HealthMax = 1000f;

    public float CurrentMana = 400f;
    public int ManaMax = 500;
       
 public bool hasCollision = false;

    public void SetCollision(bool collision)
    {
        hasCollision = collision;
    }
  public int Width
        {
            get { return base_animation.FrameWidth; }
        }

        public int Height
        {
            get { return base_animation.FrameHeight; }
        }

public Player(InputHandler inputHandler, Vector2 position, int width, int height) :
    base(position, width, height)
{
    this.inputHandler = inputHandler;
    base_position = position;
    collisionWidth = width;
    collisionHeight = height;
    // Задаем координаты коллизии в центре объекта врага
   // base_rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
}
 public   float GetCurrentHealth()
    {
        return CurrentHealth;
    }
 public   float GetCurrentMana()
    {
        return CurrentMana;
    }

internal Vector2 GetPreviousPosition()
{
    return PreviousPosition;
}
public Vector2 GetPosition()
{
    return base_position;
}

public void SetPosition(Vector2 newPosition)
{
    base_position = newPosition;
}
      
        public void AddAnimation(PlayerState state, Animation animation)
        {
            animations[state] = animation;
        }

        public PlayerState GetCurrentState()
        {
            return currentState;
        }

        public bool IsAnimationActive(PlayerState state)
        {
            return currentState == state;
        }

public Rectangle GetCurrentCollision()
{
       if (collisionRectangles.Count > 0)
                return collisionRectangles[0];
            else
                return Rectangle.Empty; 
}

private Rectangle CreateMainCollision()
{
    // Здесь определите размеры и положение основной коллизии в зависимости от вашего врага.
    // Например:
    int x = (int)(base_position.X - collisionWidth / 2);
    int y = (int)(base_position.Y - collisionHeight / 2);
    return new Rectangle(x, y, collisionWidth, collisionHeight);
}


/*

public Vector2 GetAttack1CollisionPosition(bool isFacingRight)
{
    int attackX;
    // В зависимости от направления игрока определяем положение атаки
    if (isFacingRight)
    {
        // Если игрок смотрит вправо, размещаем атаку справа от врага
        attackX = (int)(base_position.X + Width / 10);
    }
    else
    {
        // Если игрок смотрит влево, размещаем атаку слева от врага
        attackX = (int)(base_position.X - 110 + Width / 10);
    }
    // Возвращаем положение верхнего левого угла коллизии для атаки
    return new Vector2(attackX, base_position.Y - Height / 5);
}

public Vector2 GetAttack2CollisionPosition(bool isFacingRight)
{
    int attackX;
    // В зависимости от направления игрока определяем положение атаки
    if (isFacingRight)
    {
        // Если игрок смотрит вправо, размещаем атаку справа от врага
        attackX = (int)(base_position.X + Width / 10);
    }
    else
    {
        // Если игрок смотрит влево, размещаем атаку слева от врага
        attackX = (int)(base_position.X - 110 + Width / 10);
    }
    // Возвращаем положение верхнего левого угла коллизии для атаки
    return new Vector2(attackX, base_position.Y - Height / 10);
}
*/


public  Rectangle CreateCustomCollision(int x, int y, int width, int height)
{
            Rectangle newCollision = new Rectangle(x, y, width, height);
            collisionRectangles.Add(newCollision);
            return newCollision;
}

public Rectangle GetAttack1CollisionBounds(bool isFacingRight)
{
    int attackX;
    // В зависимости от направления игрока определяем положение атаки
    if (isFacingRight)
    {
        // Если игрок смотрит вправо, размещаем атаку справа от врага
        attackX = (int)(base_position.X + Width / 10);
    }
    else
    {
        // Если игрок смотрит влево, размещаем атаку слева от врага
        attackX = (int)(base_position.X - 110 + Width / 10);
    }
    // Возвращаем прямоугольник коллизии для атаки
    return new Rectangle(attackX, (int)(base_position.Y - Height / 5), 70, 60);
}

public Rectangle GetAttack2CollisionBounds(bool isFacingRight)
{
    int attackX;
    // В зависимости от направления игрока определяем положение атаки
    if (isFacingRight)
    {
        // Если игрок смотрит вправо, размещаем атаку справа от врага
        attackX = (int)(base_position.X + Width / 10);
    }
    else
    {
        // Если игрок смотрит влево, размещаем атаку слева от врага
        attackX = (int)(base_position.X - 110 + Width / 10);
    }
    // Возвращаем прямоугольник коллизии для атаки
    return new Rectangle(attackX, (int)(base_position.Y - Height / 10), 70, 38);
}

public Rectangle CreateAttack1Collision(bool isFacingRight)
{
        int attackX;
        // В зависимости от направления игрока определяем положение атаки
        if (isFacingRight)
        {
            // Если игрок смотрит вправо, размещаем атаку справа от врага
            attackX = (int)(base_position.X + Width / 10);
        }
        else
        {
            // Если игрок смотрит влево, размещаем атаку слева от врага
            attackX = (int)(base_position.X - 110 + Width / 10);
        }
        // Возвращаем прямоугольник коллизии для атаки
         isPlayerAttackingThisFrame = true;
         isAttacking = true;
        return CreateCustomCollision(attackX, (int)(base_position.Y - Height / 5), 70, 60);
}

public Rectangle CreateAttack2Collision(bool isFacingRight)
{
    int attackX;
    // В зависимости от направления игрока определяем положение атаки
    if (isFacingRight)
    {
        // Если игрок смотрит вправо, размещаем атаку справа от врага
        attackX = (int)(base_position.X + Width / 10);
    }
    else
    {
        // Если игрок смотрит влево, размещаем атаку слева от врага
        attackX = (int)(base_position.X - 110 + Width / 10);
    }
    // Возвращаем прямоугольник коллизии для атаки
     isPlayerAttackingThisFrame = true;
     isAttacking = true;
    return CreateCustomCollision(attackX, (int)(base_position.Y - Height / 10), 70, 38);
}

public override void UpdateCollision()
{
    collisionRectangles.Clear(); // Очищаем список перед добавлением новых коллизий
    
    // Добавляем коллизию атаки игрока (например, атака 1)
    if (currentState == PlayerState.Attack1)
    {
        Rectangle attackCollision = CreateAttack1Collision(isFacingRight); // Создание коллизии атаки 1
        collisionRectangles.Add(attackCollision);
    }
    else if (currentState == PlayerState.Attack2)
    {
        Rectangle attackCollision = CreateAttack2Collision(isFacingRight); // Создание коллизии атаки 2
        collisionRectangles.Add(attackCollision);
    }

    // Добавляем основную коллизию игрока
    Rectangle mainCollision = CreateMainCollision(); // Создание основной коллизии
    collisionRectangles.Add(mainCollision);
}



public override void Initialize(GraphicsDevice graphicsDevice, Animation animation)
{
 
//, position
    //base.SetPosition(new Vector2(position.X / 2,position.Y / 2));
    
    base_animation = animation; // Установка анимации для игрока
    attackAnimationTimer = 0f;
    Active = true;
    base.Initialize(graphicsDevice, animation);
    
}

private bool IsJumpAnimationComplete()
{
       return jumpAnimationTimer >= jumpAnimationTime;
}
public void GetDamage(int damage)
{
    
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            // Если здоровье врага меньше или равно нулю, переводим его в состояние смерти
            //currentState = EnemyState.Death;
            base_position = new Vector2(GetPosition().X + 3, GetPosition().Y); // Смещаем позицию, чтобы анимация смерти не наслаивалась на другие объекты
        }
}
private void UpdateState(bool collision)
{
    if (CurrentHealth <= 0)
    {
        currentState = PlayerState.Death;
        return;
    }

    if (isDamageTaken)
    {
        currentState = PlayerState.TakeHit;
        isDamageTaken = false;
        damageReceivedTimer = DamageAnimationDuration;
    }
    else if (currentState == PlayerState.Fall && inputHandler.IsKeyDown(Keys.Space) && canAdditionalJump && jumpCount < 2)
    {
        currentState = PlayerState.Jump;
        jumpSpeed = initialJumpSpeed;
        groundLevel = base_position.Y;
        jumpCount++;
        canAdditionalJump = false; // Отключаем возможность дополнительного прыжка до следующего приземления
    }
    else if (currentState != PlayerState.Jump && inputHandler.IsKeyDown(Keys.Space))
    {
        if (jumpCount < 2) // Ограничение прыжков в воздухе до одного
        {
            currentState = PlayerState.Jump;
            jumpSpeed = initialJumpSpeed;
            groundLevel = base_position.Y;
            jumpCount++;
        }
        else
        {
            currentState = PlayerState.Fall;
        }
    }
    else if (currentState == PlayerState.Jump && IsJumpAnimationComplete())
    {
        currentState = PlayerState.Fall;
    }
    else if (collision && (currentState == PlayerState.Fall || currentState == PlayerState.Jump))
    {
        base_position.Y = groundLevel;
        currentState = PlayerState.Idle;
        jumpCount = 0; // Сброс счетчика прыжков при достижении земли
        canAdditionalJump = true; // Сброс возможности для дополнительного прыжка
    }
    else if (inputHandler.IsKeyDown(Keys.A) || inputHandler.IsKeyDown(Keys.D))
    {
        currentState = PlayerState.Run;
    }
    else if (inputHandler.IsKeyDown(Keys.F))
    {
        currentState = PlayerState.Attack1;
    }
    else if (inputHandler.IsKeyDown(Keys.R))
    {
        currentState = PlayerState.Attack2;
    }
    else
    {
        currentState = PlayerState.Idle;
    }
}


    private void UpdateJump()
    {
        if (currentState == PlayerState.Jump)
        {
            base_position.Y -= jumpSpeed;
            jumpSpeed -= gravity;
            if (jumpSpeed <= 0)
            {
                currentState = PlayerState.Fall;
            }
        }
    }

    private void UpdateFall(bool collision)
    {
        if (currentState == PlayerState.Fall)
        {
            base_position.Y += fallSpeed;
            if (collision)
            {
                base_position.Y = groundLevel;
                currentState = PlayerState.Idle;
                jumpCount = 0; // Сброс счетчика прыжков при достижении земли
                canAdditionalJump = true; // Сброс возможности для дополнительного прыжка
            }
        }
    }

    public void Update(GraphicsDevice graphicsDevice, GameTime gameTime, bool collision)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        attackAnimationTimer += deltaTime;
        PreviousPosition = base_position;
        additionalJumpTimer += deltaTime;

        // Проверяем, разрешено ли выполнение дополнительного прыжка после заданной задержки
        if (currentState == PlayerState.Fall && !canAdditionalJump && additionalJumpTimer >= additionalJumpDelay)
        {
            canAdditionalJump = true; // Разрешаем дополнительный прыжок после заданной задержки
            additionalJumpTimer = 0f;
        }

        // Проверка состояния игрока
        UpdateState(collision);
        UpdateJump();
        UpdateFall(collision);

        // Обновляем таймер анимации получения урона
        if (damageReceivedTimer > 0)
        {
            damageReceivedTimer -= deltaTime;
            if (damageReceivedTimer <= 0)
            {
                // Время анимации получения урона истекло, переводим игрока в состояние Idle
                currentState = PlayerState.Idle;
            }
        }

        // Обновляем состояние анимации и коллизии
        if (animations.ContainsKey(currentState))
        {
            base_animation = animations[currentState];
        }
        base_animation.Position = base_position;
        base_animation.Update(gameTime);

        // Управление движением
        if (inputHandler.IsKeyDown(Keys.A))
        {
            base_position.X -= playerSpeed;
            isFacingRight = false;
        }
        if (inputHandler.IsKeyDown(Keys.D))
        {
            base_position.X += playerSpeed;
            isFacingRight = true;
        }

        UpdateCollision();
        base.Update(graphicsDevice, gameTime);
    }


public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
{ 
    spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    base_animation.Draw(spriteBatch, spriteEffects);
    base.Draw(spriteBatch, spriteEffects);
}

    
    }
}