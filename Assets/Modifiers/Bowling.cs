using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bowling : BulletAttachment
{
    public GameObject hitboxObject;
    public float dur;
    public Bowling(Bullet b) : base(b)
    {
        type = Type.ONHIT;
    }

    public override void Apply()
    {
        BowlingHitbox newBowlingHitbox;
        for (int i = 0; i < HitboxManager.instance.inactiveHitboxes.Count; i++)
        {
            if (HitboxManager.instance.inactiveHitboxes[i] is BowlingHitbox)
            {
                newBowlingHitbox = (BowlingHitbox)HitboxManager.instance.inactiveHitboxes[i];
                newBowlingHitbox.duration = dur;
                newBowlingHitbox.gameObject.transform.position = owner.transform.position;
                newBowlingHitbox.gameObject.transform.rotation = owner.transform.rotation;
                newBowlingHitbox.gameObject.transform.localScale = owner.transform.localScale;
                setupBowlingHitbox(newBowlingHitbox);

                HitboxManager.instance.activeHitboxes.Add(newBowlingHitbox);
                HitboxManager.instance.inactiveHitboxes.Remove(newBowlingHitbox);
                return;
            }
        }
        GameObject newBowlingHitboxObj = HitboxManager.instance.InstantiateNewHitbox(hitboxObject);

        newBowlingHitboxObj.transform.position = owner.transform.position;
        newBowlingHitboxObj.transform.rotation = owner.transform.rotation;
        newBowlingHitboxObj.transform.localScale = owner.transform.localScale;
        newBowlingHitbox = newBowlingHitboxObj.GetComponentInChildren<BowlingHitbox>();
        if (newBowlingHitboxObj != null)
        {
            setupBowlingHitbox(newBowlingHitbox);
            HitboxManager.instance.activeHitboxes.Add(newBowlingHitbox);
        }
        else
        {
            Debug.Log("Attempted to instantiate a windhitbox without hitboxScript");
        }
    }

    void setupBowlingHitbox(BowlingHitbox bowl)
    {
        bowl.duration = dur;
        bowl.Reset();
        bowl.ren.sprite = owner.ren.sprite;
        bowl.speed = owner.baseSpeed;
        bowl.velocity = owner.velocity;
        if (owner.thisCollider is CircleCollider2D)
        {
            bowl.circleCollider.enabled = true;
            bowl.circleCollider.offset = owner.thisCollider.offset;
            bowl.circleCollider.radius = owner.thisCollider.radius;
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
