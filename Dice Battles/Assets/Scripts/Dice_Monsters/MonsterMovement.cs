using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed;
    public float distance;
    public Transform movePoint;

    public LayerMask emptyGridLayer;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // monster always move towards movePoint, need to turn off script when not needed
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // if monster is on movePoint, allow moster to be moved by player input while checking for collision
        if (Vector3.Distance(transform.position, movePoint.position) < 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
            {
                Collider[] colls = Physics.OverlapBox(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") * distance, 0, 0), transform.localScale, Quaternion.identity, emptyGridLayer);

                if (colls.Length == 0)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * distance, 0, 0);
                }
                else
                {
                    for (int i = 0; i < colls.Length; i++)
                    {
                        colls[i] = null;
                    }
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
            {
                Collider[] colls = Physics.OverlapBox(movePoint.position + new Vector3(0, 0, Input.GetAxisRaw("Vertical") * distance), transform.localScale, Quaternion.identity, emptyGridLayer);

                if (colls.Length == 0)
                {
                    movePoint.position += new Vector3(0, 0, Input.GetAxisRaw("Vertical") * distance);
                }
                else
                {
                    for (int i = 0; i < colls.Length; i++)
                    {
                        colls[i] = null;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(movePoint.position, transform.localScale);
    }
}
