using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;

public class Aero : BulletAttachment
{
    public GameObject hitboxObject;
    public float dur;
    public AeroBullets originalModifier;

    public Aero(Bullet b) : base(b)
    {
        type = Type.ONFIRE;
    }
    public override void Apply()
    {
        //Debug.Log(originalModifier.spawnWindboxTimer);
        if (originalModifier == null || originalModifier.spawnWindboxTimer < originalModifier.spawnInterval)
        {
            return;
        }
        if (originalModifier.spawnWindboxTimer >= originalModifier.spawnInterval)
        {
            originalModifier.spawnWindboxTimer -= originalModifier.spawnInterval + Random.Range(-(originalModifier.spawnInterval / 10), (originalModifier.spawnInterval / 10));
        }
        //Debug.Log(originalModifier.spawnWindboxTimer);
        WindHitbox newWindHitbox;
        for (int i = 0; i < HitboxManager.instance.inactiveHitboxes.Count; i++)
        {
            if (HitboxManager.instance.inactiveHitboxes[i] is WindHitbox && !HitboxManager.instance.inactiveHitboxes[i].spawning)
            {
                newWindHitbox = (WindHitbox)HitboxManager.instance.inactiveHitboxes[i];
                newWindHitbox.duration = dur;
                newWindHitbox.transform.parent.transform.position = owner.firingOrigin;
                newWindHitbox.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, LookAtPoint(owner.transform.position) + 270);
                newWindHitbox.Reset();
                newWindHitbox.spawning = true;
                HitboxManager.instance.toSpawnHitboxes.Add(newWindHitbox);
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
            newWindHitbox.spawning = true;
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

    public override void Update()
    {
    }
}
