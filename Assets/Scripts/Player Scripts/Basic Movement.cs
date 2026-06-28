using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class BasicMovement : MonoBehaviour
{

    public Vector2 vel;
    public float velDecayX = 0;
    public float velDecayY = 0;
    public float maxwalkvelX = 1;
    public float speed= 0.2f;
    public Vector2 velmod;
    float pgrav = 9.8f;

    MyPhysics P;


    //movement keys

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        P = gameObject.AddComponent<MyPhysics>();
    }
    void Start()
    {
    }

    void MoveLeft()
    {
        vel.x -= speed;
        if(Math.Abs(vel.x) > maxwalkvelX)
            {
                vel.x = vel.x/Math.Abs(vel.x) * maxwalkvelX;
            }
    }
    void MoveRight()
    {
        vel.x += speed;
        if(Math.Abs(vel.x) > maxwalkvelX)
            {
                vel.x = vel.x/Math.Abs(vel.x) * maxwalkvelX;
            }
    }
    void Jump()
    {
        vel.y += speed;
    }
    void decaySpeedX()
    {
        if(Math.Abs(vel.x)> 0.001)
        {
            vel.x -= vel.x/Math.Abs(vel.x) * velDecayX*Time.deltaTime;            
        }

        if(Math.Abs(vel.x) < 0.001)
        {
            vel.x = 0;
        }
    }
    void grav()
    {
        if(vel.y > -pgrav)
        {
            vel.y -= velDecayY;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("d"))
        {
            MoveRight();
        }
        if(Input.GetKey("a"))
        {
            MoveLeft();
        }
        if (Input.GetKey("j"))
        {
            Jump();
        }
        grav();
        decaySpeedX();
        P.MoveObject(vel* Time.deltaTime);
    }
    void FixedUpdate()
    {
    }
}
