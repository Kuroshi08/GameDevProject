using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Collider2D col;
    List<Collider2D> a;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        a = new List<Collider2D>();
        col = GetComponent<Collider2D>();

        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    // Update is called once per frame
    void Update()
    {
        col.Overlap(a);
        if(a.Count != 0)
        {Debug.Log(a[0].gameObject.name);}
    }
}
