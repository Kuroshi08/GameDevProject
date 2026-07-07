using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using System.Linq;
using Unity.VisualScripting;
using NUnit.Framework.Internal;
using System.Runtime.CompilerServices;



public class MyPhysics : MonoBehaviour
{
    public Vector2 vel;
    public float velDecayX = 100;
    Mathstuff MyMathstuff = new Mathstuff();
    float mass;
    public float gravity = 1;
    public float gravityMod = 1;
    bool[] lockXY = new bool[2];
    float maxgrav = 9.8f;
    public bool isGrounded;
    public int xwallc = 0;
    MyCollider vCollider;
    MyCollider eCollider;
    MyCollider selfcollider;
    public bool DoXdecay;


    //make this also a comp, use My colliderdirectly
    void Awake()
    {
        selfcollider = gameObject.GetComponent<MyCollider>();
        vCollider = gameObject.AddComponent<MyCollider>();
        vCollider.AddTags("MoveCol");
        vCollider.switchState(false);
        eCollider = gameObject.AddComponent<MyCollider>();
        eCollider.AddTags("MoveCol");
        eCollider.switchState(false);
    }
    public void MoveObject(Vector2 vel, string onlyAffectTag = null)
    {
        Vector2 newVel = checkCol(vel);
        isGrounded = vel.y < 0 && newVel.y > vel.y;
        if(vel.x != newVel.x)
        {
            xwallc = (int)(vel.x/Math.Abs(vel.x));
        }
        else 
        {
            xwallc = 0;
        }

        transform.Translate(newVel);
    }
    Vector2 checkCol(Vector2 vel)
    {
        bool testbool = false;
        List<float> xvalues = new List<float>();
        List<float> yvalues = new List<float>();
        
        ///////////////write point based( gen points based on positive x * y + vector speed, left right based on positive x)
        /// 
        /// 
        /// 
        /// if only x
        /// 

        if(vel.y == 0 && vel.x != 0)
        {
            vCollider.AutoPoints();
            xvalues = new List<float>();
            float xpos = vel.x/Math.Abs(vel.x);
            vCollider.offset = (new Vector2(vel.x/2 + (selfcollider.size.x/2 * xpos) ,0));
            vCollider.size = (new Vector2(Math.Abs(vel.x)/2,selfcollider.size.y));
            List<MyCollider> vcolliderr = vCollider.getallcollisions();
            if(vcolliderr.Count != 0)
            {
                
                foreach(MyCollider col in vcolliderr)
                {

                    if(vel.x > 0)
                    {
                        if(col.Pos.x - col.size.x/2 >= (selfcollider.Pos.x + selfcollider.size.x / 2))
                        {
                            xvalues.Add(col.Pos.x - col.size.x/2);
                        }
                        
                    }
                    if(vel.x < 0)
                    {
                        if(col.Pos.x + col.size.x/2 <= (selfcollider.Pos.x - selfcollider.size.x / 2))
                        {
                            xvalues.Add(col.Pos.x + col.size.x/2);
                        }
                    }
                    
                }
            }
            
        }
        /// if only y
        if(vel.y != 0 && vel.x == 0)
        {
            
            vCollider.AutoPoints();
            yvalues = new List<float>();
            float ypos = vel.y/Math.Abs(vel.y);
            vCollider.offset = (new Vector2(0 ,vel.y/2 + (selfcollider.size.y/2 * ypos)));
            vCollider.size = (new Vector2(selfcollider.size.x,Math.Abs(vel.y)));
            List<MyCollider> vcolliderr = vCollider.getallcollisions();
            if(vcolliderr.Count != 0)
            {

                foreach(MyCollider col in vcolliderr)
                {
                    if(vel.y > 0)
                    {
                        if(col.Pos.y - col.size.y/2 >= (selfcollider.Pos.y + selfcollider.size.y / 2))
                        {
                            yvalues.Add(col.Pos.y - col.size.y/2);
                        }
                        
                    }
                    if(vel.y < 0)
                    {
                        if(col.Pos.y + col.size.y/2 <= (selfcollider.Pos.y - selfcollider.size.y / 2))
                        {
                            yvalues.Add(col.Pos.y + col.size.y/2);
                        }
                    }
                    
                    
                }
            }
            
        } 
        /// 
        /// 
        /// 
        /// both xy
        
        if(vel.y != 0 && vel.x != 0)
        {
            
            xvalues = new List<float>();
            yvalues = new List<float>();
            eCollider.offset = vel;
            eCollider.size = (selfcollider.size);
            Vector2[] spoints = new Vector2[2];
            Vector2[] rpoints = new Vector2[4];
            if((vel.x/Math.Abs(vel.x)) == (vel.y / Math.Abs(vel.y))){
                spoints[0] = new Vector2(selfcollider.Pos.x - (selfcollider.size.x/2),selfcollider.Pos.y + (selfcollider.size.y/2)); //top left
                spoints[1] = new Vector2(selfcollider.Pos.x + (selfcollider.size.x/2),selfcollider.Pos.y - (selfcollider.size.y/2)); //bottom right
                
                
            }
            else
            {
                spoints[0] = selfcollider.Pos + (selfcollider.size/2); //top right
                spoints[1] = selfcollider.Pos - (selfcollider.size/2); //bottom left

            }
            if(vel.x > 0)
            {
                
                rpoints[0] = spoints[0] + vel;
                rpoints[1] = spoints[0];
                rpoints[2] = spoints[1] + vel;
                rpoints[3] = spoints[1];
            }
            else
            {
                rpoints[1] = spoints[0] + vel;
                rpoints[0] = spoints[0];
                rpoints[3] = spoints[1] + vel;
                rpoints[2] = spoints[1];
            }
            vCollider.ChangePoints(rpoints);


            List<MyCollider> vcolliderr = vCollider.getallcollisions();
            List<MyCollider> ecolliderr = eCollider.getallcollisions(); 
            foreach(MyCollider col in ecolliderr)
            {
                if(!vcolliderr.Contains(col))
                {
                   vcolliderr.Add(col); 
                }
            }
            ///////

            /// now detect if object should take x or y val () use intesect point, if d1 < d2 get d1 x or y to list, get min of list
            Vector2 cpoint = selfcollider.Pos + new Vector2(selfcollider.size.x/2 * vel.x / Math.Abs(vel.x),selfcollider.size.y * vel.y / Math.Abs(vel.y)/2) ;
            Vector2[] cline = new Vector2[2] {cpoint,cpoint+vel};
            foreach(MyCollider col in vcolliderr)
            {
                List<Vector2[]> walls = col.ColliderIntersectLine(cline);
                if(walls != null)
                {
                    List<Vector2[]> rwall = new List<Vector2[]>();
                    Vector2? rwallp;
                    float rwallsd = 0;
                    foreach(Vector2[] wall in walls)
                    {
                        Vector2? wp = MyMathstuff.GetIntersection(wall,cline) - cpoint;
                        if(wp == null)
                        {
                            break;
                        }

                        if(rwall.Count == 0)
                        {
                            rwall = new List<Vector2[]>();
                            rwall.Add(wall);
                            rwallp = (Vector2)wp;
                            rwallsd = ((Vector2)wp).sqrMagnitude;
                        }
                        else if(((Vector2)wp).sqrMagnitude < rwallsd)
                        {
                            rwall = new List<Vector2[]>();
                            rwall.Add(wall);
                            rwallp = (Vector2)wp;
                            rwallsd = ((Vector2)wp).sqrMagnitude;
                        }
                        else if(((Vector2)wp).sqrMagnitude == rwallsd)
                        {
                            rwall.Add(wall);
                            rwallp = (Vector2)wp;
                            rwallsd = ((Vector2)wp).sqrMagnitude;
                        }
                    }
                    if(rwall.Count == 0)
                    {
                        break;
                    }
                    foreach(Vector2[] w in rwall)
                    {
                        if(w[0].x == w[1].x)
                            {
                                xvalues.Add(w[0].x);
                            }
                            if(w[0].y == w[1].y)
                            {
                                yvalues.Add(w[0].y);
                            }
                    }

                }
                else
                {
                    testbool = true;
                    Vector2 point = col.getcalpos();
                    float correctedy = cpoint.y + ((point.x - cpoint.x) * vel.y/vel.x );
                    if(vel.y > 0)
                    {
                        if(point.y >= correctedy)
                        {
                            if((col.Pos.x + col.size.x/2) > selfcollider.Pos.x - selfcollider.size.x/2)
                            {
                                yvalues.Add(col.Pos.y + col.size.y/2);
                                yvalues.Add(col.Pos.y - col.size.y/2);
                            }
                        }
                        else
                        {
                            if((col.Pos.y + col.size.y/2) > selfcollider.Pos.y - selfcollider.size.y/2)
                            {
                                xvalues.Add(col.Pos.x + col.size.x/2);
                                xvalues.Add(col.Pos.x - col.size.x/2);
                            }
                            
                        }
                    }
                    else
                    {
                        if(point.y <= correctedy)
                        {
                            if((col.Pos.x - col.size.x/2) < selfcollider.Pos.x + selfcollider.size.x/2)
                            {
                                yvalues.Add(col.Pos.y + col.size.y/2);
                                yvalues.Add(col.Pos.y - col.size.y/2);
                            }
                        }
                        else
                        {
                            if((col.Pos.y - col.size.y/2) < selfcollider.Pos.y + selfcollider.size.y/2)
                            {
                                xvalues.Add(col.Pos.x + col.size.x/2);
                                xvalues.Add(col.Pos.x - col.size.x/2);
                            }
                        }
                    }
                    
                }

            }
            
            
        }
        while(yvalues.Count > 0)
        {
            if(vel.y > 0)
            {
                if( yvalues.Min() - (selfcollider.Pos.y + selfcollider.size.y/2) >= 0 && yvalues.Min() - (selfcollider.Pos.y + selfcollider.size.y/2) <= vel.y)
                {
                    vel.y = yvalues.Min() - (selfcollider.Pos.y + selfcollider.size.y/2);
                    break;
                }
                yvalues.Remove(yvalues.Min());

            }
            else
            {
                if( yvalues.Max() - (selfcollider.Pos.y - selfcollider.size.y/2) <= 0 &&  yvalues.Max() - (selfcollider.Pos.y - selfcollider.size.y/2) >= vel.y)
                {
                    vel.y = yvalues.Max() - (selfcollider.Pos.y - selfcollider.size.y/2);
                    break;
                }
                yvalues.Remove(yvalues.Max());
            }
        }
        if(vel.x != 0 && xvalues.Count != 0)
        {
            Debug.Log(testbool);
        }
        while(xvalues.Count > 0)
        {
            if(vel.x > 0)
            {
                if( xvalues.Min() - (selfcollider.Pos.x + selfcollider.size.x/2) >= 0 && xvalues.Min() - (selfcollider.Pos.x + selfcollider.size.x/2) <= vel.x)
                {
                    vel.x = xvalues.Min() - (selfcollider.Pos.x + selfcollider.size.x/2);
                    break;
                }
                xvalues.Remove(xvalues.Min());

            }
            else
            {
                if( xvalues.Max() - (selfcollider.Pos.x - selfcollider.size.x/2) <= 0 && xvalues.Max() - (selfcollider.Pos.x - selfcollider.size.x/2) >= vel.x)
                {
                    vel.x = xvalues.Max() - (selfcollider.Pos.x - selfcollider.size.x/2);
                    break;
                }
                xvalues.Remove(xvalues.Max());
            }
        }
        return vel;
    }
    /// <summary>
    /// BUG
    /// issue when moving at high speeds x > 20 and y != 0
    /// </summary>
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
    void addvel(Vector2 vel)
    {
        this.vel = vel + this.vel;
    }
    void grav()
    {
        if(vel.y > -maxgrav * gravityMod)
        {
            vel.y -= gravity * gravityMod;
        }
        if(vel.y < -maxgrav * gravity)
        {
            if((vel.y - (-maxgrav * gravity)) > 0.5)
            {
                vel.y += (vel.y - (-maxgrav * gravity)) * 0.4f;
            }
            else
            {
                vel.y = -maxgrav * gravity;
            }
            
        }
    }
    void FixedUpdate()
    {
        grav();
        if (DoXdecay)
        {
            decaySpeedX();
        }
    }
    void Update()
    {
        MoveObject(vel * Time.deltaTime);
    }
}