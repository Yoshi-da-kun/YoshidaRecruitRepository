
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ControllerInputFunction;

/// --------------------------------------------------
/// #PlayerInputAndOperation.cs
/// 作成者:吉田雄伍
/// 
/// フィールドのオブジェクトにアタッチしてください
/// プレイヤーの入力を受け取り、ぷよを操作するスクリプトです
/// --------------------------------------------------

public class PlayerOperation : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // 各パラメータ(ScriptableObject)
    private PlayerParameter _playerParameter;
    private FieldParameter _fieldParameter;

    // サウンドをまとめたスクリプト
    private InGameSounds _inGameSounds;

    // フィールドを管理するスクリプト
    private FieldControl _fieldControl;

    // ぷよを操作するコントローラの入力処理を行うスクリプト
    private PuyoControlInput _puyoControlInput;

    // ぷよを操作可能か
    private bool _canPlayerControl = default;

    // 操作するぷよの 色, Transform, 行, 列(ぷよのデータ)
    private List<PuyoStructs.ControlPuyoData> _controlPuyoDatas = new List<PuyoStructs.ControlPuyoData>();

    // 各行と列の中心座標
    private float[] _eachRowFieldPositions = default;
    private float[] _eachColumnFieldPositions = default;

    // 各行に積んでいるぷよの高さ
    private sbyte[] _fieldHeightData = default;


    // ぷよの設置猶予時間中の計測時間
    private float _installGraceElapsedTime = default;

    // ぷよの効果音を鳴らすためのオーディオソース
    private AudioSource _puyoSeAudioSource;


    #region ぷよの操作と入力に関する変数

    // 落下のための入力フラグ
    private bool _isFallInput = default;

    // 移動中のフラグ
    private PuyoControlInput.RightLeftInputFlag _isMoving = default;

    // 横移動の移動回数をカウントする
    private int _horizontalMoveCounter = default;

    // ぷよを左右に移動するときの移動量
    private float _horizontalMoveDistance = default;

    // 落下の計測時間
    private float _fallElapsedTime = default;

    // 落下をするフラグ
    private bool _isFalling = default;

    // 一回の落下あたりの落下量
    private float _fallQuantity = default;

    // 回転中のフラグ
    private PuyoControlInput.RightLeftInputFlag _isRotation = default;

    // 現在の回転状態番号
    private int _rotationNumber = default;

    // 回転開始時の子ぷよのSin,Cosの振れ幅の値
    List<float> _childOriginAmplitude = new List<float>();

    // 回転時間を計測時間
    private float _rotationElapsedTime = default;

    #endregion


    /// <summary>
    /// Sceneを開始するときに変数の初期値を設定する処理
    /// </summary>
    private void Start()
    {
        // 各パラメータを取得
        _playerParameter = _summarizeScriptableObjects._playerParameter;
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;

        // サウンドをまとめたスクリプトを取得
        _inGameSounds = _summarizeScriptableObjects._inGameSounds;

        // フィールドを管理するスクリプトを取得
        _fieldControl = this.GetComponent<FieldControl>();

        // ぷよ操作の入力に関する処理を行うスクリプトを取得
        _puyoControlInput = this.GetComponent<PuyoControlInput>();

        // 左右の移動量を求める
        _horizontalMoveDistance = _fieldParameter._scaleOfOneMass.x / _playerParameter._horizontalMoveCounts;

        // フィールドの高さ(列)データを取得
        _fieldHeightData = _fieldControl.GetFieldHeightData();

        // ぷよの落下量を計算する
        _fallQuantity = _fieldParameter._scaleOfOneMass.y / _playerParameter._fallSmoothness;

        // フィールド内のマスごとの座標を取得
        (_eachRowFieldPositions, _eachColumnFieldPositions) = _fieldControl.GetFieldMassPos();

        // 効果音を出すためのオーディオソースを取得
        _puyoSeAudioSource = this.GetComponent<AudioSource>();
    }



    /// <summary>
    /// プレイヤーの入力判定を取得するメソッド
    /// </summary>
    private void Update()
    {
        // リスタートする処理
        if (ControllerInput.RestartInput())
        {
            // シーンを読み直す
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        // プレイヤーの操作ができない状態なら処理しない
        if (!_canPlayerControl)
        {
            return;
        }

        ///-- 各入力処理の結果を受け取る処理 --///

        // 左右移動をしていないとき、左右入力処理の結果を格納する
        if (!_isMoving.Right && !_isMoving.Left)
        {
            _isMoving = _puyoControlInput.HorizontalMoveInput(ControllerInput.HorizontalInput());
        }
        // 左右移動をしているとき、左右入力処理のみをを行う
        else
        {
            _puyoControlInput.HorizontalMoveInput(ControllerInput.HorizontalInput());
        }


        // 落下の入力がされているかを受け取る処理
        _isFallInput = _puyoControlInput.FallInput(ControllerInput.VerticalInput());


        // 回転のボタンの入力
        PuyoControlInput.RightLeftInputFlag rotationButtonInput = default;

        // 回転ボタンの入力を取得する
        rotationButtonInput.Right = ControllerInput.RightRotationInput();
        rotationButtonInput.Left = ControllerInput.LeftRotationInput();

        // 回転をしていないとき、回転入力処理の結果を格納する
        if (!_isRotation.Right && !_isRotation.Left)
        {
            _isRotation = _puyoControlInput.PuyoRotationInput(rotationButtonInput);
        }
    }


    /// <summary>
    /// ぷよを実際に動かす処理を行う
    /// </summary>
    private void FixedUpdate()
    {
        // 操作できるぷよが盤面に出ている状態か
        if (!_canPlayerControl)
        {
            return;
        }

        // ぷよを回転する
        PuyoRotation();

        // ぷよを左右移動する
        PuyoHorizontalMove();

        // ぷよが着地したかを判定する
        PuyoInstalCheck();

        // ぷよを落下する
        PuyoFall();
    }


    /// <summary>
    /// ぷよの左右移動を行う
    /// </summary>
    private void PuyoHorizontalMove()
    {
        // 移動していないときは処理しない
        if (!_isMoving.Right && !_isMoving.Left)
        {
            return;
        }

        // 移動を開始したとき
        if (_horizontalMoveCounter == 0)
        {
            // 移動量(行数)と移動方向の行の端
            sbyte compareRow = default;
            int endRowValue = default;

            // 右移動のときの移動する行数と行の右端を格納する
            if (_isMoving.Right)
            {
                compareRow = 1;
                endRowValue = _fieldParameter._fieldRowSize - 1;
            }
            // 左移動のときの移動する行数と行の左端を格納する
            else
            {
                compareRow = -1;
                endRowValue = 0;
            }

            // 移動が可能かを調べる
            for (int i = 0; i < _controlPuyoDatas.Count; i++)
            {
                // 現在地がフィールドの端ではなく、移動先の段が同じ高さ以下なら次の要素へ
                if (_controlPuyoDatas[i].Row != endRowValue && _controlPuyoDatas[i].Column >= _fieldHeightData[_controlPuyoDatas[i].Row + compareRow])
                {
                    continue;
                }

                // ぷよのどれかが移動できないなら、移動をやめる
                _isMoving.Right = false;
                _isMoving.Left = false;

                return;
            }
            
            // ぷよの位置情報を更新(1マス移動)する
            for (int i = 0; i < _controlPuyoDatas.Count; i++)
            {
                // 操作するぷよのデータ
                PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[i];

                // 操作するぷよの段を一段上げて、格納する
                controlPuyoData.Row += compareRow;
                _controlPuyoDatas[i] = controlPuyoData;
            }
        }

        // 移動する距離
        float horizontalMoveDistance = default;

        // 右移動のときの距離を格納する
        if (_isMoving.Right)
        {
            horizontalMoveDistance = _horizontalMoveDistance;
        }
        // 左移動のときの距離を格納する
        else
        {
            horizontalMoveDistance = -_horizontalMoveDistance;
        }

        // Scene上のぷよを移動する
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            _controlPuyoDatas[i].Transform.position += new Vector3(horizontalMoveDistance, 0, 0);
        }

        // 移動カウントを増やす
        _horizontalMoveCounter++;

        // 移動回数が設定回数に到達または、目的座標に到達したら移動を終了する
        if (_horizontalMoveCounter == _playerParameter._horizontalMoveCounts)
        {
            // 移動を終了
            _isMoving.Right = false;
            _isMoving.Left = false;

            // 移動カウントを初期化
            _horizontalMoveCounter = 0;
        }
    }


    /// <summary>
    /// ぷよの回転を行う
    /// </summary>
    private void PuyoRotation()
    {
        // 回転フラグがセットされていなければ処理しない
        if (!_isRotation.Right && !_isRotation.Left)
        {
            return;
        }


        #region 回転を開始するときに一度行う処理

        // 回転を始めた時に一度だけ行う処理
        if (_rotationElapsedTime == 0)
        {
            // 回転先の回転番号を格納する
            int rotationDestinationNumber = _rotationNumber;

            // 右回転のときの、回転番号と方向を設定
            if (_isRotation.Right)
            {
                // 回転先の番号を設定
                rotationDestinationNumber++;

                // 回転番号の最大値を超えたら番号を０にする
                if (rotationDestinationNumber >= _fieldParameter._puyosEachRotationDirections[0].Length)
                {
                    rotationDestinationNumber = 0;
                }
            }
            // 左回転のときの、回転番号と方向を設定
            else
            {
                // 回転先の番号を設定
                rotationDestinationNumber--;

                // 回転番号が０未満なら番号を最大値にする
                if (rotationDestinationNumber < 0)
                {
                    rotationDestinationNumber = _fieldParameter._puyosEachRotationDirections[0].Length - 1;
                }
            }

            // 回転ができるかを判定する処理
            for (int i = 1; i < _controlPuyoDatas.Count; i++)
            {
                // 回転後に移動する位置
                int destinationRow = _controlPuyoDatas[0].Row + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x;
                int destinationColumn = _controlPuyoDatas[0].Column + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].y;

                // 回転後の子ぷよの位置がフィールドの端ではなく、設置済みのぷよがないなら次の要素へ
                if (destinationRow < _fieldParameter._fieldRowSize && destinationRow >= 0 && destinationColumn >= 0
                    && destinationColumn >= _fieldHeightData[destinationRow])
                {
                    continue;
                }

                // 回転後のぷよが当たったのが右側のとき
                if (_fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x > 0)
                {
                    // 反対側にフィールドの端またはぷよがあったら回転しない
                    if (_controlPuyoDatas[0].Row - 1 < 0 || _controlPuyoDatas[0].Column < _fieldHeightData[_controlPuyoDatas[0].Row - 1])
                    {
                        _isRotation.Right = false;
                        _isRotation.Left = false;

                        return;
                    }

                    // 右移動を終了して、左移動をする
                    _isMoving.Right = false;
                    _isMoving.Left = true;

                    PuyoHorizontalMove();

                    break;
                }
                // 回転後のぷよが当たったのが左側のとき
                if (_fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x < 0)
                {
                    // 反対側にフィールドの端またはぷよがあったら回転しない
                    if (_controlPuyoDatas[0].Row + 1 >= _fieldParameter._fieldRowSize || _controlPuyoDatas[0].Column < _fieldHeightData[_controlPuyoDatas[0].Row + 1])
                    {
                        _isRotation.Right = false;
                        _isRotation.Left = false;

                        return;
                    }

                    // 左移動を終了して、右移動をする
                    _isMoving.Right = true;
                    _isMoving.Left = false;

                    PuyoHorizontalMove();

                    break;
                }
                // 回転後のぷよが当たったのが下側のとき
                if (_fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].y < 0)
                {
                    // ぷよを一段上に移動する
                    for (int j = 0; j < _controlPuyoDatas.Count; j++)
                    {
                        // 操作するぷよのデータ
                        PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[j];

                        // 操作するぷよの段を一段上げる
                        controlPuyoData.Column += 1;
                        controlPuyoData.Transform.position += new Vector3(0, _fieldParameter._scaleOfOneMass.y, 0);

                        // 更新したぷよのデータを格納する
                        _controlPuyoDatas[j] = controlPuyoData;
                    }

                    // 落下するまでの計測時間を初期化する
                    _fallElapsedTime = 0;

                    break;
                }
            }

            // ぷよを設置したときの効果音をならす
            _puyoSeAudioSource.PlayOneShot(_inGameSounds._puyoRotationSE);

            // 子ぷよを回転開始位置の値を格納し、位置データを更新する処理
            for (int i = 1; i < _controlPuyoDatas.Count; i++)
            {
                // ぷよの回転開始位置(Sin,Cosの振れ幅の値)
                float initialAmplitudeValue = 0;

                // 左回転のときのぷよの回転開始位置を設定する
                if (_isRotation.Left)
                {
                    initialAmplitudeValue += 0.5f;
                }

                // ぷよが４分の１回転しているときの振れ幅の値
                float quaterRotationAmplitudeValue = 0.5f;

                // 子ぷよが親ぷよの左右にあるとき、左右の振れ幅の値を設定
                if (_fieldParameter._puyosEachRotationDirections[_rotationNumber][i].x != 0)
                {
                    initialAmplitudeValue += quaterRotationAmplitudeValue * _fieldParameter._puyosEachRotationDirections[_rotationNumber][i].x;
                }

                // 子ぷよが親ぷよの下か横にあるなら半回転の値を足した振れ幅の値を求める
                if (_fieldParameter._puyosEachRotationDirections[_rotationNumber][i].y != 1)
                {
                    initialAmplitudeValue = quaterRotationAmplitudeValue * 2 - initialAmplitudeValue;
                }

                // 回転用の値に正規化する
                initialAmplitudeValue = initialAmplitudeValue * Mathf.PI;

                // 子ぷよの回転開始時のSin,Cosに対する振れ幅の値を格納する
                _childOriginAmplitude.Add(initialAmplitudeValue);

                // 子ぷよのデータ
                PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[i];

                // 子ぷよの位置データを更新してする
                controlPuyoData.Row = _controlPuyoDatas[0].Row + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x;
                controlPuyoData.Column = _controlPuyoDatas[0].Column + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].y;

                // 更新したぷよデータを格納する
                _controlPuyoDatas[i] = controlPuyoData;
            }

            // 回転番号を更新する
            _rotationNumber = rotationDestinationNumber;
        }

        #endregion 回転を開始するときに一度だけ行う処理


        // 回転中の時間を計測する
        _rotationElapsedTime += Time.fixedDeltaTime;

        // 回転時間が終わったときの処理
        if (_rotationElapsedTime >= _playerParameter._quarterRotationTime)
        {
            // 子ぷよを回転終了時の位置に移動する
            for (int i = 1; i <= _childOriginAmplitude.Count; i++)
            {
                // ぷよの親ぷよに対するぷよの相対座標を求める
                Vector2 puyoRelativePosition = _fieldParameter._puyosEachRotationDirections[_rotationNumber][i] * _fieldParameter._scaleOfOneMass;

                // ぷよのワールド座標を求める
                Vector3 childPuyoPosition = _controlPuyoDatas[0].Transform.position + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

                // 移動する
                _controlPuyoDatas[i].Transform.position = childPuyoPosition;
            }

            // 回転を終了し、計測時間の初期化
            _isRotation.Right = false;
            _isRotation.Left = false;
            _rotationElapsedTime = 0;

            // 振れ幅の値を初期化
            _childOriginAmplitude.Clear();

            return;
        }

        // 親ぷよを中心に子ぷよを右回転する
        if (_isRotation.Right)
        {
            // 振れ幅の値を求めて、親の座標から回転中の子ぷよの座標を求める
            for (int i = 0; i < _childOriginAmplitude.Count; i++)
            {
                // 回転に使うSin,Cosの現在の振れ幅の値を求める
                float rotationAmplitude = _playerParameter._normalizedRotationSpeed * _rotationElapsedTime + _childOriginAmplitude[i];

                _controlPuyoDatas[i + 1].Transform.position = _controlPuyoDatas[0].Transform.position + new Vector3(
                    Mathf.Sin(rotationAmplitude) * _fieldParameter._scaleOfOneMass.x, Mathf.Cos(rotationAmplitude) * _fieldParameter._scaleOfOneMass.y, 0);
            }
        }
        // 親ぷよを中心に子ぷよを左回転する
        else
        {
            // 振れ幅の値を使い、親の座標から回転中の子ぷよの座標を求める
            for (int i = 0; i < _childOriginAmplitude.Count; i++)
            {
                // 回転に使うSin,Cosの現在の振れ幅の値を求める
                float rotationAmplitude = _playerParameter._normalizedRotationSpeed * _rotationElapsedTime + _childOriginAmplitude[i];

                _controlPuyoDatas[i + 1].Transform.position = _controlPuyoDatas[0].Transform.position + new Vector3
                    (Mathf.Cos(rotationAmplitude) * _fieldParameter._scaleOfOneMass.x, Mathf.Sin(rotationAmplitude) * _fieldParameter._scaleOfOneMass.y, 0);
            }
        }
    }


    /// <summary>
    /// ぷよが落下する処理を行う
    /// </summary>
    private void PuyoFall()
    {
        // ぷよが落下中か
        if(!_isFalling)
        {
            return;
        }

        // 落下している時間を計測
        _fallElapsedTime += Time.fixedDeltaTime;

        // ぷよが落下をするかを示すフラグ
        bool isPuyoFall = false;

        // 下入力時の高速落下判定
        if (_isFallInput)
        {
            // 落下時間経過していなければ落下しない
            if (_fallElapsedTime < _playerParameter._highSpeedFallTime)
            {
                return;
            }

            // ぷよ落下フラグをセット
            isPuyoFall = true;
        }
        // 無入力時の通常落下判定
        else
        {
            // 落下時間経過していなければ落下しない
            if (_fallElapsedTime < _playerParameter._normalSpeedFallTime)
            {
                return;
            }

            // ぷよ落下フラグをセット
            isPuyoFall = true;
        }
        
        // ぷよが落下タイミングじゃないなら処理しない
        if (!isPuyoFall)
        {
            return;
        }

        ///-- ぷよを落下させる処理 --///

        // ぷよを落下値分落下させる
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            _controlPuyoDatas[i].Transform.position -= new Vector3(0, _fallQuantity, 0);
        }

        // 計測時間を初期化
        _fallElapsedTime = 0;

        // ぷよの高さが現在いる段の高さ以上ならぷよの段を更新しない
        if (_controlPuyoDatas[0].Transform.position.y >= _eachColumnFieldPositions[_controlPuyoDatas[0].Column])
        {
            return;
        }

        // フィールドの高さより低い位置にいるぷよがあるか
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            // ぷよが現在いる高さがフィールドの高さ以上なら次の要素へ
            if (_controlPuyoDatas[i].Transform.position.y > _eachColumnFieldPositions[_fieldHeightData[_controlPuyoDatas[i].Row]])
            {
                continue;
            }

            // ぷよを正しい着地位置に修正する処理
            for (int j = 0; j < _controlPuyoDatas.Count; j++)
            {
                _controlPuyoDatas[j].Transform.position = new Vector3(_eachRowFieldPositions[_controlPuyoDatas[j].Row], _eachColumnFieldPositions[_controlPuyoDatas[j].Column], 0);
            }
        
            return;
        }

        // ぷよの現在いる段を一段下げる
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            // 操作するぷよのデータ
            PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[i];

            // 操作するぷよの段を一段下げて、格納する
            controlPuyoData.Column -= 1;
            _controlPuyoDatas[i] = controlPuyoData;
        }
    }


    /// <summary>
    /// ぷよが着地したかを検知する
    /// </summary>
    private void PuyoInstalCheck()
    {
        // 設置猶予時間中かを示すフラグ
        bool inInstallGraceTime = false;

        // ぷよが着地できるかを判断する処理
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            // 操作ぷよの高さがフィールドの高さより高く、操作ぷよの座標が行の中心座標よりも上にあるなら次の要素へ
            if (_controlPuyoDatas[i].Column > _fieldHeightData[_controlPuyoDatas[i].Row] ||
                _controlPuyoDatas[i].Transform.position.y > _eachColumnFieldPositions[_fieldHeightData[_controlPuyoDatas[i].Row]])
            {
                continue;
            }

            // 着地できるときのフラグをセット
            inInstallGraceTime = true;

            // 落下を停止する
            _isFalling = false;

            break;
        }

        // ぷよが着地できるときの処理
        if (inInstallGraceTime)
        {
            _installGraceElapsedTime += Time.fixedDeltaTime;

            // 回転中、移動中なら設置しない
            if (_isMoving.Left || _isMoving.Right ||
                _isRotation.Left || _isRotation.Right)
            {
                return;
            }

            // 設置猶予時間を超えるまたは、下入力が入っていたらぷよを設置する
            if (_installGraceElapsedTime >= _playerParameter._installGraceTime || _isFallInput)
            {
                // プレイヤーの操作を終了する
                _canPlayerControl = false;

                // 回転番号を初期値にする
                _rotationNumber = 0;

                // ぷよを設置したときの処理をする
                _fieldControl.PuyoInstallProcess(_controlPuyoDatas);
            }
        }
        else
        {
            // 設置猶予時間を初期化
            _installGraceElapsedTime = 0;

            // ぷよの落下をできる状態にする
            _isFalling = true;
        }
    }


    /// <summary>
    /// プレイヤーの操作を開始するときの処理
    /// </summary>
    /// <param name="controlPuyoDatas"> 操作するぷよの 色, Transform, 行, 列 </param>
    public void PlayerControlStart(List<PuyoStructs.ControlPuyoData> controlPuyoDatas)
    {
        // 操作するぷよのデータを格納する
        _controlPuyoDatas = controlPuyoDatas;

        // ぷよの高さデータを更新する
        _fieldHeightData = _fieldControl.GetFieldHeightData();

        // ぷよの操作を開始する
        _canPlayerControl = true;

        // ぷよが落下できる状態にする
        _isFalling = true;
    }
}
