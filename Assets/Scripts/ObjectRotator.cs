using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // rotate only on y
            float yRot = Input.GetAxis("Mouse X") * -rotationSpeed;
            transform.Rotate(0, yRot, 0);
        }
    }
}
