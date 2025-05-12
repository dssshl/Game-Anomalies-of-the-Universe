using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Game_Anomalies_of_the_Universe.Code.Models;

namespace Game_Anomalies_of_the_Universe.Code.Controllers
{
    public class PlayerController
    {
        private readonly PlayerModel player;
        private const float playerSpeed = 500f;
        private const float jump = -500f;
        private const float gravity = 1100f;

        public PlayerController(PlayerModel player)
        {
            this.player = player;
        }

        public void Update(float time, KeyboardState keyboardState, MouseState mouseState)
        {
            float moveX = 0;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                moveX -= 1;
                player.DirectionRight = false;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                moveX += 1;
                player.DirectionRight = true;
            }
            player.Move = new Vector2(moveX * playerSpeed, player.Move.Y);
            player.Position += player.Move * time;

            Jumping(time, keyboardState);

            player.Position = new Vector2(MathHelper.Clamp(player.Position.X, 0, player.screenWidth - player.Hitbox.Width), player.Position.Y);

            Shooting(time, mouseState);
            UpdateBullets(time);

            if (player.Invulnerability > 0)
                player.Invulnerability -= time;
        }

        private void Jumping(float time, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Space) && player.IsGround)
            {
                player.Move = new Vector2(player.Move.X, jump);
                player.IsGround = false;
            }

            player.Move = new Vector2(player.Move.X, player.Move.Y + gravity * time);
            player.Position += new Vector2(0, player.Move.Y * time);

            if (player.Position.Y >= player.Ground - player.Hitbox.Height)
            {
                player.Position = new Vector2(player.Position.X, player.Ground - player.Hitbox.Height);
                player.Move = new Vector2(player.Move.X, 0);
                player.IsGround = true;
            }
        }

        private void Shooting(float time, MouseState mouseState)
        {
            if (player.Shoot > 0)
                player.Shoot -= time;

            if (mouseState.LeftButton == ButtonState.Pressed && player.Shoot <= 0)
            {
                Vector2 bulletPosition = new Vector2( player.Position.X + player.Hitbox.Width / 2 - 10f, player.Position.Y + player.Hitbox.Height / 2 + 90f);
                Vector2 direction = player.DirectionRight ? Vector2.UnitX : -Vector2.UnitX;

                player.Bullets.Add(new BulletModel(bulletPosition, direction));
                player.Shoot = PlayerModel.ShootCount;
            }
        }

        private void UpdateBullets(float time)
        {
            for (int i = player.Bullets.Count - 1; i >= 0; i--)
            {
                player.Bullets[i].Position += player.Bullets[i].Direction * player.Bullets[i].Speed * time;

                if (player.Bullets[i].Position.X < 0 || player.Bullets[i].Position.X > player.screenWidth ||
                    player.Bullets[i].Position.Y < 0 || player.Bullets[i].Position.Y > player.screenHeight)
                    player.Bullets.RemoveAt(i);
            }
        }
    }
}
