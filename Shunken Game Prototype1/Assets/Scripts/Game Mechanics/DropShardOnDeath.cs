using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShardOnDeath : MonoBehaviour
{
    EnemyBehavior enemyBehavior;
    [SerializeField] GameObject shardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        enemyBehavior.onEnemyDeath += EnemyBehavior_onEnemyDeath;
    }

    private void EnemyBehavior_onEnemyDeath(object sender, System.EventArgs e)
    {
        Instantiate(shardPrefab, this.transform.position, Quaternion.identity);
        //throw new System.NotImplementedException();
    }

   
}
