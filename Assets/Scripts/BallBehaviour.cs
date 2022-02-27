using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallSpawner.instance.StartRemoveFunction(transform.position.x, transform.position.y, transform.position.z);
        Destroy(gameObject);
    }
}
