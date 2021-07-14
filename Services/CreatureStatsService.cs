using UnityEditor;
using UnityEngine;


public static class CreatureStatsService
{
    public static CreatureModel GetCreatureStats()
    {
        // TODO: Currently Mocked....this needs to call an API eventually
        return new CreatureModel()
        {
            Name = "Generic Creature " + Random.Range(1, 100),
            Initiative = Random.Range(1, 20),
            Health = 3,
            ActionPoint = 2,
            Attack = 1,
            Defense = 0,
            Speed = 3
        };
    }
}
