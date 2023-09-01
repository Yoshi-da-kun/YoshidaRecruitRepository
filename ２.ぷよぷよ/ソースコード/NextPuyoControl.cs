
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #NextPuyoControl.cs
/// 作成者:吉田雄伍
/// 
/// フィールドのオブジェクトにアタッチしてください
/// ネクストのぷよをコントロール、生成するスクリプト
/// --------------------------------------------------

public class NextPuyoControl : MonoBehaviour
{
    [SerializeField, Header("パラメータのScriptableObjectをまとめたスクリプト")]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // フィールドのパラメータ
    private FieldParameter _fieldParameter;

    // プレイヤーに関するパラメータ(ScriptableObject)
    private PlayerParameter _playerParameter;

    // プレイヤーの入力と動作を行うスクリプト
    private PlayerOperation _playerOperation;

    // ネクストぷよの抽選をするスクリプト
    private NextPuyoLottery _nextPuyoLottery;

    // ゲーム進行を示すフラグをまとめたスクリプト
    private PlayerFlags _playerFlags;


    // ぷよの色番号とTransformを格納するジャグ配列と、次に操作するぷよのポインタ
    private sbyte[][] _puyosColors = default;
    private Transform[][] _puyosTransforms = default;
    private int _controlPuyoPointer = default;

    [SerializeField, Header("ネクストのぷよの位置(番号の低い順に次のぷよ座標を入れる)")]
    private Transform[] _nextPuyoPositions = default;

    // ネクストのぷよの移動距離(一秒あたり)
    private Vector3[] _nextPuyoMoveSpeed = default;

    // ぷよの出現位置
    private Vector3 _controlPuyoGeneratePosition;


    // ネクストぷよが動いている間の計測時間
    private float _nextMoveElapsedTime = default;

    // ネクストの中をぷよが移動しているか
    private bool _nextPuyosInstantiated = default;


    private void Start()
    {
        // フィールドに関するパラメータを取得
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
        // プレイヤーに関するパラメータを取得
        _playerParameter = _summarizeScriptableObjects._playerParameter;

        // ぷよの抽選を行うスクリプトを取得
        _nextPuyoLottery = this.GetComponent<NextPuyoLottery>();
        // ゲームの進行にかかわるフラグを管理するスクリプトを取得
        _playerFlags = this.GetComponent<PlayerFlags>();
        // プレイヤーの入力と動作を行うスクリプトを取得
        _playerOperation = this.GetComponent<PlayerOperation>();

        // ジャグ配列の一次元目の要素数を定義
        _puyosColors = new sbyte[_nextPuyoPositions.Length - 1][];
        _puyosTransforms = new Transform[_nextPuyoPositions.Length - 1][];

        // ジャグ配列の二次元目の要素数を定義
        for (int i = 0; i < _puyosColors.Length; i++)
        {
            // ぷよが二つ以外の場合の動作を作っていないため、２(ぷよの数)としています
            _puyosColors[i] = new sbyte[2];
            _puyosTransforms[i] = new Transform[2];
        }

        // ネクスト内の移動距離を示す配列の大きさを定義する
        _nextPuyoMoveSpeed = new Vector3[_nextPuyoPositions.Length - 1];

        // ネクスト内の移動速度を求める
        for (int i = 0; i < _nextPuyoMoveSpeed.Length; i++)
        {
            // ネクスト内の一秒あたりの移動距離を計算する
            _nextPuyoMoveSpeed[i] = (_nextPuyoPositions[i].position - _nextPuyoPositions[i + 1].position) / _playerParameter._nextPuyoMoveTime;
        }

        // フィールドの左下端の座標を求める
        Vector2 bottomLeftEdgeMassPos = this.transform.position - this.transform.lossyScale / 2;

        //　フィールドの左下端のマスの中心求める
        bottomLeftEdgeMassPos += _fieldParameter._scaleOfOneMass / 2;

        // 操作するぷよの生成座標を求める
        _controlPuyoGeneratePosition = new Vector3(bottomLeftEdgeMassPos.x + _fieldParameter._scaleOfOneMass.x * _playerParameter._puyoInstantRow, 
            bottomLeftEdgeMassPos.y + _fieldParameter._scaleOfOneMass.y * _fieldParameter._fieldColumnSize - 1, 0);

        // ゲームを開始する
        GamePlayStart();
    }


