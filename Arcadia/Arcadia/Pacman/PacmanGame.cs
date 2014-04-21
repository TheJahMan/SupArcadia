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
    enum PacmanState
    {
        game,
        pause,
        death,
        finish,
        over,
        inter,
        menu
        
    };

    class PacmanGame
    {
        // vie pacman 
        int life;


        // declaration classe
        private Pacman pacman = new Pacman();

        // declaration de l'etat de jeu du pacman
        PacmanState pacman_state = new PacmanState();

        // tableau pour boule jaune
        int[,] tab_boulette = new int[25, 25];


        // decalration tableau pour boulette, fantome et pacman
        Map tab = new Map();

        // texture pour cadre à droite
        private Texture2D cadre;

        //texure fantomes
         public Texture2D fantome_1;
         public Texture2D fantome_2; 
         public Texture2D fantome_3;
         public Texture2D fantome_4; 
        
        // declaration du font
        SpriteFont sprite_font;
        
        // declaration fantomes
        Fantome fantome1 = new Fantome();
        Fantome fantome2 = new Fantome();
        Fantome fantome3 = new Fantome();
        Fantome fantome4 = new Fantome();
        
        //  booléen qui verifie si retour menu apres mort pacman 
        public bool check_retour_menu;

        //boolen qui verifie si passage au niveau 2
        public bool check_level_next;

        // texture pour intermediraire entre niveau, perdre vie...
        private Texture2D inter_vie;
        Vector2 position_inter_vie;
        private Texture2D rejouer;
        Vector2 position_rejouer;



        // objet pour tableau de langue 
        Langue langue = new Langue();

        // lecture du fichier .txt pour boulette
        string tab_string;

        // int transfere score
        int score_transfere;

        // pour savoir si win int nb boulete 
        int nb_boulette;
        public virtual void Initialize( string maptxt, string map, int level)
        {
            // initialisation vie + score
            life = 3;

            

            // initialisation de pacman
            pacman.Initialize(map);

            // etat du jeu de base
            pacman_state = PacmanState.game;

            // lecture du fichier .txt pour boulette 
            tab_string = System.IO.File.ReadAllText("../../../"+maptxt);

            // initialisation boulette
            tab.InitializeMaps(tab_string, ref tab_boulette);
            nb_boulette = tab.nb_boul;
            
            // initialisation fantomes
            fantome1.Initialize(map,1,level);
            fantome2.Initialize(map,2,level);
            fantome3.Initialize(map,3,level);
            fantome4.Initialize(map,1,level);

            // initialisation du booleen qui sert à verifier qu'apres mort total on clique ou non sur menu
            check_retour_menu = false;

            // initialisation du booleen qui sert à verifier si passage au level 2 après win
            check_level_next = false;

           
            
        }


        public virtual void LoadContent(ContentManager content)
        {
            // pacman
            pacman.LoadContent(content);

            //boulette jaune
            tab.LoadContent(content);

            fantome_1 = content.Load<Texture2D>("Pacman/fantome");
            fantome_2 = content.Load<Texture2D>("Pacman/fantome_1");
            fantome_3 = content.Load<Texture2D>("Pacman/fantome_3");
            fantome_4 = content.Load<Texture2D>("Pacman/fantome");

            // chargement du font 
            sprite_font = content.Load<SpriteFont>("MaPolice");
            
           
            // chargement fantomes 
            fantome1.LoadContent(content);
            fantome2.LoadContent(content);
            fantome3.LoadContent(content);
            fantome4.LoadContent(content);


            // chargemnt sprite inter vie niveau...
            inter_vie = content.Load<Texture2D>("Pacman/image_inter_pacman");
            rejouer = content.Load<Texture2D>("Pacman/bouton rejouer");

            // chargement cadre à droite
            cadre = content.Load<Texture2D>("Pacman/cadre");

        }


        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, GameState gameState, MouseState old_mouseState, KeyboardState old_keyboardState, string map, ref int score,int level)
        {
           
            // deroulement de jeu quand normal 
            if (pacman_state == PacmanState.game)
            {
                if (nb_boulette  == 0 || keyboardState.IsKeyDown(Keys.W)) 
                {
                    pacman_state = PacmanState.finish;
                    score_transfere = life * 200;
                    
                }

                if (pacman.IsAlive(fantome1, ref score) || pacman.IsAlive(fantome2, ref score) || pacman.IsAlive(fantome3, ref score) || pacman.IsAlive(fantome4, ref score)) pacman_state = PacmanState.death;

                pacman.Update(gameTime, keyboardState, ref tab_boulette, ref score, ref nb_boulette);
                fantome1.Update(ref tab_boulette, map,level);
                fantome2.Update(ref tab_boulette, map,level);
                fantome3.Update(ref tab_boulette, map,level);
                fantome4.Update(ref tab_boulette, map,level);

                if (keyboardState.IsKeyUp(Keys.Space) && old_keyboardState.IsKeyDown(Keys.Space))
                {
                    pacman_state = PacmanState.pause;
                    
                }

            }

            else if (pacman_state == PacmanState.death)
            {
                life = life - 1;

                if (life == 0)
                    pacman_state = PacmanState.over;
                else
                {
                    pacman_state = PacmanState.inter;
                    fantome1.Initialize(map,1,level);
                    fantome2.Initialize(map,2,level);
                    fantome3.Initialize(map,3,level);
                    fantome4.Initialize(map,1,level);
                    pacman.Initialize(map);
                }

            }

            else if (pacman_state == PacmanState.inter)
            {
                // initialisation vecteurs pour sprite inter vie niveau..
                position_inter_vie = new Vector2(500 - inter_vie.Width / 2, 350 - inter_vie.Height / 2);
                position_rejouer = new Vector2(500 - rejouer.Width / 2, 350 - rejouer.Height / 2 + 50);

                

                if (mouseState.X < (position_rejouer.X + rejouer.Width) && mouseState.X > (position_rejouer.X - rejouer.Width)
                && mouseState.Y < (position_rejouer.Y + rejouer.Height) && mouseState.Y > (position_rejouer.Y - rejouer.Height)
                && mouseState.LeftButton != ButtonState.Pressed && old_mouseState.LeftButton == ButtonState.Pressed)
                    pacman_state = PacmanState.game;
            }

            else if (pacman_state == PacmanState.over)
            {
                // initialisation vecteurs pour sprite inter vie niveau..
                position_inter_vie = new Vector2(500 - inter_vie.Width / 2, 350 - inter_vie.Height / 2);
                position_rejouer = new Vector2(500 - rejouer.Width / 2, 350 - rejouer.Height / 2 +50);

                if (mouseState.X < (position_rejouer.X + rejouer.Width) && mouseState.X > (position_rejouer.X - rejouer.Width)
                && mouseState.Y < (position_rejouer.Y + rejouer.Height) && mouseState.Y > (position_rejouer.Y - rejouer.Height)
                && mouseState.LeftButton != ButtonState.Pressed && old_mouseState.LeftButton == ButtonState.Pressed)
                    check_retour_menu = true;

            }


            else if (pacman_state == PacmanState.finish)
            {
                // initialisation vecteurs pour sprite inter vie niveau..
                position_inter_vie = new Vector2(500 - inter_vie.Width / 2, 350 - inter_vie.Height / 2);
                position_rejouer = new Vector2(500 - rejouer.Width / 2, 350 - rejouer.Height / 2 + 50);

                if (score_transfere != 0) { score++; score_transfere--; score++; score_transfere--; }
                

                if (mouseState.X < (position_rejouer.X + rejouer.Width) && mouseState.X > (position_rejouer.X - rejouer.Width)
                && mouseState.Y < (position_rejouer.Y + rejouer.Height) && mouseState.Y > (position_rejouer.Y - rejouer.Height)
                && mouseState.LeftButton != ButtonState.Pressed && old_mouseState.LeftButton == ButtonState.Pressed)
                    check_level_next = true;
            }
            else  if (pacman_state == PacmanState.pause)
            {
                if (keyboardState.IsKeyUp(Keys.Space) && old_keyboardState.IsKeyDown(Keys.Space))
                {
                    pacman_state = PacmanState.game;

                }

            }


        }


        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref int score, int level)
        {
            if (pacman_state == PacmanState.game)
            {
              //map
                tab.Afficheboulette(ref tab_boulette, spriteBatch);

                
                // pacman
                pacman.Draw(spriteBatch, gameTime);

                // fantomes
                fantome1.Draw(spriteBatch,fantome_1);
                fantome2.Draw(spriteBatch,fantome_2);
                fantome3.Draw(spriteBatch,fantome_3);
                fantome4.Draw(spriteBatch,fantome_4);

                // cadre à droite
                spriteBatch.Draw(cadre, new Vector2(700, 0), Microsoft.Xna.Framework.Color.White);

                // ecritre "pacman level x"
                spriteBatch.DrawString(sprite_font, langue.francais[11] +level, new Vector2(850 - sprite_font.MeasureString(langue.francais[11]).X /2, 0), Microsoft.Xna.Framework.Color.Yellow);


                // ecriture du score 
                spriteBatch.DrawString(sprite_font, langue.francais[3] + score, new Vector2(705, 50), Microsoft.Xna.Framework.Color.White);

                // ecriture des vies 
                spriteBatch.DrawString(sprite_font, langue.francais[4] + life, new Vector2(705, 100), Microsoft.Xna.Framework.Color.White); 
            }

            else if (pacman_state == PacmanState.inter)
            {
               
                // map
                tab.Afficheboulette(ref tab_boulette, spriteBatch);

                // pacman
                pacman.Draw(spriteBatch, gameTime);

                // fantomes
                fantome1.Draw(spriteBatch, fantome_1);
                fantome2.Draw(spriteBatch, fantome_2);
                fantome3.Draw(spriteBatch, fantome_3);
                fantome4.Draw(spriteBatch, fantome_4);

                // cadre à droite
                spriteBatch.Draw(cadre, new Vector2(700, 0), Microsoft.Xna.Framework.Color.White);

                // ecritre "pacman level x"
                spriteBatch.DrawString(sprite_font, langue.francais[11] + level, new Vector2(850 - sprite_font.MeasureString(langue.francais[11]).X/2, 0), Microsoft.Xna.Framework.Color.Yellow);

                // ecriture du score 
                spriteBatch.DrawString(sprite_font, langue.francais[3] + score, new Vector2(705, 50), Microsoft.Xna.Framework.Color.White);

                // ecriture des vies 
                spriteBatch.DrawString(sprite_font, langue.francais[4] + life, new Vector2(705, 100), Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(inter_vie, position_inter_vie, Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(rejouer, position_rejouer, Microsoft.Xna.Framework.Color.White);

               // ecriture indications 
                spriteBatch.DrawString(sprite_font, langue.francais[5], new Vector2(position_rejouer.X + rejouer.Width/2 - sprite_font.MeasureString(langue.francais[5]).X / 2, position_rejouer.Y +rejouer.Height/2 - sprite_font.MeasureString(langue.francais[5]).Y / 2), Microsoft.Xna.Framework.Color.White);
                spriteBatch.DrawString(sprite_font, langue.francais[6], new Vector2(position_inter_vie.X + inter_vie.Width/2 - sprite_font.MeasureString(langue.francais[6]).X / 2, position_inter_vie.Y + inter_vie.Height/3 - sprite_font.MeasureString(langue.francais[6]).Y / 2), Microsoft.Xna.Framework.Color.White);

            }

            else if (pacman_state == PacmanState.over)
            {
               // map
                tab.Afficheboulette(ref tab_boulette, spriteBatch);

                // pacman
                pacman.Draw(spriteBatch, gameTime);

                // fantomes
                fantome1.Draw(spriteBatch, fantome_1);
                fantome2.Draw(spriteBatch, fantome_2);
                fantome3.Draw(spriteBatch, fantome_3);
                fantome4.Draw(spriteBatch, fantome_4);

                // cadre à droite
                spriteBatch.Draw(cadre, new Vector2(700, 0), Microsoft.Xna.Framework.Color.White);

                // ecritre "pacman level x"
                spriteBatch.DrawString(sprite_font, langue.francais[11] + level, new Vector2(850 - sprite_font.MeasureString(langue.francais[11]).X /2, 0), Microsoft.Xna.Framework.Color.Yellow);

                // ecriture du score 
                spriteBatch.DrawString(sprite_font, langue.francais[3] + score, new Vector2(705, 50), Microsoft.Xna.Framework.Color.White);

                // ecriture des vies 
                spriteBatch.DrawString(sprite_font, langue.francais[4] + life, new Vector2(705, 100), Microsoft.Xna.Framework.Color.White);

                spriteBatch.Draw(inter_vie, position_inter_vie, Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(rejouer, position_rejouer, Microsoft.Xna.Framework.Color.White);


                // ecriture indications 
                spriteBatch.DrawString(sprite_font, langue.francais[8], new Vector2(position_rejouer.X + rejouer.Width / 2 - sprite_font.MeasureString(langue.francais[8]).X / 2, position_rejouer.Y + rejouer.Height / 2 - sprite_font.MeasureString(langue.francais[8]).Y / 2), Microsoft.Xna.Framework.Color.White);
                spriteBatch.DrawString(sprite_font, langue.francais[7], new Vector2(position_inter_vie.X + inter_vie.Width / 2 - sprite_font.MeasureString(langue.francais[7]).X / 2, position_inter_vie.Y + inter_vie.Height / 3 - sprite_font.MeasureString(langue.francais[8]).Y / 2), Microsoft.Xna.Framework.Color.White);
            }


            else if (pacman_state == PacmanState.finish)
            {
               
                // map
                tab.Afficheboulette(ref tab_boulette, spriteBatch);

                // pacman
                pacman.Draw(spriteBatch, gameTime);

                // fantomes
                fantome1.Draw(spriteBatch, fantome_1);
                fantome2.Draw(spriteBatch, fantome_2);
                fantome3.Draw(spriteBatch, fantome_3);
                fantome4.Draw(spriteBatch, fantome_4);

                // cadre à droite
                spriteBatch.Draw(cadre, new Vector2(700, 0), Microsoft.Xna.Framework.Color.White);

                // ecritre "pacman level x"
                spriteBatch.DrawString(sprite_font, langue.francais[11] + level, new Vector2(850 - sprite_font.MeasureString(langue.francais[11]).X /2, 0), Microsoft.Xna.Framework.Color.Yellow);

                // ecriture du score 
                spriteBatch.DrawString(sprite_font, langue.francais[3] + score, new Vector2(705, 50), Microsoft.Xna.Framework.Color.White);

                // ecriture des vies 
                spriteBatch.DrawString(sprite_font, langue.francais[4] + life, new Vector2(705, 100), Microsoft.Xna.Framework.Color.White);

                spriteBatch.Draw(inter_vie, position_inter_vie, Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(rejouer, position_rejouer, Microsoft.Xna.Framework.Color.White);


                // ecriture indications 
                spriteBatch.DrawString(sprite_font, langue.francais[10], new Vector2(position_rejouer.X + rejouer.Width / 2 - sprite_font.MeasureString(langue.francais[10]).X / 2, position_rejouer.Y + rejouer.Height / 2 - sprite_font.MeasureString(langue.francais[10]).Y / 2), Microsoft.Xna.Framework.Color.White);
                spriteBatch.DrawString(sprite_font, langue.francais[9], new Vector2(position_inter_vie.X + inter_vie.Width / 2 - sprite_font.MeasureString(langue.francais[9]).X / 2, position_inter_vie.Y + inter_vie.Height / 3 - sprite_font.MeasureString(langue.francais[9]).Y / 2), Microsoft.Xna.Framework.Color.White);
            }

            else if (pacman_state == PacmanState.pause)
            {


                tab.Afficheboulette(ref tab_boulette, spriteBatch);

                pacman.Draw(spriteBatch, gameTime);

                fantome1.Draw(spriteBatch, fantome_1);
                fantome2.Draw(spriteBatch, fantome_2);
                fantome3.Draw(spriteBatch, fantome_3);
                fantome4.Draw(spriteBatch, fantome_4);

                // cadre à droite
                spriteBatch.Draw(cadre, new Vector2(700, 0), Microsoft.Xna.Framework.Color.White);

                // ecritre "pacman level x"
                spriteBatch.DrawString(sprite_font, langue.francais[11] + level, new Vector2(850 - sprite_font.MeasureString(langue.francais[11]).X/2, 0), Microsoft.Xna.Framework.Color.Yellow);
                
                // ecriture vie plus score
                spriteBatch.DrawString(sprite_font, langue.francais[3] + score, new Vector2(705, 50), Microsoft.Xna.Framework.Color.White);

                spriteBatch.DrawString(sprite_font, langue.francais[4] + life, new Vector2(705, 100), Microsoft.Xna.Framework.Color.White);
            }
        }

        
        
      
    } 
}
