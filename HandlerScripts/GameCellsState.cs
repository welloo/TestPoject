using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3Test.HandlerScripts
{
    public interface IBonusCell
    {
        bool IsActivated { get; }
        void UseBonus();
    }
    public abstract class GameCellState
    {
        protected GameCell Cell;
        public GameCellState(GameCell cell)
        {
            Cell = cell;
        }

        public abstract void Start();
        public abstract void Update(GameTime gameTime);
    }

    public sealed class DestroyCellState : GameCellState
    {
        public DestroyCellState(GameCell cell) : base(cell)
        {
        }

        public override void Start()
        {
            Cell.CallAddScore();
            if (Cell is IBonusCell)
            {
                (Cell as IBonusCell).UseBonus();
            }
            else
                Cell.Dispose();
        }

        public override void Update(GameTime gameTime)
        {

        }

    }
    public sealed class InputCellState : GameCellState
    {
        public InputCellState(GameCell cell) : base(cell)
        {
        }


        public override void Start() { }

        public override void Update(GameTime gameTime)
        {
            if (Cell == null)
                return;
            if (Cell.Rectangle.Contains(GlobalTemplate.CURRENT_MOUSE_STATE.Position) && GlobalTemplate.CURRENT_MOUSE_STATE.LeftButton == ButtonState.Pressed && GlobalTemplate.LAST_MOUSE_STATE.LeftButton == ButtonState.Released)
            {
                Cell.CallCellSelected();
            }
        }
    }
    public sealed class MoveCellState : GameCellState
    {

        private bool IsOnPosition = false;
        public MoveCellState(GameCell Cell) : base(Cell)
        {

        }

        public override void Start()
        {

        }
        public override void Update(GameTime gameTime)
        {
            if (IsOnPosition)
            {
                Cell.CurrentState = new WaitCellState(Cell);
                return;
            }
            Vector2 moveDirection = (Cell.CurrentCell.GetPosition() - Cell.GetPosition()).ToVector2();
            moveDirection.Normalize();
            float moveLength = (Cell.CurrentCell.GetPosition() - Cell.GetPosition()).ToVector2().Length();
            if (moveLength <= GlobalTemplate.STANDART_GO_SPEED * gameTime.ElapsedGameTime.TotalSeconds)
            {
                Cell.SetPosition(Cell.CurrentCell.GetPosition());
                IsOnPosition = true;
            }
            else
            {
                Vector2 newMovementOfset = moveDirection * GlobalTemplate.STANDART_GO_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                int x = (int)newMovementOfset.X;
                int y = (int)newMovementOfset.Y;
                Cell.SetPosition(new Point(x, y) + Cell.GetPosition());
            }
        }
    }
    public sealed class WaitCellState : GameCellState
    {
        public WaitCellState(GameCell Cell) : base(Cell)
        {
        }

        public override void Start() { }
        public override void Update(GameTime gameTime) { }
    }
    public sealed class HorizontalLineCell : GameCell, IBonusCell
    {

        public bool IsActivated { get; private set; } = false;

        public HorizontalLineCell(eCellColor cellColor) : base(cellColor)
        {
            PointsForDestruction = GlobalTemplate.DESTROYER_POINTS;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            texture = GlobalTemplate.GAME.Content.Load<Texture2D>("HBonus");
        }

        public void UseBonus()
        {
            var gridNode = GetParent() as GridGameModel;
            var destroyer1 = new Destroyer(CurrentCell, new Point(1, 0));
            var destroyer2 = new Destroyer(CurrentCell, new Point(-1, 0));
            gridNode.AddChild(destroyer1);
            gridNode.AddChild(destroyer2);
            gridNode.Destroyers.Add(destroyer1);
            gridNode.Destroyers.Add(destroyer2);
            Dispose();
        }
    }
    public sealed class VerticalLineCell : GameCell, IBonusCell
    {

        public bool IsActivated { get; private set; } = false;


        public VerticalLineCell(eCellColor cellColor) : base(cellColor)
        {
            PointsForDestruction = GlobalTemplate.DESTROYER_POINTS;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            texture = GlobalTemplate.GAME.Content.Load<Texture2D>("VBonusLine");
        }

        public void UseBonus()
        {
            var gridNode = GetParent() as GridGameModel;
            var destroyer1 = new Destroyer(CurrentCell, new Point(0, 1));
            var destroyer2 = new Destroyer(CurrentCell, new Point(0, -1));
            gridNode.AddChild(destroyer1);
            gridNode.AddChild(destroyer2);
            gridNode.Destroyers.Add(destroyer1);
            gridNode.Destroyers.Add(destroyer2);
            Dispose();
        }
    }

}




