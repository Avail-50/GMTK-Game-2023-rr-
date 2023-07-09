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
    public GameObject heroPrefab;

    public int width;
    public int height;
    //public int seed;
    public int level;
    public int spaceX;
    public int spaceY;

    private Map map;

    // Start is called before the first frame update
    void Start()
    {
        int number_of_players = level + 4;

        System.Random rand = new();

        map = new(width, height, rand.Next(), level, spaceX, spaceY);
        for (int x = -1; x <= width; x++)
        {
            for (int y = -1; y <= height; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height || map.tiles[x, y])
                {
                    tilemap.SetTile(new Vector3Int(x - map.offsetX, y - map.offsetY, 0), wallTile);
                }   
            }
        }
        navMeshSurface.BuildNavMesh();
        /*HeroController hero = */Instantiate(heroPrefab, new Vector3(0, 0, 0), Quaternion.identity)/*.GetComponent<HeroController>()*/;
        foreach (Vector2 playerSpawnPos in map.playerSpawnPoses.OrderBy(x => rand.Next()).Take(number_of_players))
        {
            Instantiate(playerPrefab, new Vector3(playerSpawnPos.x, playerSpawnPos.y, 0), Quaternion.identity)/*.GetComponent<EnemyController>().hero = hero*/;
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
