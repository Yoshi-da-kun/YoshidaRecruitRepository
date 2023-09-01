
using UnityEngine;

/// --------------------------------------------------
/// #NextPuyoLotteryProcess.cs
/// 作成者:吉田雄伍
/// 
/// ネクストのぷよの番号を抽選するスクリプト
/// NextPuyoControl.csに継承しています
/// --------------------------------------------------

public class NextPuyoLottery : MonoBehaviour
{
    [SerializeField, Header("パラメータのScriptableObjectをまとめたスクリプト")]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    private FieldParameter _fieldParameter;

    private void Awake()
    {
        // フィールドに関するパラメータを取得
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
    }


    /// <summary>
    /// ランダムにぷよを抽選するスクリプトです
    /// </summary>
    public sbyte NextPuyoNumberLottery()
    {
        return (sbyte)Random.Range(1, _fieldParameter._puyoSprits.Length);
    }
}