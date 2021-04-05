

namespace Match3Test.HandlerScripts
{
    public interface IGridState
    {
        void Start(GridGameModel gridNode);

        void Update(GridGameModel gridNode);
    }
    public static class GridStates
    {
        public class StartGridState : IGridState
        {
            public void Start(GridGameModel grid)
            {
                grid.CurrentGridState = null;
                grid.FillEmptySpaceInGrid();
                while (grid.HasMatchesOnStart())
                {
                    grid.GameCells.Clear();
                    foreach (Dispencer dispencer in grid.Dispencers)
                    {
                        dispencer.ClearOrder();
                    }
                    foreach (var cell in grid.Cells)
                    {
                        cell.ClearCell();
                    }
                    grid.FillEmptySpaceInGrid();
                }
                grid.DispenceNewCells();
                grid.CurrentGridState = new MoveGridState();
            }

            public void Update(GridGameModel grid)
            {

            }
        }
        public class SpawnAdnDropGridState : IGridState
        {
            public void Start(GridGameModel grid)
            {
                grid.SpawnBonusCells();
                grid.DropCells();
                grid.FillEmptySpaceInGrid();
                grid.DispenceNewCells();
                grid.CurrentGridState = new MoveGridState();
            }

            public void Update(GridGameModel grid)
            {

            }
        }
        public class MoveGridState : IGridState
        {
            public void Start(GridGameModel grid)
            {
                foreach (GameCell Cell in grid.GameCells)
                {
                    Cell.CurrentState = new MoveCellState(Cell);
                }
            }

            public void Update(GridGameModel gridNode)
            {

                if (!gridNode.CellsAreMoving())
                {
                    gridNode.CurrentGridState = new CellMatchState();
                }
            }
        }
        public class InputGridState : IGridState
        {
            public void Start(GridGameModel gridNode)
            {
                foreach (GameCell Cell in gridNode.GameCells)
                {
                    Cell.CurrentState = new InputCellState(Cell);
                }
            }

            public void Update(GridGameModel gridNode)
            {

            }
        }
        public class DestroyCellsGridState : IGridState
        {
            public void Start(GridGameModel gridNode)
            {
                gridNode.DestroyMatchedCells();
            }

            public void Update(GridGameModel gridNode)
            {
                if (gridNode.Destroyers.Count != 0 || gridNode.HasDestructingCells()) { return; }
                gridNode.CurrentGridState = new SpawnAdnDropGridState();

            }
        }
        public class CellMatchState : IGridState
        {
            public void Start(GridGameModel gridNode)
            {

                if (gridNode.CheckMatch())
                {
                    gridNode.lastMovedGameCells.Clear();
                    gridNode.CurrentGridState = new DestroyCellsGridState();
                }
                else if (gridNode.lastMovedGameCells.Count == 2)
                {
                    gridNode.SwapBack();
                    gridNode.CurrentGridState = new MoveGridState();
                }
                else
                    gridNode.CurrentGridState = new InputGridState();


            }

            public void Update(GridGameModel gridNode)
            {
            }
        }
    }

}
