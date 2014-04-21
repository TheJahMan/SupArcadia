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

namespace Arcadia
{
    class Menu
    {

        Langue langue = new Langue();

        // variables bouttons menus 
        private Texture2D start_button;
        private Texture2D exit_button;
        private Texture2D option_button;
        private Texture2D arcadia_logo;



        //variables positionsn bouton menu 
        private Vector2 start_position;
        private Vector2 exit_position;
        private Vector2 option_position;



        //Font position & declaration
        public SpriteFont langueFont;
        public Vector2 demarrert_pos;
        public Vector2 optiont_pos;
        public Vector2 exitt_pos;

        public virtual void Initialize(int screenWidth, int screenHeight)
        {
            //initialisation des positions des bouttons du menu 
            start_position = new Vector2(0, screenHeight * 1 / 3 +150);
            option_position = new Vector2(0, start_position.Y + 100);
            exit_position = new Vector2(0, option_position.Y  + 100);
            //Initialisation des positions des textes
            demarrert_pos = new Vector2(screenWidth / 2 - langueFont.MeasureString(langue.francais[0]).X / 2, start_position.Y + langueFont.MeasureString(langue.francais[0]).Y / 2);
            optiont_pos = new Vector2(screenWidth / 2 - langueFont.MeasureString(langue.francais[1]).X / 2, option_position.Y + langueFont.MeasureString(langue.francais[1]).Y / 2);
            exitt_pos = new Vector2(screenWidth / 2 - langueFont.MeasureString(langue.francais[2]).X / 2, exit_position.Y + langueFont.MeasureString(langue.francais[2]).Y / 2);

        }

        public virtual void LoadContent(ContentManager Content)
        {
            start_button = Content.Load<Texture2D>("Menu/bouton_start");
            exit_button = Content.Load<Texture2D>("Menu/Bouton_exit");
            option_button = Content.Load<Texture2D>("Menu/bouton_option");
            langueFont = Content.Load<SpriteFont>("LangueFont");
            arcadia_logo = Content.Load<Texture2D>("Menu/Arcadia Logo");

        }



        public int Udapte(GameTime gametime, MouseState mouseState, GameState gameState,MouseState old_mousestate )
        {



            // si souris sur boutton start et boutton gauche encoché alors start
            if (mouseState.X < (start_position.X + start_button.Width) && mouseState.X > (start_position.X - start_button.Width)
                && mouseState.Y < (start_position.Y + start_button.Height) && mouseState.Y > (start_position.Y - start_button.Height)
                && mouseState.LeftButton == ButtonState.Released && old_mousestate.LeftButton == ButtonState.Pressed)
                return 1;
                

            // si souris sur boutton exit et boutton gauche encoché alors exit
            else if (mouseState.X < (exit_position.X + exit_button.Width) && mouseState.X > (exit_position.X - exit_button.Width)
                && mouseState.Y < (exit_position.Y + exit_button.Height) && mouseState.Y > (exit_position.Y - exit_button.Height)
                && mouseState.LeftButton == ButtonState.Released && old_mousestate.LeftButton == ButtonState.Pressed)
                return 6;
            else return 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(arcadia_logo, new Vector2(500 - arcadia_logo.Width/2,-20), Color.White);
            spriteBatch.Draw(start_button, start_position, Color.White);
            spriteBatch.Draw(exit_button, exit_position, Color.White);
            spriteBatch.Draw(option_button, option_position, Color.White);
            spriteBatch.DrawString(langueFont, langue.francais[0], demarrert_pos, Microsoft.Xna.Framework.Color.Black);
            spriteBatch.DrawString(langueFont, langue.francais[1], optiont_pos, Microsoft.Xna.Framework.Color.Black);
            spriteBatch.DrawString(langueFont, langue.francais[2], exitt_pos, Microsoft.Xna.Framework.Color.Black);

        }

    }
}
