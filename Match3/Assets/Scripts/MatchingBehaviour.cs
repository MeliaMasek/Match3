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
    
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<DotBehaviour>().isRowBomb || leftDot.GetComponent<DotBehaviour>().isRowBomb || rightDot.GetComponent<DotBehaviour>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));   
                                }

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
                        GameObject downDot = board.allDots[i, j - 1];
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<DotBehaviour>().isColumnBomb || upDot.GetComponent<DotBehaviour>().isColumnBomb || downDot.GetComponent<DotBehaviour>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i));
                                }
                                
                                if (currentDot.GetComponent<DotBehaviour>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));
                                }
                                
                                if (upDot.GetComponent<DotBehaviour>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j+1));
                                }
                                
                                if (downDot.GetComponent<DotBehaviour>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j-1));
                                }
                                
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
            {
                
            }
        }
    }
}
