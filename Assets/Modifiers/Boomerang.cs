using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Boomerang : BulletAttachment
{
    public bool triggered = false;
    public float rampUp = 0.2f;
    public float flipTimer = 0.0f;
    public float baseSpeedVal;
    float reversal = 1f;
    public Boomerang(Bullet b) : base(b)
    {
        triggered = false;
        type = Type.CONTINUOUS;
        baseSpeedVal = b.baseSpeed;
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
            if (reversal > -1f)
            {
                flipTimer += Time.deltaTime;
                reversal = Mathf.Lerp(1, -1, flipTimer / rampUp);
            }
            owner.modifiedSpeed *= reversal;
        }
    }

    public override void Setup()
    {
        if (owner != null)
        {
            flipTimer = 0.0f;
            baseSpeedVal = owner.baseSpeed;
            reversal = 1f;
            triggered = false;
        }
    }
}
