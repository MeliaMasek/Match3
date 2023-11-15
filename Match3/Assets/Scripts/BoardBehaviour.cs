using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

//Code borrowed and modified from Mister Taft Creates https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public enum GamesState
{
    wait,
    move
}
public class BoardBehaviour : MonoBehaviour
{
    public int width;
    public int height;
    public int offset;

    public IntData scoreData;

    private BackgroundTile[,] allTiles;

    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject[,] allDots;
    
    public GamesState currentState = GamesState.move;
    
    private MatchingBehaviour findMatches;

    private SoundManager soundManager;
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        findMatches = FindObjectOfType<MatchingBehaviour>();
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
                Vector3 tempPosition = new Vector3(i, j + offset, 0);
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
            findMatches.currentMatches.Remove(allDots[column, row]);
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
        soundManager.PlayMatchingSound();
    }

    public void DestroyMatches()
    {
        int scoreToAdd = 5;

        bool scoreAdded = false;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                    
                    if (!scoreAdded) // Add score only once per match.
                    {
                        scoreData.value += scoreToAdd; // Increment the score.
                        scoreAdded = true;
                    }
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
                    Vector3 tempPosition = new Vector3(i, j + offset, 0);
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
        yield return new WaitForSeconds(.25f);
        currentState = GamesState.move;
    }
}