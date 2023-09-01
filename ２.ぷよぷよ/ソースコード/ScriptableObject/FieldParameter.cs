
using UnityEngine;

/// --------------------------------------------------
/// #FieldParameter.cs
/// 作成者:吉田雄伍
/// 
/// フィールドの大きさ等、フィールドやぷよの情報。ぷよの状態をまとめたスクリプトです
/// --------------------------------------------------

[CreateAssetMenu (menuName = "Parameters/FieldParameters", fileName = "NewFieldParameter")]
public class FieldParameter : ScriptableObject
{
    [field: Header("フィールドの大きさ")]
    [field: SerializeField, Label("(縦)段数")]
    public sbyte _fieldColumnSize { get; private set; }

    [field: SerializeField, Label("(横)行数")]
    public sbyte _fieldRowSize { get; private set; }

    [field: SerializeField, Label("表示するフィールドのPrefab")]
    private GameObject _fieldPrefab;

    // フィールドのオブジェクトのワールド座標上での大きさ(Scale)
    public Vector2 _scaleOfOneMass { get; private set; }

    // ぷよのデフォルトの大きさ
    public Vector2[] _puyoLocalScales { get; private set; }


    [field: SerializeField, Header("ぷよの見た目")]
    public GameObject[] _puyoSprits{ get; private set; }

    [field: Header("ぷよに関する値")]
    [field: SerializeField, Label("ぷよが消える時間")]
    public sbyte _puyoEraceTime { get; private set; }

    [field: SerializeField, Label("ぷよが消える数")]
    public sbyte _puyoEraceLinkCount { get; private set; }


    [field: Header("ゲームオーバーになるマス(左下端のマスを０とする)")]

    [field: SerializeField, Label("段")]
    public sbyte _gameOverColumn { get; private set; }
    [field: SerializeField, Label("行")]
    public sbyte _gameOverRow { get; private set; }


    // 回転時の親ぷよに対する子ぷよの位置(回転状態が一次元要素。各ぷよの位置が二次元要素で、親ぷよを要素０とする)
    public Vector2Int[][] _puyosEachRotationDirections 
    { 
        get 
        { 
            return new[] 
            {
                new[]{ new Vector2Int(0, 0), new Vector2Int( 0, 1), new Vector2Int( 1, 0), new Vector2Int( 1, 1) },
                new[]{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 0,-1), new Vector2Int( 1,-1) },
                new[]{ new Vector2Int(0, 0), new Vector2Int( 0,-1), new Vector2Int(-1, 0), new Vector2Int(-1,-1) },
                new[]{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 0, 1), new Vector2Int(-1, 1) }
            }; 
        } 
    }

    // 回転番号の初期値
    public readonly sbyte _initialRotateNumber = 0;


    /// <summary>
    /// フィールドのワールド上でのScaleを求める
    /// </summary>
    private void OnEnable()
    {
        // フィールドのワールド座標上の大きさを格納する
        Vector2 fieldWorldScale = _fieldPrefab.transform.lossyScale;

        // １マスあたりの大きさを求める(行はフィールドの見た目から1マス上にはみ出るため1引く)
        _scaleOfOneMass = new Vector2(fieldWorldScale.x / _fieldRowSize, fieldWorldScale.y / (_fieldColumnSize - 1));

        // 配列の大きさを定義
        _puyoLocalScales = new Vector2[_puyoSprits.Length];

        // ぷよのPrefabの大きさを１マスの大きさに変更する
        for (int i = 0; i < _puyoSprits.Length; i++)
        {
            _puyoLocalScales[i] = _puyoSprits[i].transform.localScale * _scaleOfOneMass;
        }

        // ゲームオーバーマスがフィールドの外の時の処理
        if (_gameOverColumn >= _fieldColumnSize || _gameOverRow >= _fieldRowSize)
        {
            Debug.LogError("ゲームオーバーマスがフィールドの外です");
        }
    }
}
