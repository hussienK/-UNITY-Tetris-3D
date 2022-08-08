/* < 8 - 5 - 2022 >
 * Hussien kenaan
 * 
 * This script is for Managing the blocks and keeping store of game data in list
 */
using UnityEngine;

public class PlayField : MonoBehaviour
{
    

    [SerializeField] private int gridSizeX, gridSizeY, gridSizeZ;
    [SerializeField] Transform[,,] theGrid;

    [Header("Blocks")]
    [SerializeField] private GameObject[] blockList;
    //MAKE SURE GHOST LIST ORDER IS SAME AS BLOCLIST
    [SerializeField] private GameObject[] GhostList;

    [Header("Gane Visuals")]
    [SerializeField] Transform CameraTarget;
    [SerializeField] private GameObject bottomPlane;
    [SerializeField] private GameObject N, S, W, E;

    private int randomIndex;
    public static PlayField Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        SpawnNewBlock();
    }

    //round floats to integers
    public Vector3 Round(Vector3 _v)
    {
        return new Vector3(Mathf.RoundToInt(_v.x),
                            Mathf.RoundToInt(_v.y),
                            Mathf.RoundToInt(_v.z));
    }

    //check if still inside the playfield
    public bool CheckInsidePlayField(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX &&
                (int)pos.z >= 0 && (int)pos.z < gridSizeZ &&
                (int)pos.y >= 0);
    }

    //keep grid data updated with current visuals
    public void UpdateGrid(TetrisBlock block)
    {
        //Delete possible parent objects
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (theGrid[x, y, z] != null)
                    {
                        if (theGrid[x, y, z].parent == block.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        //fill in all child objects
        foreach (Transform child in block.transform)
        {
            Vector3 pos = Round(child.position);
            if (pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }

    //get position in grid for GO
    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        if (pos.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    //create a block from list
    public void SpawnNewBlock()
    {
        //calculate middle of the grid
        Vector3 spawnPos = new Vector3((int)(transform.position.x + (float)gridSizeX / 2),
                                        (int)transform.position.y + gridSizeY,
                                        (int)(transform.position.z + (float)gridSizeZ / 2));
        //select random block and spawn with ghost and preview
        GameObject newBlock = Instantiate(blockList[randomIndex], spawnPos, Quaternion.identity) as GameObject;
        GameObject newGhostBlock = Instantiate(GhostList[randomIndex], spawnPos, Quaternion.identity) as GameObject;
        newGhostBlock.GetComponent<GhostBlock>().setParent(newBlock, newBlock.GetComponent<TetrisBlock>());
        CalculatePreview();
        Previewer.Instance.ShowPreview(randomIndex);
    }

    public void CalculatePreview()
    {
        randomIndex = Random.Range(0, blockList.Length);
    }

    //loops through all layers and deletes complete ones
    public void DeleteLayer()
    {
        int layersCleared = 0;
        for (int y = gridSizeY - 1; y >= 0; y--)
        {
            //check full layer
            if (CheckFullLayer(y))
            {
                //delete All blocks
                DeleteLayerAt(y);
                //move all down by 1
                MoveAllLayerDown(y);

                layersCleared++;
            }
        }
        if (layersCleared > 0)
        {
            GameManager.Instance.SetlayerCleared(layersCleared); 
        }
    }

    //return if the cuurent layer is complete
    private bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (theGrid[x,y,z] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    //delete the selected layer
    private void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                //delete GO
                Destroy(theGrid[x, y, z].gameObject);
                //empt the list
                theGrid[x, y, z] = null;
            }
        }
    }

    //loop through all layers and move down
    private void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    //move exactly one layer down
    private void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (theGrid[x, y, z] != null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (bottomPlane != null)
        {
            //resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;

            //Reposition based on size change to  keep in center 
            bottomPlane.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                            transform.position.y,
                                                            transform.position.z + (float)gridSizeZ / 2);

            //retile material
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }

        if (N != null)
        {
            //resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            N.transform.localScale = scaler;

            //Reposition based on size change to  keep in center 
            N.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                            transform.position.y + (float)gridSizeY / 2,
                                                            transform.position.z + gridSizeZ);

            //retile material
            N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (S != null)
        {
            //resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            S.transform.localScale = scaler;

            //Reposition based on size change to  keep in center 
            S.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                            transform.position.y + (float)gridSizeY / 2,
                                                            transform.position.z);
        }

        if (E != null)
        {
            //resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            E.transform.localScale = scaler;

            //Reposition based on size change to  keep in center 
            E.transform.position = new Vector3(transform.position.x + (float)gridSizeX,
                                                            transform.position.y + (float)gridSizeY / 2,
                                                            transform.position.z + (float)gridSizeZ / 2);

            //retile material
            E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

        if (W != null)
        {
            //resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            W.transform.localScale = scaler;

            //Reposition based on size change to  keep in center 
            W.transform.position = new Vector3(transform.position.x,
                                                            transform.position.y + (float)gridSizeY / 2,
                                                            transform.position.z + (float)gridSizeZ / 2);

        }

        if (CameraTarget != null)
        {
            CameraTarget.position = new Vector3((float)gridSizeX / 2, (float)gridSizeY / 2, (float)gridSizeZ / 2);
        }
    }

}
