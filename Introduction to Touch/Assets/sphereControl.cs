using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereControl : MonoBehaviour, IControllable
{

    public Vector3 drag_position;

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

    public void youveBeenTouched()
    {
        transform.position += Vector3.right;
    }

    public void youveBeenTapped()
    {
        Debug.Log("Tapped");
    }

    public void moveTo(Vector3 destination)
    {
        //Debug.Log("moveTo");
        drag_position = new Vector3(destination.x, destination.y, transform.position.z);
    }
}
