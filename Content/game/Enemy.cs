#region Includes
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
#endregion

namespace swordSlash {    

public enum EnemyState
{
    Idle,
    Run,
    Jump,
    Fall,
    Death,
    Attack1,
    Attack2,
    TakeHit,

   
}
  public class Enemy : Base2D
    {
        private Dictionary<EnemyState, Animation> animations = new Dictionary<EnemyState, Animation>();
        public Rectangle attackCollisionRect;
        public bool isAttacking = false;
        private Vector2 playerPosition;

        public bool isDamageTaken = false;

        public Vector2 PreviousPosition { get;  set; }
        private float attackDuration = 1f; // Примерная длительность анимации атаки (в секундах)
private float attackTimer = 0f;

public float damageReceivedTimer = 0f; // Текущий таймер
    public float damageReceivedDuration = 2f; // Длительность анимации
        public float enemySpeed = 10.0f;
         private float playerX;

          public bool isDamageTakenThisFrame;
public bool IsAttacking { get; set; }
         private float patrolTimer = 0f;
        private float patrolInterval = 5f;
        private bool isPatrollingRight = true;
               private float attackAnimationTimer;
        private float attackAnimationTime = 1.5f;
         public float DamageAnimationDuration = 1.5f; 
   
        private EnemyState currentState = EnemyState.Idle;
        public bool isFacingRight = false;
             public bool isDeathAnimationCompleted = false; 
         public float deathAnimationTimer = 0f; // Таймер для анимации смерти
         public float deathAnimationDuration = 2f;

