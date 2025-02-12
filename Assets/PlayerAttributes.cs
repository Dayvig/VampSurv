using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    public float bulletSpeed;
    public float bulletDamage;
    public float firingSpeed;
    public int numBullets;
    public float bulletSpread;
    public float bulletDeviation;
    public float effectRange;
    public List<Bullet> bulletPayloads = new List<Bullet>();
    public int bulletHP;
    public float weaponHeat;
    public Vector3 weaponScale;
    public float moveSpeed;

}
