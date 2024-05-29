#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace swordSlash
{
public static class CollisionManager
{
public static bool CheckPlayerTileCollisions(Player player, List<Tiles> tilesList)
{
    foreach (var tile in tilesList)
    {
        if (player.CheckCollision(tile.GetCurrentCollision()))
        {
            player.SetPosition(player.GetPreviousPosition());
            return true;
        }
    }
    return false;
}



public static bool CheckEnemyTileCollisions(List<Enemy> enemies, List<Tiles> tilesList)
{
    foreach (var enemy in enemies)
    {
        foreach (var tile in tilesList)
        {
            if (enemy.CheckCollision(tile.GetCurrentCollision()))
            {
                enemy.SetPosition(enemy.GetPreviousPosition());
                return true;
            }
        }
    }
    return false;
}

public static bool CheckPlayerEnemyCollisions(Player player, List<Enemy> enemies)
{
    bool playerHasCollisionWithEnemy = false;

    foreach (var enemy in enemies)
    {
        if (player.CheckCollision(enemy.GetCurrentCollision()))
        {
            // Если произошла коллизия, сообщаем о ней
          
            // Возвращаем игрока на предыдущее положение, чтобы избежать коллизий
            player.SetPosition(player.GetPreviousPosition());

            enemy.SetPosition(enemy.GetPreviousPosition());
            
        }
    }

    return playerHasCollisionWithEnemy;
}

public static void CheckEnemyAttackCollision(Player player, List<Enemy> enemies)
{
    //Rectangle attack1Collision = .GetAttack1CollisionBounds(player.isFacingRight);

    foreach (Enemy enemy in enemies)
    {
        if (enemy.GetAttack1CollisionBounds(enemy.isFacingRight).Intersects(player.GetCurrentCollision()))
        {
            player.GetDamage(1);
            player.isDamageTaken = true;
            enemy.isAttacking = false;
        }
        
      
    }
}

public static void CheckPlayerAttackCollision(Player player, List<Enemy> enemies)
{
    Rectangle attack1Collision = player.GetAttack1CollisionBounds(player.isFacingRight);
    Rectangle attack2Collision = player.GetAttack2CollisionBounds(player.isFacingRight);

    foreach (Enemy enemy in enemies)
    {
        if (attack1Collision.Intersects(enemy.GetCurrentCollision()))
        {
            enemy.GetDamage(10);
            enemy.isDamageTaken = true;
            player.isAttacking = false;
            Console.WriteLine("Player attacked Enemy 1 and dealt 10 damage!");
        }
        
        if (attack2Collision.Intersects(enemy.GetCurrentCollision()))
        {
            enemy.GetDamage(10);
            enemy.isDamageTaken = true;
            player.isAttacking = false;
            Console.WriteLine("Player attacked Enemy 2 and dealt 10 damage!");
        }
    }
}

    }
}           


  /*
                foreach (var enemy in enemies)
                {
                    if (enemy.CheckCollision(tile.GetCurrentCollision()))
                    {
                        enemy.SetPosition(enemy.GetPreviousPosition());
                    }
                }
*/

 // Проверка коллизий между игроком и врагами
            /*
            List<Enemy> enemies, 
            foreach (Enemy enemy in enemies)
            {
                if (player.CheckCollision(enemy.GetCurrentCollision()))
                {
                    // Восстанавливаем игрока на предыдущую позицию перед коллизией
                    player.SetPosition(player.GetPreviousPosition());
                }

                if (enemy.CheckCollision(player.GetCurrentCollision()))
                {
                    enemy.SetPosition(enemy.GetPreviousPosition());
                }
            }
*/