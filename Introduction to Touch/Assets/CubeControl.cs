using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour, IControllable
{

    public void moveTo(Vector3 destination, RaycastHit otherSurface, float starting_distance_to_selected_object)
    {
        throw new System.NotImplementedException();
    }
    public void rotateBy(Quaternion rotation)
    {
        throw new System.NotImplementedException();
    }

    public void scaleBy(float distance, float scaleSpeed)
    {
        //transform.localScale = distance * Vector3.one;
    }

    public void youveBeenLongTapped()
    {
        //throw new System.NotImplementedException();
        GetComponent<Renderer>().material.color = Color.blue;
        Debug.Log(name + " long tapped");
    }

    public void youveBeenSelected()
    {
        //throw new System.NotImplementedException();
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void youveBeenTapped()
    {
        //throw new System.NotImplementedException();
        GetComponent<Renderer>().material.color = Color.red;
        Debug.Log(name + " tapped");
    }

    public void youveBeenUnselected()
    {
        //throw new System.NotImplementedException();
        GetComponent<Renderer>().material.color = Color.white;
        Debug.Log("Unselected");
    }

    public void moveToAccel(Vector3 dir)
    {
        transform.Translate(dir * 10f);
    }

    public void rotateByGyro(Quaternion rot)
    {
        transform.rotation = rot;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
