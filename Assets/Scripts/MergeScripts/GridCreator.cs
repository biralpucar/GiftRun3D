using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridCreator : MonoSingleton<GridCreator>
{
    public int Height;
    public int Width;
    public float gridSpaceSize;
    public float gridDisparity;
    public GameObject cellPrefab;
    GameObject[,] Grids;
    public List<GameObject> AllGrids = new List<GameObject>();
    public Transform parentGrid;


    protected override void Awake()
    {
        base.Awake();
        CreateGrid();
    }

    private void Start()
    {

    }

    void CreateGrid()
    {
        if (cellPrefab == null) return;
        Grids = new GameObject[Height, Width];
        for (int x = 0; x < Height; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                Grids[x, z] = Instantiate(cellPrefab, new Vector3(x * gridSpaceSize - gridDisparity, transform.position.y, z * gridSpaceSize - gridDisparity), Quaternion.identity);
                Grids[x, z].transform.parent = parentGrid;
                Grids[x, z].gameObject.name = "Cell ( X: " + x.ToString() + " , Y: " + z.ToString() + ")";
                AllGrids.Add(Grids[x, z].gameObject);
            }
        }

        GridPositions();
    }

    void GridPositions()
    {
        for (int i = 0; i < AllGrids.Count; i++)
        {
            ItemThrowManager.instance.GetCellPos(AllGrids[i].transform.position);

        }
    }


}
