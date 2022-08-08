/* < 8 - 7 - 2022 >
 * Hussien kenaan
 * 
 * This script is for creating and positioning the ghost block
 */
using System.Collections;
using UnityEngine;

public class GhostBlock : MonoBehaviour
{
    private GameObject parent;
    private TetrisBlock parentTetris;

    private void Start()
    {
        StartCoroutine(RepositionBlock());
    }

    //set my variables from outside
    public void setParent(GameObject _parent, TetrisBlock _ParentTetris)
    {
        parent = _parent;
        parentTetris = _ParentTetris;
    }

    //set transformation same as normal block
    private void PositionGhost()
    {
        transform.position = parent.transform.position;
        transform.rotation = parent.transform.rotation;
    }

    //applly the changes evey 0.1 seconds
    private IEnumerator RepositionBlock()
    {
        while (parentTetris.enabled)
        {
            PositionGhost();
            //move downPos
            MoveDown();
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
        yield return null;
    }

    //apply the downword move
    private void MoveDown()
    {
        while (CheckValidMove())
        {
            transform.position += Vector3.down;
        }
        if (!CheckValidMove())
        {
            transform.position -= Vector3.down;
        }
    }

    //check if can move 
    private bool CheckValidMove()
    {
        //if child inside grid
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

            if (t != null && t.parent == parent.transform)
            {
                return true;
            }

            if (t != null && t.parent != transform)
            {
                return false;
            }
        }

        return true;
    }
}
