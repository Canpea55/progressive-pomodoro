using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apoptosis : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Update()
    {
        Vector2 currentPos = transform.position;
        if(currentPos.x > maxX || currentPos.x < minX || currentPos.y > maxY || currentPos.y < minY)
        {
            Destroy(gameObject);
        }
    }
}
