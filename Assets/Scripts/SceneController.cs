using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneController : MonoBehaviour
{
    public Tilemap tilemap;
    public RuleTile wallTile;
    public NavMeshPlus.Components.NavMeshSurface navMeshSurface;

    //public pref navMeshSurface;

    public int width;
    public int height;
    public int seed;
    public int level;
    public int spaceX;
    public int spaceY;

    private Map map;

    // Start is called before the first frame update
    void Start()
    {
        map = new(width, height, seed, level, spaceX, spaceY);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map.tiles[x, y])
                {
                    tilemap.SetTile(new Vector3Int(x - map.offsetX, y - map.offsetY, 0), wallTile);
                }   
            }
        }
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 offset = new(map.offsetX, map.offsetY);
        //foreach (System.Tuple<int, int> corridor in map.corridors)
        //{
        //    Debug.DrawLine(map.centres[corridor.Item1] - offset, map.centres[corridor.Item2] - offset, Color.red);
        //}
    }
}
