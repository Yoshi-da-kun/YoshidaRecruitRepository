
using UnityEngine;

/// --------------------------------------------------------
/// #BreakableObjects.cs
/// 
/// 破壊可能なオブジェクトの継承元になるスクリプト
/// --------------------------------------------------------

public class BreakableObjects : MonoBehaviour
{
    // オブジェクトが持っている残りHP
    private int _currentHp = default;

    private int _initializeHp = 3;

    // ダメージを受けた時のフラグ
    protected bool _isDamaged = default;

    // 壊れたときのフラグ
    protected bool _isBreaked = default;


    private void OnEnable()
    {
        // Hpを初期化
        _currentHp = _initializeHp;
    }



    /// <summary>
    /// 攻撃を受けたときの処理
    /// </summary>
    public void DamagedProcess(int damageVolume)
    {
        // 現在HPをダメージ分減らす
        _currentHp -= damageVolume;

        // 体力がなくなったとき、壊れたときの処理を行う
        if (_currentHp < 0)
        {
            // 壊れたフラグをセット
            _isBreaked = true;

            // オブジェクトを無効化する
            this.gameObject.SetActive(false);
        }

        // ダメージをうけたフラグをセットする
        _isDamaged = true;
    }
}
