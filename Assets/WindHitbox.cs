using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindHitbox : TimedHitbox
{
    public WindBoost boostModifier;
    public override void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.name);
            bool found = false;
            foreach (Modifier m in EnemyManager.instance.player.modifiers)
            {
                if (m is WindBoost)
                {
                    found = true;
                    return;
                }
            }
            if (!found)
            {
                boostModifier.Setup();  
                EnemyManager.instance.player.modifiers.Add(boostModifier);
            }
        }
    }
}
