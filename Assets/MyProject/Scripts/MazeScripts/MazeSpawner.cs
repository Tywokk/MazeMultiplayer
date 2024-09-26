using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;

public class MazeSpawner : NetworkBehaviour
{
    public GameObject _maze;
    public Cell CellPrefab;
    public Cell CellPrefab1;
    public Cell CellPrefab2;
    public Cell CellPrefab3;
    public Cell CellPrefab4;
    public Cell CellPrefab5;
    public Vector3 CellSize;
    //public HintRenderer HintRenderer;
    [SerializeField] private List<Cell> spawnedCellsWithDeactivateLeftWall = new List<Cell>();
    [SerializeField] private List<Cell> spawnedCellsWithDeactivateBottomWall = new List<Cell>();
    int random;
    int random2;
    public Maze maze;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        if (IsHost)
        {
            MazeGenerator generator = new MazeGenerator();
            maze = generator.GenerateMaze();
            int lastX = 0;
            int lastY = 0;
            for (int x = 0; x < maze.cells.GetLength(0); x++)
            {
                for (int y = 0; y < maze.cells.GetLength(1); y++)
                {
                    bool canSpawn = true;
                    Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x * CellSize.x, 0, y * CellSize.z), 50f);
                    //DrawSphere(new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), 20f);
                    foreach (var iter in hitColliders)
                    {
                        if (iter.gameObject.tag == "Tower")
                        {
                            canSpawn = false;
                            Debug.Log("Tower");
                            break;
                        }
                    }
                    random = UnityEngine.Random.Range(0, 1000);
                    if (random < 870)
                    {
                        CellPrefab = CellPrefab1;
                    }
                    else if (random >= 870 && random < 930)
                    {
                        CellPrefab = CellPrefab3;
                    }
                    else if (random >= 930 && random < 980 && x != 0 && x != generator.Width - 1)
                    {
                        CellPrefab = CellPrefab2;
                    }
                    else if (random >= 980 && random < 990 && x != 0 && x != generator.Width - 1 && y != 0 && y < generator.Height - 2 && canSpawn)
                    {
                        CellPrefab = CellPrefab4;
                        lastX = x;
                        lastY = y;
                    }
                    else if (random >= 990 && random < 1000 && x != 0 && x != generator.Width - 1 && y != 0 && y < generator.Height - 2 && canSpawn)
                    {
                        CellPrefab = CellPrefab5;
                        lastX = x;
                        lastY = y;
                    }
                    Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity).GetComponent<Cell>();
                    MeshRenderer wallLeftMeshRenderer = c.WallLeft.GetComponent<MeshRenderer>();
                    MeshRenderer wallBottomMeshRenderer = c.WallBottom.GetComponent<MeshRenderer>();
                    Collider wallLeftCollider = c.WallLeft.GetComponent<Collider>();
                    Collider wallBottomCollider = c.WallBottom.GetComponent<Collider>();
                    wallLeftMeshRenderer.enabled = maze.cells[x, y].WallLeft;
                    wallLeftCollider.enabled = maze.cells[x, y].WallLeft;
                    wallBottomMeshRenderer.enabled = maze.cells[x, y].WallBottom;
                    wallBottomCollider.enabled = maze.cells[x, y].WallBottom;
                    CellPrefab = CellPrefab1;
                    if (c.Boosts.Count != 0)
                    {
                        c.Boosts[UnityEngine.Random.RandomRange(0, c.Boosts.Count)].SetActive(true);
                    }
                    random2 = UnityEngine.Random.Range(0, 1000);
                    if (random2 >= 850 && random2 < 900 && x != 0 && x != generator.Width - 1)
                    {
                        wallLeftMeshRenderer.enabled = false;
                        wallLeftCollider.enabled = false;
                    }
                    else if (random2 >= 950 && random2 < 1000 && y != 0 && y != generator.Height - 1)
                    {
                        wallBottomMeshRenderer.enabled = false;
                        wallBottomCollider.enabled = false;
                    }
                    /*
                    if (c.WallLeft.GetComponent<MeshRenderer>().enabled == false)
                    {
                        spawnedCellsWithDeactivateLeftWall.Add(c);
                    }
                    if (c.WallBottom.GetComponent<MeshRenderer>().enabled == false)
                    {
                        spawnedCellsWithDeactivateBottomWall.Add(c);
                    }
                    */
                    c.GetComponent<Cell>().parent = this;
                    c.GetComponent<NetworkObject>().Spawn(true);
                }
            }
            //HintRenderer.DrawPath();
        }
    }
}
