using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3Test.HandlerScripts
{
    public class MainMenu : GameObjectModel
    {
        private GameButton StartGameButton;
        private GameButton ExitButton;

        public MainMenu() : base("MainMenu")
        {
            StartGameButton = new GameButton();
            StartGameButton.SetPosition(GlobalTemplate.START_BUTTON_LOCATION.X, GlobalTemplate.START_BUTTON_LOCATION.Y);
            StartGameButton.SetSize(GlobalTemplate.BUTTON_SIZE.X, GlobalTemplate.BUTTON_SIZE.Y);
            StartGameButton.Name = "StartGameButton";
            StartGameButton.OnButtonPressed += () => GlobalTemplate.GAME.ChangeLayout(new GameplayScene());
            AddChild(StartGameButton);

            ExitButton = new GameButton();
            ExitButton.SetPosition(GlobalTemplate.EXIT_BUTTON_LOCATION.X, GlobalTemplate.EXIT_BUTTON_LOCATION.Y);
            ExitButton.SetSize(GlobalTemplate.BUTTON_SIZE.X, GlobalTemplate.BUTTON_SIZE.Y);
            ExitButton.Name = "ExitGameButton";
            ExitButton.OnButtonPressed += () => GlobalTemplate.GAME.Exit() ;
            AddChild(ExitButton);
        }


        public override void LoadContent()
        {
            GetModel<GameButton>("StartGameButton").SetTexture(GlobalTemplate.GAME.Content.Load<Texture2D>("StartGameBTN"));
            GetModel<GameButton>("ExitGameButton").SetTexture(GlobalTemplate.GAME.Content.Load<Texture2D>("ExitGameBTN"));
        }
        public override void Initialize()=> base.Initialize();
        public override void Update(GameTime gameTime) => base.Update(gameTime);
        public override void Draw(GameTime gameTime) => base.Draw(gameTime);
        public override void UnloadContent()=> base.UnloadContent();
    }
}
