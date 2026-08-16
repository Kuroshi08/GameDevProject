using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharmItem : MonoBehaviour
{
    SpriteRenderer SR;
    MyCollider MC;
    public Charm Charmdata;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        MC = gameObject.AddComponent<MyCollider>();
        if(Charmdata != null)
        {
            Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Init()
    {
        
    }
}
