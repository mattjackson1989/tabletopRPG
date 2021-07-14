using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnMechanic : MonoBehaviour
{
    public int Round;

    private bool InitiativeReady;

    public int TurnNumberLimit;
    public int CurrentTurnNumber;

    public List<GameObject> Enemies;
    public List<GameObject> Players;
    public List<GameObject> InitiativeList;
    // Start is called before the first frame update
    void Start()
    {
        Round = 0;
        CurrentTurnNumber = 0;
        TurnNumberLimit = Players.Count + Enemies.Count;
        InitiativeReady = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckInitiativeStep())
        {
            ProcessTurn();
        }

    }

    private void ProcessTurn()
    {
        // Check to see if we need to switch turns
        if (InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().TotalActionPointsLeft <= 0)
        {
            InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().IsTurnActive = false;
            InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().TotalActionPointsLeft = InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().CreatureStats.ActionPoint;
            Behaviour newHalo = (Behaviour)InitiativeList[CurrentTurnNumber].GetComponent("Halo");
            newHalo.enabled = false;

            PlayerSelectionGlobalMechanics.isPieceSelected = true;

            if (PlayerSelectionGlobalMechanics.CurrentSelectedPiece)
            {
                PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = false;
            }

            if (CurrentTurnNumber + 1 >= TurnNumberLimit)
            {
                CurrentTurnNumber = 0;
                Round++;

                InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().IsTurnActive = true;

                PlayerSelectionGlobalMechanics.CurrentSelectedPiece = InitiativeList[CurrentTurnNumber];
                PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = true;

                newHalo = (Behaviour)InitiativeList[CurrentTurnNumber].GetComponent("Halo");
                newHalo.enabled = true;

                return;
            }
            CurrentTurnNumber++;

            InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().IsTurnActive = true;

            PlayerSelectionGlobalMechanics.CurrentSelectedPiece = InitiativeList[CurrentTurnNumber];
            PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = true;

            newHalo = (Behaviour)InitiativeList[CurrentTurnNumber].GetComponent("Halo");
            newHalo.enabled = true;
        }
    }

    private bool CheckInitiativeStep()
    {
        if (!InitiativeReady)
        {
            InitiativeReady = CheckIfInitiativeIsReady();

            if (InitiativeReady)
            {
                SortPiecesByInitiative();
                InitiativeList[CurrentTurnNumber].GetComponent<GenericCreature>().IsTurnActive = true;

                PlayerSelectionGlobalMechanics.CurrentSelectedPiece = InitiativeList[CurrentTurnNumber];
                PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = true;

                Behaviour newHalo = (Behaviour)InitiativeList[CurrentTurnNumber].GetComponent("Halo");
                newHalo.enabled = true;
            }
        }

        return InitiativeReady;
    }

    private void SortPiecesByInitiative()
    {
        // sort by initiative
        // Mix in players and enemies based on their initiative score (highest gets priority)
        InitiativeList.AddRange(Players);
        InitiativeList.AddRange(Enemies);

        InitiativeList.Sort((x, y)=> x.GetComponent<GenericCreature>().CurrentInitiative.CompareTo(y.GetComponent<GenericCreature>().CurrentInitiative));
        InitiativeList.Reverse();
    }

    private bool CheckIfInitiativeIsReady()
    {
        foreach (GameObject enemy in Enemies)
        {
            if (enemy.GetComponent<GenericCreature>().CreatureStats == null)
            {
                return false;
            }
        }

        foreach (GameObject player in Players)
        {
            if (player.GetComponent<GenericCreature>().CreatureStats == null)
            {
                return false;
            }
        }

        return true;
    }
}
