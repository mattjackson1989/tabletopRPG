using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCreature : MonoBehaviour
{
    public CreatureModel CreatureStats;

    public bool IsTurnActive;
    public int CurrentInitiative;
    public int TotalSpeedLeft;
    public int TotalActionPointsLeft;
    public int TotalHitPointsLeft;

    void AttackAction()
    {

    }

    void BlockAction()
    {

    }

    void GainHealth()
    {

    }

    void LoseHealth()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        IsTurnActive = false;
        // The stats will get loaded in via an API that stores a creature card on a DB  
        CreatureStats = CreatureStatsService.GetCreatureStats();

        CurrentInitiative = CreatureStats.Initiative;
        TotalActionPointsLeft = CreatureStats.ActionPoint;
        TotalHitPointsLeft = CreatureStats.Health;
        TotalSpeedLeft = CreatureStats.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
