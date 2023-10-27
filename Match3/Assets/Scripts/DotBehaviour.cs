using UnityEngine;

public class DotBehaviour : MonoBehaviour

{
    public Camera cam;
    
    public Vector3 firstTouchPosition;
    public Vector3 finalTouchPosition;
    public Vector3 tempPosition;
    public float swipeAngle;
    
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    
    private GameObject comparedDot;
    private BoardBehaviour board;

    private void Start()
    {
        cam = Camera.main;
        
        board = FindObjectOfType<BoardBehaviour>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    private void Update()
    {
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move toward target
            tempPosition = new Vector3(targetX, transform.position.y);
            transform.position = Vector3.Lerp(transform.position, tempPosition, .2f);
        }
        else
        {
            //directly set position
            tempPosition = new Vector3(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move toward target
            tempPosition = new Vector3(transform.position.x, targetY);
            transform.position = Vector3.Lerp(transform.position, tempPosition, .2f);
        }
        else
        {
            //directly set position
            tempPosition = new Vector3(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    public void OnMouseDown()
    {
        firstTouchPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }
    
    public void OnMouseUp()
    {
        finalTouchPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        CalcuateAngle();
    }
    
     void CalcuateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        MoveObjects();
    }

    void MoveObjects()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            //Right swipe
            comparedDot = board.allDots[column + 1, row];
            comparedDot.GetComponent<DotBehaviour>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            //Up swipe
            comparedDot = board.allDots[column, row + 1];
            comparedDot.GetComponent<DotBehaviour>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135  || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            comparedDot = board.allDots[column - 1, row];
            comparedDot.GetComponent<DotBehaviour>().column += 1;
            column -= 1;
        }   
        else if ((swipeAngle < -45 && swipeAngle >= -135) && row > 0)
        {
            //Down swipe
            comparedDot = board.allDots[column, row - 1];
            comparedDot.GetComponent<DotBehaviour>().row += 1;
            row -= 1;
        }
    }
}
