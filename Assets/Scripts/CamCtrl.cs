/* <8 - 5 - 2022>
 * Hussien Kenaan 
 *
 * This script is responsible for controlling the camera and orbiting around the playfield
 */
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    private Transform target;
    private Transform rotTarget;
    private Vector3 lastPos;

    public float sensitivity = 0.25f;

    private void Awake()
    {
        //set correct rotation pivot points
        rotTarget = transform.parent;
        target = rotTarget.transform.parent;
    }

    private void Update()
    {
        //stay looking at field always
        transform.LookAt(target);

        //get player input
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }

        //apply rotation function
        Orbit();
    }

    private void Orbit()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPos;
            float angleY = -delta.y * sensitivity;
            float angleX = delta.x * sensitivity;

            //X axis
            Vector3 angles = rotTarget.transform.eulerAngles;
            angles.x += angleY;
            angles.x = ClampAngles(angles.x, -85f, 85f);
            rotTarget.transform.eulerAngles = angles;

            //Y axis
            target.RotateAround(target.position, Vector3.up, angleX);

            lastPos = Input.mousePosition;
        }
    }

    //stop camera from rotatinf too far
    float ClampAngles(float angle, float from, float to)
    {
        if (angle < 0) angle = 360 + angle;

        if (angle > 180f) return Mathf.Max(angle, 360 + from);

        return Mathf.Min(angle, to);
    }
}
