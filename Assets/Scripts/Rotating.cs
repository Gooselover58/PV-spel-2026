using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField] Vector3 axis;
    [SerializeField] float rotateSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(axis * rotateSpeed);
    }
}
