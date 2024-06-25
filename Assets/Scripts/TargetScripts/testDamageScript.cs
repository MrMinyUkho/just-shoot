using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamageScript : MonoBehaviour
{
    public float testTargethealth = 30f;

    public void TakenDamage(float amount)
    {
        testTargethealth -= amount;
        if (testTargethealth < 0f)
        {
            Destroy(gameObject);
        }
    }

}
