using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceControl : MonoBehaviour
{
    public static bool isGrabbingPiece;

    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 startPos;

    GameObject CurrentCubeBelowPiece;
    GameObject PreviousCubeBelowPiece;

    public bool IsSelected = false;

    private GenericCreature MyCreatureStats;

    Color oldColor;

    public bool FinalMoveAdjustment { get; private set; }

    void Start()
    {
        isGrabbingPiece = false;
        startPos = transform.position;
        MyCreatureStats = GetComponent<GenericCreature>();
    }

    // Start is called before the first frame update
    void OnMouseDown()
    {
        if (GetComponent<GenericCreature>().CreatureStats.ActionPoint > 0 && MyCreatureStats.IsTurnActive)
        {
            isGrabbingPiece = true;
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
            startPos = transform.position;
        }
    }

    private void OnMouseUp()
    {
        isGrabbingPiece = false;
        if (MyCreatureStats.IsTurnActive)
        {
            if (CurrentCubeBelowPiece != null)
            {
                transform.position = new Vector3(CurrentCubeBelowPiece.gameObject.transform.position.x, startPos.y, CurrentCubeBelowPiece.gameObject.transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
            }
        }
    }

    void Update()
    {
        if (GetComponent<GenericCreature>().CreatureStats.ActionPoint > 0 && MyCreatureStats.IsTurnActive)
        {
            CheckIfClickedByMouse();
            PieceOutOfBounds();
            CheckKeyboardInputMovement();
        }
    }

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Update is called once per frame
    void OnMouseDrag()
    {
        if (MyCreatureStats.IsTurnActive)
        {
            transform.position = GetMouseWorldPos() + mOffset;
        }
    }

    void PieceOutOfBounds()
    {
        if (gameObject.transform.position.y < 0  && !isGrabbingPiece)
        {
            gameObject.transform.position = startPos;
        }

    }
    void CheckKeyboardInputMovement()
    {
        if (IsSelected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && MyCreatureStats.TotalSpeedLeft > 0)
            {
                transform.position += new Vector3(0, 0, 1);
                MyCreatureStats.TotalSpeedLeft--;

                if (MyCreatureStats.TotalSpeedLeft <= 0)
                {
                    MyCreatureStats.TotalActionPointsLeft--;

                    if (MyCreatureStats.TotalActionPointsLeft >= 0)
                    {
                        MyCreatureStats.TotalSpeedLeft = MyCreatureStats.CreatureStats.Speed;

                        FinalMoveAdjustment = true;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && MyCreatureStats.TotalSpeedLeft > 0)
            {
                transform.position += new Vector3(0, 0, -1);
                MyCreatureStats.TotalSpeedLeft--;

                if (MyCreatureStats.TotalSpeedLeft <= 0)
                {
                    MyCreatureStats.TotalActionPointsLeft--;

                    if (MyCreatureStats.TotalActionPointsLeft >= 0)
                    {
                        MyCreatureStats.TotalSpeedLeft = MyCreatureStats.CreatureStats.Speed;

                        FinalMoveAdjustment = true;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && MyCreatureStats.TotalSpeedLeft > 0)
            {
                transform.position += new Vector3(1, 0, 0);
                MyCreatureStats.TotalSpeedLeft--;

                if (MyCreatureStats.TotalSpeedLeft <= 0)
                {
                    MyCreatureStats.TotalActionPointsLeft--;

                    if (MyCreatureStats.TotalActionPointsLeft >= 0)
                    {
                        MyCreatureStats.TotalSpeedLeft = MyCreatureStats.CreatureStats.Speed;

                        FinalMoveAdjustment = true;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && MyCreatureStats.TotalSpeedLeft > 0)
            {
                transform.position += new Vector3(-1, 0, 0);
                MyCreatureStats.TotalSpeedLeft--;

                if (MyCreatureStats.TotalSpeedLeft <= 0)
                {
                    MyCreatureStats.TotalActionPointsLeft--;

                    if (MyCreatureStats.TotalActionPointsLeft >= 0)
                    {
                        MyCreatureStats.TotalSpeedLeft = MyCreatureStats.CreatureStats.Speed;

                        FinalMoveAdjustment = true;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, 1000))
        {
            // For debugging
            if (CurrentCubeBelowPiece != hit.collider.gameObject)
            {
                Debug.Log("Moved to new tile: " + hit.collider.gameObject);
            }
            PreviousCubeBelowPiece = CurrentCubeBelowPiece;

            if (PreviousCubeBelowPiece)
            {
                PreviousCubeBelowPiece.GetComponent<TileMechanics>().IsOccupied = false;
            }

            CurrentCubeBelowPiece = hit.collider.gameObject;
        }
        else
        {
            if (MyCreatureStats.IsTurnActive || FinalMoveAdjustment == true) //TODO: can end turn on invalid square on last move because
            {
                Debug.Log("Invalid Move! Try moving a different location.");
                if (FinalMoveAdjustment == true)
                {
                    FinalMoveAdjustment = false;
                }

                MyCreatureStats.TotalSpeedLeft++;
                float yLocation = transform.position.y;
                transform.position = new Vector3(CurrentCubeBelowPiece.transform.position.x, yLocation, CurrentCubeBelowPiece.transform.position.z);
            }
        }
    }

    void CheckIfClickedByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // TODO: eventually make something that will make it known that this is different but they are in fact grabable board pieces - problem is that you can select multiple pieces as the active piece
                if (hit.transform.gameObject.tag == "BoardPiece")
                {
                    PlayerSelectionGlobalMechanics.isPieceSelected = true;

                    if (PlayerSelectionGlobalMechanics.CurrentSelectedPiece)
                    {
                        PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = false;
                    }

                    PlayerSelectionGlobalMechanics.CurrentSelectedPiece = hit.transform.gameObject;


                    PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = true;
                }
                else
                {
                    PlayerSelectionGlobalMechanics.isPieceSelected = false;

                    PlayerSelectionGlobalMechanics.CurrentSelectedPiece.GetComponent<GamePieceControl>().IsSelected = false;
                    PlayerSelectionGlobalMechanics.CurrentSelectedPiece = null;
                }
            }
        }
    }
}
