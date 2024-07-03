using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float i;
    private void Update()
    {
        transform.position = new Vector3(i*2, Erf(i)*2, 0);
        i += 0.1f;
        if (i > 5f) i = -5f;
    }

    static float Erf(double x)
    {
        // constants
        const float a1 =  0.254829592f;
        const float a2 = -0.284496736f;
        const float a3 =  1.421413741f;
        const float a4 = -1.453152027f;
        const float a5 =  1.061405429f;
        const float p  =  0.3275911f;
 
        // Save the sign of x
        var sign = 1;
        if (x < 0)
            sign = -1;
        x = Math.Abs(x);
 
        // A&S formula 7.1.26
        var t = 1.0f / (1.0f + p*x);
        var y = 1.0f - (((((a5*t + a4)*t) + a3)*t + a2)*t + a1)*t*Math.Exp(-x*x);
        
        return (float)(sign*y);
    }
}
