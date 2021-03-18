using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IControllable
{
    void youveBeenTapped();
    void youveBeenLongTapped();
    void moveTo(Vector3 inputDestination, RaycastHit otherSurface, float starting_distance_to_selected_object);
    void youveBeenSelected();
    void youveBeenUnselected();
    void scaleBy(float distance, float scaleSpeed);
    void rotateBy(Quaternion rotation);
    void moveToAccel(Vector3 dir);
    void rotateByGyro(Quaternion rot);
}
