using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;

public class SceneController : MonoBehaviour
{
    public Tilemap tilemap;
    public RuleTile wallTile;
    public NavMeshPlus.Components.NavMeshSurface navMeshSurface;

    public GameObject playerPrefab;

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
        int number_of_players = level + 4;

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
        System.Random rand = new();
        foreach (Vector2 playerSpawnPos in map.playerSpawnPoses.OrderBy(x => rand.Next()).Take(number_of_players))
        {
            Instantiate(playerPrefab, new Vector3(playerSpawnPos.x, playerSpawnPos.y, 0), Quaternion.identity);
        }
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
