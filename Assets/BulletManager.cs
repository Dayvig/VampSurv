using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<Bullet> activeBullets = new List<Bullet>();
    public List<Bullet> newlyCreatedBullets = new List<Bullet>();
    public List<Bullet> inactiveBullets = new List<Bullet>();
    public List<Bullet> markedForDeathBullets = new List<Bullet>();
    public float xBounds;
    public float yBounds;

    public GameObject baseBulletObject;
    public float baseBulletSpeed = 0.1f;
    public static BulletManager instance { get; private set; }

    public enum BulletOwner
    {
        PLAYER,
        ENEMY,
        NONE,
        MISC
    }
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
        foreach (Bullet b in newlyCreatedBullets)
        {
            if (!activeBullets.Contains(b))
            {
                activeBullets.Add(b);
            }
        }
        newlyCreatedBullets.Clear();
        foreach (Bullet b in activeBullets)
        {
            b.BulletUpdate();
        }
    }

    void Trash()
    {
        foreach (Bullet b in markedForDeathBullets)
        {
            activeBullets.Remove(b);
            if (!inactiveBullets.Contains(b))
            {
                inactiveBullets.Add(b);
            }
        }
        markedForDeathBullets.Clear();
    }
}
