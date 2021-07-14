using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMechanic : MonoBehaviour
{
    public bool IsCurrentTurn;
    public bool ShouldSkipTurn;
    // Start is called before the first frame update
    void Start()
    {
        IsCurrentTurn = false;
        ShouldSkipTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
