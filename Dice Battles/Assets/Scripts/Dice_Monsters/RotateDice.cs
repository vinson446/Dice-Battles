using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDice : MonoBehaviour
{
    public float rotateX;
    public float rotateY;
    public float rotateZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateX, rotateY, rotateZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Summon Dice(Clone)" && collision.gameObject.name != "Attack Dice(Clone)" && collision.gameObject.name != "Defense Dice(Clone)")
        {
            rotateX = 0;
            rotateY = 0;
            rotateZ = 0;
        }
    }
}
