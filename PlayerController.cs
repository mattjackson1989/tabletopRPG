using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsMyTurn;

    public GameObject MyGamePiece;

    /// <summary>
    /// Use this if the player has other pieces it can control
    /// </summary>
    public List<GameObject> AuxillaryPieces;

    // Start is called before the first frame update
    void Start()
    {
        IsMyTurn = false;
        MyGamePiece.GetComponent<GenericCreature>().IsTurnActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsMyTurn = MyGamePiece.GetComponent<GenericCreature>().IsTurnActive;
    }
}
