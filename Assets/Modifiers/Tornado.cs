using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : BulletAttachment
{
    public float magnitude = 0f;
    public float spinGrowthDivisor = 1f;
    public bool triggered = false;
    public Tornado(Bullet b) : base(b)
    {
        triggered = false;
        type = Type.CONTINUOUS;
    }

    public override void Apply()
    {
        if (triggered)
        {
            magnitude += (owner.modifiedSpeed / spinGrowthDivisor);
            if (owner.bonusVelocity.ContainsKey("Tornado"))
            {
                Vector2 perp = new Vector2(-(owner.transform.position.y - owner.firingOrigin.y), owner.transform.position.x - owner.firingOrigin.x).normalized * magnitude * -1;
                owner.bonusVelocity["Tornado"] = perp;
            }
            else
            {
                Vector2 perp = new Vector2(-(owner.transform.position.y - owner.firingOrigin.y), owner.transform.position.x - owner.firingOrigin.x).normalized * magnitude * -1;
                owner.bonusVelocity.Add("Tornado", perp);
            }
            float rads = Mathf.Deg2Rad * -2;
            owner.velocity = new Vector2((Mathf.Cos(rads) * owner.velocity.x - (Mathf.Sin(rads) * owner.velocity.y)), (Mathf.Sin(rads) * owner.velocity.x + (Mathf.Cos(rads) * owner.velocity.y)));
        }
        else
        {
            if (Vector2.Distance(owner.transform.position, owner.firingOrigin) > owner.effectRange)
            {
                triggered = true;
            }
        }
    }

    public override void Setup()
    {
        triggered = false;
        magnitude = owner.modifiedSpeed / (spinGrowthDivisor/2);
    }
}
