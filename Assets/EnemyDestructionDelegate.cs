
using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour
{

    public delegate void EnemyDelegate(GameObject enemy);
    public EnemyDelegate enemyDelegate;

    
    void Start()
    {

    }

   
    void Update()
    {

    }

    void OnDestroy()
    {
        if (enemyDelegate != null)
        {
            enemyDelegate(gameObject);
        }
    }

}
