using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.EventSystems;


public class BasicMovement : MonoBehaviour
{



    public float maxwalkvelX = 1;
    public float speed= 0.2f;
    public float jumpv = 10;
    public float walljumpv = 3;
    float dashV = 10;
    public Vector2 velmod;
    public bool jumpbool = true;
    bool dashb = false;
    bool oguriCap = true;
    bool movement = true;
    int stopx = 0;
    bool iswallgrab = false;

    bool xinputs = false;
    Vector2 moveDir = new Vector2();
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
                StartCoroutine(WallJumpStop(P.xwallc));
            }
            P.vel.y = 0;
            P.vel.y += jumpv;
            jumpbool = false;
        }
        
    }
    void Dash()
    {
        if (dashb)
        {
            dashb = false;
            oguriCap = false;
            Debug.Log(moveDir);
            P.vel = moveDir.normalized * dashV;
            StartCoroutine(DashTimer());
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
    IEnumerator WallJumpStop(int v)
    {
        stopx = v;
        yield return new WaitForSeconds(0.15f);
        stopx = 0;
        
    }
    IEnumerator DashTimer()
    {
        P.DoXdecay = false;
        movement = false;
        yield return new WaitForSeconds(0.50f);
        movement = true;
        P.DoXdecay = true;
        yield return new WaitForSeconds(0.2f);
        oguriCap = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector2();
        if (P.isGrounded && oguriCap)
        {
            dashb = true;
        }
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
            if(Input.GetKey("d") && stopx != 1)
            {
                xinputs=true;
                MoveRight();
            }
            if(Input.GetKey("a") && stopx != -1)
            {
                xinputs=true;
                MoveLeft();
            }
            if(Input.GetKey("d"))
            {
                moveDir.x = 1;
            }
            if(Input.GetKey("a"))
            {
                moveDir.x = -1;
            }
            if(Input.GetKey("w"))
            {
                moveDir.y = 1;
            }
            if(Input.GetKey("s"))
            {
                moveDir.y = -1;
            }
            if (Input.GetKey("j"))
            {
                Jump();
            }
            if (Input.GetKey("k"))
            {
                Dash();
            }
            P.DoXdecay = !xinputs;
            
        }
        
        
        xinputs = false;
        
    }
    void FixedUpdate()
    {
    }
}
