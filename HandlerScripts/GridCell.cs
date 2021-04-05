using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Match3Test.HandlerScripts
{
    public class GridCell : GameObjectModel
    {
        private GameCell currentCell;
        public readonly Point PositionInGrid;
        private Texture2D texture;
        private Color Color = Color.White;
        public GameCell CurrentCell
        {
            get => currentCell; 
            set
            {
                currentCell = value;
                if (currentCell != null)
                    currentCell.CurrentCell = this;
            }
        }
        public GridCell(Point position) : base("Cell") => PositionInGrid = position;

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GlobalTemplate.SPRITE_BATCH.Draw(texture, Rectangle, Color);
        }

        public void OnBang() => Color = Color.MonoGameOrange;

        public void OnSelect() => Color = Color.YellowGreen;

        public void ClearColor() => Color = Color.White;

        public override void LoadContent()
        {
            base.LoadContent();
            texture = GlobalTemplate.GAME.Content.Load<Texture2D>("Cell");
        }

        public void ClearCell()
        {
            currentCell?.Dispose();
            currentCell = null;
        }

        public void DestroyCurrentCell()
        {
            if (currentCell != null && currentCell.CurrentState.GetType() != typeof(DestroyCellState))
                currentCell.CurrentState = new DestroyCellState(currentCell);
            currentCell = null;
        }
    }
}

