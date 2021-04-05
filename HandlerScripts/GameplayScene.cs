using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Match3Test.HandlerScripts
{
    public class GameplayScene : GameObjectModel
    {
        private GridGameModel GridNode;
        private GameTimer GameTimer;
        private GameScores PlayerScoreSystem;


        public GameplayScene() : base("GameScene")
        {
            GridNode = new GridGameModel();
            GameTimer = new GameTimer();
            GameTimer.TimeEnded += OnGameTimeEnds;
            PlayerScoreSystem = new GameScores();
            GridNode.OnScoreAdded += PlayerScoreSystem.AddScore;
            
            AddChild(PlayerScoreSystem);
            AddChild(GridNode);
            AddChild(GameTimer);
        }

        

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
            GameTimer.SetPosition(GlobalTemplate.TIMER_LOCATION.X, GlobalTemplate.TIMER_LOCATION.Y);
            PlayerScoreSystem.SetPosition(GlobalTemplate.PLAYER_SCORE_GAME_LOCATION.X, GlobalTemplate.PLAYER_SCORE_GAME_LOCATION.Y);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void OnGameTimeEnds()
        {
            PlayerScoreSystem.SetGlobalPosition(GlobalTemplate.PLAYER_SCORE_END_GAME_LOCATION.X, GlobalTemplate.PLAYER_SCORE_END_GAME_LOCATION.Y);
            GridNode.Dispose();

            Texture2D exitButtonTexture = GlobalTemplate.GAME.Content.Load<Texture2D>("AcceptGameBTN");
            GameButton OkButton;
            OkButton = new GameButton();
            OkButton.SetGlobalPosition(GlobalTemplate.OK_BUTTON_LOCATION.X, GlobalTemplate.OK_BUTTON_LOCATION.Y);
            OkButton.SetSize(GlobalTemplate.BUTTON_SIZE.X, GlobalTemplate.BUTTON_SIZE.Y);
            OkButton.Name = "ExitButton";
            OkButton.OnButtonPressed += () => GlobalTemplate.GAME.ChangeLayout(new MainMenu());
            OkButton.SetTexture(exitButtonTexture);
            AddChild(OkButton);

        }

    }

  
}
