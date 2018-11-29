using System;
using UnityEngine;

public class RotateForword : MonoBehaviour
{
    public float Speed = 400f;

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime *Speed);
        return;
    }
}