using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class Missile : MonoBehaviour, IDamage
{
    /*
    private Player _player;
    private GameObject _targetUI;
    private ScoreManager _scoreManager;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosion;

    private bool isDamage = true;

    public void Setup(Player player, GameObject targetUI, ScoreManager scoreManager)
    {
        _player = player;
        _targetUI = targetUI;
        _scoreManager = scoreManager;
    }

    private void Update()
    {
        this.transform.position += this.transform.forward * (speed * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(this.transform.forward), Quaternion.LookRotation(_player.transform.position - this.transform.position), Time.deltaTime * rotateSpeed);
        if (isDamage && Vector3.SqrMagnitude(_player.transform.position - this.transform.position) < 4)
        {
            _player.Hit(10);
            Instantiate(explosionPrefab, null);
            explosion.SetActive(true);
            explosion.transform.parent = null;
            Destroy(_targetUI.gameObject);
            Destroy(this.gameObject);
        }
    }
    
    public void AddDamage(float damage)
    {
        _scoreManager.AddScore(300);
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        isDamage = false;
        yield return new WaitForSecondsRealtime(0.5f);
        Instantiate(explosionPrefab, null);
        explosion.SetActive(true);
        explosion.transform.parent = null;
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(_targetUI.gameObject);
        Destroy(this.gameObject);
    }
    */
    
    public void AddDamage(float damage) {}
}