    private void FixedUpdate()
    {
        // ネクストのぷよ更新中じゃなければ処理しない
        if (!_playerFlags._isNextPuyoUpdate)
        {
            return;
        }

        // ぷよを生成していないときの処理
        if (!_nextPuyosInstantiated)
        {
            // ぷよを生成する処理
            NextPuyoInstantiate();

            return;
        }

        // ネクストの枠内を移動する処理
        NextPuyoPositionUpdate();
    }


    /// <summary>
    /// ネクストぷよの移動を行う処理
    /// </summary>
    private void NextPuyoPositionUpdate()
    {
        // ネクストぷよの移動時間を計測
        _nextMoveElapsedTime += Time.fixedDeltaTime;

        // ネクストぷよすべての移動を行うまで繰り返す
        for (int i = 0; i < _puyosColors.Length; i++)
        {
            // 動かすネクストぷよのポインタ
            int moveNextPuyoPointer = _controlPuyoPointer + i;

            // ポインタが要素数を超えた時に正しい位置にする
            if (moveNextPuyoPointer >= _puyosColors.Length)
            {
                moveNextPuyoPointer -= _puyosColors.Length;
            }

            // ネクストぷよを移動をする
            for (int j = 0; j < _puyosColors[i].Length; j++)
            {
                // ぷよを移動する
                _puyosTransforms[moveNextPuyoPointer][j].position += _nextPuyoMoveSpeed[i] * Time.fixedDeltaTime;
            }
        }
        // ネクストぷよの移動が終了したか
        if (_nextMoveElapsedTime < _playerParameter._nextPuyoMoveTime)
        {
            return;
        }


        ///-- ネクストぷよの移動が終了したときの処理 --///

        // ネクストぷよの生成、更新フラグと移動時間を初期化
        _playerFlags._isNextPuyoUpdate = false;
        _nextPuyosInstantiated = false;
        _nextMoveElapsedTime = 0;

        // ネクストのぷよが正しい位置(ネクストの枠)にあるかを確認する処理
        for (int i = 0; i < _puyosColors.Length; i++)
        {
            // 正しい位置にあるかを確認するぷよのポインタ
            int checkPuyoPointer = _controlPuyoPointer + i;

            // ポインタがぷよを格納している配列の数を超えたら、ポインタの位置を正しくなおす
            if (checkPuyoPointer >= _puyosColors.Length)
            {
                checkPuyoPointer -= _puyosColors.Length;
            }

            // ネクストぷよが正しい座標にあるなら次の要素へ
            if (_puyosTransforms[checkPuyoPointer][0].position == _nextPuyoPositions[i].position)
            {
                continue;
            }

            // ネクストぷよが正しい座標にないなら正しい座標にする
            for (int j = 0; j < _puyosColors[i].Length; j++)
            {
                // ぷよの親ぷよに対するぷよの相対座標を求める
                Vector2 puyoRelativePosition =
                    _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][j] * _fieldParameter._scaleOfOneMass;

                // ぷよのワールド座標を求める
                Vector3 puyoPosition = _nextPuyoPositions[i].position + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

                // ぷよを移動する
                _puyosTransforms[checkPuyoPointer][j].position = puyoPosition;
            }
        }

        // 操作するぷよの 色, Transform, 行, 段
        List<PuyoStructs.ControlPuyoData> controlPuyoDatas = new List<PuyoStructs.ControlPuyoData>();

        // ネクストぷよからフィールド上に出す処理
        for (int i = 0; i < _puyosTransforms[_controlPuyoPointer].Length; i++)
        {
            // ぷよの親ぷよに対する相対座標を求める
            Vector2 puyoRelativePosition =
                _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i] * _fieldParameter._scaleOfOneMass;

            // フィールドに出すぷよの座標を求める
            Vector3 puyoPosition =
                _controlPuyoGeneratePosition + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

            // ぷよを移動する
            _puyosTransforms[_controlPuyoPointer][i].position = puyoPosition;

            // 操作するぷよのデータ
            PuyoStructs.ControlPuyoData controlPuyoData = default;

