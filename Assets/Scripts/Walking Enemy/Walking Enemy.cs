using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class WalkingEnemy : MonoBehaviour, IHealth
{
    bool IFrames = false;
    public float maxwalkvelX = 1;
    public float health = 100;
    public float speed= 0.2f;
    MyPhysics P;
    Vector2 dir = new Vector2(1,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
