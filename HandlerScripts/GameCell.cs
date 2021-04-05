using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Match3Test.HandlerScripts
{
    public class GameCell : GameObjectModel
    {
        public enum eCellColor : byte
        {
            Red,
            Blue,
            Green,
            Yellow,
            Purple
        }
        private GameCellState currentState;

        public event Action<GameCell> CellSelected;
        public event Action<GameCell> CellDisposed;
        public event Action<int> CellScoreAdd;
        public readonly eCellColor CellColor;
        public float spriteOpacity = 1;
        public bool IsOnPosition = true;
        public GridCell CurrentCell
        {
            get => currentCell;

            set
            {
                if (currentCell == value) { return; }
                currentCell = value;
                IsOnPosition = false;
            }

        }        
        public GameCellState CurrentState
        {
            get => currentState;
            set
            {
                if (currentState != null && CurrentState.GetType() == value.GetType()) { return; }
                currentState = value;
                currentState?.Start();
            }
        }

        protected int PointsForDestruction = GlobalTemplate.STANDART_POINTS;

        protected Point positionInGrid = new Point(-1, -1);
        protected GridCell currentCell;
        protected Color spriteColor;
        protected Texture2D texture;

        public GameCell(eCellColor cellColor) : base("Cell")
        {
            CellColor = cellColor;
            spriteColor = ConvertCellColor(cellColor);

        }

        public void CallAddScore() => CellScoreAdd?.Invoke(PointsForDestruction);
        public override void Initialize()
        {
            base.Initialize();
            SetSize(GlobalTemplate.GRID_CELLS_SIZE, GlobalTemplate.GRID_CELLS_SIZE);
        }

        private Color ConvertCellColor(eCellColor cellColor)
        {
            switch (cellColor)
            {
                case eCellColor.Blue:
                    return Color.Blue;
                case eCellColor.Green:
                    return Color.Green;
                case eCellColor.Purple:
                    return Color.Purple;
                case eCellColor.Red:
                    return Color.Red;
                case eCellColor.Yellow:
                    return Color.Yellow;
                default:
                    return (Color.White);
            }
        }
        public override void LoadContent()
        {
            base.LoadContent();
            texture = GlobalTemplate.GAME.Content.Load<Texture2D>("StandartSprite");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GlobalTemplate.SPRITE_BATCH.Draw(texture, Rectangle, new Color(spriteColor, spriteOpacity));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CurrentState?.Update(gameTime);
        }

        public override void UnloadContent()
        {
            GridGameModel gridNode = GetParent() as GridGameModel;
            gridNode?.GameCells.Remove(this);
            base.UnloadContent();
        }


        public void CallCellSelected() => CellSelected?.Invoke(this);

        internal void SelectCell() => currentCell.OnSelect();

        internal void DeselectCell() => currentCell.ClearColor();
        public override void Dispose()
        {
            CellDisposed?.Invoke(this);
            base.Dispose();
        }
    }
}




