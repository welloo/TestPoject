using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3Test.HandlerScripts
{
    public class GameButton : GameObjectModel
    {

        public event Action OnButtonPressed;

        private Texture2D backgroundTexture;

        public GameButton() : base("UiButon")
        {
            
        }



        public void SetTexture(Texture2D texture) => backgroundTexture = texture;

        public override void Initialize() => base.Initialize();

        public override void LoadContent() => base.LoadContent();

        public override void UnloadContent() => base.UnloadContent();

        public override void Draw(GameTime gameTime)
        {

            GlobalTemplate.SPRITE_BATCH.Draw(backgroundTexture, Rectangle, Color.White);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Rectangle.Contains(GlobalTemplate.CURRENT_MOUSE_STATE.Position) && 
                                   GlobalTemplate.CURRENT_MOUSE_STATE.LeftButton == ButtonState.Pressed && 
                                   GlobalTemplate.LAST_MOUSE_STATE.LeftButton == ButtonState.Released)
            {
                OnButtonPressed?.Invoke();
            }

        }
    }

}

