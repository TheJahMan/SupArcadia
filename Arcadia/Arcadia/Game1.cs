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
using Arcadia.Space_Invaders;

namespace Arcadia
{
    


    public enum GameState
    {
        StartMenu,
        Option,
        Loading,
        Playing1,
        Playing2,
        Playing3,
        Paused,
        Exit,
        

    };

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //variable etat du jeu
        public static GameState gameState;
        public GameState GameState { get { return gameState; } }

        //variables positions souris
        MouseState mouseState;
        MouseState old_mouseState;

        //variable taille fenetre
        int screenWidth, screenHeight;
        //int screenHeight;

        //etat du clavier
        KeyboardState keyboard_state;
        KeyboardState old_kerboardState;

        // déclaration des classes
        private PacmanGame pacman;
        private Menu menu;
        private SpaceInvaders_game space_invaders;
        
        // sprite font 
        private SpriteFont sprite_font;

        // score 
        int score;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            // variable taille de l'écran
            screenWidth = Window.ClientBounds.Width;
            screenHeight = Window.ClientBounds.Height;

            // on initialise l'etat du jeu
            gameState = GameState.StartMenu;

            //initialisation menu 
            menu = new Menu();


            // initialisation pacman 
            pacman = new PacmanGame();

            // init space invaders
            space_invaders = new SpaceInvaders_game();

            /*old_mouseState = Mouse.GetState();
            old_kerboardState = Keyboard.GetState();*/

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // chargement du menu
            menu.LoadContent(Content);
            menu.Initialize(screenWidth, screenHeight);

            // Chargement Option

            // chargment du pacman
            pacman.LoadContent(Content);

            // chargelent du font
            sprite_font = Content.Load<SpriteFont>("MaPolice");

            // chargment du pacman
            pacman.LoadContent(Content);

            // chargement du space invaders
            

            

        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            

            // variable état du clavier
            keyboard_state = Keyboard.GetState();

            //variable état de la souris
            mouseState = Mouse.GetState();

            // deroulement du jeu si gamestate == menu
            if (gameState == GameState.StartMenu)
            {

                if (menu.Udapte(gameTime, mouseState, gameState, old_mouseState) == 0)
                    gameState = GameState.StartMenu;

                if (menu.Udapte(gameTime, mouseState, gameState,old_mouseState) == 1)
                {
                    gameState = GameState.Playing1;

                    // initialisation du pacman
                    pacman.Initialize("map.txt", "map.bmp",1);
                    score = 0;

                    

                }

                if (menu.Udapte(gameTime, mouseState, gameState,old_mouseState) == 6)
                    Exit();
            }

            //deroulement du jeu si gamestate == pacman
            else if (gameState == GameState.Playing1)
            {
                if (!pacman.check_retour_menu)
                {
                    // pacman
                    if (!pacman.check_level_next)
                        pacman.Update(gameTime, keyboard_state, mouseState, gameState, old_mouseState, old_kerboardState, "map.bmp", ref score,1);
                    else
                    {
                        gameState = GameState.Playing2;
                        pacman.Initialize("map2.txt" , "map2.bmp",2);
                    }
                }
                else gameState = GameState.StartMenu;

            }
                // deroulement du jeux pour pacman lvl 2
             else if (gameState == GameState.Playing2 )
            {
                if (!pacman.check_retour_menu)
                    pacman.Update(gameTime, keyboard_state, mouseState, gameState, old_mouseState, old_kerboardState, "map2.bmp", ref score,2);
                else
                    gameState = GameState.StartMenu;

                if (pacman.check_level_next)
                {
                    gameState = GameState.Playing3;

                    // initialisation space invaders
                    space_invaders.Initialize(Content);
                }
            }
           else if (gameState == GameState.Playing3) 
            {
                space_invaders.Update(gameTime);
            }
              
            base.Update(gameTime);

            old_kerboardState = keyboard_state;
            old_mouseState = mouseState;
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            if (gameState == GameState.StartMenu)
            {
                //GraphicsDevice.Clear(Color.Black);
                menu.Draw(spriteBatch, gameTime);
            }

            else if (gameState == GameState.Playing1)
            {
                pacman.Draw(spriteBatch, gameTime, ref score, 1);
               // GraphicsDevice.Clear(Color.White);
            }
            else if (gameState == GameState.Playing2)
            {
                pacman.Draw(spriteBatch, gameTime, ref score,2);
               // GraphicsDevice.Clear(Color.White);

            }
            else if (gameState == GameState.Playing3)
            {
                space_invaders.Draw(spriteBatch, gameTime, 0, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}