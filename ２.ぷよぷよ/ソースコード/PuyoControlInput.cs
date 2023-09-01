
using UnityEngine;

/// --------------------------------------------------
/// #PuyoControlInput.cs
/// 作成者:吉田雄伍
/// 
/// ぷよを操作するコントローラの入力処理を行うスクリプト
/// --------------------------------------------------

public class PuyoControlInput : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _scriptableObjects;

    private PlayerParameter _playerParameter;

    void Start()
    {
        _playerParameter = _scriptableObjects._playerParameter;
    }


    /// <summary>
    /// 移動をするために必要な変数
    /// </summary>
    public struct HorizontalMoveVariables
    {
        public bool isMoving;
        public bool isInput;
        public bool isHighSpeed;
        public float inputElapsedTime;
    }

    // 左右移動それぞれの移動のための変数
    private HorizontalMoveVariables _leftMoveVariables;
    private HorizontalMoveVariables _rightMoveVariables;

    // 左右回転の入力を示すフラグ
    public struct RightLeftInputFlag
    {
        public bool Right;
        public bool Left;
    }

    // 回転の入力がされているかのフラグ
    public RightLeftInputFlag _isRotationButtonInput;

    /// <summary>
    /// 回転用に関する入力を返す
    /// </summary>
    public RightLeftInputFlag PuyoRotationInput(RightLeftInputFlag currentRotationButtonInput)
    {
        ///-- 回転ボタンの入力に応じて各フラグをセットする --///

        // 左右回転の入力を示すフラグ
        RightLeftInputFlag isRotation = default;

        // 右回転入力を押下したとき、フラグをセットする
        if (currentRotationButtonInput.Right && !_isRotationButtonInput.Right)
        {
            isRotation.Right = true;
        }

        // 左回転入力を押下したとき、フラグをセットする
        if (currentRotationButtonInput.Left && !_isRotationButtonInput.Left)
        {
            isRotation.Left = true;
        }

        // 現在の左右回転のフラグを格納する
        _isRotationButtonInput.Right = currentRotationButtonInput.Right;
        _isRotationButtonInput.Left = currentRotationButtonInput.Left;

        return isRotation;
    }


    /// <summary>
    /// 左右移動に関する入力を返す
    /// </summary>
    /// <param name="horizontalInput"> 左右入力 </param>
    public RightLeftInputFlag HorizontalMoveInput(float horizontalInput)
    {
        RightLeftInputFlag isMoveInput = default;

        // 移動フラグを初期化
        _rightMoveVariables.isMoving = false;
        _leftMoveVariables.isMoving = false;

        // 右入力がされているか
        if (horizontalInput >= _playerParameter._horizontalDeadZone)
        {
            // 右移動入力用の処理を行う
            _rightMoveVariables = MoveInput(horizontalInput, _rightMoveVariables);
        }
        // 入力されていないときのフラグをセット
        else
        {
            _rightMoveVariables.isInput = false;
        }
        // 左入力がされているか
        if (horizontalInput <= -_playerParameter._horizontalDeadZone)
        {
            // 左移動入力用の処理を行う
            _leftMoveVariables = MoveInput(horizontalInput, _leftMoveVariables);
        }
        // 入力されていないときのフラグをセット
        else
        {
            _leftMoveVariables.isInput = false;
        }

        // 右移動入力を格納する
        isMoveInput.Right = _rightMoveVariables.isMoving;

        // 左移動入力を格納する
        isMoveInput.Left = _leftMoveVariables.isMoving;

        return isMoveInput;
    }


    /// <summary>
    /// 左右移動をするかを判定し、結果を返す
    /// </summary>
    /// <param name="horizontalInput"> 左右移動の入力値 </param>
    /// <param name="moveVariables"> 左右移動を行うための変数 </param>
    private HorizontalMoveVariables MoveInput(float horizontalInput, HorizontalMoveVariables moveVariables)
    {
        // 前フレームで入力されているか
        if (moveVariables.isInput)
        {
            // 入力し続けている時間を計測する
            moveVariables.inputElapsedTime += Time.deltaTime;
        }
        else
        {
            // 計測時間を初期化し、移動するフラグをセット
            moveVariables.inputElapsedTime = 0;
            moveVariables.isMoving = true;

            // 高速移動状態を解除
            moveVariables.isHighSpeed = false;
        }

        // 高速移動状態にはいっているとき
        if (moveVariables.isHighSpeed)
        {
            // 移動硬直時間を超えたら
            if (moveVariables.inputElapsedTime >= _playerParameter._moveStoppingTime)
            {
                // 計測時間を初期化し、移動するフラグをセット
                moveVariables.inputElapsedTime = 0;
                moveVariables.isMoving = true;
            }
        }
        // 入力し続けている時間が高速移動入力時間を超えたら
        if (moveVariables.inputElapsedTime >= _playerParameter._intoHighSpeedTime)
        {
            // 計測時間を初期化し、移動するフラグをセット
            moveVariables.inputElapsedTime = 0;
            moveVariables.isMoving = true;

            // 高速移動フラグをセット
            moveVariables.isHighSpeed = true;
        }

        // 入力フラグをセット
        moveVariables.isInput = true;

        return moveVariables;
    }


    /// <summary>
    /// 下入力がされているかを返す
    /// </summary>
    /// <param name="verticalInput"> 上下の入力値 </param>
    public bool FallInput(float verticalInput)
    {
        // 下入力に応じて下入力フラグをセットする
        if (verticalInput <= -_playerParameter._verticalDeadZone)
        {
            return true;
        }
        // 下入力されていないとき
        else
        {
            return false;
        }
    }
}
