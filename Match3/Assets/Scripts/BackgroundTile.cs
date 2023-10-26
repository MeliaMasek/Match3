using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] dots;

    private void Start()
    {
        SpawnDots();
    }

    private void SpawnDots()
    {
        int usableDots = Random.Range(0, dots.Length);
        GameObject dot = Instantiate(dots[usableDots], transform.position, Quaternion.identity);
        dot.transform.parent = this.transform;
        dot.name = this.gameObject.name;
    }
}
