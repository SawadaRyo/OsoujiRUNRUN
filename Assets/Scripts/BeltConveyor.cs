using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Garbage") || 
            collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position += transform.up * _speed * Time.deltaTime;
        }
    }
}
