/* < 8 - 7 - 2022 >
 * Hussien kenaan
 * 
 * This script is for creating the preview GO when called
 */
using UnityEngine;

public class Previewer : MonoBehaviour
{
    public static Previewer Instance;

    //MAKE SURE PREIVEW GO ARE SAME AS THERE ORIGINAL BLOCKS IN ORDER
    public GameObject[] previewBlocks;

    private GameObject currentActive;

    private void Awake()
    {
        Instance = this;
    }

    //destroy old preview and create new one when needed
    public void ShowPreview(int index)
    {
        Destroy(currentActive);

        currentActive = Instantiate(previewBlocks[index], transform.position, Quaternion.identity) as GameObject;
    }
}
