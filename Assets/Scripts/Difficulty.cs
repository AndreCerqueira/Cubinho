using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}


public static class DifficultyExtensions
{
    public static float GetSpawnChance(this Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 0.55f; // 55% de chance
            case Difficulty.Medium:
                return 0.35f; // 35% de chance
            case Difficulty.Hard:
                return 0.1f; // 10% de chance
            default:
                return 0.5f; // Valor padrão de 50% de chance para qualquer outro caso
        }
    }
}
