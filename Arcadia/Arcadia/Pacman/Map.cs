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


    class Map
    {

        private Texture2D boulette_jaune;
        private Texture2D sans_boulette;
        private Texture2D mur_c;
        private Texture2D mur_h;
        private Texture2D mur_v;
        private Texture2D neant;
        private Texture2D grosse_boulette;

        int nb_boulette;
        public int nb_boul { get { return nb_boulette; } }

        public virtual void LoadContent(ContentManager content)
        {
            boulette_jaune = content.Load<Texture2D>("Pacman/boulettejaune");
            sans_boulette = content.Load<Texture2D>("Pacman/map2");
            mur_c = content.Load<Texture2D>("mur_c");
            mur_h = content.Load<Texture2D>("mur_h");
            mur_v = content.Load<Texture2D>("mur_v");
            neant = content.Load<Texture2D>("Pacman/map2");
            grosse_boulette = content.Load<Texture2D>("Pacman/grossebouelette");
        }

        // pour afficher la map
        public void Afficheboulette(ref int[,] tab_boulette, SpriteBatch spriteBatch)
        {
            

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (tab_boulette[i, j] == '0') { spriteBatch.Draw(boulette_jaune, new Vector2(i * 28, j * 28), Color.White); }
                    else if (tab_boulette[i, j] == '2') { spriteBatch.Draw(sans_boulette, new Vector2(i * 28, j * 28), Color.White); }
                    else if (tab_boulette[i, j] == 'c') { spriteBatch.Draw(mur_c, new Vector2(i * 28, j * 28), Color.White); }
                    else if (tab_boulette[i, j] == 'h') { spriteBatch.Draw(mur_h, new Vector2(i * 28, j * 28), Color.White); }
                    else if (tab_boulette[i, j] == 'v') { spriteBatch.Draw(mur_v, new Vector2(i * 28, j * 28), Color.White); }
                    else if (tab_boulette[i, j] == 'n') { spriteBatch.Draw(neant, new Vector2(i * 28, j * 28), Color.White); }
                    else if (tab_boulette[i, j] == '3') { spriteBatch.Draw(grosse_boulette, new Vector2(i * 28, j * 28), Color.White); }
                }
            }
        }


        // methode pour convertir string representant la map en tableau de la map 
        public void InitializeMaps(string map_string, ref int[,] tab_boulette)
        {
            int element_map = 0;
            nb_boulette = 0;
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                        tab_boulette[j,i] = Convert.ToInt32( map_string[element_map] );
                        element_map++;
                        if (tab_boulette[j, i] == '0' || tab_boulette[j, i] == '3') 
                            { nb_boulette = nb_boulette + 1; }

                      
                }
            }
        }

    }
}
