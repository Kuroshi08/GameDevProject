using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using System.Linq;



/// <summary>
/// make into monobehavior
/// </summary>
public class MyPhysics : MonoBehaviour
{
    Mathstuff MyMathstuff = new Mathstuff();
    float mass;
    bool[] lockXY = new bool[2];
    bool grav;
    Vector2 vel;
    bool isGrounded;
    float Gravity = 9.8f;
    MyCollider vCollider;
    MyCollider eCollider;
    MyCollider selfcollider;


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

        List<MyCollider> vcolliderr = new List<MyCollider>();
        ///////////////write point based( gen points based on positive x * y + vector speed, left right based on positive x)
        /// 
        /// 
        /// 
        /// if only x
        /// 

        if(vel.y == 0 && vel.x != 0)
        {
            List<float> xvalues = new List<float>();
            float xpos = vel.x/Math.Abs(vel.x);
            vCollider.ChangeOffset(new Vector2(vel.x/2 + (selfcollider.getscale().x/2 * xpos) ,0));
            vCollider.ChangeScale(new Vector2(Math.Abs(vel.x)/2,selfcollider.getscale().y));
            vcolliderr = vCollider.getallcollisions();
            if(vcolliderr.Count != 0)
            {
                
                foreach(MyCollider col in vcolliderr)
                {

                    xvalues.Add(col.getpos().x + col.getscale().x/2);
                    xvalues.Add(col.getpos().x - col.getscale().x/2);
                    
                }
                if(vel.x > 0)
                {

                    vel.x = xvalues.Min() - (selfcollider.getpos().x + selfcollider.getscale().x/2);

                }
                else
                {
                   vel.x = xvalues.Max() - (selfcollider.getpos().x - selfcollider.getscale().x/2); 
                }
            }
            
        }
        /// if only y
        if(vel.y != 0 && vel.x == 0)
        {
            List<float> yvalues = new List<float>();
            float ypos = vel.y/Math.Abs(vel.y);
            vCollider.ChangeOffset(new Vector2(0 ,vel.y/2 + (selfcollider.getscale().y/2 * ypos)));
            vCollider.ChangeScale(new Vector2(selfcollider.getscale().x,Math.Abs(vel.y)));
            vcolliderr = vCollider.getallcollisions();
            if(vcolliderr.Count != 0)
            {
                
                foreach(MyCollider col in vcolliderr)
                {
                    yvalues.Add(col.getpos().y + col.getscale().y/2);
                    yvalues.Add(col.getpos().y - col.getscale().y/2);
                    
                }
                if(vel.y > 0)
                {

                    vel.y = yvalues.Min() - (selfcollider.getpos().y + selfcollider.getscale().y/2);

                }
                else
                {
                   vel.y = yvalues.Max() - (selfcollider.getpos().y - selfcollider.getscale().y/2); 
                }
            }
            
        } 
        /// 
        /// 
        /// 
        /// both xy
        
        if(vel.y != 0 && vel.x != 0)
        {
            List<float> xvalues = new List<float>();
            List<float> yvalues = new List<float>();
            eCollider.ChangeOffset(vel);
            eCollider.ChangeScale(selfcollider.getscale());
            Vector2[] spoints = new Vector2[2];
            Vector2[] rpoints = new Vector2[4];
            if((vel.x/Math.Abs(vel.x)) == (vel.y / Math.Abs(vel.y))){
                spoints[0] = new Vector2(selfcollider.getpos().x - (selfcollider.getscale().x/2),selfcollider.getpos().y + (selfcollider.getscale().y/2)); //top left
                spoints[1] = new Vector2(selfcollider.getpos().x + (selfcollider.getscale().x/2),selfcollider.getpos().y - (selfcollider.getscale().y/2)); //bottom right
                
                
            }
            else
            {
                spoints[0] = selfcollider.getpos() + (selfcollider.getscale()/2); //top right
                spoints[1] = selfcollider.getpos() - (selfcollider.getscale()/2); //bottom left

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


            vcolliderr = vCollider.getallcollisions();
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
            Vector2 cpoint = selfcollider.getpos() + new Vector2(selfcollider.getscale().x/2 * vel.x / Math.Abs(vel.x),selfcollider.getscale().y * vel.y / Math.Abs(vel.y)/2) ;
            Vector2[] cline = new Vector2[2] {cpoint,cpoint+vel};
            foreach(MyCollider col in vcolliderr)
            {
                List<Vector2[]> walls = col.ColliderIntersectLine(cline);
                if(walls != null)
                {
                        Vector2[] rwall = null;
                        Vector2? rwallp;
                        float rwallsd = 0;
                        foreach(Vector2[] wall in walls)
                        {
                            Vector2? wp = MyMathstuff.GetIntersection(wall,cline) - cpoint;
                            if(wp == null)
                            {
                                break;
                            }

                            if(rwall == null)
                            {
                                rwall = wall;
                                rwallp = (Vector2)wp;
                                rwallsd = ((Vector2)wp).sqrMagnitude;
                            }
                            else if(((Vector2)wp).sqrMagnitude < rwallsd)
                            {
                                rwall = wall;
                                rwallp = (Vector2)wp;
                                rwallsd = ((Vector2)wp).sqrMagnitude;
                            }
                        }
                        if(rwall == null)
                        {
                            break;
                        }
                        if(rwall[0].x == rwall[1].x)
                        {
                            xvalues.Add(rwall[0].x);
                        }
                        if(rwall[0].y == rwall[1].y)
                        {
                            yvalues.Add(rwall[0].y);
                        }
                }
                else
                {
                    Vector2 point = col.getcalpos();
                    float correctedy = cpoint.y + ((point.x - cpoint.x) * vel.y/vel.x );
                    if(vel.y > 0)
                    {
                        if((point.y - selfcollider.getcalpos().y) > (correctedy - selfcollider.getcalpos().y))
                        {
                            yvalues.Add(col.getpos().y + col.getscale().y/2);
                            yvalues.Add(col.getpos().y - col.getscale().y/2);
                        }
                        else
                        {
                            xvalues.Add(col.getpos().x + col.getscale().x/2);
                            xvalues.Add(col.getpos().x - col.getscale().x/2);
                        }
                    }
                    else
                    {
                        if((point.y - selfcollider.getcalpos().y) < (correctedy - selfcollider.getcalpos().y))
                        {
                            yvalues.Add(col.getpos().y + col.getscale().y/2);
                            yvalues.Add(col.getpos().y - col.getscale().y/2);
                        }
                        else
                        {
                            xvalues.Add(col.getpos().x + col.getscale().x/2);
                            xvalues.Add(col.getpos().x - col.getscale().x/2);
                        }
                    }
                    
                }

            }
            if(yvalues.Count > 0)
            {
                if(vel.y > 0)
                {

                    vel.y = yvalues.Min() - (selfcollider.getpos().y + selfcollider.getscale().y/2);

                }
                else
                {
                    vel.y = yvalues.Max() - (selfcollider.getpos().y - selfcollider.getscale().y/2); 
                }
            }
            if(xvalues.Count > 0)
            {
                if(vel.x > 0)
                {

                    vel.x = xvalues.Min() - (selfcollider.getpos().x + selfcollider.getscale().x/2);

                }
                else
                {
                    vel.x = xvalues.Max() - (selfcollider.getpos().x - selfcollider.getscale().x/2); 
                }
            }
            
        }
        transform.Translate(vel);
    }
    bool IsGround(GameObject o)
    {
        BoxCollider2D colliderground = o.AddComponent<BoxCollider2D>();
        return false;
    }
}
