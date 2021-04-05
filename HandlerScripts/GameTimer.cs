using System;
using Microsoft.Xna.Framework;

namespace Match3Test.HandlerScripts
{
    public sealed class GameTimer : GameLabel
    {
        public event Action TimeEnded;
        
        private const float GamePlayTime = 60;
        
        public float RemainTime = GamePlayTime;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            RemainTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            SetText($"End of round: {RemainTime:F0}");
            if (RemainTime <= 0)
            {
                TimeEnded?.Invoke();
                TimeEnded = null;
                Dispose();
            }
        }
    }


}



