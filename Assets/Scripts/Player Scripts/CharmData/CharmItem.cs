using UnityEngine;
using System.Collections.Generic;

public class CharmItem : MonoBehaviour
{
    SpriteRenderer SR;
    MyCollider MC;
    public Charm Charmdata = null;
    public string CharmName;
    public TextAsset assetjson;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        MC = gameObject.AddComponent<MyCollider>();
        Init(); 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Charmdata.name);
        List<MyCollider> player = MC.getallcollisions(new List<string>() {"Player"}, true);
        if(player.Count != 0)
        {
            CharmSystem C = player[0].gameObject.GetComponent<CharmSystem>();
            Collect(C);
        }
    }
    void Init()
    {
        CharmsGroup CG = JsonUtility.FromJson<CharmsGroup>(assetjson.text);
        Debug.Log(CG.Charms);
        foreach(Charm c in CG.Charms)
        {
            if (c.name == CharmName)
            {
                Charmdata = c;
            }
        }
        if(Charmdata == null)
        {
            Destroy(this.gameObject);
        }
    }
    void Collect(CharmSystem CS)
    {
        CS.ApplyChanges(Charmdata);
        Destroy(gameObject);
    }
}