        public bool isDamageAnimationCompleted = false;
        public bool Active;
        public int Health = 3000;
        private   bool notTakingDamage = true;

public int Width
{
  get { return base_animation.FrameWidth; }
}

public int Height
{
  get { return base_animation.FrameHeight; }
}
public Vector2 GetPosition()
{
    return base_position;
}
  public void TakeDamage(int damage)
    {
        Health -= damage;
    }
public void SetPosition(Vector2 newPosition)
{
    base_position = newPosition;
}


public Enemy(Vector2 position, int width, int height) :
    base(position, width, height)
{
    base_position = position;
    collisionWidth = width;
    collisionHeight = height;

}
      

public void followPlayerX(Vector2 playerPosition)
{
    this.playerPosition = playerPosition;
    this.playerX = playerPosition.X;
    // Проверяем позицию игрока по оси X относительно позиции врага
    if (playerPosition.X > base_position.X)
    {
        // Игрок справа, двигаемся вправо
        isFacingRight = true;
        base_position.X += 0.3f;
    }
    else if(playerPosition.X < base_position.X)
    {   
     
        // Игрок слева, двигаемся влево
        isFacingRight = false;
        base_position.X -= 0.3f;
    }
   
}


public Rectangle GetAttack1CollisionBounds(bool isFacingRight)
{
    int attackX;
    // В зависимости от направления игрока определяем положение атаки
    if (isFacingRight)
        {
            // Если игрок смотрит вправо, размещаем атаку справа от врага
            attackX = (int)(base_position.X + Width / 1.7);
        }
        else
        {
            // Если игрок смотрит влево, размещаем атаку слева от врага
            attackX = (int)(base_position.X - 110 + Width / 1.7);
        }
    // Возвращаем прямоугольник коллизии для атаки
    return new Rectangle(attackX, (int)(base_position.Y - Height / 3), 50, 55);
}

public Rectangle CreateAttack1Collision(bool isFacingRight)
{
        int attackX;
        // В зависимости от направления игрока определяем положение атаки
        if (isFacingRight)
        {
            // Если игрок смотрит вправо, размещаем атаку справа от врага
            attackX = (int)(base_position.X + Width / 1.7);
        }
        else
        {
            // Если игрок смотрит влево, размещаем атаку слева от врага
            attackX = (int)(base_position.X - 110 + Width / 1.7);
        }
        // Возвращаем прямоугольник коллизии для атаки
         
         isAttacking = true;
        return CreateCustomCollision(attackX, (int)(base_position.Y - Height / 3), 50, 55);
}
public void AddAnimation(EnemyState state, Animation animation)
{
    animations[state] = animation;
}

public EnemyState GetCurrentState()
{
    return currentState;
}

public bool IsAnimationActive(EnemyState state)
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

public void SetCurrentState(EnemyState newState)
{
        currentState = newState;
}
public  Rectangle CreateCustomCollision(int x, int y, int width, int height)
{
        Rectangle newCollision = new Rectangle(x, y, width, height);
        collisionRectangles.Add(newCollision);
        return newCollision;
}

private Rectangle CreateMainCollision()
{
    // Здесь определите размеры и положение основной коллизии в зависимости от вашего врага.
    // Например:
    int x = (int)(base_position.X + collisionWidth / 6);
    int y = (int)(base_position.Y - collisionHeight / 2);
    return new Rectangle(x, y, collisionWidth, collisionHeight);
}

public override void UpdateCollision()
{
    collisionRectangles.Clear(); // Очищаем список перед добавлением новых коллизий    

    // Добавляем коллизию атаки врага
    if (currentState == EnemyState.Attack1)
    {
        Rectangle attackCollision = CreateAttack1Collision(isFacingRight); // Создание коллизии атаки 1
        collisionRectangles.Add(attackCollision);
    }
    
    // Добавляем основную коллизию врага
    Rectangle mainCollision = CreateMainCollision(); // Создание основной коллизии
    collisionRectangles.Add(mainCollision);
}



public override void Initialize(GraphicsDevice graphicsDevice, Animation animation)
{
    
    base_animation = animation; // Установка анимации для игрока
  //  takeHitAnimationTimer = 0f;
    Active = true;


   base.Initialize(graphicsDevice, animation);
}

public void GetDamage(int damage)
{
    Health -= damage;
    if (Health <= 0)
    {
        // Если здоровье врага меньше или равно нулю, переводим его в состояние смерти
        currentState = EnemyState.Death;
        base_position = new Vector2(GetPosition().X + 3, GetPosition().Y); // Смещаем позицию, чтобы анимация смерти не наслаивалась на другие объекты
    }
    else
    {
        isDamageTaken = true; // Устанавливаем флаг получения урона
        damageReceivedTimer = DamageAnimationDuration; // Устанавливаем таймер анимации получения урона
    }
}
public override void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
{
    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    attackAnimationTimer += deltaTime;
    PreviousPosition = base_position;
    patrolTimer += deltaTime;

    UpdateState(gameTime);

    base_animation = animations[currentState];
    base_animation.Position = base_position;
    base_animation.Update(gameTime);

    followPlayerX(playerPosition);

    // Проверяем, находится ли игрок в зоне видимости врага
    bool playerInSight = Math.Abs(base_position.X - playerX) < 100; // Здесь 100 - это ширина зоны видимости врага

    // Проверяем, находится ли игрок в зоне видимости атаки сзади
    bool playerInAttackZoneBehind = isFacingRight ? playerX < base_position.X && playerX > base_position.X - 100 : playerX > base_position.X && playerX < base_position.X + 100;

    // Проверяем, прошло ли достаточно времени для завершения анимации получения урона
    bool damageAnimationCompleted = damageReceivedTimer <= 0;

    // Проверяем, не находится ли враг в состоянии получения урона перед тем, как разрешить атаку
    bool notTakingDamage = currentState != EnemyState.TakeHit;

    if ((playerInSight || playerInAttackZoneBehind) && currentState != EnemyState.Death && damageAnimationCompleted && notTakingDamage) // Добавлено условие для атаки сзади и проверка времени анимации получения урона
    {
        currentState = EnemyState.Attack1;
    }
    else if (currentState != EnemyState.Death) // Добавлено условие, чтобы враг не мог бежать во время смерти
    {
        currentState = EnemyState.Run;
    }

    if (isDamageTakenThisFrame)
    {
        TakeDamage(10); // Пример урона, который вы хотите применить
        isDamageTakenThisFrame = false; // Сбрасываем флаг
        
    }

    // Проверяем, завершилась ли анимация смерти
    if (currentState == EnemyState.Death && isDeathAnimationCompleted == false)
    {
        // Обновляем таймер анимации смерти
        deathAnimationTimer -= deltaTime;
        if (deathAnimationTimer <= 0)
        {
            isDeathAnimationCompleted = true;
        }
    }

    // Обновление коллизий врага независимо от его текущего состояния
    UpdateCollision();

    base.Update(graphicsDevice, gameTime);
}


private void UpdateState(GameTime gameTime)
{
    // Обновляем таймер анимации смерти и проверяем, завершилась ли анимация
    deathAnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    if (deathAnimationTimer <= 0)
    {
        isDeathAnimationCompleted = true;
    }

    if (isDamageTaken)
    {
        currentState = EnemyState.TakeHit;
        // Флаг получения урона устанавливается только один раз, здесь его сбрасываем
        isDamageTaken = false;
        notTakingDamage = false; // Устанавливаем флаг, что враг не принимает урон
    }
    else if (currentState == EnemyState.TakeHit)
    {
        damageReceivedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (damageReceivedTimer <= 0)
        {
            if (Health > 0)
            {
                currentState = EnemyState.Run; // Изменение состояния только если враг не умер
                notTakingDamage = true; // Возвращаем флаг обратно в исходное состояние после завершения анимации получения урона
            }
        }
    }
    else if (Health <= 0)
    {
        currentState = EnemyState.Death;
        // Сброс флага завершения анимации смерти при смерти
        isDeathAnimationCompleted = false;
        deathAnimationTimer = deathAnimationDuration; // Установка таймера анимации смерти
        base_position = new Vector2(GetPosition().X + 3, GetPosition().Y);
    }
    else if (currentState != EnemyState.Attack1)
    {
        bool playerInSight = Math.Abs(base_position.X - playerX) < 100; // Ширина зоны видимости врага
        bool playerInAttackZoneBehind = isFacingRight ? playerX < base_position.X && playerX > base_position.X - 100 : playerX > base_position.X && playerX < base_position.X + 100;
        bool damageAnimationCompleted = damageReceivedTimer <= 0;

        if ((playerInSight || playerInAttackZoneBehind) && currentState != EnemyState.Death && damageAnimationCompleted && notTakingDamage)
        {
            currentState = EnemyState.Attack1;
        }
        else if (currentState != EnemyState.Death)
        {
            currentState = EnemyState.Run;
        }
    }
}


public override void Draw(SpriteBatch spriteBatch, SpriteEffects parentSpriteEffects)
{ 
    // Определяем эффекты анимации на основе направления взгляда врага
    SpriteEffects animationSpriteEffects = isFacingRight ? SpriteEffects.FlipHorizontally  : SpriteEffects.None;

    // Комбинируем эффекты родительского объекта с эффектами анимации
   

    // Отрисовываем анимацию с учетом полученных эффектов
   

    // Передаем эффекты родительскому объекту для последующей отрисовки
    base.Draw(spriteBatch, animationSpriteEffects);
}

