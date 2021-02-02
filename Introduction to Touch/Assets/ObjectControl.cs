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
        transform.position += Vector3.down;
    }

    public void youveBeenTapped()
    {
        Debug.Log("Tapped");
    }

    public void moveTo(Vector3 destination)
    {
        
    }
}
