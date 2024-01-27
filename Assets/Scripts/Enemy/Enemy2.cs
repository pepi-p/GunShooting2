using System;
using System.Collections;
using UnityEngine;

public class Enemy2 : Enemy
{
    [Space(5), Header("Enemy2Setting")]
    [SerializeField] private float shotInterval;
    [SerializeField] private SEPlayer sePlayer;
    
    [Space(5), Header("Bone")]
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject pistonL1;
    [SerializeField] private GameObject pistonL2;
    [SerializeField] private GameObject pistonR1;
    [SerializeField] private GameObject pistonR2;

    [Space(5), Header("Prefab")]
    [SerializeField] private GameObject muzleFlash;

    [Space(5), Header("Prefab")]
    [SerializeField] private float recoil;

    private float recoilWeight = 0;

    private void Update()
    {
        PistonRotate();
        root.transform.LookAt(player.transform);
        body.transform.localPosition = new Vector3(0, 0.00607321f, -recoil * recoilWeight);
        recoilWeight = Mathf.Lerp(recoilWeight, 0, 5 * Time.deltaTime);
    }

    private IEnumerator ShotCoroutine(float frame)
    {
        var endTime = Time.time + (frame / 60f);
        sePlayer.PlaySound(0);
        while(Time.time <= endTime)
        {
            if(Physics.Raycast(shotPos.transform.position, player.transform.position + Vector3.up * 0.75f - shotPos.transform.position, out var hit, 50))
                if(hit.collider.CompareTag("Player")) player.Hit(shotDamage);
                else muzleFlash.SetActive(false);
            Instantiate(bullet, shotPos.transform.position, Quaternion.LookRotation(player.transform.position + Vector3.up * 0.75f - shotPos.transform.position));
            recoilWeight = 1;
            yield return new WaitForSeconds(shotInterval / 60f);
        }
        muzleFlash.SetActive(false);
    }

    private void PistonRotate()
    {
        pistonR2.transform.rotation = Quaternion.LookRotation(pistonR1.transform.position - pistonR2.transform.position) * Quaternion.Euler(90, 0, 0);
        pistonR1.transform.rotation = Quaternion.LookRotation(pistonR2.transform.position - pistonR1.transform.position) * Quaternion.Euler(90, 0, 0);
        pistonL2.transform.rotation = Quaternion.LookRotation(pistonL1.transform.position - pistonL2.transform.position) * Quaternion.Euler(90, 0, 0);
        pistonL1.transform.rotation = Quaternion.LookRotation(pistonL2.transform.position - pistonL1.transform.position) * Quaternion.Euler(90, 0, 0);
    }
    
    public void TimelineLoop(float frame)
    {
        timeline.time = frame / 60f;
    }
    
    public void TargetUIDisplay()
    {
        targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
        hp = maxHP;
    }
    
    public void Shot(float frame)
    {
        StartCoroutine(ShotCoroutine(frame));
    }
}