        internal Vector2 GetPreviousPosition()
        {
            return PreviousPosition;
        }
    }
}



/*
  public Rectangle CreateAttack1Collision(bool isFacingRight)
    {
        int attackX;
        // В зависимости от направления игрока определяем положение атаки
        if (isFacingRight)
        {
            // Если игрок смотрит вправо, размещаем атаку справа от врага
          Понял вопрос. Если проблема в том, что коллизии не располагаются должным образом, то важно убедиться, что вы добавляете и обновляете коллизии в правильном порядке.

У вас есть несколько объектов с коллизиями (игрок и враг), и вы храните их в списке collisionRectangles. При обновлении коллизий убедитесь, что вы добавляете их в список в правильном порядке.omCollision(attackX, (int)(base_position.Y - Height / 5), 70, 60);
    }  
    */
 /*
            if (currentState == EnemyState.Jump || currentState == EnemyState.Fall)
            {
                return;
            }
            if (currentState != EnemyState.Jump && inputHandler.IsKeyDown(Keys.Space))
            {
                currentState = EnemyState.Jump;
                jumpSpeed = initialJumpSpeed;
                groundLevel = base_position.Y;
            }
            else if (inputHandler.IsKeyDown(Keys.A) || inputHandler.IsKeyDown(Keys.D))
            {
                currentState = EnemyState.Run;
            }
            else if (inputHandler.IsKeyDown(Keys.F))
            {
                currentState = EnemyState.Attack1;
            }
            else if (inputHandler.IsKeyDown(Keys.R))
            {
                currentState = EnemyState.Attack2;
            }
            else
            {
                currentState = EnemyState.Idle;
            }
            */