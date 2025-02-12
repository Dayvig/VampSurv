using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Aero : BulletAttachment
{
    public GameObject hitboxObject;
    public float dur;
    public Aero(Bullet b) : base(b)
    {
        type = Type.ONFIRE;
    }

    public override void Apply()
    {
        WindHitbox newWindHitbox;
        for (int i = 0; i < HitboxManager.instance.inactiveHitboxes.Count; i++)
        {
            if (HitboxManager.instance.inactiveHitboxes[i] is WindHitbox)
            {
                newWindHitbox = (WindHitbox)HitboxManager.instance.inactiveHitboxes[i];
                newWindHitbox.duration = dur;
                newWindHitbox.transform.parent.transform.position = owner.firingOrigin;
                newWindHitbox.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, LookAtPoint(owner.transform.position) + 270);
                newWindHitbox.Reset();

                HitboxManager.instance.activeHitboxes.Add(newWindHitbox);
                HitboxManager.instance.inactiveHitboxes.Remove(newWindHitbox);
                return;
            }
        }
        GameObject newWindHitboxObject = HitboxManager.instance.InstantiateNewHitbox(hitboxObject);

        newWindHitboxObject.transform.position = owner.firingOrigin;
        newWindHitboxObject.transform.rotation = Quaternion.Euler(0f, 0f, LookAtPoint(owner.transform.position) + 270);
        newWindHitbox = newWindHitboxObject.GetComponentInChildren<WindHitbox>();
        if (newWindHitbox != null)
        {
            newWindHitbox.duration = dur;
            newWindHitbox.Reset();
            HitboxManager.instance.activeHitboxes.Add(newWindHitbox);
        }
        else
        {
            Debug.Log("Attempted to instantiate a windhitbox without hitboxScript");
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
