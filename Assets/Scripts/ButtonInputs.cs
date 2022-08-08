/* < 8 - 6 - 2022 >
 * Hussien kenaan
 * 
 * This script is for getting input from canvas and repostioning canvas at correct position
 */

using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public static ButtonInputs Instance { get; private set; }

    [SerializeField] private GameObject moveCanvas;
    [SerializeField] private GameObject[] rotationCanvases;

    bool moveIsOn = true;

    private GameObject activeBlock;
    private TetrisBlock activeTetris;

    private void Awake()
    {
        Instance = this;
    }

    //set position same as falling block
    private void RepositionToActiveBlock()
    {
        if (activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    private void Update()
    {
        RepositionToActiveBlock();
    }

    //assign the falling block from outside
    public void SetActiveBlock(GameObject block, TetrisBlock tetrisBlock)
    {
        activeBlock = block;
        activeTetris = tetrisBlock;
    }

    //get input and send to falling tetris scrupt
    public void MoveBlock(string direction)
    {
        if (activeBlock != null)
        {
            if (direction == "Left")
            {
                activeTetris.setMoveInput(Vector3.left);
            }
            else if (direction == "Right")
            {
                activeTetris.setMoveInput(Vector3.right);
            }
            else if (direction == "Forward")
            {
                activeTetris.setMoveInput(Vector3.forward);
            }
            else if (direction == "Backward")
            {
                activeTetris.setMoveInput(Vector3.back);
            }
        }
    }
    public void RotateBlock(string rotation)
    {
        if (rotation == "PosX")
        {
            activeTetris.SetRotationInput(new Vector3(0, 0, 90));
        }
        else if (rotation == "NegX")
        {
            activeTetris.SetRotationInput(new Vector3(0, 0, 90));
        }
        else if (rotation == "PosY")
        {
            activeTetris.SetRotationInput(new Vector3(0, 90, 0));
        }
        else if (rotation == "NegY")
        {
            activeTetris.SetRotationInput(new Vector3(0, -90, 0));
        }
        else if (rotation == "PosZ")
        {
            activeTetris.SetRotationInput(new Vector3(90, 0, 0));
        }
        else if (rotation == "NegZ")
        {
            activeTetris.SetRotationInput(new Vector3(-90, 0, 0));
        }
    }

    //switch between rotate and move input mods
    public void SwitchInputs()
    {
        moveIsOn = !moveIsOn;
        moveCanvas.SetActive(moveIsOn);

        foreach (GameObject c in rotationCanvases)
        {
            c.SetActive(!moveIsOn);
        }
    }
    //switch block from slow to fast falling
    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}
