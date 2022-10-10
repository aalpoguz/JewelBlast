using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewels : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;

    [HideInInspector]
    public Board board;


    public Vector2 firstTouchedPos;
    public Vector2 lastTouchedPos;

    bool isClicked;
    float moveAngle;
    Jewels otherJewel;

    public bool isMatch;

    Vector2Int firstPos;



    public enum JewelType { blue, pink, yellow, green, darkerGreen };
    public JewelType type;

    private void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.jewelSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
        }


        if (isClicked == true && Input.GetMouseButtonUp(0))
        {

            isClicked = false;
            lastTouchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angelCalculate();

        }
    }


    public void EditJewel(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    private void OnMouseDown()
    {
        firstTouchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isClicked = true;
    }

    void angelCalculate()
    {
        float dx = lastTouchedPos.x - firstTouchedPos.x;
        float dy = lastTouchedPos.y - firstTouchedPos.y;

        moveAngle = Mathf.Atan2(dy, dx);

        moveAngle = moveAngle * 180 / Mathf.PI; //change radian to degree

        print(moveAngle);

        if (Vector3.Distance(lastTouchedPos, firstTouchedPos) > 0.5f)
        {
            TileMove();
        }
    }

    void TileMove()
    {

        firstPos = posIndex;
        if (moveAngle < 45 && moveAngle > -45 && posIndex.x < board.width - 1)
        {
            otherJewel = board.allJewels[posIndex.x + 1, posIndex.y];
            otherJewel.posIndex.x--;
            posIndex.x++;
        }
        else if (moveAngle > 45 && moveAngle <= 135 && posIndex.y < board.height - 1)
        {
            otherJewel = board.allJewels[posIndex.x, posIndex.y + 1];
            otherJewel.posIndex.y--;
            posIndex.y++;
        }
        else if (moveAngle < -45 && moveAngle >= -135 && posIndex.y > 0)
        {
            otherJewel = board.allJewels[posIndex.x, posIndex.y - 1];
            otherJewel.posIndex.y++;
            posIndex.y--;
        }
        else if (moveAngle > 135 || moveAngle < 135 && posIndex.x > 0)
        {
            otherJewel = board.allJewels[posIndex.x - 1, posIndex.y];
            otherJewel.posIndex.x++;
            posIndex.x--;
        }

        board.allJewels[posIndex.x, posIndex.y] = this;
        board.allJewels[otherJewel.posIndex.x, otherJewel.posIndex.y] = otherJewel;

        StartCoroutine(MoveControl());

    }


    public IEnumerator MoveControl()
    {

        yield return new WaitForSeconds(.5f);
        board.matchController.FindMatches();
        if (otherJewel != null)
        {
            if (!isMatch && !otherJewel.isMatch)
            {
                otherJewel.posIndex = posIndex;
                posIndex = firstPos;

                board.allJewels[posIndex.x, posIndex.y] = this;
                board.allJewels[otherJewel.posIndex.x, otherJewel.posIndex.y] = otherJewel;
            }
            else
            { 
                board.allMatchesDestroy();
            }
        }
    }
}
