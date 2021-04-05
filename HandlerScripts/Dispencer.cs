using System.Collections.Generic;



namespace Match3Test.HandlerScripts
{
    public class Dispencer : GameObjectModel
    {
        private int Colomn;
        private Stack<GameCell> CellsOrder;

        public Dispencer(int colomn) : base("Dispencer")
        {
            Colomn = colomn;
            CellsOrder = new Stack<GameCell>();
        }
        public void ClearOrder()=> CellsOrder.Clear();
        public override void Initialize()
        {
            base.Initialize();
            SetPosition((Colomn) * GlobalTemplate.GRID_CELLS_SIZE, -200);
        }

        public void AddToOrder(GameCell Cell)
        {
            CellsOrder.Push(Cell);
            Cell.spriteOpacity = 0;
        }


        public void DispenceCells()
        {
            int count = CellsOrder.Count;
            for (int i = 0; i < count; i++)
            {
                var Cell = CellsOrder.Pop();
                Cell.SetPosition(GetPosition().X, GetPosition().Y - i * GlobalTemplate.GRID_CELLS_SIZE);
                Cell.spriteOpacity = 1;
            }

        }

    }
}

