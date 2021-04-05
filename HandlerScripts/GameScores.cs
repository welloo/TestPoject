using System;


namespace Match3Test.HandlerScripts
{
    public sealed class GameScores : GameLabel
    {
        public int CurrentScore { get; private set; }
        public void Reset() => CurrentScore = 0;
        public override void LoadContent()
        {
            base.LoadContent();            
            SetText("Your score is " + CurrentScore.ToString());
        }
        public void AddScore(int amount)
        {
            CurrentScore += amount;
            SetText("Your score is " + CurrentScore.ToString());
        }

    }
}

