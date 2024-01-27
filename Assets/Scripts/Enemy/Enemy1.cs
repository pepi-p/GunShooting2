using UnityEngine;

public class Enemy1 : Enemy
{
    [Header("Enemy1")]
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject barrel;

    private void Update()
    {
        // Debug.Log(Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.x - this.transform.position.x, player.transform.position.z - this.transform.position.z));
    }

    // 砲塔をプレイヤーに向ける
    private void LookPlayer()
    {
        Vector3 foward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        var angle = Vector3.Angle(direction, foward);
        if(transform.InverseTransformVector(direction).x < 0) angle *= -1;

        turret.transform.localRotation = Quaternion.Euler(-89.98f, 0, angle);

        angle = Vector3.Angle(player.transform.position - barrel.transform.position, -turret.transform.up);
        if(player.transform.position.y - (transform.position.y + 1.789078f) > 0) angle *= -1;

        // barrel.transform.localRotation = Quaternion.Euler(angle, 0, 35f);
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
}
