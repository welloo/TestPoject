using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Match3Test.HandlerScripts;

namespace Match3Test
{
    public class MainFrame : Game
    {
        private IGameObjectModel _activeLayout;
        private SpriteBatch SpriteBatch;

        public MainFrame()
        {
            GlobalTemplate.SetGame(this);
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GlobalTemplate.MAIN_LAYOUT_WIDTH,
                PreferredBackBufferHeight = GlobalTemplate.MAIN_LAYOUT_HEIGHT
            };            
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
            IsMouseVisible = true;
            _activeLayout =  new MainMenu();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _activeLayout.Initialize();
        }
        public void ChangeLayout(IGameObjectModel newScene)
        {
            if (newScene == null)
                return;
            newScene.Initialize();
            newScene.LoadContent();
            IGameObjectModel previusScene = _activeLayout;
            _activeLayout = newScene;
            previusScene?.Dispose();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GlobalTemplate.SetSpriteBatch(SpriteBatch);
            _activeLayout.LoadContent();
        }


        protected override void UnloadContent()
        {
            base.UnloadContent();
            _activeLayout?.UnloadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            GlobalTemplate.SPRITE_BATCH.Begin();
            base.Draw(gameTime);
            _activeLayout?.Draw(gameTime);
            GlobalTemplate.SPRITE_BATCH.End();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GlobalTemplate.LAST_MOUSE_STATE = GlobalTemplate.CURRENT_MOUSE_STATE;
            GlobalTemplate.CURRENT_MOUSE_STATE = Mouse.GetState();
            _activeLayout?.Update(gameTime);
        }
    }
}
