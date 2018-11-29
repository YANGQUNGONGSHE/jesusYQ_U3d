using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBack : MonoBehaviour 
{
    public float Speed = 400f;

    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime *Speed);
        return;
    }
}

