using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Accel : BulletAttachment
{
    public bool triggered = false;
    public float speedMod = 1f;
    public float damageMod = 1f;
    public Accel(Bullet b) : base(b)
    {
        triggered = false;
        type = Type.CONTINUOUS;
    }

    public override void Apply()
    {
        if (!triggered)
        {
            if (Vector2.Distance(owner.transform.position, owner.firingOrigin) > owner.effectRange)
            {
                triggered = true;
            }
        }
        else
        {
            owner.bonusSpeedMult += speedMod;
            owner.damage += damageMod;
        }
    }

    public override void Setup()
    {
        if (owner != null)
        {
            triggered = false;
        }
    }
}
