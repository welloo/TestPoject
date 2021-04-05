using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using static Match3Test.HandlerScripts.GameCell;
using static Match3Test.HandlerScripts.GridStates;

namespace Match3Test.HandlerScripts
{
    public sealed class GridGameModel : GameObjectModel
    {
        private struct GridData
        {
            public GridCell Cell;
            public eCellColor CellColor;

            public GridData(GridCell cell, eCellColor cellColor)
            {
                Cell = cell;
                CellColor = cellColor;
            }
        }
        private IGridState currentGridState;
        public event Action<int> OnScoreAdded; 
        public GridCell[,] Cells;
        public List<GameCell> GameCells;
        public Dispencer[] Dispencers;
        public List<Destroyer> Destroyers = new List<Destroyer>();
        public List<GameCell> LastMovedCell;

        private List<GridCell> cellsToClear = new List<GridCell>();
        private List<GridData> bombSpawnData = new List<GridData>();
        private List<GridData> hLineSpawnData = new List<GridData>();
        private List<GridData> vLineSpawnData = new List<GridData>();
        private Random random = new Random();
        private GameCell lastSelectedGameCell;
        public List<GameCell> lastMovedGameCells = new List<GameCell>(2);

        public IGridState CurrentGridState
        {
            get => currentGridState;
            set
            {
                currentGridState = value;
                currentGridState?.Start(this);
            }
        }

        public GridGameModel() : base("Grid")
        {
            Cells = new GridCell[GlobalTemplate.GRID_CULUMN_SIZE, GlobalTemplate.GRID_ROW_SIZE];
            GameCells = new List<GameCell>();
            LastMovedCell = new List<GameCell>();
            Dispencers = new Dispencer[GlobalTemplate.GRID_CULUMN_SIZE];
            SetGlobalPosition(100, 100);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
            CreateCellGrid();
            CreateDispencers();
            CurrentGridState = new StartGridState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CurrentGridState?.Update(this);
        }

        public void FillEmptySpaceInGrid()
        {
            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                for (int y = 0; y < GlobalTemplate.GRID_ROW_SIZE; y++)
                {                    
                    if (Cells[x, y].CurrentCell == null)
                        CreateRegularCell(Cells[x, y]);
                }
            }
        }

        public void DispenceNewCells()
        {
            foreach (var dispencer in Dispencers)
                dispencer.DispenceCells();
            
        }
        public void CreateRegularCell(GridCell cell)
        {

            eCellColor cellColor = (eCellColor)random.Next(Enum.GetNames(typeof(eCellColor)).Length);
            GameCell Cell = SetCellSettings(new GameCell(cellColor), cell);
            Cell.spriteOpacity = 0;
            Dispencers[cell.PositionInGrid.X].AddToOrder(Cell);

        }

        public void CreateBombCell(eCellColor cellColor, GridCell cell)=> SetCellSettings(new Bomb(cellColor), cell);

        public void CreateHLineCell(eCellColor cellColor, GridCell cell)=> SetCellSettings(new HorizontalLineCell(cellColor), cell);

        public void CreateVLineCell(eCellColor cellColor, GridCell cell)=> SetCellSettings(new VerticalLineCell(cellColor), cell);

        private GameCell SetCellSettings(GameCell Cell, GridCell cell)
        {
            AddChild(Cell);
            Cell.SetSize(GlobalTemplate.GRID_CELLS_SIZE, GlobalTemplate.GRID_CELLS_SIZE);
            cell.CurrentCell = Cell;
            GameCells.Add(Cell);
            Cell.SetPosition(cell.GetPosition());
            Cell.CellSelected += OnCellSelected;
            Cell.CellScoreAdd += (int amount) => OnScoreAdded?.Invoke(amount);
            Cell.CellDisposed += OnCellDestructed;
            return Cell;
        }

