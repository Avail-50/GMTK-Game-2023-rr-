using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

using System.Linq;
using UnityEngine;

namespace Roguelike
{
    public class Map
    {
        public bool[,] tiles;
        public int offsetX;
        public int offsetY;
        public int width;
        public int height;

        public List<Vector2> playerSpawnPoses;

        public int seed;
        public int nextSeed;
        public int level;

        public Point end;

        public List<Vector2> centres;
        public List<Tuple<int, int>> corridors; // remove eventually
        public Map(int width, int height, int seed, int level, int spaceX, int spaceY)
        {
            tiles = new bool[width, height];
            this.width = width;
            this.height = height;
            offsetX = 0;//width / 2;
            offsetY = 0;//height / 2;

            playerSpawnPoses = new();

            this.level = level;
            seed = this.seed = seed;

            System.Random rand = new(seed);
            nextSeed = rand.Next();

            Rectangle entire = new(0, 0, width - 1, height - 1);

            int numrooms = (int)Mathf.Round(Gaussian(rand, 9, 1));
            //Debug.Log("numrooms : " + numrooms); // DEBUG

            #region rooms
            List<Rectangle> rooms = new List<Rectangle>();
            for (int i = 0, j = 0; i < 1000 && j < numrooms; i++)
            {
                Rectangle newroom = new Rectangle((int)Math.Round(Gaussian(rand, width / 2, width / 4)), (int)Math.Round(Gaussian(rand, height / 2, height / 4)), (int)Math.Round(Gaussian(rand, 8, 1)), (int)Math.Round(Gaussian(rand, 8, 1)));
                if (entire.Contains(newroom))
                {
                    Rectangle bordernewroom = Rectangle.Inflate(newroom, 1 + spaceX, 2 + spaceY);
                    bool intersect = false;
                    foreach (Rectangle room in rooms)
                    {
                        if (bordernewroom.IntersectsWith(room))
                        {
                            intersect = true;
                            break;
                        }
                    }
                    if (!intersect)
                    {
                        rooms.Add(newroom);
                        j++;
                    }
                }
            }
            #endregion

            #region centres
            centres = rooms.Select((Rectangle r) => (new Vector2(r.Left + r.Width / 2f, r.Top + r.Height / 2f))).ToList();
            /*List<Tuple<int, int>> */corridors = new List<Tuple<int, int>>(); // Relative Neighbourhood Graph
            List<Tuple<Vector2, Vector2>> corridorPoses = new List<Tuple<Vector2, Vector2>>();
            for (int i = 0; i < centres.Count - 1; i++)
            {
                for (int j = i + 1; j < centres.Count; j++)
                {
                    bool closercentre = false;
                    float sqrDistance = (centres[i] - centres[j]).sqrMagnitude;
                    foreach (Vector2 centre in centres)
                    {
                        if (centre != centres[i] && centre != centres[j])
                        {
                            if ((centre - centres[j]).sqrMagnitude < sqrDistance && (centre - centres[i]).sqrMagnitude < sqrDistance)
                            {
                                closercentre = true;
                                break;
                            }
                        }
                    }
                    if (!closercentre)
                    {
                        corridors.Add(new Tuple<int, int>(i, j));
                        corridorPoses.Add(new Tuple<Vector2, Vector2>(centres[i], centres[j]));
                    }
                }
            }
            #endregion

            #region corridors
            List<Tuple<int, int, int, int, int, int>> corridorNums = new List<Tuple<int, int, int, int, int, int>>(); // segmentNum1, room1Segment, segmentNum2, room2Segment, elbowJoint1, elbowJoint
            foreach (Tuple<int, int> corridor in corridors)
            {
                Rectangle room1 = rooms[corridor.Item1];
                Rectangle room2 = rooms[corridor.Item2];
                Vector2 line1 = centres[corridor.Item2] - centres[corridor.Item1];
                int segmentNum1 = RectSegment(room1, line1, centres[corridor.Item1]);
                int room1Segment = (segmentNum1 / 2 % 2 == 0) ? rand.Next(room1.Height / 2) : rand.Next(room1.Width / 2); // 0,1,4,5 : x side , 2,3,6,7 : y side
                Vector2 line2 = centres[corridor.Item1] - centres[corridor.Item2];
                int segmentNum2 = RectSegment(room2, -1 * line2, centres[corridor.Item2]);
                int room2Segment = (segmentNum2 / 2 % 2 == 0) ? rand.Next(room2.Height / 2) : rand.Next(room2.Width / 2); // 0,1,4,5 : x side , 2,3,6,7 : y side
                int elbowJoint1 = 0;
                int elbowJoint2 = 0;
                int elbowJoint = 0;
                if (segmentNum1 != segmentNum2)
                {
                    if (segmentNum1 / 2 == 0) elbowJoint1 = room2.Left + room2Segment - room1.Right;
                    else if (segmentNum1 / 2 == 2) elbowJoint1 = room1.Left + room2Segment - room2.Right;
                    else if (segmentNum1 / 2 == 1) elbowJoint1 = room2.Top + room2Segment - room1.Bottom;
                    else if (segmentNum1 / 2 == 3) elbowJoint1 = room1.Top + room2Segment - room2.Bottom;

                    if (segmentNum2 / 2 == 2) elbowJoint2 = room1.Left + room1Segment - room2.Right;
                    else if (segmentNum2 / 2 == 0) elbowJoint2 = room2.Left + room1Segment - room1.Right;
                    else if (segmentNum2 / 2 == 3) elbowJoint2 = room1.Top + room1Segment - room2.Bottom;
                    else if (segmentNum2 / 2 == 1) elbowJoint2 = room2.Top + room1Segment - room1.Bottom;
                }
                else
                {
                    float gaussian = Gaussian(rand, 0.5f, 0.05f);
                    if (segmentNum1 / 2 == 0)
                    {
                        elbowJoint = (int)((room2.Left - room1.Right));
                        elbowJoint1 = (int)((room2.Left - room1.Right) * gaussian);
                        elbowJoint2 = elbowJoint - elbowJoint1;//(int)((room2.Left - room1.Right) * (1 - gaussian));
                    }
                    else if (segmentNum1 / 2 == 2)
                    {
                        elbowJoint = (int)((room1.Left - room2.Right));
                        elbowJoint1 = (int)((room1.Left - room2.Right) * gaussian);
                        elbowJoint2 = elbowJoint - elbowJoint1;//(int)((room1.Left - room2.Right) * (1 - gaussian));
                    }
                    else if (segmentNum1 / 2 == 1)
                    {
                        elbowJoint = (int)((room2.Top - room1.Bottom));
                        elbowJoint1 = (int)((room2.Top - room1.Bottom) * gaussian);
                        elbowJoint2 = elbowJoint - elbowJoint1;//(int)((room2.Top - room1.Bottom) * (1 - gaussian));
                    }
                    else if (segmentNum1 / 2 == 3)
                    {
                        elbowJoint = (int)((room1.Top - room2.Bottom));
                        elbowJoint1 = (int)((room1.Top - room2.Bottom) * gaussian);
                        elbowJoint2 = elbowJoint - elbowJoint1;//(int)((room1.Top - room2.Bottom) * (1 - gaussian));
                    }
                }
                corridorNums.Add(new Tuple<int, int, int, int, int, int>(segmentNum1, room1Segment, segmentNum2, room2Segment, elbowJoint1, elbowJoint2));
            }
            #endregion

            #region tiles
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = true;
                    foreach (Rectangle room in rooms)
                    {
                        if (room.Contains(x, y))
                        {
                            tiles[x, y] = false;
                            break;
                        }
                    }
                    if (tiles[x, y])
                    {
                        for (int i = 0; i < corridors.Count; i++)
                        {
                            if (InCorridor(x, y, rooms[corridors[i].Item1], corridorNums[i].Item1, corridorNums[i].Item2, corridorNums[i].Item5, corridorNums[i].Item1 != corridorNums[i].Item3) || InCorridor(x, y, rooms[corridors[i].Item2], (corridorNums[i].Item3 + 4) % 8, corridorNums[i].Item4, corridorNums[i].Item6, corridorNums[i].Item1 != corridorNums[i].Item3))
                            {
                                tiles[x, y] = false;
                                break;
                            }
                            else
                            {
                                if (corridorNums[i].Item1 == corridorNums[i].Item3)
                                {
                                    bool rightAxis = false;
                                    if ((corridorNums[i].Item1 - ((3 + 2 * (corridorNums[i].Item1 % 2)) * (corridorNums[i].Item1 / 4))) == 0) // 0,5
                                    {
                                        int y1 = rooms[corridors[i].Item1].Top + corridorNums[i].Item2;
                                        int y2 = rooms[corridors[i].Item2].Bottom - corridorNums[i].Item4 - 1;
                                        if (y < Math.Max(y1, y2) && y > Math.Min(y1, y2)) rightAxis = true;
                                    }
                                    else if ((corridorNums[i].Item1 - ((3 + 2 * (corridorNums[i].Item1 % 2)) * (corridorNums[i].Item1 / 4))) == 1) // 1,4
                                    {
                                        int y1 = rooms[corridors[i].Item1].Bottom - corridorNums[i].Item2 - 1;
                                        int y2 = rooms[corridors[i].Item2].Top + corridorNums[i].Item4;
                                        if (y < Math.Max(y1, y2) && y > Math.Min(y1, y2)) rightAxis = true;
                                    }
                                    else if ((corridorNums[i].Item1 - ((3 + 2 * (corridorNums[i].Item1 % 2)) * (corridorNums[i].Item1 / 4))) == 2) // 2,7
                                    {
                                        int x1 = rooms[corridors[i].Item1].Right - corridorNums[i].Item2 - 1;
                                        int x2 = rooms[corridors[i].Item2].Left + corridorNums[i].Item4;
                                        if (x < Math.Max(x1, x2) && x > Math.Min(x1, x2)) rightAxis = true;
                                    }
                                    else if ((corridorNums[i].Item1 - ((3 + 2 * (corridorNums[i].Item1 % 2)) * (corridorNums[i].Item1 / 4))) == 3) // 3,6
                                    {
                                        int x1 = rooms[corridors[i].Item1].Left + corridorNums[i].Item2;
                                        int x2 = rooms[corridors[i].Item2].Right - corridorNums[i].Item4 - 1;
                                        if (x < Math.Max(x1, x2) && x > Math.Min(x1, x2)) rightAxis = true;
                                    }
                                    if (rightAxis)
                                    {
                                        bool rightRange = false;
                                        if (corridorNums[i].Item1 / 2 == 0) // 0,1
                                        {
                                            if (x == rooms[corridors[i].Item1].Right + corridorNums[i].Item5) rightRange = true;
                                        }
                                        else if (corridorNums[i].Item1 / 2 == 1) // 2,3
                                        {
                                            if (y == rooms[corridors[i].Item1].Bottom + corridorNums[i].Item5) rightRange = true;
                                        }
                                        else if (corridorNums[i].Item1 / 2 == 2) // 4,5
                                        {
                                            if (x == rooms[corridors[i].Item1].Left - corridorNums[i].Item5) rightRange = true;
                                        }
                                        else if (corridorNums[i].Item1 / 2 == 3) // 6,7
                                        {
                                            if (y == rooms[corridors[i].Item1].Top - corridorNums[i].Item5) rightRange = true;
                                        }
                                        if (rightRange)
                                        {
                                            tiles[x, y] = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region furthest rooms
            Tuple<int, int> furthestRooms = new Tuple<int, int>(0, 0);
            float furthestDistance = 0;
            for (int i = 0; i < rooms.Count - 1; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    float distance = ShortestDistanceBetweenNodes(i, j, rooms.Count, (int a, int b) => ((new Vector2(rooms[a].Left + rooms[a].Width / 2f, rooms[a].Top + rooms[a].Height / 2f) - new Vector2(rooms[b].Left + (float)rooms[b].Width / 2f, rooms[b].Top + (float)rooms[b].Height / 2f)).magnitude), delegate (int node)
                    {
                        List<int> neighbours = new List<int>();
                        foreach (Tuple<int, int> corridor in corridors)
                        {
                            if (corridor.Item1 == node) neighbours.Add(corridor.Item2);
                            else if (corridor.Item2 == node) neighbours.Add(corridor.Item1);
                        }
                        return neighbours;

                    });
                    if (distance > furthestDistance && !float.IsPositiveInfinity(distance))
                    {
                        furthestDistance = distance;
                        furthestRooms = new Tuple<int, int>(i, j);
                    }
                }
            }
            offsetX = (int)centres[furthestRooms.Item1].x;
            offsetY = (int)centres[furthestRooms.Item1].y;
            end = new Point((int)centres[furthestRooms.Item2].x - offsetX, (int)centres[furthestRooms.Item2].y - offsetY);
#if DEBUG
            Console.WriteLine("furthest : " + furthestRooms.Item1 + "," + furthestRooms.Item2 + " : " + furthestDistance);
#endif
            #endregion

            #region room population (with enemies)
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == furthestRooms.Item1) continue; // THE PLAYER / START OF THE DUNGEON
                Rectangle acRoom = rooms[i];
                acRoom.Offset(-offsetX, -offsetY);
                if (i == furthestRooms.Item2)
                {
                    //new Skeleton(end.x, end.y, new Tuple<string, string, string>("helmet", "chestpiece", "leggings")) // THE BOSS / END OF THE DUNGEON
                    //{
                    //    originalRoom = acRoom,
                    //    originalPos = new Vector2(end.x + 0.5f, end.y + 0.5f),
                    //    boss = true
                    //};
                    continue;
                }
                Point[] enemies = new Point[(int)Math.Round(Gaussian(rand, 5, 0.5f))];
                for (int j = 0; j < enemies.Length; j++)
                {
                    int x = rand.Next(acRoom.Left+1, acRoom.Right-1);
                    int y = rand.Next(acRoom.Top+1, acRoom.Bottom-1);
                    Point enemy = new Point(x, y);
                    if (!enemies.Contains(enemy)) enemies[j] = enemy;
                }
                foreach (Point enemy in enemies)
                {
                    if (enemy != default)
                    {
                        // spawn enemy
                        if (rand.Next(5) == 0)
                        {
                            rand.Next(3);
                        }
                        else
                        {
                            rand.Next(3);
                            rand.Next(3);
                            rand.Next(3);
                        }
                        playerSpawnPoses.Add(new Vector2(enemy.X, enemy.Y));
                        //if (rand.Next(5) == 0) new Skeleton_Mage(enemy.x + 0.5f, enemy.y + 0.5f, rand.Next(3) == 0)
                        //{
                        //    originalRoom = acRoom,
                        //    originalPos = new Vector2(enemy.x + 0.5f, enemy.y + 0.5f, 0)
                        //};
                        //else new Skeleton(enemy.x + 0.5f, enemy.y + 0.5f, new Tuple<string, string, string>(rand.Next(3) == 0 ? "helmet" : null, rand.Next(3) == 0 ? "chestpiece" : null, rand.Next(3) == 0 ? "leggings" : null))
                        //{
                        //    originalRoom = acRoom,
                        //    originalPos = new Vector2(enemy.x + 0.5f, enemy.y + 0.5f, 0)
                        //};
                    }
                }
            }
#endregion
        }

#region Extra Methods
        //      6 7 
        //    5 \ / 0
        //    4 / \ 1
        //      3 2 
        int RectSegment(Rectangle rect, Vector2 lineDelta, Vector2? centre)
        {
            if (!centre.HasValue) centre = new Vector2(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
            if (LineIntersectDistance(new Vector2(rect.Right, rect.Top), new Vector2(0, rect.Height / 2f), centre.Value, lineDelta, true).HasValue) return 0;
            else if (LineIntersectDistance(new Vector2(rect.Right, rect.Top + rect.Height / 2f), new Vector2(0, rect.Height / 2f), centre.Value, lineDelta, true).HasValue) return 1;
            else if (LineIntersectDistance(new Vector2(rect.Right, rect.Bottom), new Vector2(-rect.Width / 2f, 0), centre.Value, lineDelta, true).HasValue) return 2;
            else if (LineIntersectDistance(new Vector2(rect.Right - rect.Width / 2f, rect.Bottom), new Vector2(-rect.Width / 2f, 0), centre.Value, lineDelta, true).HasValue) return 3;
            else if (LineIntersectDistance(new Vector2(rect.Left, rect.Bottom), new Vector2(0, -rect.Height / 2f), centre.Value, lineDelta, true).HasValue) return 4;
            else if (LineIntersectDistance(new Vector2(rect.Left, rect.Bottom - rect.Height / 2f), new Vector2(0, -rect.Height / 2f), centre.Value, lineDelta, true).HasValue) return 5;
            else if (LineIntersectDistance(new Vector2(rect.Left, rect.Top), new Vector2(rect.Width / 2f, 0), centre.Value, lineDelta, true).HasValue) return 6;
            else if (LineIntersectDistance(new Vector2(rect.Left + rect.Width / 2f, rect.Top), new Vector2(rect.Width / 2f, 0), centre.Value, lineDelta, true).HasValue) return 7;
            else return 8; // ?
        }
        public static float? LineIntersectDistance(Vector2 segmentStart, Vector2 segmentDelta, Vector2 lineStart, Vector2 lineDelta, bool includeEnds = false)
        {
            Vector2 temp1 = lineStart - segmentStart;
            float temp2 = CrossProduct(segmentDelta, lineDelta);
            float t = CrossProduct(temp1, (lineDelta / temp2));
            float u = CrossProduct(temp1, (segmentDelta / temp2));
            if (u > 0 && ((t > 0 && t < 1) || (includeEnds && t >= 0 && t <= 1)))
            {
                return u;
            }
            return null;

        }
        public static float CrossProduct(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

        bool InCorridor(int acX, int acY, Rectangle room, int segmentNum, int segment, int elbowJoint, bool singleElbow)
        {
            int elbowJointNum = singleElbow ? -1 : 0;
            if (segmentNum / 2 % 2 == 0) // 0,1,4,5  (along x-axis)
            {
                if ((segmentNum + segmentNum / 4) % 2 == 0) // 0,5  (above / lower on y-axis)
                {
                    if (acY != room.Top + segment) return false; // right Y value
                }
                else
                {
                    if (acY != room.Bottom - segment - 1) return false;
                }
                if (segmentNum / 2 == 0) // 0,1  (front / higher on x-axis)
                {
                    if (acX < room.Right + elbowJointNum || acX > room.Right + elbowJoint) return false; // right X values
                }
                else
                {
                    if (acX > room.Left || acX < room.Left - elbowJoint + elbowJointNum) return false;
                }
            }
            else // 2,3,6,7
            {
                if ((segmentNum + segmentNum / 4) % 2 == 1) // 3,6  (left / lower on x-axis)
                {
                    if (acX != room.Left + segment) return false; // right X value
                }
                else
                {
                    if (acX != room.Right - segment - 1) return false;
                }
                if (segmentNum / 2 == 1) // 2,3  (front / higher on y-axis)
                {
                    if (acY < room.Bottom + elbowJointNum || acY > room.Bottom + elbowJoint) return false; // right Y values
                }
                else
                {
                    if (acY > room.Top || acY < room.Top - elbowJoint + elbowJointNum) return false;
                }
            }
            return true;
        }
#endregion

#region shortest distance
        public float ShortestDistanceBetweenNodes(int start, int end, int numNodes, Func<int, int, float> acDistance, Func<int, List<int>> getNeighbours)
        {
            List<int> openSet = new List<int>() { start };
            ///int[] cameFrom = new int[numNodes];
            float[] gScore = Tools.Populate(new float[numNodes], float.PositiveInfinity);
            gScore[start] = 0;
            float[] fScore = Tools.Populate(new float[numNodes], float.PositiveInfinity);
            fScore[start] = acDistance(start, end);

            while (openSet.Count != 0)
            {
                int current = openSet[0];
                foreach (int node in openSet) if (fScore[node] < fScore[current]) current = node;
                if (current == end) return gScore[current];

                openSet.Remove(current);
                foreach (int neighbour in getNeighbours(current))
                {
                    float tentative_gScore = gScore[current] + acDistance(current, neighbour);
                    if (tentative_gScore < gScore[neighbour])
                    {
                        //cameFrom[neighbour] = current;
                        gScore[neighbour] = tentative_gScore;
                        fScore[neighbour] = gScore[neighbour] + acDistance(neighbour, end);
                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
            return float.PositiveInfinity;
        }
#endregion

        public float Gaussian(System.Random rand, float mean, float deviation)
        {
            float u1 = 1 - (float)rand.NextDouble();
            float u2 = 1 - (float)rand.NextDouble();
            float randStdNotmal = (float)(Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2));
            return mean + deviation * randStdNotmal;
        }

        //float enemyCooldown = 1;
        public void Tick()
        {
            /*bool enemy = false;
            foreach (Entity entity in Program.world.entities)
            {
                if (entity is AI_Entity)
                {
                    if (!enemy)
                    {
                        enemy = true;
                        enemyCooldown = 1;
                        break;
                    }
                }
            }
            if (!enemy)
            {
                enemyCooldown -= Program.deltaTime;
                if (enemyCooldown <= 0) new AI_Entity(Program.world.map.end.x, Program.world.map.end.y, "enemy", new Tuple<string, string, string>(Program.Chance(2) ? "helmet" : null, Program.Chance(2) ? "chestpiece" : null, Program.Chance(2) ? "leggings" : null));
            }*/
        }

        public bool GetWall(int x, int y)
        {
            x += offsetX;
            y += offsetY;
            if (x >= 0 && x < width && y >= 0 && y < height) return tiles[x, y];
            else return true;
        }

        public void NextDungeonPortal()
        {
            //new Portal(end.x, end.y, nextSeed, level + 1);
        }
    }
}