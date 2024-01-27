using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class EnemyTarget : MonoBehaviour
{
    public Transform targetPos;
    public Vector3 uiDisplayOffset;
    [SerializeField] private RectTransform targetUI;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image atkBar;
    [SerializeField] private Color atkBarColorFrom;
    [SerializeField] private Color atkBarColorTo;
    
    private RectTransform rootUI;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        rootUI = targetUI.root.GetComponent<RectTransform>();
    }

    private void Update()
    {
        var targetScreenPos = cam.WorldToScreenPoint(targetPos.position + uiDisplayOffset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rootUI, targetScreenPos, null, out var uiPos);
        targetUI.localPosition = uiPos;
    }

    public void Damage(float hpRate)
    {
        hpBar.fillAmount = hpRate;
        if (hpRate <= 0) StartCoroutine(DestroyTargetUI());
    }

    public void AttackBar(float amount)
    {
        atkBar.fillAmount = amount;
        atkBar.color = Color.Lerp(atkBarColorFrom, atkBarColorTo, amount);
    }

    private IEnumerator DestroyTargetUI()
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
