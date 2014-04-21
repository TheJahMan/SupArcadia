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
    public class Langue
    {
        
        public string[] francais = new string[12]
                                                {"Jouer",
                                                 "Options",
                                                 "Quitter",
                                                 "Score : ",
                                                 "Vie : ",
                                                "Continuer",
                                                "Vous avez perdu une vie",
                                                "GAME OVER !",
                                                "Retour menu",
                                                "Bravo !",
                                                "Suivant",
                                                "Pacman niveau "
                                                };
        public string[] english = new string[5]
                                               {"Start",
                                                "Option",
                                                "Exit",
                                                "Score : ",
                                                "Lifes : "};
        public string[] langue = new string[2]
                                               {"francais",
                                                "english"};
        
    }
}
