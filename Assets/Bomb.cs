using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius = 4;
    public float force = 1;
    public LayerMask layerToHit;
    public GameObject explodePrefab;

    void Update()
    {
        switch(Input.GetKeyDown(KeyCode.B))
        {
            case true:
                Explode();
                break;
        }
    }

    void Explode()
    {
        Instantiate(explodePrefab, new Vector2(0, 0), transform.rotation, transform);

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius, layerToHit);

        foreach(Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
