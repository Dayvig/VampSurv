using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float bounceInterval = 0.4f;
    public float bounceTimer = 0.0f;
    public SpriteRenderer ren;
    Vector2 destination;
    public List<Vector2> waypoints = new List<Vector2>();
    public List<float> wayPointSpeedMults = new List<float>();
    public float life = 1;
    public List<Bullet> immuneBullets = new List<Bullet>();
    public List<HitboxScript> immuneHitboxes = new List<HitboxScript>();

    public float lifeTimer = 0.0f;
    public float lastImmunityCheck = 0.0f;

    public enum State
    {
        MOVEMENT,
        KNOCKBACK,
        BOWLINGKNOCKBACK
    }

    public State currentMovementState = State.MOVEMENT;
    public void EnemyUpdate()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer - lastImmunityCheck > 1f) {
            immuneBullets.Clear();
            lastImmunityCheck = lifeTimer;
        }
        if (waypoints.Count > 0)
        {
            destination = Vector2.MoveTowards(transform.position, waypoints[0], Time.deltaTime * moveSpeed * wayPointSpeedMults[0]);
            if (Vector2.Distance(transform.position, waypoints[0]) < 0.1f)
            {
                waypoints.Remove(waypoints[0]);
                wayPointSpeedMults.Remove(wayPointSpeedMults[0]);
                if (currentMovementState == State.BOWLINGKNOCKBACK)
                {
                    currentMovementState = State.MOVEMENT;
                }
            }
        }
        else
        {
            destination = Vector2.MoveTowards(transform.position, EnemyManager.instance.player.gameObject.transform.position, Time.deltaTime * moveSpeed);
        }
        bounceTimer += Time.deltaTime;
        switch (currentMovementState)
        {
            case State.MOVEMENT:
                if (bounceTimer > bounceInterval)
                {
                    applyStandardPushCollisions();
                    bounceTimer = 0.0f;
                }
                break;
        }
        applyFrameByFrameCollisions();

        transform.position = destination;
    }

    public void applyStandardPushCollisions()
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, (ren.sprite.bounds.size.x / 2));
        Vector2 newDest = destination;
        if (hitColliders.Length > 1)
        {
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.enabled && collider.gameObject.CompareTag("Enemy") && !collider.gameObject.Equals(gameObject))
                {
                    newDest = Vector2.MoveTowards(transform.position, collider.gameObject.transform.position, -Time.deltaTime * moveSpeed * 2);
                }
                if (collider.gameObject.CompareTag("Player"))
                {
                    waypoints.Add(Vector2.MoveTowards(transform.position, EnemyManager.instance.player.gameObject.transform.position, -Time.deltaTime * moveSpeed * 100));
                    wayPointSpeedMults.Add(6f);
                    EnemyManager.instance.player.touchEnemy(this);
                }
            }
            destination = newDest;
        }
    }

    public void applyFrameByFrameCollisions()
    {
        if (currentMovementState == State.MOVEMENT)
        {
            return;
        }
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, (ren.sprite.bounds.size.x / 2));
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy") && currentMovementState == State.BOWLINGKNOCKBACK)
            {
                BaseEnemy touchedEnemy = collider.gameObject.GetComponent<BaseEnemy>();
                touchedEnemy.addBowlingKnockback(transform.position, wayPointSpeedMults[0] * 0.8f);
            }
        }
    }

    public void addBowlingKnockback(Vector2 bowlingBallPosition, float speed)
    {
        life -= (int)speed;
        if (currentMovementState == State.MOVEMENT)
        {
            if (wayPointSpeedMults.Count > 0 && wayPointSpeedMults[0] < 2f)
            {
                return;
            }
            waypoints.Clear();
            wayPointSpeedMults.Clear();
            wayPointSpeedMults.Add(speed);
            waypoints.Add(Vector2.MoveTowards(transform.position, bowlingBallPosition, -Time.deltaTime * moveSpeed * (400 * (speed/20f))));
            currentMovementState = State.BOWLINGKNOCKBACK;
        }
    }

    public Vector2 findNormalBowlingPoint(BowlingHitbox bHitbox)
    {
        float angleTowardsYAxis = (float)Vector2.SignedAngle(bHitbox.velocity, new Vector2(0f, 1f));

        float angleTowardsNormal = angleTowardsYAxis - 90;

        float rads = angleTowardsNormal * Mathf.Deg2Rad;

        Vector2 final = new Vector2(Mathf.Sin(rads), Mathf.Cos(rads)).normalized;

        Vector3 normal = Vector3.zero;
        if (LineLineIntersection(out normal, bHitbox.transform.position, bHitbox.velocity, transform.position, final)){
            return (Vector2)normal;
        }

        return final;
    }

    public static bool LineLineIntersection(out Vector3 intersection,
        Vector3 linePoint1, Vector3 lineDirection1,
        Vector3 linePoint2, Vector3 lineDirection2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineDirection1, lineDirection2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineDirection2);
        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineDirection1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }

    public void touchBullet(Bullet b)
    {
        TakeDamage(b.damage + Random.Range(-(b.damage * 0.2f), (b.damage * 0.2f)));
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Die();
        }
        SpawnDamageNumber(damage);
    }

    public void touchHitbox(HitboxScript h)
    {
        TakeDamage(100);
    }

    public void Die()
    {
        EnemyManager.instance.markedForDeathEnemies.Add(this);
        gameObject.SetActive(false);
        SpawnExpOrb(10);
    }

    public void SpawnDamageNumber(float damage)
    {
        bool found = false;
        for (int i = 0; i < VfxManager.instance.inactiveDamageNumbers.Count; i++)
        {
            if (VfxManager.instance.inactiveDamageNumbers[i].active == false)
            {
                VfxManager.instance.inactiveDamageNumbers[i].Spawn(transform.position, 1f, Color.white, (int)damage);
                VfxManager.instance.toSpawnDamageNumbers.Add(VfxManager.instance.inactiveDamageNumbers[i]);
                found = true;
                break;
            }
        }
        if (!found)
        {
            VfxManager.instance.InstantiateNewDamageNumber(transform.position, 1f, Color.white, (int)damage);
        }
    }

    public void SpawnExpOrb(float value)
    {
        bool found = false;
        for (int i = 0; i < ProgressionManager.instance.inactiveOrbs.Count; i++)
        {
            if (ProgressionManager.instance.inactiveOrbs[i].spawning == false)
            {
                ProgressionManager.instance.inactiveOrbs[i].Spawn((Vector2)transform.position + Random.insideUnitCircle * 0.2f, value);
                ProgressionManager.instance.toSpawnOrbs.Add(ProgressionManager.instance.inactiveOrbs[i]);
                found = true;
                break;
            }
        }
        if (!found)
        {
            ProgressionManager.instance.InstantiateNewOrb(ProgressionManager.instance.basicOrb, value, transform.position);
        }
    }

    public BaseEnemy ResetValues()
    {
        waypoints.Clear();
        bounceTimer = 0.0f;
        immuneBullets.Clear();
        return this;
    }
}
