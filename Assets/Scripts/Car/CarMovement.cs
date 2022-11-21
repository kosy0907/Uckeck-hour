using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CarMovement : MonoBehaviour
{

    Plane clickPlane = new Plane(Vector3.up, Vector3.zero);
    public float planeDistance = 0f;
    Vector3 clickOffset = Vector3.zero;

    BoxCollider col;
    public float carHalfSize = 0.5f;
    Vector3 gridOffset = Vector3.zero;
    Orientation orientation;

    Vector3 validBackPos = Vector3.negativeInfinity;
    Vector3 validFrontPos = Vector3.positiveInfinity;

    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;
    public float smoothTime = .1f;

    bool inputEnabled = true;

    bool isDrag = false;

    void Start()
    {
        col = GetComponent<BoxCollider>();
        targetPosition = transform.position;

        SetOrientation();
        SetGridOffset();
    }

    void Update()
    {
        GotoValidPosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void GotoValidPosition()
    {
        if (transform.position.z > validFrontPos.z + 0.25f)
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y, validFrontPos.z - 0.5f);
            isDrag = false;
        }
        else if (transform.position.z < validBackPos.z - 0.25f)
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y, validBackPos.z + 0.5f);
            isDrag = false;
        }
        else if (transform.position.x > validFrontPos.x + 0.25f)
        {
            targetPosition = new Vector3(validFrontPos.x - 0.5f, transform.position.y, transform.position.z);
            isDrag = false;
        }
        else if (transform.position.x < validBackPos.x - 0.25f)
        {
            targetPosition = new Vector3(validBackPos.x + 0.5f, transform.position.y, transform.position.z);
            isDrag = false;
        }
    }

    void OnDrawGizmos()
    {
        ShowValidMoveRange();
    }

    void OnMouseDown()
    {
        if (!inputEnabled)
            return;

        Ray ray = GetScreenPointToRay();

        if (clickPlane.Raycast(ray, out planeDistance))
        {
            clickOffset = transform.position - ray.GetPoint(planeDistance);
        }

        isDrag = true;

        GetValidMoveRange();
    }

    void OnMouseDrag()
    {
        if (!inputEnabled)
            return;

        Ray ray = GetScreenPointToRay();

        if (isDrag == true)
        {

            if (clickPlane.Raycast(ray, out planeDistance))
            {
                MoveCar(ray.GetPoint(planeDistance));
                Vector3 offsetPosition = ray.GetPoint(planeDistance) + clickOffset;

                print(offsetPosition);
                Vector3 lockedPosition = LockPositionToLocalForwardAxis(offsetPosition);

                switch (orientation)
                {
                    case Orientation.NORTH:
                    case Orientation.SOUTH:
                        if (offsetPosition.z > 0 || offsetPosition.z < 0)
                        {
                            print("Check");
                            Vector3 bouncePosition = SnapToGrid(ClampToBouncePosition(lockedPosition));
                            targetPosition = bouncePosition;
                        }
                        break;
                    case Orientation.EAST:
                    case Orientation.WEST:
                        if (offsetPosition.x > 0 || offsetPosition.x < 0)
                        {
                            Vector3 bouncePosition = SnapToGrid(ClampToBouncePosition(lockedPosition));
                            targetPosition = bouncePosition;
                            return;
                        }
                        break;
                }
            }
        }
    }

    void OnMouseUp()
    {
        if (!inputEnabled)
            return;

        GotoValidPosition();

        validBackPos = Vector3.negativeInfinity;
        validFrontPos = Vector3.positiveInfinity;
    }

    public void DisablePlayerInput()
    {
        inputEnabled = false;
    }

    void GetValidMoveRange()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        RaycastHit hit;
        LayerMask wallMask = LayerMask.GetMask("Wall");

        col.enabled = false;

        ray.direction = transform.forward;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, wallMask))
        {
            validFrontPos = Utils.RoundVector(hit.point - transform.forward * carHalfSize);
        }

        ray.direction = -transform.forward;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, wallMask))
        {
            validBackPos = Utils.RoundVector(hit.point + transform.forward * carHalfSize);
        }

        col.enabled = true;
    }

    void ShowValidMoveRange()
    {
        if (validBackPos != Vector3.negativeInfinity)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(validBackPos, .25f);
        }

        if (validFrontPos != Vector3.positiveInfinity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(validFrontPos, .25f);
        }

        if (validBackPos != Vector3.negativeInfinity && validFrontPos != Vector3.positiveInfinity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(validBackPos, validFrontPos);
        }
    }

    void MoveCar(Vector3 screenPosition)
    {
        Vector3 offsetPosition = screenPosition + clickOffset;
        Vector3 lockedPosition = LockPositionToLocalForwardAxis(offsetPosition);
        Vector3 clampedPosition = ClampToValidPosition(lockedPosition);
        Vector3 gridPosition = SnapToGrid(clampedPosition);
        targetPosition = gridPosition;
    }

    Vector3 LockPositionToLocalForwardAxis(Vector3 pos)
    {
        switch (orientation)
        {
            case Orientation.NORTH:
            case Orientation.SOUTH:
                pos.x = transform.position.x;
                break;
            case Orientation.EAST:
            case Orientation.WEST:
                pos.z = transform.position.z;
                break;
        }

        return Utils.RoundVector(pos);
    }

    Vector3 ClampToValidPosition(Vector3 pos)
    {
        switch (orientation)
        {
            case Orientation.NORTH:
                pos.z = Mathf.Clamp(pos.z, validBackPos.z, validFrontPos.z);
                break;
            case Orientation.SOUTH:
                pos.z = Mathf.Clamp(pos.z, validFrontPos.z, validBackPos.z);
                break;
            case Orientation.EAST:
                pos.x = Mathf.Clamp(pos.x, validBackPos.x, validFrontPos.x);
                break;
            case Orientation.WEST:
                pos.x = Mathf.Clamp(pos.x, validFrontPos.x, validBackPos.x);
                break;
        }

        return Utils.RoundVector(pos);
    }
    Vector3 ClampToBouncePosition(Vector3 pos)
    {
        switch (orientation)
        {
            case Orientation.NORTH:
                pos.z = Mathf.Clamp(pos.z, validBackPos.z - 0.5f, validFrontPos.z + 1f);
                break;
            case Orientation.SOUTH:
                pos.z = Mathf.Clamp(pos.z, validFrontPos.z - 0.5f, validBackPos.z + 1f);
                break;
            case Orientation.EAST:
                pos.x = Mathf.Clamp(pos.x, validBackPos.x - 0.5f, validFrontPos.x + 1f);
                break;
            case Orientation.WEST:
                pos.x = Mathf.Clamp(pos.x, validFrontPos.x - 0.5f, validBackPos.x + 1f);
                break;
        }

        return Utils.RoundVector(pos);
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        Vector3 gridPosition = new Vector3(
          Mathf.Floor(pos.x) + gridOffset.x,
          pos.y,
          Mathf.Floor(pos.z) + gridOffset.z
        );

        return Utils.RoundVector(gridPosition);
    }

    void SetOrientation()
    {
        Vector3 localFwd = Utils.RoundVector(transform.forward);
        Vector3 worldFwd = Utils.RoundVector(Vector3.forward);

        float angle = Vector3.SignedAngle(worldFwd, localFwd, Vector3.up);

        if (angle == 0)
            orientation = Orientation.NORTH;
        else if (angle == 90)
            orientation = Orientation.EAST;
        else if (angle == 180)
            orientation = Orientation.SOUTH;
        else
            orientation = Orientation.WEST;

        switch (orientation)
        {
            case Orientation.NORTH:
            case Orientation.SOUTH:
                carHalfSize = col.bounds.extents.z;
                break;
            case Orientation.EAST:
            case Orientation.WEST:
                carHalfSize = col.bounds.extents.x;
                break;
        }
    }

    void SetGridOffset()
    {
        gridOffset = new Vector3(
          Mathf.Abs(transform.position.x % 1f),
          0f,
          Mathf.Abs(transform.position.z % 1f)
        );
    }

    Ray GetScreenPointToRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}

enum Orientation
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}