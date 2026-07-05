using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;


public class BasicMovement : MonoBehaviour
{



    public float maxwalkvelX = 1;
    public float speed= 0.2f;
    public float jumpv = 10;
    public float walljumpv = 3;
    public Vector2 velmod;
    public bool jumpbool = true;
    bool movement = true;
    bool iswallgrab = false;

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
    void MoveRight()
    {
        if(maxwalkvelX - P.vel.x > speed)
        {
            P.vel.x += speed;
        }
        else
        {
            P.vel.x = maxwalkvelX;
        }
        
    }
    void MoveLeft()
    {
        if(-maxwalkvelX - P.vel.x < -speed)
        {
            P.vel.x -= speed;
        }
        else
        {
            P.vel.x = -maxwalkvelX;
        }
    }

    void Jump()
    {
        if (jumpbool)
        {
            if(P.xwallc != 0 && !P.isGrounded)
            {
                P.vel.x = P.xwallc * walljumpv * -1;
                StartCoroutine(StopMove());
            }
            P.vel.y = 0;
            P.vel.y += jumpv;
            jumpbool = false;
        }
        
    }


    IEnumerator StartGTime()
    {
        yield return new WaitForSeconds(0.05f);
        if (!(P.isGrounded || P.xwallc != 0))
        {
            jumpbool = false;
        }
        
    }
    IEnumerator StopMove()
    {
        movement = false;
        yield return new WaitForSeconds(0.15f);
        movement = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (P.isGrounded || P.xwallc != 0)
        {
            jumpbool = true;
        }
        else
        {
            StartCoroutine(StartGTime());
        }
        if(P.xwallc != 0)
        {
            if (!iswallgrab)
            {
                P.gravityMod -= 0.5f;
            }
            iswallgrab = true;
        }
        else
        {
            if (iswallgrab)
            {
                P.gravityMod += 0.5f;
            }
            iswallgrab = false;
        }
        if (movement)
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
        }
        if (Input.GetKey("j"))
        {
            Jump();
        }
        P.DoXdecay = !xinputs;
        xinputs = false;
    }
    void FixedUpdate()
    {
    }
}
