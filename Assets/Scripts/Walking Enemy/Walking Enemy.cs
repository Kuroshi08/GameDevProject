using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class WalkingEnemy : MonoBehaviour, IHealth
{
    bool IFrames = false;
    public float maxwalkvelX = 1;
    public float health = 100;
    public float speed= 0.2f;
    public float damage = 10;
    public float immuneFrame = 0.1f;
    MyPhysics P;
    Vector2 dir = new Vector2(1,0);
    MyCollider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<MyCollider>();
        if(gameObject.GetComponent<MyPhysics>() != null)
        {
            P = gameObject.GetComponent<MyPhysics>();
        }
        else
        {
            P = gameObject.AddComponent<MyPhysics>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            death();
        }
        if(P.xwallc != 0)
        {
            dir.x = -P.xwallc;
        }
        Move();
        checkHitPlayer();
    }
    void checkHitPlayer()
    {
        List<MyCollider> colret = col.getallcollisions(new List<string>() {"Hurtbox"}, true);
        if(colret.Count != 0)
        {
            Debug.Log("aaaa");
            foreach (MyCollider c in colret)
            {
                IHealth hpscript = c.gameObject.GetComponent<IHealth>();
                if(hpscript != null && c.gameObject.name == "Player")
                {
                    hpscript.Damage(damage,immuneFrame);
                }
                
            }
            
            
        }
    }
    void Move()
    {
        P.vel.x = dir.x * speed;
    }
    public bool Damage(float d, float iframe)
    {
        if (!IFrames)
        {
            health -= d;
        }
        StartCoroutine(startiframe(iframe));
        return true;
    }
    IEnumerator startiframe(float d)
    {
        IFrames = true;
        for(int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(d);
        }
        IFrames = false;
    }
    void death()
    {
        Destroy(gameObject);
    }
}
