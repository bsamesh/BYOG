using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroZone : MonoBehaviour
{

    public Transform relatedEnemy;
    Enemy enemy;

    private void Awake()
    {
        enemy = relatedEnemy.GetComponent<Enemy>();
        if (enemy == null)
            Debug.LogError("Enemy not found?");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            enemy.PlayerTransform = collision.transform;
            enemy.startAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            enemy.stopAttack();
        }
    }
}
