/* < 8 - 5 - 2022 >
 * Hussien kenaan
 * 
 * This script is for moving the block and rotating it
 */
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    private float prevTime;
    private float fallTime = 3f;

    private void Start()
    {
        //assign vars
        ButtonInputs.Instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.Instance.ReadFallSpeed();

        //check if player lost
        if (!CheckValidMove())
        {
            GameManager.Instance.SetGameOver();
        }
    }

    private void Update()
    {
        //check if time to move
        if (Time.time - prevTime > fallTime)
        {
            //move down
            transform.position += Vector3.down;

            //if is outside grid return to inside and stop movement
            if (!CheckValidMove())
            {
                transform.position += Vector3.up;
                PlayField.Instance.DeleteLayer();
                enabled = false;

                if (!GameManager.Instance.ReadGameOver())
                {
                    PlayField.Instance.SpawnNewBlock();
                }
            }
            //if can move
            else
            {
                //Update the grid
                PlayField.Instance.UpdateGrid(this);
            }

            prevTime = Time.time;
        }
    }

    //check if still inside field
    private bool CheckValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = PlayField.Instance.Round(child.position);
            if (!PlayField.Instance.CheckInsidePlayField(pos))
            {
                return false;
            }
        }

        //check if other block in position
        foreach (Transform child in transform)
        {
            Vector3 pos = PlayField.Instance.Round(child.position);
            Transform t = PlayField.Instance.GetTransformOnGridPos(pos);

            if (t != null && t.parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    //move the block
    public void setMoveInput(Vector3 direction)
    {
        transform.position += direction;

        //if not allowed to move return else update the grid
        if (!CheckValidMove())
        {
            transform.position -= direction;
        }
        else
        {
            PlayField.Instance.UpdateGrid(this);
        }
    }

    //rotate the block
    public void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);

        if (!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);
        }
        else
        {
            PlayField.Instance.UpdateGrid(this);
        }
    }

    //change my speed
    public void SetSpeed()
    {
        fallTime = 0.1f;
    }
}
