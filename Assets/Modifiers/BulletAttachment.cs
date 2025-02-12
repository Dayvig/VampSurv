using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletAttachment
{
    public BulletAttachment(Bullet b) { owner = b; }

    public Bullet owner;
    public enum Type
    {
        ONETIME,
        CONTINUOUS,
        ONDEATH,
        ONFIRE,
        ONHIT
    }
    public Type type;
    public abstract void Apply();
    public abstract void Setup();
}
