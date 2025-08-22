using System;

public static class StatsController
{
    public static int CurrentScore = 0;

    public static int RecordScore = 0;

    public static int Score = 0;

    public static Action<int> OnScoreChange;

    public static void AddScore()
    {
        CurrentScore++;
        OnScoreChange?.Invoke(CurrentScore);
    }

    public static void ResetScore()
    {
        CurrentScore = Score;
    }
}
