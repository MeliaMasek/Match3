using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardBehaviour : MonoBehaviour
{
    public int width;
    public int height;
    public int offset;
    
    private BackgroundTile[,] allTiles;

    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject[,] allDots;

    private void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offset);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                int usableDots = Random.Range(0, dots.Length);
                int maxIterations = 0;

                while (MatchesAt(i, j, dots[usableDots]) && maxIterations < 100)
                {
                    usableDots = Random.Range(0, dots.Length);
                    maxIterations++;
                }

                maxIterations = 0;

                GameObject dot = Instantiate(dots[usableDots], tempPosition, Quaternion.identity);
                dot.GetComponent<DotBehaviour>().row = j;
                dot.GetComponent<DotBehaviour>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";
                allDots[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject obj)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == obj.tag && allDots[column - 2, row].tag == obj.tag)
            {
                return true;
            }

            if (allDots[column, row - 1].tag == obj.tag && allDots[column, row - 2].tag == obj.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1].tag == obj.tag && allDots[column, row - 2].tag == obj.tag)
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (allDots[column - 1, row].tag == obj.tag && allDots[column - 2, row].tag == obj.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<DotBehaviour>().isMatched)
        {
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        StartCoroutine(DecreaseRow());
    }

    private IEnumerator DecreaseRow()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<DotBehaviour>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }

            nullCount = 0;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FillBoard());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector3 tempPosition = new Vector3(i, j + offset);
                    int usableDots = Random.Range(0, dots.Length);
                    GameObject dot = Instantiate(dots[usableDots], tempPosition, Quaternion.identity);
                    allDots[i, j] = dot;
                    dot.GetComponent<DotBehaviour>().row = j;
                    dot.GetComponent<DotBehaviour>().column = i;

                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<DotBehaviour>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}