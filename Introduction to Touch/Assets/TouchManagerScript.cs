using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManagerScript : MonoBehaviour
{
    public float timeTouched;
    public float touchLength;
    public bool touchMoved;
    IControllable selectedObject;
    float starting_distance_to_selected_object;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ourCameraPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ourCameraPlane.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        ourCameraPlane.transform.up = (Camera.main.transform.position - ourCameraPlane.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touchMoved = false;
            Ray ourRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
            Debug.DrawRay(ourRay.origin, 30 * ourRay.direction);

            RaycastHit info;

            if(Physics.Raycast(ourRay, out info))
            {
                IControllable object_hit = info.transform.GetComponent<IControllable>();
                if(object_hit != null)
                {
                    object_hit.youveBeenTouched();
                    selectedObject = object_hit;
                    starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, info.transform.position);
                }
            }

            switch(Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    timeTouched = Time.time;
                    break;

                case TouchPhase.Moved:
                    touchMoved = true;

                    Debug.Log("Dragging");
                    if(selectedObject != null)
                    {
                        //Drag Code
                        Ray new_position_ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                        selectedObject.moveTo(new_position_ray.GetPoint(starting_distance_to_selected_object));
                    }
                    break;

                case TouchPhase.Ended:
                    //Debug.Log("Touch Phase - Ended");
                    touchLength = Time.time - timeTouched;
                    //Debug.Log("Touch Length - " + touchLength);

                    if(touchLength < 0.3f && !touchMoved)
                    {
                        Debug.Log("Tap!");
                        if(Physics.Raycast(ourRay, out info))
                        {
                            IControllable object_hit = info.transform.GetComponent<IControllable>();
                            if(object_hit != null)
                            {
                                object_hit.youveBeenTapped();
                            }
                        }
                    }
                    break;

                
            }

            // if(Input.GetTouch(0).phase == TouchPhase.Began)
            // {
            //     //Debug.Log("Touch Phase - Began");
            //     timeTouched = Time.time;
            // }

            // if(Input.GetTouch(0).phase == TouchPhase.Moved)
            // {
            //     touchMoved = true;

            //     Debug.Log("Dragging");
            //     if(selectedObject != null)
            //     {
            //         //Drag Code
            //         Ray new_position_ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            //         selectedObject.moveTo(new_position_ray.GetPoint(starting_distance_to_selected_object));
            //     }
            // }

            // if(Input.GetTouch(0).phase == TouchPhase.Ended)
            // {
            //     //Debug.Log("Touch Phase - Ended");
            //     touchLength = Time.time - timeTouched;
            //     //Debug.Log("Touch Length - " + touchLength);

            //     if(touchLength < 0.3f && !touchMoved)
            //     {
            //         Debug.Log("Tap!");
            //         if(Physics.Raycast(ourRay, out info))
            //         {
            //             IControllable object_hit = info.transform.GetComponent<IControllable>();
            //             if(object_hit != null)
            //             {
            //                 object_hit.youveBeenTapped();
            //             }
            //         }
            //     }
            // }
        }
    }
}
