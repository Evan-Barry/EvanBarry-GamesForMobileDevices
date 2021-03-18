using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchManagerScript_method1 : MonoBehaviour
{
    public float timeTouched;
    public float touchLength;
    public bool touchMoved;
    public Vector2 posTouch1Started;
    public Vector2 posTouch1Moved;
    public Vector2 posTouch2Started;
    public Vector2 posTouch2Moved;
    IControllable selectedObject;
    IControllable object_hit;
    float starting_distance_to_selected_object;

    public enum touchType {tap, long_tap, drag, scale, rotate, determining, none};
    public touchType touchValue = touchType.none;

    public float initScaleDistance;
    public float newScaleDistance;
    public float newScaleDistanceDelta;
    public float scaleAmount;
    public float initAngle;
    public float newAngle;
    public float angleDelta;
    public Vector3 rotateAmount;
    public float scaleSensitivity = 0.1f;
    public bool accelerometer = false;
    public bool gyroscope = false;
    public float cameraStrafeSpeed;
    public float cameraRotateSpeed;
    RaycastHit objectHit;
    public float wiggleRoom;
    public float scaleSpeed;
    public float tempAngleX, angleX;
    public float tempAngleY, angleY;

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touchMoved = false;
            Ray ourRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
            Debug.DrawRay(ourRay.origin, 30 * ourRay.direction);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ourRay);

            RaycastHit info;

            for(int i = 0; i < hits.Length; i++)
            {
               RaycastHit hit = hits[i];
               Collider hitGO = hit.transform.GetComponent<Collider>();

               if(hitGO)
               {
                   if(hitGO.transform.parent.name == "Objects for Collision")
                   {
                       objectHit = hits[i];
                       Debug.Log("Ray Hitting Object");
                   }

                   if(hitGO.transform.GetComponent<IControllable>() != null && Input.touches[0].phase == TouchPhase.Began)
                   {
                       if(selectedObject != null && selectedObject == object_hit)
                       {
                           selectedObject.youveBeenUnselected();
                           selectedObject = null;
                       }
                       
                       else
                       {
                            object_hit = hitGO.transform.GetComponent<IControllable>();
                            object_hit.youveBeenSelected();
                            selectedObject = object_hit;
                            starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, hitGO.transform.position);
                       }
                   }
               }
            }

            if(Input.touches.Length == 1)
            {
                switch(Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        timeTouched = Time.time;
                        
                        posTouch1Started = Input.touches[0].position;

                        break;

                    case TouchPhase.Moved:
                        touchMoved = true;

                        touchValue = touchType.drag;
                        break;

                    case TouchPhase.Stationary:
                        touchLength = Time.time - timeTouched;
                        if(touchLength > 0.3f)
                        {
                            //Debug.Log("Long Tap");
                            touchValue = touchType.long_tap;
                        }
                        break;

                    case TouchPhase.Ended:
                        touchLength = Time.time - timeTouched;

                        if(touchLength < 0.3f && !touchMoved)
                        {
                            //Debug.Log("Tap!");
                            touchValue = touchType.tap;
                        }
                        break;   
                }
            }

            else if(Input.touches.Length == 2)
            {
                switch(Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                    posTouch1Started = Input.touches[0].position;
                    break;

                    case TouchPhase.Moved:
                    posTouch1Moved = Input.touches[0].position;
                    break;

                }

                switch(Input.touches[1].phase)
                {
                    case TouchPhase.Began:
                    posTouch2Started = Input.touches[1].position;

                    initScaleDistance = Vector2.Distance(posTouch1Started, posTouch2Started);

                    break;

                    case TouchPhase.Moved:

                    //Debug.Log("Touch 1 movement rotation" + Input.touches[0].deltaPosition.magnitude);

                    if(Input.touches[0].deltaPosition.magnitude < wiggleRoom)//rotate
                    {
                        initAngle = Vector2.Angle(Input.touches[0].position, Input.touches[1].position);
                        newAngle = Vector2.Angle(Input.touches[0].position - Input.touches[0].deltaPosition, Input.touches[1].position - Input.touches[1].deltaPosition);

                        angleDelta = Mathf.DeltaAngle(newAngle, initAngle);
                        
                        touchValue = touchType.rotate;
                    }

                    else//scale
                    {
                        posTouch2Moved = Input.touches[1].position;

                        newScaleDistanceDelta = Vector2.Distance(posTouch1Moved - Input.touches[0].deltaPosition, posTouch2Moved - Input.touches[1].deltaPosition);


                        if(newScaleDistanceDelta > 300)
                        {
                            newScaleDistance = Vector2.Distance(posTouch1Moved,posTouch2Moved);
                            scaleAmount = newScaleDistance/initScaleDistance;
                            touchValue = touchType.scale;
                        }

                        else
                        {
                            touchValue = touchType.drag;
                        }
                    }
                    break;
                }
            }

            switch(touchValue)
            {
                case touchType.tap:
                    Debug.Log("Tap");

                    if(Physics.Raycast(ourRay, out info))
                        {
                            IControllable object_hit = info.transform.GetComponent<IControllable>();
                            if(object_hit != null)
                            {
                                object_hit.youveBeenTapped();
                            }
                        }
                    break;
                
                case touchType.long_tap:
                    Debug.Log("Long Tap");

                    if(Physics.Raycast(ourRay, out info))
                    {
                        IControllable object_hit = info.transform.GetComponent<IControllable>();
                        if(object_hit != null)
                        {
                            object_hit.youveBeenLongTapped();
                        }
                    }

                    break;

                case touchType.drag:
                    Debug.Log("Drag");

                    if(selectedObject != null)//Drag object
                    {
                        //Drag Code
                        //Strategy 1
                        //Keep same distance from camera to selected object
                        //Ray new_position_ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                        //selectedObject.moveTo(new_position_ray.GetPoint(starting_distance_to_selected_object));

                        //Stragey 2
                        //Theoretical Plane, move to plane - Screen to world point
                        //Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 10f));
                        //selectedObject.moveTo(point);

                        //Stategy 3
                        //Move on oblique surface(floor) - Raycast
                        //Vector3 point = new Vector3(objectHit.point.x, objectHit.point.y+0.5f, objectHit.point.z);
                        //selectedObject.moveTo(point);

                        //Strategy 4
                        //General Surface(Collider)


                        selectedObject.moveTo(Input.touches[0].position, objectHit, starting_distance_to_selected_object);
                    }

                    else if(Input.touches.Length == 1)//Strafe camera
                    {
                        Camera.main.transform.Translate(-Input.touches[0].deltaPosition.x * cameraStrafeSpeed * Time.deltaTime, 0, 0);
                    }

                    else if(selectedObject == null && Input.touches.Length == 2)//rotate camera up/down, left/right
                    {
                        Debug.Log("drag camera");
                        //Camera.main.transform.rotation = Quaternion.Euler(angleY, angleX, 0.0f);
                        Camera.main.transform.Rotate(Input.touches[0].deltaPosition.y * cameraRotateSpeed * Time.deltaTime, -Input.touches[0].deltaPosition.x * cameraRotateSpeed * Time.deltaTime, 0);
                    }

                    break;

                case touchType.scale:

                    if(selectedObject != null)
                    {
                        Debug.Log("Scale - Object");

                        selectedObject.scaleBy(scaleAmount, scaleSpeed);
                    }

                    else
                    {
                        Debug.Log("Scale - Zoom");

                        if(Camera.main.transform.position.y <= 0)
                        {
                            Debug.Log("Camera cannot move below floor (y is trying to go below 0)");
                            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y+0.1f, Camera.main.transform.position.z);
                        }

                        else if(Camera.main.transform.position.y >= 15)
                        {
                            Debug.Log("Camera cannot go too high (y is trying to go above 15)");
                            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-0.1f, Camera.main.transform.position.z);
                        }

                        else if(Camera.main.transform.position.x <= -15)
                    
                        {
                            Debug.Log("Camera cannot move outside bounds of floor (X is trying to go below -15)");
                            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+0.1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                        }

                        else if(Camera.main.transform.position.x >= 15)
                        {
                            Debug.Log("Camera cannot move outside bounds of floor (X is trying to go above 15)");
                            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-0.1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                        }

                        else if(Camera.main.transform.position.z <= -15)
                        {
                            Debug.Log("Camera cannot move outside bounds of floor (Z is trying to go below -15)");
                            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z+0.1f);
                        }

                        else if(Camera.main.transform.position.z >= 15)
                        {
                            Debug.Log("Camera cannot move outside bounds of floor (Z is trying to go above 15)");
                            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z-0.1f);
                        }

                        else
                        {
                            if(scaleAmount > 1)
                            {
                                //Camera.main.transform.position += (scaleAmount*scaleSensitivity) * transform.forward;
                                Camera.main.transform.Translate(Vector3.forward * (scaleAmount*scaleSensitivity));
                            }

                            else
                            {
                                //Camera.main.transform.position += (-scaleAmount*scaleSensitivity) * transform.forward;
                                Camera.main.transform.Translate(Vector3.forward * (-scaleAmount*scaleSensitivity));
                            }
                        }                        
                    }

                    break;

                case touchType.rotate:

                    // Vector3 v = Input.touches[1].position - Input.touches[0].position;

                    // float theta = Mathf.Atan(v.y/v.x);

                    // Quaternion rotation = Quaternion.AngleAxis(theta, Camera.main.transform.forward);

                    rotateAmount = new Vector3(0,0,-angleDelta);

                    if(selectedObject != null)
                    {
                        Debug.Log("Rotate - Object");

                        selectedObject.rotateBy(Quaternion.Euler(rotateAmount));
                    }

                    else
                    {
                        Debug.Log("Rotate - Camera");

                        Camera.main.transform.rotation *= Quaternion.Euler(rotateAmount);
                    }

                    break;

                case touchType.determining:
                    Debug.Log("Determining");
                    break;
            }

        }

        else//accelerometer/gyroscope
        {
            if(selectedObject != null)
            {
                if(touchValue == touchType.tap && accelerometer)//acceleromter move
                {
                    Vector3 dir = Vector3.zero;

                    dir.x = Input.acceleration.x;
                    dir.z = Input.acceleration.y;

                    if(dir.sqrMagnitude > 1)
                    {
                        dir.Normalize();
                    }

                    dir *= Time.deltaTime;

                    selectedObject.moveToAccel(dir);
                }

                else if(touchValue == touchType.long_tap && gyroscope)//gyroscope rotate
                {
                    selectedObject.rotateByGyro(new Quaternion(Input.gyro.attitude.x, -Input.gyro.attitude.y, -Input.gyro.attitude.w, -Input.gyro.attitude.z));
                }
            }
        }
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
        GUILayout.Label("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
        if(Input.touches.Length == 1)
        {
            GUILayout.Label("Touch 1 Screen Pos - " + Camera.main.ScreenToWorldPoint(Input.touches[0].position));
        }
        if(Input.touches.Length == 2)
        {
            GUILayout.Label("Touch 1 Screen Pos - " + Camera.main.ScreenToWorldPoint(Input.touches[0].position));
            GUILayout.Label("Touch 2 Screen Pos - " + Camera.main.ScreenToWorldPoint(Input.touches[1].position));
        }
    }

    public void onClick()
    {
        SceneManager.LoadScene("2_Touches");
        Debug.Log("Reload Scene");
    }
}
