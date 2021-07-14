using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragMovement : MonoBehaviour
{
    public float dragSpeed = 0.2f;
    private Vector3 dragOrigin;

    private float dist;
    private Vector3 MouseStart, MouseMove;
    private Vector3 derp;

    public float SensitivityX;
    public float SensitivityY;
    public float TargetAngleX;
    public float TargetAngleY;

    private void Start()
    {
        dist = transform.position.z;  // Distance camera is above map
        SensitivityX = 1000;
        SensitivityY = 1000;
}

    void Update()
    {
        if(CheckIfPlayerIsHoldingPiece()) return;

        CheckXAndYMousePosition();
        CheckRotationOfCamera();
        CheckCameraZoom();

    }

    private bool CheckIfPlayerIsHoldingPiece()
    {
        if (GamePieceControl.isGrabbingPiece)
        {
            return true;
        }

        return false;
    }

    const float DRAG_SPEED = -1.0f;
    private void CheckXAndYMousePosition()
    {
        if (Input.GetMouseButtonDown(2))
        {
            MouseStart = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            MouseMove = new Vector3(Input.mousePosition.x - MouseStart.x, Input.mousePosition.y - MouseStart.y, transform.position.z);
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
            transform.position = new Vector3(transform.position.x + MouseMove.x * Time.deltaTime * DRAG_SPEED, transform.position.y + MouseMove.y * Time.deltaTime * DRAG_SPEED, transform.position.z);
        }
    }

    private void CheckRotationOfCamera()
    {
        if (Input.GetMouseButton(1))
        {
            TargetAngleX += Input.GetAxis("Mouse X") * SensitivityX * Time.deltaTime;
            TargetAngleY += Input.GetAxis("Mouse Y") * SensitivityY * Time.deltaTime;

            // Make limits on vertical rotation
            TargetAngleY = Mathf.Clamp(TargetAngleY, -90, 90);

            transform.eulerAngles = new Vector3(TargetAngleY, TargetAngleX, 0);
        }
    }

    void CheckCameraZoom()
    {
        Camera camera = this.gameObject.GetComponent<Camera>();
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !Input.GetMouseButton(2))
        {
            transform.Translate(transform.forward, Space.World);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !Input.GetMouseButton(2))
        {
            transform.Translate(-transform.forward, Space.World);
        }
    }
}
