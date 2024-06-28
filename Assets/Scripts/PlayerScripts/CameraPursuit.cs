using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPursuit : MonoBehaviour
{
    public GameObject target;
    public float lerpMoveK = 0.5f;
    public float lerpRotK = 0.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, target.transform.position, lerpMoveK);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, lerpRotK);
        
    }
}
