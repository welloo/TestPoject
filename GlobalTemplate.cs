using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3Test
{
    public static class GlobalTemplate
    {
        private static MouseState _mouseState;
        private static MouseState _lastMouseState;
        public static SpriteBatch SPRITE_BATCH { get; private set; }
        public static MouseState CURRENT_MOUSE_STATE { get => _mouseState; set => _mouseState = value; }
        public static MouseState LAST_MOUSE_STATE { get => _lastMouseState; set => _lastMouseState = value; }

        //Main window settings
        public static MainFrame GAME { get; private set; }
        public static int MAIN_LAYOUT_WIDTH = 800;
        public static int MAIN_LAYOUT_HEIGHT = 800;

        //Gameplay settings
        public static (int X, int Y) TIMER_LOCATION = (100, 50);
        public static (int X, int Y) PLAYER_SCORE_GAME_LOCATION = (500, 50);
        public static (int X, int Y) PLAYER_SCORE_END_GAME_LOCATION = (300, 400);
        public const int STANDART_GO_SPEED = 400;
        public const int STANDART_POINTS = 10;
        public const int DESTROYER_GO_SPEED = 700;
        public const int DESTROYER_POINTS = 30;
        public const float DETONATE_BOMB = 250f;
        public const int BOMB_POINTS = 45;

        //Grid settings
        public const int GRID_CELLS_SIZE = 75;
        public const int GRID_CULUMN_SIZE = 8;
        public const int GRID_ROW_SIZE = 8;

        //Buttons settings
        public static (int X, int Y) START_BUTTON_LOCATION = (300, 300);
        public static (int X, int Y) EXIT_BUTTON_LOCATION = (300, 475);
        public static (int X, int Y) OK_BUTTON_LOCATION = (300, 475);
        public static (int X, int Y) BUTTON_SIZE = (200, 100);


        public static void SetGame(MainFrame game) => GAME = game;
        public static void SetSpriteBatch(SpriteBatch spriteBatch) => SPRITE_BATCH = spriteBatch;
    }
}
