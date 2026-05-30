using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

public class MyCollider
{
    GameObject o;
    Vector2 Pos;
    Vector2 Size;
    Vector2 offset;
    public MyCollider(GameObject o, Vector2 Pos, Vector2 Size, Vector2 offset)
    {
        this.o = o;
    }
}
