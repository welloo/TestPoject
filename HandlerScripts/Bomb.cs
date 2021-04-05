using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Match3Test.HandlerScripts
{
    public sealed class Bomb : GameCell, IBonusCell
    {

        List<GridCell> cellsToBlast = new List<GridCell>(9);
        
        public bool IsActivated { get; private set; } = false;

        private float TimeBeforeDetonate = GlobalTemplate.DETONATE_BOMB;
        public Bomb(eCellColor cellColor) : base(cellColor)
        {
            PointsForDestruction = GlobalTemplate.BOMB_POINTS;
        }


        public override void LoadContent()
        {
            base.LoadContent();
            texture = GlobalTemplate.GAME.Content.Load<Texture2D>("BombSprite");
        }

        public void UseBonus()
        {
            if (IsActivated) { return; }
            IsActivated = true;
            var grid = GetParent() as GridGameModel;
            for (int i = -1; i <= 1; i++)
            {
                if (currentCell.PositionInGrid.X + i < 0 || currentCell.PositionInGrid.X + i > GlobalTemplate.GRID_CULUMN_SIZE - 1)
                    continue;
                for (int j = -1; j <= 1; j++)
                {
                    if (0 > currentCell.PositionInGrid.Y + j || currentCell.PositionInGrid.Y + j > GlobalTemplate.GRID_ROW_SIZE - 1)
                        continue; 
                    var cell = grid.Cells[i + currentCell.PositionInGrid.X, currentCell.PositionInGrid.Y + j];
                    cellsToBlast.Add(cell);
                    cell.OnBang();
                }
            }
        }

        private void Detonate()
        {
            foreach (GridCell cell in cellsToBlast)
            {
                cell.DestroyCurrentCell();
                cell.ClearColor();
            }
            Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActivated)
            {
                if (TimeBeforeDetonate <= 0)
                {
                    Detonate();
                }
                TimeBeforeDetonate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

    }


}




