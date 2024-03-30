public class HitPoint
{
    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }

    public HitPoint(float maxHP)
    {
        CurrentHP = maxHP;
        MaxHP = maxHP;
    }

    /// <summary>
    /// HPを減らす
    /// </summary>
    /// <param name="amount">減らす量</param>
    public void ReduceHP(float amount)
    {
        CurrentHP -= amount;
    }

    /// <summary>
    /// HPの割合を返す
    /// </summary>
    /// <returns>HP割合(0~1)</returns>
    public float GetHPRate()
    {
        return CurrentHP / MaxHP;
    }
}
