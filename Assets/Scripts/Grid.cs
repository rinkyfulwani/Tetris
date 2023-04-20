using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour
{
    [Header("Blocks")]
    public GameObject[] blockList;
    public GameObject[] ghostList;
    // create public variables for the different planes
    public GameObject bottom;
    public GameObject north;
    public GameObject south;
    public GameObject west;
    public GameObject east;
    
    // create public variables for the size of the grid and the actual grid
    private int xGrid;
    private int yGrid;
    private int zGrid;
    public Transform[,,] grid;
    public static Grid Instance;

    private int randomIndex;
    private int currentBlockIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        grid = new Transform[xGrid, yGrid, zGrid];
        RandomizeIndex();
        GenerateNewBlocks();

    }

    public void ChangeGridSize(int x, int y, int z) {
        xGrid = x;
        yGrid = y;
        zGrid = z;
        ResizeWholeGrid();
        
        CameraController.currentInstance.MoveFocusPoint(new Vector3((float) xGrid/2, (float) yGrid/2, (float) zGrid/2));
    }

    public Vector3 VectorRound(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x),
                            Mathf.Round(position.y),
                            Mathf.Round(position.z));
    }

    public void UpdateGrid(Block block)
    {
        for (int x = 0; x < xGrid; x++)
        {
            for (int y = 0; y < yGrid; y++)
            {
                for (int z = 0; z < zGrid; z++)
                {
                    //print("The grid parent" + grid[x, y, z].parent);
                    if(grid[x, y,z] != null)
                    {
                        if (grid[x, y, z].parent == block.transform)
                        {
                            grid[x, y, z] = null;
                        }
                    }
                    
                }
            }
        }
        foreach(Transform childTransform in block.transform)
        {
            Vector3 childPosition = VectorRound(childTransform.position);
            if(childPosition.y < yGrid)
            {
                grid[(int)childPosition.x, (int)childPosition.y, (int)childPosition.z] = childTransform;
            }
        }
    }

    public Transform GetTransformOnGridPosition(Vector3 position)
    {
        if(position.y > yGrid - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)position.x, (int)position.y, (int)position.z];
        }
    }

    public void GenerateNewBlocks()
    {
        GameManager.currentInstance.SetNormalSpeed();
        Vector3 blockPosition = new Vector3((int)(transform.position.x + (float)xGrid / 2),
                                            (int)transform.position.y + yGrid,
                                            (int)(transform.position.z + (float)zGrid / 2));
        GameObject newBlock = Instantiate(blockList[randomIndex], blockPosition, Quaternion.identity) as GameObject;
        //Generate ghost block 
        GameObject newGhostBlock = Instantiate(ghostList[randomIndex], blockPosition, Quaternion.identity) as GameObject;
        currentBlockIndex = randomIndex;
        newGhostBlock.GetComponent<GhostBlock>().SetParent(newBlock);
        RandomizeIndex();
    }

    private void RandomizeIndex() {
        randomIndex = Random.Range(0, blockList.Length);
        BlockPreview.currentInstance.AppearPreviewBlock(randomIndex);
    }

    public bool IsInsideGrid(Vector3 position)
    {
        return ((int)position.x >= 0 && (int)position.x < xGrid &&
        (int)position.y >= 0 &&
        (int)position.z >= 0 && (int)position.z < zGrid);
    }
    // function that will rezise the grid
    private void ResizeWholeGrid() {

        // if we have dragged something into bottom plane slot in editor
        if (bottom != null) {
            ResizeGrid(bottom);
        }
        if (north != null) {
            ResizeGrid(north);
        }
        if (south != null) {
            ResizeGrid(south);
        }
        if (west != null) {
            ResizeGrid(west);
        }
        if (east != null) {
            ResizeGrid(east); 
        }

    }

    // function for resizing the grid.
    void ResizeGrid(GameObject gridSide) {

        var currentXPos = transform.position.x;
        var currentYPos = transform.position.y;
        var currentZPos = transform.position.z;

        float xScaled = (float) xGrid / 10;
        float yScaled = (float) yGrid / 10;
        float zScaled = (float) zGrid / 10;

        Vector3 newPosition = new Vector3();
        Vector3 reziser = new Vector3();
        Vector2 newScale = new Vector2();

        if (gridSide == bottom) {
            
            // rezise the  plane
            reziser = new Vector3(xScaled, 1, zScaled);

            // reposition the plane
            newPosition = new Vector3(currentXPos + (float) xGrid/2, currentYPos, currentZPos + (float) zGrid/2);

            // resize the tiling scaling of the plane
            newScale =  new Vector2(xGrid, zGrid);

        } else if (gridSide == north || gridSide == south) {

            reziser = new Vector3(xScaled, 1, yScaled);

            if (gridSide == north) {
                newPosition = new Vector3(currentXPos + (float) xGrid/2, currentYPos + (float) yGrid/2, currentZPos + (float) zGrid);
            } else if (gridSide == south) {
                newPosition = new Vector3(currentXPos + (float) xGrid/2, currentYPos + (float) yGrid/2, currentZPos);
            }

            newScale =  new Vector2(xGrid, yGrid);

        } else if (gridSide == west || gridSide == east) {

            reziser = new Vector3(zScaled, 1, yScaled);

            if (gridSide == west) {
                newPosition = new Vector3(currentXPos, currentYPos + (float) yGrid/2, currentZPos + (float) zGrid/2);
            } else if (gridSide == east) {
                newPosition = new Vector3(currentXPos + xGrid, currentYPos + (float) yGrid/2, currentZPos + (float) zGrid/2);
            }

            newScale =  new Vector2(zGrid, yGrid);
        }          

        // resize 
        gridSide.transform.localScale = reziser;

        // reposition
        gridSide.transform.position = newPosition;

        // resize the tiling
        var meshRenderer = gridSide.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial.mainTextureScale = newScale;

    }

    // function that checks if grid layer is full. if full --> remove layer
    public void RemoveGridLayer() {
        int numOfLayersDeleted = 0;
        // check which layer to remove
        for (int y = yGrid - 1; y >= 0; y--) {
            // we will check each layer 
            // if layer full --> delete blocks --> move everything down
            if (IsLayerFull(y))
            {   
                // remove the actual layer
                RemoveLayer(y);
                // move all the layers down one step
                MoveLayers(y);
                numOfLayersDeleted += 1;
            }
        }
        if (numOfLayersDeleted != 0) {
            GameManager.currentInstance.UpdateClearedLayers(numOfLayersDeleted);
        }
    }

    private void RemoveLayer(int y) {
        for (int x = 0; x < xGrid; x++)
        {
            for (int z = 0; z < zGrid; z++)
            {
                Destroy(grid[x,y,z].gameObject);
                grid[x,y,z] = null;
            }
        }
    }

    // function that checks if full layer
    private bool IsLayerFull(int y) {
        for (int x = 0; x < xGrid; x++)
        {
            for (int z = 0; z < zGrid; z++)
            {
                if (grid[x, y, z] == null) {
                    return false;
                }
            }
        }
        return true;
    }

    // function that move layers one step down
    private void MoveLayers(int y) {
        // iterate through all the y values 
        for (int i = y; i < yGrid; i++)
        {
            MoveLayer(i);
        }
    }

    // move one layer at a time
    private void MoveLayer(int y) {
        for (int x = 0; x < xGrid; x++)
        {
            for (int z = 0; z < zGrid; z++)
            {
                if (grid[x,y,z] != null) {
                    // copy the current grid down one step
                    grid[x,y-1,z] = grid[x,y,z];
                    // empty the current grid
                    grid[x,y,z] = null;
                    // move down the position of the grid
                    grid[x,y-1,z].position += Vector3.down;
                }
            }
        }
    }
}
