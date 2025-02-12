using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : ScriptableObject
{

    public int priority;
    public List<float> factors = new List<float>();
    public List<Bullet> payloads = new List<Bullet>();
    public List<GameObject> objects = new List<GameObject>();
    public List<HitboxScript> hitboxes = new List<HitboxScript>();
    public enum Type
    {
        BULLET,
        BULLETATTACHMENT,
        SWORD
    }

    public Type type;
    public PlayerCharacter owner;

    public Modifier(PlayerCharacter newOwner)
    {
        owner = newOwner;
    }
    public virtual void Apply()
    {
        if (owner == null)
        {
            return;
        }
    }
    public virtual void Setup() { }

    public virtual void OnFire() { }

    public virtual void OnCoolingReset() { }

}
