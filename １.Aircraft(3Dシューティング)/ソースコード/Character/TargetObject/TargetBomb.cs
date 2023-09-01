
using UnityEngine;

/// --------------------------------------------------
/// #TargetBomb.cs
/// 作成者:吉田雄伍
/// 
/// 爆弾型の的の処理を行うスクリプト
/// --------------------------------------------------

public class TargetBomb : TargetManager
{
    [SerializeField, Label("爆弾のパラメータ")]
    TargetBombParameter _bombParameter;

    [SerializeField, Label("音を流すスクリプト")]
    private SoundController _soundController;


    private void Update()
    {
        // 壊された時の処理
        if (_isDead)
        {
            // エフェクトを出す
            Instantiate(_bombParameter._breakedEffectPrefab, this.transform.position, Quaternion.identity);

            // 音を出す
            _soundController.PlaySeSound(_bombParameter._breakedSound);

            // オブジェクトを無効化する
            this.gameObject.SetActive(false);
        }
    }
}
