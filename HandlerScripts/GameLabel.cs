using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3Test.HandlerScripts
{
    public class GameLabel : GameObjectModel 
    {
        public string Text { get; protected set; }
        public SpriteFont TextFont { get; protected set ; }
        public Color TextColor { get; set; }



        public GameLabel(string name = "Label") : base(name)
        {
            TextColor = Color.Black;
            Text = "";
        }

        public void SetText(string text)
        {
            Text = text;
            Vector2 newSize = TextFont.MeasureString(text);
            int x = (int)newSize.X;
            int y = (int)newSize.Y;
            Rectangle.Size = new Point(x, y);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            TextFont = GlobalTemplate.GAME.Content.Load<SpriteFont>("Pecita");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);            
            GlobalTemplate.SPRITE_BATCH.DrawString(TextFont, Text, GetGlobalPosition().ToVector2(), TextColor);
        }

    }


}



