using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IControllable
{
    void youveBeenTouched();
    void youveBeenTapped();
    void moveTo(Vector3 destination);
}
