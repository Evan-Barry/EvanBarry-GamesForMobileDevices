using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour, IControllable
{
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void youveBeenTouched()
    {
        
    }

    public void youveBeenSelected()
    {
        //transform.position += Vector3.down;
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void youveBeenUnselected()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void youveBeenTapped()
    {
        //Debug.Log("Tapped");
        //transform.position += Vector3.right*Time.deltaTime;
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void youveBeenLongTapped()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    public void moveTo(Vector3 destination, RaycastHit otherSurface, float starting_distance_to_selected_object)
    {
        transform.Translate(destination * 10f);
    }

    public void scaleBy(float distance, float scaleSpeed)
    {
        transform.localScale = distance * Vector3.one;
    }

    public void rotateBy(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void moveToAccel(Vector3 dir)
    {

    }

    public void rotateByGyro(Quaternion rot)
    {
        
    }
}