            // 操作するぷよの色とTransformを格納する
            controlPuyoData.Color = _puyosColors[_controlPuyoPointer][i];
            controlPuyoData.Transform = _puyosTransforms[_controlPuyoPointer][i];
            // 操作するぷよの列と行を格納する
            controlPuyoData.Row = _playerParameter._puyoInstantRow + _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i].x;
            controlPuyoData.Column = _fieldParameter._fieldColumnSize - 1 + _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i].y;

            // 操作するぷよのデータを格納する
            controlPuyoDatas.Add(controlPuyoData);
        }

        // 操作するぷよのデータを渡し、プレイヤーの操作を開始する
        _playerOperation.PlayerControlStart(controlPuyoDatas);
    }


    /// <summary>
    /// ぷよを新しく生成する処理
    /// </summary>
    private void NextPuyoInstantiate()
    {
        // 操作するぷよのポインタをインクリメント
        _controlPuyoPointer++;

        // ポインタがぷよを格納している配列の数を超えたら0に戻す
        if (_controlPuyoPointer >= _puyosColors.Length)
        {
            _controlPuyoPointer = 0;
        }

        // 一番遠いネクストぷよデータのポインタ
        int lastNextPuyoPointer = _controlPuyoPointer - 1;

        // 一番遠いネクストぷよのポインタが0未満になってしまったら要素の末尾にする
        if (lastNextPuyoPointer < 0)
        {
            lastNextPuyoPointer = _puyosColors.Length - 1;
        }

        // ぷよが連続で生成されているとき
        if (_nextPuyosInstantiated)
        {
            // ネクストのぷよが正しい位置(ネクストの枠)にあるかを確認する処理
            for (int i = 0; i < _puyosColors.Length; i++)
            {
                // 正しい位置にあるかを確認するぷよのポインタ
                int puyoPointer = _controlPuyoPointer + i;

                // ポインタがぷよを格納している配列の数以上なら、ポインタの位置を正しくなおす
                if (puyoPointer >= _puyosColors.Length)
                {
                    puyoPointer -= _puyosColors.Length;
                }

                // ぷよがまだ生成されていないならチェックを終了する
                if (_puyosTransforms[puyoPointer][0] == null)
                {
                    continue;
                }

                // ネクストの移動先の座標を格納している配列のポインタ
                int nextPositionsPointer = i + 1;

                // ネクストぷよが正しい座標にあるなら次の要素へ
                if (_puyosTransforms[puyoPointer][0].position == _nextPuyoPositions[nextPositionsPointer].position)
                {
                    continue;
                }

                // ネクストぷよの正しい座標にないとき、正しい座標にする
                for (int j = 0; j < _puyosColors[puyoPointer].Length; j++)
                {
                    // ぷよの親ぷよに対する相対座標を求める
                    Vector2 puyoRelativePosition =
                        _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][j] * _fieldParameter._scaleOfOneMass;

                    // そのぷよの座標を求める
                    Vector3 puyoPosition = _nextPuyoPositions[nextPositionsPointer].position + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

                    // ぷよを移動する
                    _puyosTransforms[puyoPointer][j].position = puyoPosition;
                }
            }
        }

        // ネクストのぷよを抽選する
        sbyte[] lotteryNextPuyosColors = new sbyte[] { _nextPuyoLottery.NextPuyoNumberLottery(), _nextPuyoLottery.NextPuyoNumberLottery() };

        // 一番遠いネクストにぷよを追加し、Scene上に生成する処理
        for (int i = 0; i < lotteryNextPuyosColors.Length; i++)
        {
            // 生成したぷよを色配列に追加
            _puyosColors[lastNextPuyoPointer][i] = lotteryNextPuyosColors[i];

            // ぷよの親ぷよに対する相対座標を求める
            Vector2 puyoRelativePosition =
                _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i] * _fieldParameter._scaleOfOneMass;

            // そのぷよのワールド座標を求める
            Vector3 puyoPosition = _nextPuyoPositions[_nextPuyoPositions.Length - 1].position
                + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

            // ぷよを生成し、Transform配列に格納する(Scene上に生成するぷよは色番号 - 1(色番号の空マスが０であるため)
            _puyosTransforms[lastNextPuyoPointer][i] =
                Instantiate(_fieldParameter._puyoSprits[_puyosColors[lastNextPuyoPointer][i] - 1], puyoPosition, Quaternion.identity).transform;

            // 生成したぷよの大きさを変更する
            _puyosTransforms[lastNextPuyoPointer][i].localScale = _fieldParameter._puyoLocalScales[lotteryNextPuyosColors[i]];
        }

        // ネクストぷよを生成済みフラグをセット
        _nextPuyosInstantiated = true;
    }


    /// <summary>
    /// ゲームプレイを開始するときの処理
    /// </summary>
    private void GamePlayStart()
    {
        // ネクストの数に応じて、初期ぷよを生成する
        for (int i = 0; i < _nextPuyoPositions.Length - 1; i++)
        {
            NextPuyoInstantiate();
        }

        // ぷよの操作を開始する
        _playerFlags._isNextPuyoUpdate = true;
    }
}
