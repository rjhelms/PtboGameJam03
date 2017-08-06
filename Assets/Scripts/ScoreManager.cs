using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    protected ScoreManager() { }

    public int HitPoints = 3;
    public int DefaultHitPoints = 3;
    public int MaxHitPoints = 5;
    public int Score = 0;

    public int NewLifeScore = 5000;
    public int NextNewLifeScore = 5000;
    public void Reset()
    {
        HitPoints = DefaultHitPoints;
        Score = 0;
    }
}