using UnityEngine;
using Random = UnityEngine.Random;

public class BoardBehaviour : MonoBehaviour
{
    public int width;
    public int height;
    
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
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab,tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";
                
                int usableDots = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[usableDots], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";
                allDots[i, j] = dot;
            }
        }   
    }
}   
