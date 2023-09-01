
/// --------------------------------------------------
/// #TargetManager.cs
/// 作成者:吉田雄伍
/// 
/// ターゲットオブジェクト(敵や的)に共通する処理をまとめたスクリプト
/// --------------------------------------------------

public class TargetManager : CharacterBase
{
    /// <summary>
    /// オブジェクトが有効化されたとき、ターゲットを追加する
    /// </summary>
    new protected void OnEnable()
    {
        base.OnEnable();

        // ターゲットを追加する
        WaveState.NewTargetAdd(this.gameObject);
    }

    // オブジェクトが無効化されたとき、ターゲット数を更新する
    protected void OnDisable()
    {

        // ターゲット数を更新する
        WaveState.TaragetCountUpdate();
    }
}
