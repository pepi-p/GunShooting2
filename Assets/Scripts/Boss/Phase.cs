namespace Boss
{
    public enum Phase
    {
        AppearMovie,   // 登場ムービー
        GatlingRPhase, // 右ガトリング破壊まで
        GatlingLPhase, // 左ガトリング破壊まで
        ChangeMovie,   // 形態変化ムービー
        BeamPhase,     // 完全破壊まで
        DestroyMovie   // 撃破ムービー
    }
}