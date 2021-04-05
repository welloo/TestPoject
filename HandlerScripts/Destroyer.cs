using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Match3Test.HandlerScripts
{
    public class Destroyer : GameObjectModel
    {        
        private GridCell StartCell;
        private Texture2D Texture;
        private Point Direction;
        private Vector2 SpriteOrigin;
        private float Rotation;
        private SpriteEffects SpriteEffects;
        private Point CourseOfDestroyer;
        
        public Destroyer(GridCell startCell, Point direction) : base("Destroyer")
        {
            StartCell = startCell;
            Direction = direction;
            if (direction.X != 0)
            {
                Rotation = MathHelper.ToRadians(90);
                SpriteOrigin = new Vector2(0, GlobalTemplate.GRID_CELLS_SIZE);
                if (direction.X > 0)
                {
                    SpriteEffects = SpriteEffects.None;
                    CourseOfDestroyer = new Point(75, 35);
                }
                else if(direction.X < 0)
                {
                    SpriteEffects = SpriteEffects.FlipVertically;
                    CourseOfDestroyer = new Point(0, 35);
                }


            }
            else if (direction.Y != 0)
            {
                Rotation = MathHelper.ToRadians(0);
                SpriteOrigin = Vector2.Zero;
                if (direction.Y > 0)
                {
                    SpriteEffects = SpriteEffects.FlipVertically;
                    CourseOfDestroyer = new Point(35, 0);
                }
                else if(direction.Y < 0)
                {
                    SpriteEffects = SpriteEffects.None;
                    CourseOfDestroyer = new Point(35, 75);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MoveAndDestroy(gameTime);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Texture = GlobalTemplate.GAME.Content.Load<Texture2D>("DestroyerSprite");
            SetPosition(StartCell.GetPosition());
            SetSize(75, 75);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GlobalTemplate.SPRITE_BATCH.Draw(Texture, Rectangle, null, Color.Red, Rotation, SpriteOrigin, SpriteEffects, 0);
        }

        private void MoveAndDestroy(GameTime gameTime)
        {
            var vector = new Vector2(Direction.X *
                                     GlobalTemplate.DESTROYER_GO_SPEED *
                                     (float)gameTime.ElapsedGameTime.TotalSeconds, 
                                     Direction.Y * GlobalTemplate.DESTROYER_GO_SPEED *
                                     (float)gameTime.ElapsedGameTime.TotalSeconds);
            var newOffSet = new Point((int)vector.X, (int)vector.Y);
            SetPosition( GetPosition() + newOffSet);
            var cell = (GetParent() as GridGameModel).GetCellInPoint(GetGlobalPosition() + CourseOfDestroyer);
            if (cell == null)
            {
                (GetParent() as GridGameModel).Destroyers.Remove(this);
                Dispose();
                return;
            }
            cell.DestroyCurrentCell();
        }



    }


}




