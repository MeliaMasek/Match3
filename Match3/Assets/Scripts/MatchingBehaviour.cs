using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Code borrowed and modified from Mister Taft Creates https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public class MatchingBehaviour : MonoBehaviour
{
    private BoardBehaviour board;

    public List<GameObject> currentMatches = new List<GameObject>();

    private void Start()
    {
        board = FindObjectOfType<BoardBehaviour>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private List<GameObject> isRowbomb(DotBehaviour dot1, DotBehaviour dot2, DotBehaviour dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }

        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }

        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }

        return currentDots;
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                DotBehaviour currentDotDot = currentDot.GetComponent<DotBehaviour>();   
                if (currentDot != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        DotBehaviour leftDotDot = leftDot.GetComponent<DotBehaviour>();
                        GameObject rightDot = board.allDots[i + 1, j];
                        DotBehaviour rightDotDot = rightDot.GetComponent<DotBehaviour>();
                        if (leftDot != null && rightDot != null)
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                currentMatches.Union(isRowbomb(leftDotDot, currentDotDot, rightDotDot));

                                if (currentDot.GetComponent<DotBehaviour>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i));
                                }
                                
                                if (leftDot.GetComponent<DotBehaviour>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i-1));
                                }
                                
                                if (rightDot.GetComponent<DotBehaviour>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i+1));
                                }
                                
                                if (!currentMatches.Contains(leftDot))
                                {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<DotBehaviour>().isMatched = true;
                                
                                if (!currentMatches.Contains(rightDot))
                                {
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<DotBehaviour>().isMatched = true;
                                
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<DotBehaviour>().isMatched = true;
                            }
                        }
                    }
                    
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        DotBehaviour UpDotDot = upDot.GetComponent<DotBehaviour>();
                        GameObject downDot = board.allDots[i, j - 1];
                        DotBehaviour DownDotDot = downDot.GetComponent<DotBehaviour>();
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<DotBehaviour>().isColumnBomb || upDot.GetComponent<DotBehaviour>().isColumnBomb || downDot.GetComponent<DotBehaviour>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i));
                                }

                                currentMatches.Union(isRowbomb(UpDotDot,currentDotDot, DownDotDot));

                                if (!currentMatches.Contains(upDot))
                                {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<DotBehaviour>().isMatched = true;
                                
                                if (!currentMatches.Contains(downDot))
                                {
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<DotBehaviour>().isMatched = true;
                                
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<DotBehaviour>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
    
    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<DotBehaviour>().isMatched = true;
            }
        }
        return dots;
    }
    
    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<DotBehaviour>().isMatched = true;
            }
        }
        return dots;
    }

    public void CheckBombs()
    {
        if (board.currentDot != null)
        {
            if (board.currentDot.isMatched)
            {
                board.currentDot.isMatched = false;
                int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 50)
                {
                    board.currentDot.MakeRowBomb();
                }
                else if (typeOfBomb >= 50)
                {
                    board.currentDot.MakeColumnBomb();
                }
            }
            else if (board.currentDot.comparedDot != null)
            {
                DotBehaviour otherDot = board.currentDot.comparedDot.GetComponent<DotBehaviour>();
                if (otherDot.isMatched)
                {
                    otherDot.isMatched = false;
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        otherDot.MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
        }
    }

    public void MatchColorPieces(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (board.allDots[i, j].tag == color)
                    {
                        board.allDots[i, j].GetComponent<DotBehaviour>().isMatched = true;
                    }
                }
            }
        }
    }
}
