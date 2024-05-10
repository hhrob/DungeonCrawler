using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float enemySpeed = 5f;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * enemySpeed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")    
        {
            Debug.Log("Player Hit.");
            Time.timeScale = 0;
        }
    }
}
