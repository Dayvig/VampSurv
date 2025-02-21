using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [HideInInspector]
    public List<ExpOrb> activeOrbs = new List<ExpOrb>();
    [HideInInspector]
    public List<ExpOrb> inactiveOrbs = new List<ExpOrb>();
    [HideInInspector]
    public List<ExpOrb> markedForDeathOrbs = new List<ExpOrb>();
    [HideInInspector]
    public List<ExpOrb> toSpawnOrbs = new List<ExpOrb>();

    public GameObject basicOrb;

    public static ProgressionManager instance { get; private set; }

    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        foreach (ExpOrb h in toSpawnOrbs)
        {
            h.spawning = false;
            activeOrbs.Add(h);
            inactiveOrbs.Remove(h);
        }
        toSpawnOrbs.Clear();
        Trash();
    }

    void Trash()
    {
        foreach (ExpOrb h in markedForDeathOrbs)
        {
            activeOrbs.Remove(h);
            inactiveOrbs.Add(h);
        }
        markedForDeathOrbs.Clear();
    }

    public void InstantiateNewOrb(GameObject orb, float value, Vector2 location)
    {
        GameObject newOrb = Instantiate(orb, location + Random.insideUnitCircle * 0.2f, Quaternion.identity);
        ExpOrb newExpOrb = newOrb.GetComponentInChildren<ExpOrb>();
        if (newExpOrb == null)
        {
            Debug.Log("Attempted to instantiate a hitbox without ExpOrb");
        }
        newExpOrb.SetupOrb(value);
        toSpawnOrbs.Add(newExpOrb);
    }
}
