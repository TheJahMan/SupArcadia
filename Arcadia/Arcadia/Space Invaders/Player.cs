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

namespace Arcadia.Space_Invaders
{
    class Player
    {
        enum Ship_action
        {
            right,
            left,
            fire
        } ;

        //Images ship
        private Texture2D ship;
        private Texture2D fire;

        //ship position
        Vector2 ship_position;
        //ship action
        Ship_action ship_action;

        public Vector2 Ship_position { get { return ship_position; } }

        //Initialisation
        public Player()
        {
            this.ship_position.X = 700 / 2 - this.ship.Width / 2;
            this.ship_position.Y = 700 - 28 + this.ship.Height;
        }


        public virtual void LoadContent(ContentManager content)
        {
            ship = content.Load<Texture2D>("SpaceInvaders/ship");
        }
        public virtual void Update(GameTime gameTime, KeyboardState keyboard_state)
        {

            //Actions
            if (keyboard_state.IsKeyDown(Keys.Right)) ship_action = Ship_action.right;
            if (keyboard_state.IsKeyDown(Keys.Left)) ship_action = Ship_action.left;
            if (keyboard_state.IsKeyDown(Keys.Up) || keyboard_state.IsKeyDown(Keys.Space)) ship_action = Ship_action.fire;
            
            //Deplacements
            if (ship_action == Ship_action.right && this.ship_position.X + this.ship.Width < 700)
                this.ship_position.X = ship_position.X + 10;

            if (ship_action == Ship_action.left && this.ship_position.X > 0 )
                this.ship_position.X = ship_position.X - 10;
        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }
    }
}
