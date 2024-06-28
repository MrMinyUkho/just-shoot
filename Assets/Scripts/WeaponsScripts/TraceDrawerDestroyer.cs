using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class TraceDrawerDestroyer : MonoBehaviour
{
    // Start is called before the first frame update

    readonly Color _c1 = new Color(0.9f, 0.5f, 0f);
    readonly Color _c2 = new Color(1, 1, 1, 0);
    private LineRenderer lr;
    
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        Debug.Log(lr);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.025f;
        lr.enabled = true;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = _c1;
        lr.endColor = _c2;
        Object.Destroy(gameObject, 1);
    }

    private void FixedUpdate()
    {
        lr.startWidth -= 0.001f;
        lr.endWidth -= 0.0005f;
    }
}
