using Microsoft.Xna.Framework;
using System;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class MonsterCreater
    {
        public static Monster CreateMonster(int level, int screenWidth, int screenHeight, Random random)
        {
            Monster monster;

            if (level == 1)
            {
                if (random.NextDouble() < 0.3)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 430), true)
                    { monsterTexture = MonsterView.FlyingMonsterLevel1, monsterSpeed = 300f, amplitude = 60f, frequency = 1.5f };
                }
                else if (random.NextDouble() < 0.5)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 440), false)
                    { monsterTexture = MonsterView.Monster2Level1, monsterSpeed = 330f };
                }
                else
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 440), false)
                    { monsterTexture = MonsterView.Monster1Level1, monsterSpeed = 360f };
                }
            }
            else
            {
                if (random.NextDouble() < 0.4)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 430), true)
                    { monsterTexture = MonsterView.FlyingMonsterLevel2, monsterSpeed = 330f, amplitude = 70f, frequency = 1.8f };
                }
                else if (random.NextDouble() < 0.5)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 530), false)
                    { monsterTexture = MonsterView.Monster2Level2, monsterSpeed = 360f };
                }
                else
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 390), false)
                    { monsterTexture = MonsterView.Monster1Level2, monsterSpeed = 390f };
                }
            }
            return monster;
        }
    }
}