using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereControl : MonoBehaviour, IControllable
{

    public Vector3 drag_position;
    public enum moveMethod {sameDistance, toPlane, onPlane, none};
    public moveMethod moveValue = moveMethod.none;

    // Start is called before the first frame update
    void Start()
    {
        drag_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, drag_position, 0.01f);
    }

    public void youveBeenTapped()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void youveBeenLongTapped()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    public void moveTo(Vector3 inputDestination, RaycastHit otherSurface, float starting_distance_to_selected_object)
    {
        //Debug.Log("moveTo");
        Vector3 moveToDestination = Vector3.zero;

        switch(moveValue)
            {
                case moveMethod.sameDistance:
                    Ray new_position_ray = Camera.main.ScreenPointToRay(inputDestination);
                    moveToDestination = new_position_ray.GetPoint(starting_distance_to_selected_object);
                    break;

                case moveMethod.toPlane:
                    moveToDestination = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 10f));
                    break;

                case moveMethod.onPlane:
                    moveToDestination = new Vector3(otherSurface.point.x, otherSurface.point.y+(transform.localScale.y/2f), otherSurface.point.z);
                    break;
            }

        drag_position = new Vector3(moveToDestination.x, moveToDestination.y, moveToDestination.z);
    }

    public void youveBeenSelected()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void youveBeenUnselected()
    {
        GetComponent<Renderer>().material.color = Color.white;
        drag_position = transform.position;
        Debug.Log("Unselected");
    }

    public void scaleBy(float distance, float scaleSpeed)
    {
        transform.localScale = distance * Vector3.one;
    }

    public void rotateBy(Quaternion rotation)
    {

    }

    public void moveToAccel(Vector3 dir)
    {

    }

    public void rotateByGyro(Quaternion rot)
    {

    }
}
