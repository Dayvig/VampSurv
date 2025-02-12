using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public List<BaseEnemy> activeEnemies = new List<BaseEnemy>();
    public List<BaseEnemy> inactiveEnemies = new List<BaseEnemy>();
    public List<BaseEnemy> markedForDeathEnemies = new List<BaseEnemy>();

    public PlayerCharacter player;

    public static EnemyManager instance { get; private set; }

    public float enemySpawnTimer = 0.0f;
    public float enemySpawnInterval = 0.2f;
    public int enemyBaseHP;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        foreach (BaseEnemy enemy in activeEnemies)
        {
            enemy.EnemyUpdate();
        }
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer > enemySpawnInterval)
        {
            SpawnEnemy();
            enemySpawnTimer = 0.0f;
        }
    }

    void SpawnEnemy()
    { 
            BaseEnemy newEnemy;
            if (inactiveEnemies.Count == 0)
            {
            GameObject newEnemyObject = Instantiate(enemy, (Vector2)player.transform.position + (Random.insideUnitCircle.normalized * 5f), Quaternion.identity);
            newEnemy = newEnemyObject.GetComponent<BaseEnemy>();
            }
            else
            {
                newEnemy = inactiveEnemies[0].ResetValues();
                newEnemy.gameObject.transform.position = (Vector2)player.transform.position + (Random.insideUnitCircle.normalized * 5f);
                inactiveEnemies.Remove(newEnemy);
            }
            if (!activeEnemies.Contains(newEnemy))
            {
                activeEnemies.Add(newEnemy);
            }
            newEnemy.gameObject.SetActive(true);
        newEnemy.life = setHP();
    }

    public int setHP()
    {
        return enemyBaseHP;
    }

    private void Update()
    {
        Trash();
    }

    void Trash()
    {
        foreach (BaseEnemy b in markedForDeathEnemies)
        {
            activeEnemies.Remove(b);
            inactiveEnemies.Add(b);
        }
        markedForDeathEnemies.Clear();
    }

}
