using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Drawing;


namespace Arcadia
{
    class Pacman
    {
        enum Direction_pacman
        {
            right,
            left,
            up,
            down
        } ;
       


        // images pacman
        private Texture2D pacman;
        private Texture2D pacman2_r;
        private Texture2D pacman2_u;
        private Texture2D pacman2_d;
        private Texture2D pacman2_l;
        private Texture2D pacman_invin;


        //position pacman
        Vector2 pacman_position;
        public Vector2 Pacman_position { get { return pacman_position; } }

        // varaible vitesse
        float speed = 1.3f;

        //variable map 
        Bitmap map ;

        // variable pour alternance pacman 
        int alternance_pacman = 0;

        Direction_pacman direction_pacman;

        // booleen pour savoir si invincible
        bool is_invincible;

        // int pour calculer le temps que pacman sera invincible
        int nb_tours_invincilble;

        public virtual void Initialize(string mapbmp)
        {
            direction_pacman = Direction_pacman.right;
            

            pacman_position = new Vector2(28, 28);
            is_invincible = false;

            nb_tours_invincilble = 0;

            map = new Bitmap(".../.../.../"+mapbmp);

        }


        public virtual void LoadContent(ContentManager content)
        {
            pacman = content.Load<Texture2D>("Pacman/pacman");
            pacman2_r = content.Load<Texture2D>("Pacman/pacman2_r");
            pacman2_d = content.Load<Texture2D>("Pacman/pacman2_d");
            pacman2_l = content.Load<Texture2D>("Pacman/pacman2_l");
            pacman2_u = content.Load<Texture2D>("Pacman/pacman2_u");
            pacman_invin = content.Load<Texture2D>("Pacman/pacman_invincible");
        }


        public virtual void Update(GameTime gameTime, KeyboardState keyboard_state, ref int[,] tab_boulette, ref int score, ref int nb_boulette)
        {
            // logique de jeu pour deplacement 

            if (keyboard_state.IsKeyDown(Keys.Down)) direction_pacman = Direction_pacman.down;
            if (keyboard_state.IsKeyDown(Keys.Up)) direction_pacman = Direction_pacman.up;
            if (keyboard_state.IsKeyDown(Keys.Right)) direction_pacman = Direction_pacman.right;
            if (keyboard_state.IsKeyDown(Keys.Left)) direction_pacman = Direction_pacman.left;


            // position pacman en x et y en entier 
            int pos_x = Convert.ToInt32(pacman_position.X);
            int pos_y = Convert.ToInt32(pacman_position.Y);

            // ajustement x et y pour le mangeage de boulette 
            float ajustement_x = 0;
            float ajustement_y = 0;

            // gestion des directions 
            if (direction_pacman == Direction_pacman.up && Check_pixel(pos_x, pos_y - 2, map) && Check_pixel(pos_x + pacman.Width, pos_y - 2, map))
            {
                pacman_position.Y = pacman_position.Y - speed;
                
                ajustement_x = 0;
                ajustement_y = 10;

                
            }
            else if (direction_pacman == Direction_pacman.down && Check_pixel(pos_x, pos_y + pacman.Height + 2, map) && Check_pixel(pos_x + pacman.Width, pos_y + pacman.Height + 2, map))
            {
                pacman_position.Y = pacman_position.Y + speed;
                
                ajustement_x = 0;
                ajustement_y = -10;

            }

            else if (direction_pacman == Direction_pacman.right && Check_pixel(pos_x + pacman.Width + 2, pos_y, map) && Check_pixel(pos_x + pacman.Width + 2, pos_y + pacman.Height, map))
            {
                pacman_position.X = pacman_position.X + speed;
                

                ajustement_x = -10;
                ajustement_y = 0;

            }
            else if (direction_pacman == Direction_pacman.left && Check_pixel(pos_x - 2, pos_y, map) && Check_pixel(pos_x - 2, pos_y + pacman.Height, map))
            {
                pacman_position.X = pacman_position.X - speed;
               

                ajustement_x = +10;
                ajustement_y = 0;

            }



            // logique de jeu pour mangeage de boulette 

            // position du pacman par rapport au tableau 
            int positionX_tab = Convert.ToInt32((pacman_position.X + ajustement_x) / 28);
            int positionY_tab = Convert.ToInt32((pacman_position.Y + ajustement_y) / 28);

            // si pacman passe et que ca fait 0 alors on met 2 et plus de boulette
            if (tab_boulette[positionX_tab, positionY_tab] == '0')
            {
                tab_boulette[positionX_tab, positionY_tab] = 2;
                score = score + 10;
                nb_boulette--;
                
            }
            else if (tab_boulette[positionX_tab, positionY_tab] == '3')
            {
                tab_boulette[positionX_tab, positionY_tab] = 2;
                score = score + 10;
                is_invincible = true;
                nb_tours_invincilble = 1;
                nb_boulette--;
            }

            if (nb_tours_invincilble > 0 && nb_tours_invincilble < 850)
            {
                nb_tours_invincilble++;
            }
            else { nb_tours_invincilble = 0; is_invincible = false; }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (alternance_pacman < 8)
            {
                if (direction_pacman == Direction_pacman.right)
                    spriteBatch.Draw(pacman2_r, pacman_position, Microsoft.Xna.Framework.Color.White);

                else if (direction_pacman == Direction_pacman.up)
                    spriteBatch.Draw(pacman2_u, pacman_position, Microsoft.Xna.Framework.Color.White);

                else if (direction_pacman == Direction_pacman.left)
                    spriteBatch.Draw(pacman2_l, pacman_position, Microsoft.Xna.Framework.Color.White);

                else if (direction_pacman == Direction_pacman.down)
                    spriteBatch.Draw(pacman2_d, pacman_position, Microsoft.Xna.Framework.Color.White);

                alternance_pacman++;
            }
            else if (alternance_pacman < 16)
            {
                if (!is_invincible)
                    spriteBatch.Draw(pacman, pacman_position, Microsoft.Xna.Framework.Color.White);
                else 
                spriteBatch.Draw(pacman_invin, pacman_position, Microsoft.Xna.Framework.Color.White);
                alternance_pacman++;
            }
            else alternance_pacman = 0;
        }


        // fonction qui verifie que pixel  la où pacman veut aller est bien noir
        public bool Check_pixel(int x, int y, Bitmap img)
        {
            if (img.GetPixel(x, y).R == 0 && img.GetPixel(x, y).G == 0 && img.GetPixel(x, y).B == 0) return true;
            else return false;
        }

        // foncition qui gere collision avec fantome
        public bool IsAlive(Fantome fantome, ref int score) 
        {
            if (!fantome.is_manger)
            {
                Microsoft.Xna.Framework.Rectangle fanto = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(fantome.Fantome_position.X), Convert.ToInt32(fantome.Fantome_position.Y), fantome.fantome_public.Width, fantome.fantome_public.Height);
                Microsoft.Xna.Framework.Rectangle pacma = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(pacman_position.X), Convert.ToInt32(pacman_position.Y), pacman.Width, pacman.Height);

                if (is_invincible == false && !fanto.Intersects(pacma))
                    return false;
                else if (is_invincible == true && fanto.Intersects(pacma))
                {
                    fantome.is_manger = true;
                    fantome.nb_tour_manger = 1;
                    score = score + 100;
                    return false;
                }
                else if (is_invincible == false && fanto.Intersects(pacma))
                    return true;
                else
                    return false;
            }
            else return false;
        }

    }
}
