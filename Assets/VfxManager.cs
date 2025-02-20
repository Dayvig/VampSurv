using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public List<DamageNumber> activeDamageNumbers = new List<DamageNumber>();
    [HideInInspector]
    public List<DamageNumber> inactiveDamageNumbers = new List<DamageNumber>();
    [HideInInspector]
    public List<DamageNumber> markedForDeathDamageNumbers = new List<DamageNumber>();
    [HideInInspector]
    public List<DamageNumber> toSpawnDamageNumbers = new List<DamageNumber>();

    public GameObject basicDNumber;

    public static VfxManager instance { get; private set; }

    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        foreach (DamageNumber d in toSpawnDamageNumbers)
        {
            activeDamageNumbers.Add(d);
            inactiveDamageNumbers.Remove(d);
        }
        toSpawnDamageNumbers.Clear();
        Trash();
    }

    public void FixedUpdate()
    {

    }

    void Trash()
    {
        foreach (DamageNumber d in markedForDeathDamageNumbers)
        {
            activeDamageNumbers.Remove(d);
            inactiveDamageNumbers.Add(d);
        }
        markedForDeathDamageNumbers.Clear();
    }

    public void InstantiateNewDamageNumber(GameObject dNumber, Vector2 loc, float dur, Color c, int num)
    {
        GameObject newDNumber = Instantiate(dNumber);
        DamageNumber dNumberScript = newDNumber.GetComponentInChildren<DamageNumber>();
        if (dNumberScript == null)
        {
            Debug.Log("Attempted to instantiate a damage number without script");
        }
        toSpawnDamageNumbers.Add(dNumberScript);
        dNumberScript.Spawn(loc, dur, c, num);
    }

    public void InstantiateNewDamageNumber(Vector2 loc, float dur, Color c, int num)
    {
        GameObject newDNumber = Instantiate(basicDNumber);
        DamageNumber dNumberScript = newDNumber.GetComponentInChildren<DamageNumber>();
        if (dNumberScript == null)
        {
            Debug.Log("Attempted to instantiate a damage number without script");
        }
        toSpawnDamageNumbers.Add(dNumberScript);
        dNumberScript.Spawn(loc, dur, c, num);
    }
}
