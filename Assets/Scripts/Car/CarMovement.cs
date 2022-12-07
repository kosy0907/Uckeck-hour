using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider))]
public class CarMovement : MonoBehaviour
{
    GameManagerController gameManagerController;

    Plane clickPlane = new Plane(Vector3.up, Vector3.zero);
    public float planeDistance = 0f;
    Vector3 clickOffset = Vector3.zero;

    BoxCollider col;
    public float carHalfSize = 0.5f;
    Vector3 gridOffset = Vector3.zero;
    Orientation orientation;

    public Vector3 validBackPos = Vector3.negativeInfinity;
    public Vector3 validFrontPos = Vector3.positiveInfinity;

    public Vector3 clearPosition = Vector3.positiveInfinity;

    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;
    public float smoothTime = .1f;

    bool inputEnabled = true;

    bool isDrag = false;

    void Start()
    {
        gameManagerController = GameObject.Find("GameManager").GetComponent<GameManagerController>();
        col = GetComponent<BoxCollider>();
        targetPosition = transform.position;

        SetOrientation();
        SetGridOffset();
    }

    void Update()
    {
        if (isDrag == true)
        {
            GotoValidPosition();
            if (transform.position.z > 6.5 || transform.position.x > 6.5)
            {
                gameManagerController.clearGame();
                print("Clear");
                // EditorApplication.isPaused = true;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void GotoValidPosition()
    {
        if (validBackPos != null && validFrontPos != null)
        {
            switch (orientation)
            {
                case Orientation.NORTH:
                    if (transform.position.z > validFrontPos.z + 0.3f)
                    {
                        targetPosition = new Vector3(transform.position.x, transform.position.y, validFrontPos.z);
                        isDrag = false;
                    }
                    else if (transform.position.z < validBackPos.z - 0.3f)
                    {
                        targetPosition = new Vector3(transform.position.x, transform.position.y, validBackPos.z);
                        isDrag = false;
                    }
                    break;
                case Orientation.SOUTH:
                    if (transform.position.z < validFrontPos.z - 0.3f)
                    {
                        targetPosition = new Vector3(transform.position.x, transform.position.y, validFrontPos.z);
                        isDrag = false;
                    }
                    else if (transform.position.z > validBackPos.z + 0.3f)
                    {
                        targetPosition = new Vector3(transform.position.x, transform.position.y, validBackPos.z);
                        isDrag = false;
                    }
                    break;
                case Orientation.EAST:
                    if (transform.position.x > validFrontPos.x + 0.3f)
                    {
                        targetPosition = new Vector3(validFrontPos.x, transform.position.y, transform.position.z);
                        isDrag = false;
                    }
                    else if (transform.position.x < validBackPos.x - 0.3f)
                    {
                        targetPosition = new Vector3(validBackPos.x, transform.position.y, transform.position.z);
                        isDrag = false;
                    }
                    break;
                case Orientation.WEST:
                    if (transform.position.x < validFrontPos.x - 0.3f)
                    {
                        targetPosition = new Vector3(validFrontPos.x, transform.position.y, transform.position.z);
                        isDrag = false;
                    }
                    else if (transform.position.x > validBackPos.x + 0.3f)
                    {
                        targetPosition = new Vector3(validBackPos.x, transform.position.y, transform.position.z);
                        isDrag = false;
                    }
                    break;
            }
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

        GetValidMoveRange();
        checkClear();
        isDrag = true;
    }

    void OnMouseDrag()
    {
        if (!inputEnabled)
            return;

        Ray ray = GetScreenPointToRay();



        if (clickPlane.Raycast(ray, out planeDistance) && isDrag == true)
        {
            MoveCar(ray.GetPoint(planeDistance));
        }
    }

    void OnMouseUp()
    {
        if (!inputEnabled)
            return;


        switch (orientation)
        {
            case Orientation.NORTH:
                validBackPos = Vector3.negativeInfinity;
                validFrontPos = Vector3.positiveInfinity;
                break;
            case Orientation.SOUTH:
                validBackPos = Vector3.positiveInfinity;
                validFrontPos = Vector3.negativeInfinity;
                break;
            case Orientation.EAST:
                validBackPos = Vector3.negativeInfinity;
                validFrontPos = Vector3.positiveInfinity;
                break;
            case Orientation.WEST:
                validBackPos = Vector3.positiveInfinity;
                validFrontPos = Vector3.negativeInfinity;
                break;
        }
    }

    public void DisablePlayerInput()
    {
        inputEnabled = false;
    }

    void checkClear()
    {
        clearPosition = new Vector3(-1, 0, 7f);
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
            validFrontPos = Utils.RoundVector(hit.point - transform.forward * carHalfSize, carHalfSize);
        }

        ray.direction = -transform.forward;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, wallMask))
        {
            validBackPos = Utils.RoundVector(hit.point + transform.forward * carHalfSize, carHalfSize);
        }

        col.enabled = true;
    }

    void ShowValidMoveRange()
    {

        float maxDistance = 100;
        RaycastHit hit;
        Gizmos.color = Color.blue;
        // Physics.Raycast (레이저를 발사할 위치, 발사 방향, 충돌 결과, 최대 거리)
        bool isHit = Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit, maxDistance);

        Gizmos.color = Color.red;
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
        }

