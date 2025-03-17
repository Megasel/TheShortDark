using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public void Cut()
    {
        Destroy(gameObject);
    }
}
