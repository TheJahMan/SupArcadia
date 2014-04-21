using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Arcadia.Space_Invaders
{
    class maison
    {

        private Texture2D mais;
        public Texture2D mais_public { get { return mais; } }
        bool is_alive_mais;


        public virtual void Initialize()
        {
            is_alive_mais = true;
        }
        public virtual void LoadContent(ContentManager content)
        {
            mais = content.Load<Texture2D>("SpaceInvaders/maison");


        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D ennemi, int pos_x, int pos_y)
        {
            //spriteBatch.Begin();
            spriteBatch.Draw(mais, new Vector2(pos_x, pos_y), Microsoft.Xna.Framework.Color.White);
            //spriteBatch.End();
        }


    }
}
