using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float hp;
    [SerializeField] private GameObject particle;

    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        hp = maxHP;
    }

    public void Hit(float damage)
    {
        if(hp > 0)
        {
            hp -= damage;
            if(hp <= 0) animator.SetTrigger("destroy");
        }
    }

    public void Explosion()
    {
        Instantiate(particle, transform.position + Vector3.up * 0.4f, Quaternion.identity);
    }

    public void DestroyThisObj()
    {
        Destroy(this.gameObject);
    }
}