        public GridCell GetCellInPoint(Point position)
        {
            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                for (int y = 0; y < GlobalTemplate.GRID_CULUMN_SIZE; y++)
                {
                    GridCell cell = Cells[x, y];
                    if (cell.Rectangle.Contains(position)){ return cell; }
                }
            }
            return null;
        }

        private void OnCellDestructed(GameCell Cell)
        {
            GameCells.Remove(Cell);
        }


        private void CreateDispencers()
        {
            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                Dispencers[x] = new Dispencer(x);
                AddChild(Dispencers[x]);

            }
        }

        public bool CellsAreMoving()
        {
            foreach (GameCell cell in GameCells)
            {
                if (cell.CurrentState is MoveCellState)
                    return true;
            }
            return false;
        }



        private void CreateCellGrid()
        {
            
            for (int x = 0; x < GlobalTemplate.GRID_ROW_SIZE; x++)
            {
                for (int y = 0; y < GlobalTemplate.GRID_CULUMN_SIZE; y++)
                {
                    var cell = new GridCell(new Point(x, y));
                    AddChild(cell);
                    Cells[x, y] = cell;
                    cell.SetSize(GlobalTemplate.GRID_CELLS_SIZE, GlobalTemplate.GRID_CELLS_SIZE);
                    cell.SetPosition(new Point(GlobalTemplate.GRID_CELLS_SIZE * x, GlobalTemplate.GRID_CELLS_SIZE * y));
                }
            }
        }


        private void OnCellSelected(GameCell Cell)
        {
            if (lastSelectedGameCell is null)
            {
                lastSelectedGameCell = Cell;
                lastSelectedGameCell.SelectCell();
                return;
            }
            if (CellsAreNotNeibors(Cell, lastSelectedGameCell))
            {
                lastSelectedGameCell.DeselectCell();
                lastSelectedGameCell = null;
                return;
            }
            Cell.SelectCell();
            SwapCells(Cell, lastSelectedGameCell);
            lastSelectedGameCell.DeselectCell();
            Cell.DeselectCell();
            lastSelectedGameCell = null;
            CurrentGridState = new MoveGridState();
        }

        public bool HasMatchesOnStart()
        {
            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE - 2; x++)
            {
                for (int y = 0; y < GlobalTemplate.GRID_ROW_SIZE; y++)
                {
                    var currentColor = Cells[x, y].CurrentCell.CellColor;
                    if (currentColor == Cells[x + 1, y].CurrentCell.CellColor && currentColor == Cells[x + 2, y].CurrentCell.CellColor)
                    {
                        if (Cells[x + 1, y].CurrentCell.GetType() != typeof(IBonusCell) && Cells[x + 2, y].CurrentCell.GetType() != typeof(IBonusCell))
                        {
                            return true;
                        }

                    }
                }
            }
            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                for (int y = 0; y < GlobalTemplate.GRID_ROW_SIZE - 2; y++)
                {
                    var currentColor = Cells[x, y].CurrentCell.CellColor;
                    if (currentColor == Cells[x, y + 1].CurrentCell.CellColor && currentColor == Cells[x, y + 2].CurrentCell.CellColor)
                    {
                        if (Cells[x, y + 1].CurrentCell.GetType() != typeof(IBonusCell) && Cells[x, y + 2].CurrentCell.GetType() !=typeof(IBonusCell))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        private bool CellsAreNotNeibors(GameCell cell1, GameCell cell2)
        {
            var tempcell1 = cell1.CurrentCell;
            var tempcell2 = cell2.CurrentCell;
            if (cell1 == cell2) { return true; }
            if (Math.Abs(tempcell1.PositionInGrid.Y - tempcell2.PositionInGrid.Y) == 1 && (tempcell1.PositionInGrid.X == tempcell2.PositionInGrid.X))
                return false;
            if (Math.Abs(tempcell1.PositionInGrid.X - tempcell2.PositionInGrid.X) == 1 && (tempcell1.PositionInGrid.Y == tempcell2.PositionInGrid.Y))
                return false;
            return true;
        }

        public void SwapCells(GameCell cell1, GameCell cell2)
        {
            lastMovedGameCells.Clear();
            var tempcell1 = cell1.CurrentCell;
            var tempcell2 = cell2.CurrentCell;
            tempcell1.CurrentCell = cell2;
            tempcell2.CurrentCell = cell1;
            lastMovedGameCells.Add(cell1);
            lastMovedGameCells.Add(cell2);
        }

        public void SwapBack()
        {
            SwapCells(lastMovedGameCells[0], lastMovedGameCells[1]);
            lastMovedGameCells.Clear();
        }
        

        internal bool CheckMatch()
        {
            bool hasMatch = false;
            var hLines = new List<HorizontalLine>();
            var vLines = new List<VerticalLine>();
            var tempHLines = new List<HorizontalLine>();
            var tempVLines = new List<VerticalLine>();


            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                hLines.AddRange(GetHorizontalLines(x));
            }
            for (int y = 0; y < GlobalTemplate.GRID_ROW_SIZE; y++)
            {
                vLines.AddRange(GetVerticalLines(y));
            }

            if (hLines.Count != 0 || vLines.Count != 0)
                hasMatch = true;

            foreach (var horizontalLine in hLines)
            {
                foreach (var verticalLine in vLines)
                {
                    if (CheckCross(horizontalLine, verticalLine))
                    {
                        var cell = Cells[verticalLine.Cells[0].PositionInGrid.X, horizontalLine.Cells[0].PositionInGrid.Y];
                        var cellColor = cell.CurrentCell.CellColor;
                        cellsToClear.AddRange(horizontalLine.Cells);
                        cellsToClear.AddRange(verticalLine.Cells);
                        bombSpawnData.Add(new GridData(cell, cellColor));
                        tempHLines.Add(horizontalLine);
                        tempVLines.Add(verticalLine);
                    }
                }

            }
            hLines = hLines.Except(tempHLines).ToList();
            vLines = vLines.Except(tempVLines).ToList();

            foreach (var line in hLines)
            {
                cellsToClear.AddRange(line.Cells);
                if (line.Cells.Count >= 4)
                {
                    var cellToSpawnBonus = line.Cells[0];
                    if (lastMovedGameCells.Count == 2)
                    {
                        cellToSpawnBonus = line.Cells.Find(cell => cell == lastMovedGameCells[0].CurrentCell || cell == lastMovedGameCells[1].CurrentCell);
                    }
                    if (line.Cells.Count >= 5)
                        bombSpawnData.Add(new GridData(cellToSpawnBonus, cellToSpawnBonus.CurrentCell.CellColor));
                    else
                        hLineSpawnData.Add(new GridData(cellToSpawnBonus, cellToSpawnBonus.CurrentCell.CellColor));

                }
            }

            foreach (var line in vLines)
            {
                cellsToClear.AddRange(line.Cells);
                if (line.Cells.Count >= 4)
                {
                    var cellToSpawnBonus = line.Cells[0];
                    if (lastMovedGameCells.Count == 2)
                    {
                        cellToSpawnBonus = line.Cells.Find(cell => cell == lastMovedGameCells[0].CurrentCell || cell == lastMovedGameCells[1].CurrentCell);
                    }
                    if (line.Cells.Count >= 5)
                        bombSpawnData.Add(new GridData(cellToSpawnBonus, cellToSpawnBonus.CurrentCell.CellColor));
                    else
                        vLineSpawnData.Add(new GridData(cellToSpawnBonus, cellToSpawnBonus.CurrentCell.CellColor));
                }
            }
            return hasMatch;

        }

        internal void SpawnBonusCells()
        {
            foreach (var data in bombSpawnData)
            {
                CreateBombCell(data.CellColor, data.Cell);
            }

            foreach (var data in hLineSpawnData)
            {
                CreateHLineCell(data.CellColor, data.Cell);
            }

            foreach (var data in vLineSpawnData)
            {
                CreateVLineCell(data.CellColor, data.Cell);
            }
            bombSpawnData.Clear();
            hLineSpawnData.Clear();
            vLineSpawnData.Clear();
        }


        internal void DropCells()
        {
            for (int x = 0; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                for (int y = GlobalTemplate.GRID_ROW_SIZE - 1; y > 0; y--)
                {
                    if (Cells[x, y].CurrentCell is null)
                    {
                        var k = y - 1;
                        while (k >= 0 && Cells[x, k].CurrentCell is null)
                        {
                            k--;
                        }
                        if (k < 0) break;
                        var Cell = Cells[x, k].CurrentCell;
                        Cells[x, k].CurrentCell = null;
                        Cells[x, y].CurrentCell = Cell;
                    }
                }
            }
        }

        private bool CheckCross(HorizontalLine horizontalLine, VerticalLine verticalLine)
        {
            if (Enumerable.Range(horizontalLine.Cells[0].PositionInGrid.X, horizontalLine.Cells[0].PositionInGrid.X + horizontalLine.Cells.Count).Contains(verticalLine.Cells[0].PositionInGrid.X) )
            {
                if (Enumerable.Range(verticalLine.Cells[0].PositionInGrid.Y, verticalLine.Cells[0].PositionInGrid.Y + verticalLine.Cells.Count).Contains(horizontalLine.Cells[0].PositionInGrid.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public void DestroyMatchedCells()
        {
            var cellsNeededToClear = cellsToClear.Distinct();
            foreach (var cell in cellsNeededToClear)
            {
                cell.DestroyCurrentCell();

            }
            cellsToClear.Clear();
        }

        public  bool HasDestructingCells()
        {
            var cellList = GameCells.ToList();
            foreach (var Cell in cellList)
            {
                if (Cell.CurrentState.GetType() == typeof(DestroyCellState))                
                    return true;
            }
            return false;
        }


        private IEnumerable<HorizontalLine> GetHorizontalLines(int y)
        {
            var color = Cells[0, y].CurrentCell.CellColor;
            var line = new HorizontalLine(Cells[0, y]);
            for (int x = 1; x < GlobalTemplate.GRID_CULUMN_SIZE; x++)
            {
                if (color == Cells[x, y].CurrentCell.CellColor)
                {
                    line.Cells.Add(Cells[x, y]);
                }
                else
                {
                    if (line.Cells.Count >= 3)
                        yield return line;
                    line = new HorizontalLine(Cells[x, y]);
                    color = Cells[x, y].CurrentCell.CellColor;
                }
            }
            if (line.Cells.Count >= 3)
                yield return line;
        }

        private IEnumerable<VerticalLine> GetVerticalLines(int x)
        {
            var color = Cells[x, 0].CurrentCell.CellColor;
            var line = new VerticalLine(Cells[x, 0]);
            for (int y = 1; y < GlobalTemplate.GRID_ROW_SIZE; y++)
            {
                if (color == Cells[x, y].CurrentCell.CellColor)
                {
                    line.Cells.Add(Cells[x, y]);
                }
                else
                {
                    if (line.Cells.Count >= 3)
                        yield return line;
                    line = new VerticalLine(Cells[x, y]);
                    color = Cells[x, y].CurrentCell.CellColor;
                }
            }
            if (line.Cells.Count >= 3)
                yield return line;
        }

    }
}

