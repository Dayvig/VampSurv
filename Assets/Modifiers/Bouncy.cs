using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bouncy : BulletAttachment
{
    public int bounces = 0;
    public Bouncy(Bullet b) : base(b)
    {
        type = Type.ONHIT;
    }

    public override void Apply()
    {
        if (bounces > 0)
        {
            float extraAngle;
            float angle = LookAtPoint(owner.transform.position);
            if (Random.Range(0, 1) == 0)
            {
                extraAngle = Random.Range(45, 135);
            }
            else
            {
                extraAngle = Random.Range(215, 285);
            }
            float xSpeed = Mathf.Sin(angle + extraAngle);
            float ySpeed = Mathf.Cos(angle + extraAngle);

            owner.velocity = new Vector2(xSpeed, ySpeed).normalized * owner.baseSpeed;
            bounces--;
        }
    }
    private float LookAtPoint(Vector3 current)
    {
        float angleDeg = (Mathf.Rad2Deg * Mathf.Atan2((current.y + owner.velocity.y) - current.y, (current.x + owner.velocity.x) - current.x));
        return angleDeg;
    }

    public override void Setup()
    {

    }
}
