
using UnityEngine;

/// --------------------------------------------------
/// #PlayerParameter.cs
/// 作成者:吉田雄伍
/// 
/// 移動速度や落下速度などプレイヤーの操作感に関する
/// パラメータをまとめたスクリプトです
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PlayerParameter", fileName = "NewPlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [field: Header("左右の移動関連")]

    [field: SerializeField, Label("左右の高速移動に入るまでの時間(秒)"), Range(0.01f, 1)]
    public float _intoHighSpeedTime { get; private set; } = 1;

    [field: SerializeField, Label("左右の高速移動ごとの硬直(秒)"), Range(0.01f, 1)]
    public float _moveStoppingTime { get; private set; } = 1;

    [field: SerializeField, Label("左右の移動にかかる移動回数")]
    public sbyte _horizontalMoveCounts { get; private set; } = 1;


    [field: Header("落下関連")]

    [field: SerializeField, Label("落下のなめらかさ"), Range(1, 10)]//////////////////////////要改良
    public int _fallSmoothness { get; private set; } = 2;

    [SerializeField, Label("落下速度(最大500くらい)")]
    private float _fallNormalSpeed = 1;

    // 通常落下中、一回落下するまでの時間
    public float _normalSpeedFallTime { get; private set; }

    [SerializeField, Label("高速落下速度(最大500くらい)")]
    private float _fallHighSpeed = 1;

    // 高速落下中、一回落下するまでの時間
    public float _highSpeedFallTime { get; private set; } = 1;


    [field: Header("ぷよの回転に関する値")]

    [field: SerializeField, Label("ぷよを回転させる速度")]
    private float _puyoRotationSpeed = 1; 

    // 正規化されたぷよの回転速度
    public float _normalizedRotationSpeed { get; private set; }

    // 半回転、４分の１回転するのにかかる時間
    public float _quarterRotationTime { get; private set; }
    public float _halfRotationTime { get; private set; }




    [field: Header("入力範囲に関する値")]

    [field: SerializeField, Label("スティック左右入力のデッドゾーン"), Range(0, 1)]
    public float _horizontalDeadZone { get; private set; }

    [field: SerializeField, Label("スティック上下入力のデッドゾーン"), Range(0, 1)]
    public float _verticalDeadZone { get; private set; }



    [field: Header("演出や硬直時間などの値")]

    [field: SerializeField, Label("ネクスト内のぷよを動かす時間"), Range(0, 10)]
    public float _nextPuyoMoveTime { get; private set; }

    [field: SerializeField, Label("ぷよが着地してから、設置されるまでの猶予時間")]
    public float _installGraceTime { get; private set; }


    [field: SerializeField, Label("左から数えたときのぷよの生成行(左端は０行目)")]
    public sbyte _puyoInstantRow { get; private set; }



    private void OnEnable()
    {
        // 通常、高速それぞれの落下までの時間を求める( 1秒 / 速度 )
        _normalSpeedFallTime = 1 / _fallNormalSpeed;
        _highSpeedFallTime = 1 / _fallHighSpeed;

        // 半回転、４分の１回転するのにかかる時間を求める( 1 と 0.5f は回転速度が 1 のとき、各回転にかかる時間)
        _halfRotationTime = 1 / _puyoRotationSpeed;
        _quarterRotationTime = 0.5f / _puyoRotationSpeed;

        // 回転速度を正規化する(Sin,Cosを使って回転するため)
        _normalizedRotationSpeed = _puyoRotationSpeed * Mathf.PI;
    }
}