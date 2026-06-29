using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class BasicMovement : MonoBehaviour
{

    public Vector2 vel;
    public float velDecayX = 0;
    public float gravity = 1;
    public float maxwalkvelX = 1;
    public float speed= 0.2f;
    public Vector2 velmod;
    float maxgrav = 9.8f;
    bool xinputs = false;

    MyPhysics P;


    //movement keys

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(gameObject.GetComponent<MyPhysics>() != null)
        {
            P=gameObject.GetComponent<MyPhysics>();
        }
        else
        {
            P = gameObject.AddComponent<MyPhysics>();
        }
        
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
        if(Math.Abs(vel.x)> Math.Abs(vel.x/Math.Abs(vel.x) * velDecayX*Time.deltaTime))
        {
            vel.x -= vel.x/Math.Abs(vel.x) * velDecayX*Time.deltaTime;            
        }
        else
        {
            vel.x = 0;
        }
    }
    void grav()
    {
        if(vel.y > -maxgrav)
        {
            vel.y -= gravity;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("d"))
        {
            xinputs=true;
            MoveRight();
        }
        if(Input.GetKey("a"))
        {
            xinputs=true;
            MoveLeft();
        }
        if (Input.GetKey("j"))
        {
            Jump();
        }
        grav();
        if (!xinputs)
        {
            decaySpeedX();
        }
        xinputs = false;
        P.MoveObject(vel* Time.deltaTime);
    }
    void FixedUpdate()
    {
    }
}
