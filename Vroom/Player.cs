using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Vroom
{
    class Player : Object
    {
        #region fields
      
        //Player attibutes
        public int health;
        public int maxHealth = 100;
        //Controls
        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        MouseState mouse;
        MouseState prevMouse;

        //FX
        private SoundEffect carFX;
        private SoundEffect shootFX;
        bool isDriving = false;
     
        //shooting bullets
        public int maxAmmo;
        public int ammo;
        public int fireRate;
        int shootingTimer = 0;
        public float bulletSpeed;
        int reloadTimer = 0;
        public int reloadTime;
        public bool reloading = false;
        //Char Selection
        public int currentSelectedCar;
        public bool set = false;
        //Vehicle stuff
        public float spd;
        Vector2 newPos;
        
        //Player score & Stats
        public int score=0;
        public int shotsFired=0;
        public int enemiesKilled=0;
        

        
        //Vehicle behaviour
        float maxSteeringRotation = 0.5f;
        public Vector2 velocity;
        public float acceleration =0.2f;
        public float maxSpeed;
        public float friction = 0.06f;
        
        //static version of himself
        public static Player player;

        #endregion fields

        #region methods
        public Player(Vector2 pos) : base(pos)
        {
            position = pos;
            spd = 5;
            player = this;
            health = maxHealth;
            spriteName = "SpriteSheet";
            imageSpeed = 0f;
            imageRow = 4;
            imageNumber = 3;
            
        }

        public override void LoadContent(ContentManager content)
        {
            spriteIndex = content.Load<Texture2D>(this.spriteName);
            frame = new Point((spriteIndex.Width / imageNumber), spriteIndex.Height/imageRow);
            area = new Rectangle(0, 0, spriteIndex.Width / imageNumber, spriteIndex.Height/imageRow);        
            carFX = content.Load<SoundEffect>("car");
            shootFX = content.Load<SoundEffect>("shoot");
            
            

        }

        public override void Update()
        {
            
            //Level Logic
            if (score >= Game1.levelOneMaxScore)
            {
                Game1.GameState = "GameOver";
            }
            player = this;
            //If Player is dead return
            if (!alive) { Game1.GameState = "GameOver"; }
           
            if (!set)
            { //Set Selected Character
                currentSelectedCar = CharacterSelection.characterSelection.currentSelectedCar;
                CharacterSelection.characterSelection.SetCharacter(currentSelectedCar);
                //do only once per session switch 
                set = true;
            }
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            player.position += player.velocity;
            newPos = position;
            if (Player.player.position.X < 500 || Player.player.position.X > 7600 || Player.player.position.Y < 500 || Player.player.position.Y > 4100)
            {

                Random rnd = new Random();
                Player.player.position.X = rnd.Next(600, 7400);
                Player.player.position.Y = rnd.Next(600, 3900);
            }
            
                if (keyboard.IsKeyDown(Keys.W))
                {
                    carFX.Play(0.3f, (acceleration / 15), 0);
                    maxSpeed = spd;
                    if (acceleration < maxSpeed)
                    {
                        acceleration += acceleration;
                        player.velocity.X = (float)Math.Cos(rotation) * acceleration;
                        player.velocity.Y = (float)Math.Sin(rotation) * acceleration;
                    }
                    else
                    {
                        player.velocity.X = (float)Math.Cos(Player.player.rotation) * Player.player.acceleration;
                        player.velocity.Y = (float)Math.Sin(Player.player.rotation) * Player.player.acceleration;
                    }


                }
                else if (player.velocity != Vector2.Zero)
                {
                    float temp1 = Player.player.velocity.X;
                    float temp2 = Player.player.velocity.Y;
                    player.velocity.X = temp1 -= Player.player.friction * temp1;
                    player.velocity.Y = temp2 -= Player.player.friction * temp2;
                }
            
            
            
                if (keyboard.IsKeyDown(Keys.S))
                {
                    carFX.Play(0.3f, (acceleration / 15), 0);
                    player.velocity.X = (float)Math.Cos(rotation) * -acceleration;
                    player.velocity.Y = (float)Math.Sin(rotation) * -acceleration;
                }
            
            if (keyboard.IsKeyDown(Keys.A))
            {
                rotation -= 0.05f;
            }
        
            
            if (keyboard.IsKeyDown(Keys.D))
            {
               rotation += 0.05f;
            }

            //shooting 
            shootingTimer++;
            if (mouse.LeftButton==ButtonState.Pressed && !reloading)
            {
                CheckShooting();
            }
            if (mouse.RightButton==ButtonState.Pressed || ammo ==0)
            {
                reloading = true;
            }
            CheckReload();
            //player pos relative to screen compaired rel pos to screen mouse 
            // rotation = PointDirection(Camera.GlobalToLocal(position).X, Camera.GlobalToLocal(position).Y, mouse.X, mouse.Y);
            //Damage Animation Player Sprite
            if (health < ((maxHealth / 4) * 3))
            {               
                if (health < ((maxHealth/4)*2))
                {
                    imageIndex = 2;
                }
                if(health < (maxHealth/4))
                {
                    imageIndex = 3;
                }
                imageIndex = 1;
            }
            //PlayerDeath
            if (health <= 0)
            {
                Game1.GameState = "GameOver";
                health = maxHealth;
            }        
            prevKeyboard = keyboard;
            prevMouse = mouse;
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //If Player is dead return
            if (!draw && !alive) return;
            Rectangle size;
            Vector2 center = new Vector2((spriteIndex.Width / 3)/2, (spriteIndex.Height / 4)/2);            
            //Animated Stuff (? and : is like if-condition)
            imageIndex += (imageIndex < imageNumber - 1) ? imageSpeed : -imageIndex;
            size = new Rectangle((int)imageIndex * frame.X, 0+(currentSelectedCar*frame.Y), frame.X, frame.Y);           
            spriteBatch.Draw(spriteIndex, position, size, Color.White,rotation, center, scale, SpriteEffects.None, 0);
            //spriteBatch.DrawString(Game1.gameOverFont,"X: "+Player.player.position.X,new Vector2(Player.player.position.X -300,Player.player.position.Y-300),Color.White);          
            //spriteBatch.DrawString(Game1.gameOverFont,"Y: "+ Player.player.position.Y,new Vector2(Player.player.position.X -400,Player.player.position.Y-400),Color.White);

        }

        //Shooting Logic
        private void CheckShooting()
        {
            if (shootingTimer > fireRate && ammo > 0)
            {
                shootingTimer = 0;
                Shoot();
            }
        }

        //Ammo Logic
        private void Shoot()
        {
            ammo--;
            
            shootFX.Play();
            //loop through ObjectList and find Bullet
            foreach (Object o in Items.objectList)
            {
                //if object in objectlist is a buller -> fire
                if (o.GetType() == typeof(Bullet) && !o.alive)
                {
                    shotsFired++;
                    //player variables used, so that bullet direction = player point of view
                    o.position = position;
                    o.UpdateArea();
                    o.rotation = PointDirection(Camera.GlobalToLocal(position).X, Camera.GlobalToLocal(position).Y,mouse.X,mouse.Y);
                    o.speed = bulletSpeed;
                    o.alive = true;
                    o.draw = true;
                    //escapesequence - if not there all bullets would be fired
                    break;
                }
            }
        }

        private void CheckReload()
        {
            if (reloading)
            {
                reloadTimer++;
            }
            if (reloadTimer > reloadTime)
            {
                ammo = maxAmmo;
                reloadTimer = 0;
                reloading = false;
            }
        }

        private float PointDirection(float x, float y, float x2, float y2)
        {
            float diffx = x - x2;
            float diffy = y - y2;
            float adj = diffx;
            float opp = diffy;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));

            res = (res + 180) % 360;
            if (res < 0)
            {
                res += 360;
            }
            return res;
        }

        public void Damage(int dmg)
        {
            health -= dmg;
        } 

        #endregion methods

    }

}
