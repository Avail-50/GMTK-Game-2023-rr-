using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public int width;
    public int height;
    public int seed;
    public int level;

    private Map map;

    // Start is called before the first frame update
    void Start()
    {
        map = new(width, height, seed, level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
