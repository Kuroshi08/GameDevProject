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
    public float dashV = 20;
    public Vector2 velmod;
    public bool jumpbool = true;
    Vector2 LastDir = new Vector2(1,0);
    bool dashb = false;
    bool oguriCap = true;
    bool movement = true;
    int stopx = 0;
    bool iswallgrab = false;

    bool xinputs = false;
    Vector2 moveDir;
    public Vector2 MoveDir
    {
        get => moveDir;
    }
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
    {   if(P.vel.x < maxwalkvelX)
        {
            if(maxwalkvelX - P.vel.x > speed)
            {
                P.vel.x += speed;
            }
            else
            {
                P.vel.x += maxwalkvelX - P.vel.x;
            } 
        }

        
    }
    void MoveLeft()
    {
        if(P.vel.x > -maxwalkvelX)
        {
            if(-maxwalkvelX - P.vel.x < -speed)
            {
                P.vel.x -= speed;
            }
            else
            {
                P.vel.x += -maxwalkvelX - P.vel.x;
            }
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
    IEnumerator Dash()
    {
        Vector2 dir = new Vector2();
        dashb = false;
        oguriCap = false;
        movement = false;
        P.DoGrav = false;
        P.vel = new Vector2();
        for(int i = 0; i < 5; i++)
        {
            if(moveDir != new Vector2())
            {
                dir = moveDir;
            }
            yield return new WaitForSeconds(0.02f);
        }
        if(dir == new Vector2())
        {
            dir = LastDir;
        }
        P.vel = dir.normalized * dashV;
        P.DoXdecay += 1;
        yield return new WaitForSeconds(0.20f);
        P.DoGrav = true;
        movement = true;
        P.DoXdecay -= 1;
        yield return new WaitForSeconds(0.2f);
        oguriCap = true;
        
        
    }
    IEnumerator WaitForTime(float s)
    {
        yield return new WaitForSeconds(s);
    }
    // Update is called once per frame
    void Update()
    {

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
        moveDir = new Vector2();
        if (movement)
        {
            if (xinputs)
            {
                xinputs = false;
                P.DoXdecay -= 1;
            }
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
                moveDir.y =  -1;
            }
            if (Input.GetKey("j"))
            {
                Jump();
            }
            if(moveDir != new Vector2())
            {
                LastDir = moveDir;
            }
            
            if (Input.GetKey("k"))
            {
                if (dashb)
                {
                    StartCoroutine(Dash());
                }
                
            }
            if (xinputs)
            {
                P.DoXdecay += 1;
            }
            
        }
        
    }
    void FixedUpdate()
    {
    }
}
