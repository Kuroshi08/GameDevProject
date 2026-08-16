using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IHealth
{
    bool IFrames = false;
    float health = 100;
    float currentbeat = 0;
    BasicMovement BM;
    public float RhythmLeniency = 0.1f;
    List<string> attackstring = new List<string>();
    public bool AttackState = false;
    public TextAsset SpellJson;

    public GameObject RhythmIndicator;
    public GameObject projectilePrefab;
    RhythmSystem rs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BM = GetComponent<BasicMovement>();
        rs = RhythmIndicator.GetComponent<RhythmSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackState)
        {
            if(CanAttack(RhythmLeniency) >= 1)
            {
                float dif = (float)CanAttack(RhythmLeniency) - currentbeat;
                if(GetSpecificKey() != null && (float)CanAttack(RhythmLeniency) != currentbeat)
                {
                    attackstring.Add($"{GetSpecificKey()},{dif}");
                    CastSpell();
                    currentbeat = (float)CanAttack(RhythmLeniency);
                }
            }
            else
            {
                if(GetSpecificKey() != null && (float)CanAttack(RhythmLeniency) != currentbeat)
                {
                    Debug.Log(CanAttack(RhythmLeniency));
                }
                
            }
        }
        if(health <= 0)
        {
            die();
        }
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
    string GetSpecificKey()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            return "j";
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            return "k";
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            return "l";
        }
        return null;
    }
    float CanAttack(float attackwindowp)
    {
        float c;
        float value = rs.GetCurrentBeatP();
        float valuef = (float)Math.Floor(value);
        float valuep = value-valuef;
        if(valuep >= 0.5)
        {
            c = (float)Math.Ceiling(value);
        }
        else
        {
            c = valuef;
        }
        float p = Math.Abs(value - c);
        if(p < attackwindowp){
            return c;
        }
        return p;
    }



    void CastSpell()
    {
        if(!(attackstring.Count >= 3))
        {
            return;
        }
        Spell s = DetectSpell();
        if(s == null)
        {
            return;
        }
        if(s.code != null)
        {
            attackstring.Add($"{s.code},{0}");
            CreateSpell(s);
            Debug.Log(s.name);
        }
        
    }
    Spell DetectSpell()
    {
        Spells slist = JsonUtility.FromJson<Spells>(SpellJson.text);
        foreach(Spell s in slist.spells)
        {
            if (checkSpell(s, attackstring.GetRange(attackstring.Count - 3, 3)))
            {
                return s;
            }
        }
        return null;
    }
    bool checkSpell(Spell s, List<string> inputs)
    {
        for(int i = 0; i < 3; i ++)
        {
            bool _check = false;
            foreach(string checkstring in s.inputs[i].PosI)
            {
                string[] checklist = checkstring.Split(",");
                string[] inputlist = inputs[i].Split(",");
                if(checklist.Length < 2)
                {
                    if(checkstring == inputlist[0])
                    {
                        _check = true;
                        
                    }
                }
                else
                {
                    if(checklist[0] == inputlist[0] && checklist[1] == inputlist[1])
                    {
                        _check = true;
                    }
                }
            }


            if (!_check)
            {
                foreach(string ss in inputs)
                {
                    Debug.Log(ss);
                }
                Debug.Log($"{i}");
                return false;
            }
        }
        return true;
    }

    void CreateSpell(Spell s)
    {
        GameObject SpellProj = Instantiate(projectilePrefab);
        BasicProjectile p = SpellProj.GetComponent<BasicProjectile>();
        projectile data = s.p;
        if(p != null)
        {
            p.spellname = s.name;
            if(data.attackSelf != true)
            {
                Debug.Log("aaa" + data.attackSelf);
                p.parent = this.gameObject;
            }
            p.Pos = this.transform.position + ((Vector3)data.Pos * BM.LastX);
            p.direction = ((Vector2)data.Pos * BM.LastX).normalized;
            p.data = data;
        }
    }
    void die()
    {
        Application.Quit();
    }

}
[Serializable]
public class Spells
{
    public Spell[] spells;
    public string[] a;

    
}
[Serializable]
public class Spell
{
    public string name;
    public input[] inputs;
    
    public string code;
    public int combo;
    public projectile p;
}
[Serializable]
public class input
{
    public string[] PosI;
}
[Serializable]
public class projectile
{
    public float damage;
    public float lifespan;
    public string ImagePath;
    public string AttackType;
    public string SpellAttribute;
    public string coltype;
    public float immuneFrame;
    public Vector2 Scale;
    public Vector2 colsize;
    public Vector2 Pos;
    public Vector2 Vel;
    public bool attackSelf;
    
}
