using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GoToPos(transform.position,2);
    }
    void GoToPos(Vector2 Pos, float dur)
    {
        float a = 0;
        Vector3 OldPos = Camera.transform.position;
        Vector2 MoveV = Pos - (Vector2)Camera.transform.position;
        while(dur > a)
        {
            a += Time.deltaTime;
            Camera.transform.position = (Vector3)(MoveV * (float)-(Math.Cos(Math.PI * (a/dur)) - 1) / 2) + OldPos;
        }
        
    }
}
