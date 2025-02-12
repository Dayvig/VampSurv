using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    [HideInInspector]
    public List<HitboxScript> activeHitboxes = new List<HitboxScript>();
    [HideInInspector]
    public List<HitboxScript> inactiveHitboxes = new List<HitboxScript>();
    [HideInInspector]
    public List<HitboxScript> markedForDeathHitboxes = new List<HitboxScript>();

    public static HitboxManager instance { get; private set; }

    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        Trash();
    }

    public void FixedUpdate()
    {
        foreach (HitboxScript h in activeHitboxes)
        {
            h.hitboxUpdate();
        }
    }

    void Trash()
    {
        foreach (HitboxScript h in markedForDeathHitboxes)
        {
            activeHitboxes.Remove(h);
            inactiveHitboxes.Add(h);
        }
        markedForDeathHitboxes.Clear();
    }

    public GameObject InstantiateNewHitbox(GameObject hitbox)
    {
        GameObject newHitboxObject = Instantiate(hitbox);
        HitboxScript hitboxScript = newHitboxObject.GetComponentInChildren<HitboxScript>();
        if (hitboxScript == null)
        {
            Debug.Log("Attempted to instantiate a hitbox without hitboxScript");
        }
        return newHitboxObject;
    }
}
