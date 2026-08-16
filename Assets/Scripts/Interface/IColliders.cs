using UnityEngine;
using System.Collections.Generic;

public interface IColliders
{
    Vector2 size {get;set;}
    public List<MyCollider> getallcollisions(List<string> s, bool Only);

}
