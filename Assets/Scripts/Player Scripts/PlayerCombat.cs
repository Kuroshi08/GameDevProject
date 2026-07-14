using UnityEngine;

public class PlayerCombat : MonoBehaviour, IHealth
{
    float Health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Damage(float d)
    {
        Health = Health - d;
        return true;
    }
    

}
