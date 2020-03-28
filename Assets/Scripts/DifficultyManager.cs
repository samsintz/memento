using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Invalid = 0,
    Easy = 1,
    Medium = 2, 
    Hard = 3,
}

[System.Serializable]
public class DifficultyManager
{
    public Difficulty difficulty;

    public float MapDifficultyToAISkill()
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                return 4f;
            case Difficulty.Medium:
                return 7f;
            case Difficulty.Hard:
                return 10f;
            default:
                Debug.LogError("Unable to map Difficulty to float for AISkill- invalid Difficulty");
                return 0;
        }
    }

    public float MapDifficultyToBallSpeed()
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                return 8f;
            case Difficulty.Medium:
                return 10f;
            case Difficulty.Hard:
                return 12f;
            default:
                Debug.LogError("Unable to map Difficulty to float for Ball speed- invalid Difficulty");
                return 0;
        }
    }
}
