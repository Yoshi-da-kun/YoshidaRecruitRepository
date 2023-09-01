
using UnityEngine;

/// --------------------------------------------------
/// #CharacterBase.cs
/// 作成者:吉田雄伍
/// 
/// キャラクターに共通する処理をまとめたスクリプト
/// キャラクターとは、HPをもつもの
/// --------------------------------------------------

public class CharacterBase : MonoBehaviour
{
    [SerializeField, Label("このキャラのパラメータ")]
    protected CharacterBaseParameter _characterParameter;

    // 現在のキャラクターのHP
    protected int _currentCharacterHp = default;

    // ダメージを受けたことを示すフラグ
    protected bool _isDamage = false;

    // キャラが死亡しているかを示すフラグ
    protected bool _isDead = false;



    protected void OnEnable()
    {
        // キャラクターがスポーンした時の処理
        SpawnProcess();
    }


    /// <summary>
    /// キャラがスポーンしたときの処理
    /// </summary>
    private void SpawnProcess()
    {
        // キャラのHPの初期化
        _currentCharacterHp = _characterParameter._maxHp;
        
        // 死亡フラグの初期化
        _isDead = false;
    }


    /// <summary>
    /// キャラクターがダメージを受ける処理
    /// </summary>
    /// <param name="DamagePoint"> 与える攻撃力(ダメージ) </param>
    public void TakesDamage(int DamagePoint)
    {
        // 現在HPから攻撃された値を引く
        _currentCharacterHp -= DamagePoint;
        
        // ダメージフラグをセットする
        _isDamage = true;

        // 体力が０以下のとき、死亡フラグをセットする
        if (_currentCharacterHp <= 0)
        {
            _isDead = true;
        }
    }
}
