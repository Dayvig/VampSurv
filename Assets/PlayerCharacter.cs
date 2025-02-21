using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCharacter : MonoBehaviour
{
    enum Inputs
    {
        UP, DOWN, LEFT, RIGHT, SHOOT, SWING
    }

    public float debugSpeed;

    public Transform gunArm;
    public Transform barrel;
    public Transform swordArm;

    private float rotationTarget = 0;

    List<Inputs> playerInputs = new List<Inputs>();

    public float shootTimer = 0.0f;

    public float swingDelay = 3f;
    public float swingTimer = 0.0f;
    public SwordAnimation swordSwingAnimationBase;
    public SwordAnimation swordSwingAnimation;

    public List<GameObject> swordHitboxes = new List<GameObject>();

    public int life = 100;

    public PlayerAttributes playerAttributes;

    #region GunFields
    public float finalBulletSpeed;
    public float finalBulletDamage;
    public float finalFiringSpeed;
    public int finalNumBullets;
    public float finalBulletSpread;
    public float finalBulletRandDeviation;
    public float finalEffectRange;
    public List<Bullet> finalPayloads =  new List<Bullet>();
    public int finalBulletHP;
    public float finalWeaponHeat;
    public Vector3 finalBulletScale;
    public float finalMoveSpeed;
    public float finalCoolingTime;
    public float currentHeat = -1f;
    public bool coolingDown = false;
    public Dictionary<string, float> bonusFiringSpeedMults = new Dictionary<string, float>();
    public List<Modifier> toDispose = new List<Modifier>();
    public float EXP = 0f;
    #endregion

    public List<Modifier> modifiers = new List<Modifier>();

    void Update()
    {
        playerInputs.Clear();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            playerInputs.Add(Inputs.UP);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            playerInputs.Add(Inputs.LEFT);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            playerInputs.Add(Inputs.RIGHT);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            playerInputs.Add(Inputs.DOWN);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            playerInputs.Add(Inputs.SHOOT);
        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(1))
        {
            playerInputs.Add(Inputs.SWING);
        }
    }

    private void Awake()
    {
        swordSwingAnimation = swordSwingAnimationBase.MakeCopy();
        swordSwingAnimation.owner = this.gameObject;
        swordSwingAnimation.Reset();
        previousPos = swordArm.transform.localPosition;
        previousRot = swordArm.transform.localRotation.z;
    }

    private void Start()
    {
        swordHitboxes = swordSwingAnimation.InstantiateHitboxes();
        ValidateModifiers();
    }

    public void ValidateModifiers()
    {
        foreach (Modifier m  in modifiers)
        {
            if (m != null)
            {
                m.owner = this;
            }
        }
    }

    private float LookAtMouse(Vector3 current)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleDeg = (Mathf.Rad2Deg * Mathf.Atan2(mousePos.y - current.y, mousePos.x - current.x));
        return angleDeg;
    }

    private void FixedUpdate()
    {
        applyModifiers();
        playerMovement();
        gunUpdate();
        swordUpdate();
        rotationTarget = LookAtMouse(transform.position);
        armTransformations(rotationTarget);
        foreach (Modifier m in toDispose)
        {
            if (modifiers.Contains(m))
            {
                modifiers.Remove(m);
            }
        }
        toDispose.Clear();
        checkAreaAround();
    }

    void armTransformations(float rotationDeg)
    {
        Quaternion rot = gunArm.transform.localRotation;
        gunArm.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, rotationDeg));

        if (swordSwingAnimation.active == false)
        {
            Quaternion rot2 = swordArm.transform.localRotation;
            swordArm.transform.localRotation = Quaternion.Euler(new Vector3(rot2.x, rot2.y, rotationDeg - 90));
        }
    }

    void playerMovement()
    {
        Vector3 velocity = Vector3.zero;
        if (playerInputs.Count == 0)
        {
            finalMoveSpeed = finalMoveSpeed / 8;
        }
        else
        {
            if (finalMoveSpeed < finalMoveSpeed)
            {
                finalMoveSpeed += (finalMoveSpeed / 4);
            }
        }

        if (playerInputs.Contains(Inputs.UP))
        {
            velocity += Vector3.up;
        }
        if (playerInputs.Contains(Inputs.LEFT))
        {
            velocity += Vector3.left;
        }
        if (playerInputs.Contains(Inputs.RIGHT))
        {
            velocity += Vector3.right;
        }
        if (playerInputs.Contains(Inputs.DOWN))
        {
            velocity += Vector3.down;
        }


        debugSpeed = finalMoveSpeed;

        Vector3 toPos = Vector3.MoveTowards(transform.position, transform.position + velocity.normalized, finalMoveSpeed * Time.deltaTime);
        transform.position = toPos;
    }

    public void checkAreaAround()
    {
        Collider2D[] hitOrbColliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D collider in hitOrbColliders)
        {
            if (collider.gameObject.CompareTag("Orb"))
            {
                Debug.Log("hit");
                ExpOrb hitOrb = collider.gameObject.GetComponent<ExpOrb>();
                if (hitOrb.collectionStage == 0)
                {
                    hitOrb.Collect();
                }
            }
        }
    }

    public void gunUpdate()
    {
        if (coolingDown)
        {
            if (shootTimer > finalCoolingTime || finalCoolingTime <= 0f)
            {
                coolingDown = false;
                currentHeat = finalWeaponHeat;
                foreach (Modifier m in modifiers)
                {
                    m.OnCoolingReset();
                    Debug.Log(bonusFiringSpeedMults["Chaingun"]);
                }
                shootTimer = 0.0f;
            }
            else
            {
                shootTimer += Time.deltaTime;
                return;
            }
        }
        if (shootTimer > finalFiringSpeed && playerInputs.Contains(Inputs.SHOOT))
        {
            FireWeapon();
            shootTimer -= finalFiringSpeed;
        }
        else
        {
            if (shootTimer <= finalFiringSpeed)
            {
                shootTimer += Time.deltaTime;
            }
        }
    }

    public Vector2 previousPos;
    public float previousRot;
    public int swordSpecialStage;
    public void swordUpdate()
    {
        swordSpecialStage = swordSwingAnimation.specialStage;
        float angle = LookAtMouse(transform.position);
        Vector2 posTarget;
        float rotTarget;

        if (swingTimer <= 0.0f && playerInputs.Contains(Inputs.SWING) && swordSwingAnimation.active == false)
        {
            swordSwingAnimation.Reset();
            swordSwingAnimation.active = true;
            swingTimer = swingDelay;
            foreach (GameObject g in swordHitboxes)
            {
                HitboxScript h = g.GetComponent<HitboxScript>();
                h.Reset();
            }
        }
        else if (swordSwingAnimation.active == false)
        {
            if (swingTimer > 0.0f)
            {
                swingTimer -= Time.deltaTime;
            }
        }

        if (swordSwingAnimation.active)
        {
            swordSwingAnimation.timer += Time.deltaTime;
            int currentStage = swordSwingAnimation.stage;

            if (swordSwingAnimation.arcLengths[currentStage] > 0)
            {
                posTarget =
                (Mathf.Sin((float)(Math.PI) * (swordSwingAnimation.timer / (swordSwingAnimation.durations[currentStage]))) * new Vector2(swordSwingAnimation.arcLengths[currentStage], 0f)) +
                Vector2.Lerp(previousPos, swordSwingAnimation.positions[currentStage], (swordSwingAnimation.timer / swordSwingAnimation.durations[currentStage]));
                posTarget = RotateAround(posTarget, Vector2.zero, angle);
            }
            else
            {
                posTarget = Vector2.Lerp(previousPos, swordSwingAnimation.positions[currentStage], (swordSwingAnimation.timer / swordSwingAnimation.durations[currentStage]));
            }

            rotTarget = Mathf.Lerp(previousRot, swordSwingAnimation.rotations[currentStage], (swordSwingAnimation.timer / swordSwingAnimation.durations[currentStage]));

            Quaternion rot = swordArm.transform.rotation;
            swordArm.transform.localPosition = posTarget;

            swordArm.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rotTarget + angle));

            if (swordSwingAnimation.specialStage < swordSwingAnimation.specialActivationTimings.Count)
            {
                swordSwingAnimation.specialTimer += Time.deltaTime;
            }
            if (swordSwingAnimation.specialTimer > swordSwingAnimation.specialActivationTimings[swordSwingAnimation.specialStage])
            {
                swordSwingAnimation.activateSpecialEffects(swordSwingAnimation.specialStage, angle, swordHitboxes, this.gameObject);
                if (swordSwingAnimation.specialStage < swordSwingAnimation.specialActivationTimings.Count - 1)
                {
                    swordSwingAnimation.specialStage++;
                }
            }

            if (swordSwingAnimation.timer > swordSwingAnimation.durations[currentStage])
            {
                swordSwingAnimation.stage++;
                swordSwingAnimation.timer = 0.0f;
                previousPos = swordArm.transform.localPosition;
                previousRot = swordSwingAnimation.rotations[swordSwingAnimation.stage - 1];

                if (swordSwingAnimation.stage >= swordSwingAnimation.durations.Count)
                {
                    swordSwingAnimation.Reset();
                }
            }

        }
    }

    public static Vector2 RotateAround(Vector2 position, Vector2 origin, float degrees)
    {
        float rads = degrees * Mathf.Deg2Rad;
        return new Vector2(((position.x * Mathf.Cos(rads)) - (position.y * Mathf.Sin(rads))), ((position.y * Mathf.Cos(rads)) + (position.x * Mathf.Sin(rads))));
    }

    public void FireWeapon()
    {
        foreach (Modifier m in modifiers)
        {
            m.OnFire();
        }
        if (finalWeaponHeat != -1f)
        {
            if (currentHeat == -1f) { currentHeat = finalWeaponHeat; }
            currentHeat--;
            if (currentHeat <= 0)
            {
                coolingDown = true;
                shootTimer = 0.0f;
            }
        }
        float firingAngle = LookAtMouse(transform.position) - 90;
        if (finalNumBullets == 1)
        {
            SpawnBullet(firingAngle, barrel.transform.position);
            return;
        }
        else
        {
            int initial = 1 - (finalNumBullets % 2);
            for (int s = initial; s < finalNumBullets + initial; s++)
            {
                float angleOffSet =
                    ((((float)s / (finalNumBullets + (1 - (2 * (finalNumBullets % 2))))) * finalBulletSpread) -
                     (finalBulletSpread / 2));
                SpawnBullet(firingAngle + angleOffSet, barrel.transform.position);
            }
        }
    }

    public Bullet SpawnBullet(float angle, Vector2 location)
    {
        return SpawnBullet(angle, location, false);
    }

    public Bullet SpawnBullet(float angle, Vector2 location, bool noPayloads)
    {
        Bullet newBullet = null;
        float bulletRot = angle + 90;
        angle = -(angle * (Mathf.PI / 180));
        Quaternion rot = gunArm.transform.rotation;

        if (BulletManager.instance.inactiveBullets.Count == 0)
        {
            GameObject newBulletObject = Instantiate(BulletManager.instance.baseBulletObject, location, Quaternion.Euler(rot.x, rot.y, bulletRot));
            newBulletObject.gameObject.transform.localScale = finalBulletScale;
            newBullet = newBulletObject.GetComponent<Bullet>();
        }
        else
        {
            for (int i = 0; i < BulletManager.instance.inactiveBullets.Count; i++)
            {
                if (!BulletManager.instance.inactiveBullets[i].isSpawning)
                {
                    newBullet = BulletManager.instance.inactiveBullets[0].resetValues();
                    break;
                }
            }
            if (newBullet == null)
            {
                Debug.Log("attempted to spawn null bullet");
                return null;
            }
            newBullet.isSpawning = true;
            newBullet.gameObject.transform.position = location;
            newBullet.gameObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, bulletRot);
            newBullet.gameObject.transform.localScale = finalBulletScale;
        }
        newBullet.positionTarget = location;

        float xSpeed = Mathf.Sin(angle);
        float ySpeed = Mathf.Cos(angle);

        Vector2 flight = new Vector2(xSpeed, ySpeed).normalized * finalBulletSpeed;
        newBullet.velocity = flight;
        newBullet.owner = BulletManager.BulletOwner.PLAYER;
        SetupBullet(newBullet, noPayloads, location);

        if (!BulletManager.instance.newlyCreatedBullets.Contains(newBullet))
        {
            BulletManager.instance.newlyCreatedBullets.Add(newBullet);
            newBullet.isSpawning = false;
            BulletManager.instance.inactiveBullets.Remove(newBullet);
        }
        return newBullet;
    }

    public void SetupBullet(Bullet b, bool noPayloads, Vector2 location)
    {
        b.gameObject.SetActive(true);
        b.firingOrigin = location;
        b.effectRange = finalEffectRange;
        b.baseSpeed = finalBulletSpeed;
        b.HP = finalBulletHP;
        b.damage = finalBulletDamage;
        b.trailRenderer.Clear();
        if (!noPayloads)
        {
            b.payloadsToSpawn = 1;
            b.payloads = finalPayloads;
        }
        else
        {
            b.payloadsToSpawn = 0;
        }

        foreach (Modifier m in modifiers)
        {
            if (m is BulletAttachmentModifier)
            {
                BulletAttachmentModifier thisModifier = (BulletAttachmentModifier)m;
                switch (thisModifier.key)
                {
                    case BulletAttachmentModifier.Key.BOOMERANG:
                        Boomerang newBoom = new Boomerang(b);
                        newBoom.baseSpeedVal = b.baseSpeed;
                        b.attachments.Add(newBoom);
                        break;
                    case BulletAttachmentModifier.Key.EGG:
                        Egg newEgg = new Egg(b);
                        newEgg.character = this;
                        bool found = false;
                        foreach (BulletAttachment attachment in b.attachments)
                        {
                            if (attachment is Egg)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            b.attachments.Add(newEgg);
                        }
                        break;
                    case BulletAttachmentModifier.Key.AERO:
                        Aero newAero = new Aero(b);
                        newAero.Setup();
                        newAero.originalModifier = (AeroBullets)thisModifier;
                        newAero.hitboxObject = thisModifier.objects[0];
                        newAero.dur = thisModifier.factors[0];
                        b.attachments.Add(newAero);
                        break;
                    case BulletAttachmentModifier.Key.BOUNCY:
                        Bouncy newBouncy = new Bouncy(b);
                        found = false;
                        foreach (BulletAttachment attachment in b.attachments)
                        {
                            if (attachment is Bouncy)
                            {
                                found = true;
                                newBouncy = (Bouncy)attachment;
                                newBouncy.bounces += (int)thisModifier.factors[0];
                                break;
                            }
                        }
                        if (!found)
                        {
                            newBouncy.bounces = (int)thisModifier.factors[0];
                            b.attachments.Add(newBouncy);
                        }
                        break;
                    case BulletAttachmentModifier.Key.BOWLING:
                        Bowling newBowling = new Bowling(b);
                        newBowling.hitboxObject = thisModifier.objects[0];
                        found = false;
                        foreach (BulletAttachment attachment in b.attachments)
                        {
                            if (attachment is Bowling)
                            {
                                found = true;
                                newBowling = (Bowling)attachment;
                                newBowling.dur += thisModifier.factors[0];
                                break;
                            }
                        }
                        if (!found)
                        {
                            newBowling.dur = thisModifier.factors[0];
                            b.attachments.Add(newBowling);
                        }
                        break;
                    case BulletAttachmentModifier.Key.ACCEL:
                        Accel newAccel = new Accel(b);
                        newAccel.speedMod = thisModifier.factors[0];
                        newAccel.damageMod = thisModifier.factors[1];
                        b.attachments.Add(newAccel);
                        break;
                    case BulletAttachmentModifier.Key.TORNADO:
                        Tornado newTornado = new Tornado(b);
                        newTornado.spinGrowthDivisor = thisModifier.factors[0];
                        newTornado.Setup();
                        b.attachments.Add(newTornado);
                        break;

                }
            }
        }

        foreach (BulletAttachment a in b.attachments)
        {
            if (a.type.Equals(BulletAttachment.Type.ONFIRE))
            {
                a.Apply();
            }
        }
    }

    public void CollectOrb(ExpOrb orb)
    {
        EXP += orb.expValue;
        //level up if enugh exp

    }



    public void touchBullet(Bullet b)
    {
        life -= 10;
        if (life <= 0)
        {
            gameObject.SetActive(false);
            //Die();
        }
    }

    public void touchEnemy(BaseEnemy e)
    {
        life -= 10;
        if (life <= 0)
        {
            gameObject.SetActive(false);
            //Die();
        }
    }

    public void applyModifiers()
    {
        ResetValues();
        foreach (Modifier m in modifiers)
        {
            if (m.owner == null)
            {
                m.owner = this;
            }
            m.Apply();
        }
    }

    public void ResetValues()
    {
        finalBulletDamage = playerAttributes.bulletDamage;
        finalBulletSpeed = playerAttributes.bulletSpeed;
        finalBulletSpread = playerAttributes.bulletSpread;
        finalBulletHP = playerAttributes.bulletHP;
        finalEffectRange = playerAttributes.effectRange;
        finalNumBullets = playerAttributes.numBullets;
        finalWeaponHeat = -1f;
        finalBulletRandDeviation = playerAttributes.bulletDeviation;
        finalFiringSpeed = playerAttributes.firingSpeed;
        finalPayloads.Clear();
        finalBulletScale = playerAttributes.weaponScale;
        finalMoveSpeed = playerAttributes.moveSpeed;
        finalCoolingTime = -1f;
    }
}