        if (clearPosition != Vector3.positiveInfinity)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(clearPosition, .25f);
        }

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
        Vector3 gridPosition;

        if (((offsetPosition.z > validFrontPos.z + 0.3f || offsetPosition.z < validBackPos.z - 0.3f) && orientation == Orientation.NORTH)
        || ((offsetPosition.z < validFrontPos.z - 0.3f || offsetPosition.z > validBackPos.z + 0.3f) && orientation == Orientation.SOUTH)
        || ((offsetPosition.x > validBackPos.x - 0.3f || offsetPosition.x < validFrontPos.x + 0.3f) && orientation == Orientation.EAST)
        || ((offsetPosition.x < validBackPos.x + 0.3f || offsetPosition.x > validFrontPos.x - 0.3f) && orientation == Orientation.WEST))
        {
            print(offsetPosition);
            Vector3 clampedPosition2 = ClampToBouncePosition(lockedPosition);
            gridPosition = SnapToGrid(clampedPosition2);
            targetPosition = gridPosition;

        }
        else
        {
            gridPosition = SnapToGrid(clampedPosition);
            targetPosition = gridPosition;
        }

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

        return Utils.RoundVector(pos, carHalfSize);
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

        return Utils.RoundVector(pos, carHalfSize);
    }
    Vector3 ClampToBouncePosition(Vector3 pos)
    {
        switch (orientation)
        {
            case Orientation.NORTH:
                pos.z = Mathf.Clamp(pos.z, validBackPos.z - 1f, validFrontPos.z + 1f);
                break;
            case Orientation.SOUTH:
                pos.z = Mathf.Clamp(pos.z, validFrontPos.z - 1f, validBackPos.z + 1f);
                break;
            case Orientation.EAST:
                pos.x = Mathf.Clamp(pos.x, validBackPos.x - 1f, validFrontPos.x + 1f);
                break;
            case Orientation.WEST:
                pos.x = Mathf.Clamp(pos.x, validFrontPos.x - 1f, validBackPos.x + 1f);
                break;
        }

        return Utils.RoundVector(pos, carHalfSize);
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        Vector3 gridPosition = new Vector3(
          Mathf.Floor(pos.x) + gridOffset.x,
          pos.y,
          Mathf.Floor(pos.z) + gridOffset.z
        );

        return Utils.RoundVector(gridPosition, carHalfSize);
    }

    void SetOrientation()
    {
        float angle = this.GetComponent<Transform>().eulerAngles.y;
        print(angle);

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
                print(col.bounds.extents.z);
                break;
            case Orientation.EAST:
            case Orientation.WEST:
                carHalfSize = col.bounds.extents.x;
                print(col.bounds.extents.x);
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