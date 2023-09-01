
using UnityEngine;
using CollisionSystem;

/// --------------------------------------------------
/// #AircraftAction.cs
/// 作成者:吉田雄伍
/// 
/// 機体の移動や旋回の挙動を行う関数をまとめたスクリプト
/// --------------------------------------------------

public class AircraftAction : MonoBehaviour
{
    [SerializeField, Label("機体のパラメータ")]
    private AircraftParameter _aircraftParameter = default;

    // 機体のTransformとコライダー
    private Transform _aircraftTransform = default;
    private OriginalCollider _aircraftCollider = default;

    // 機体の正面方向のベクトル
    private Vector3Int AIRCRAFT_FRONT_DIRECTION { get { return new Vector3Int(0, 0, 1); } }

    // 機体の最大ロール角度
    public int _maxRollAngle { get { return 90; } }
    public int _maxPitchAngle { get { return 70; } }

    // 現在の移動速度
    public float _currentMovementSpeed { get; private set; } = default;

    // 現在の機体の姿勢(角度)
    private Vector3 _currentAircraftAngle = default;
    public Vector3 _aircraftAngleGetter { get { return _currentAircraftAngle; } }

    // 機体のロールとピッチの基準となる角度
    private float _baseRoll = default;


    private void Start()
    {
        // 機体のTransformとコライダーを取得
        _aircraftTransform = this.GetComponent<Transform>();
        _aircraftCollider = this.GetComponent<OriginalCollider>();

        // 機体の初期姿勢を格納
        _currentAircraftAngle = _aircraftTransform.eulerAngles;
        _currentAircraftAngle.x = 0;
        _baseRoll = _aircraftTransform.eulerAngles.z;

        // 機体の初期速度を格納する
        _currentMovementSpeed = _aircraftParameter._initialMovementSpeed;
    }


    /// <summary>                                                   
    /// 機体の移動と旋回をするための処理
    /// </summary>
    public void AircraftMoving(float targetMovementSpeed, Vector2 turningInputVolume)
    {
        // 入力値の合計値を求める
        float sumInputVolume = Mathf.Abs(turningInputVolume.x) + Mathf.Abs(turningInputVolume.y);

        // 入力値の合計が1以上または-1未満にならないように入力値を修正する
        if (sumInputVolume > 1)
        {
            // 合計値が１になるように割合を求めて入力値に格納する
            turningInputVolume = turningInputVolume / sumInputVolume;
        }

        // 機体の旋回を行う処理
        AircraftAngleControl(turningInputVolume);

        // 機体の移動を行う処理
        AircraftMovementControl(targetMovementSpeed);
    }


    /// <summary>
    /// 機体の旋回に関する処理
    /// </summary>
    private void AircraftAngleControl(Vector2 turningInputVolume)
    {
        ///-- ロールを制御する処理 --///
        
        // 入力値に応じた最大ロール角度を求める
        float inputMaxRollAngle = _baseRoll - turningInputVolume.x * _maxRollAngle;

        // 現在ロールが入力値のロールより大きいとき
        if (_currentAircraftAngle.z > inputMaxRollAngle)
        {
            // 現在ロール角を減らす
            _currentAircraftAngle.z -= _aircraftParameter._rollingSpeed;

            // 現在ロールが入力値のロールを超えないようにする処理
            if (_currentAircraftAngle.z < inputMaxRollAngle)
            {
                _currentAircraftAngle.z = inputMaxRollAngle;
            }
        }
        // 現在ロールが入力値のロールより小さいとき
        else
        {
            // 現在ロールを増やす
            _currentAircraftAngle.z += _aircraftParameter._rollingSpeed;

            // 現在ロールが入力値のロールを超えないようにする処理
            if (_currentAircraftAngle.z > inputMaxRollAngle)
            {
                _currentAircraftAngle.z = inputMaxRollAngle;
            }
        }


        ///-- ピッチとヨーを求めるのに使う値の計算 --///

        // 入力に対する旋回量
        Vector2 turningVolume = turningInputVolume * _aircraftParameter._turningSpeed;

        // 直角の角度
        sbyte ninetyDegrees = 90;

        // 機体の姿勢が水平なときを0, 垂直な時を-1～1として、傾き(ロール)の度合いを示す
        float rollMagnitude = _currentAircraftAngle.z / ninetyDegrees;
        float rollRemainderMagnitude = 1 - Mathf.Abs(rollMagnitude);

        // ロールの度合いが負だったときにもう一つのロールの度合い(rollRemainderMagnitude)も負にする
        if (rollMagnitude < 0)
        {
            rollRemainderMagnitude = -rollRemainderMagnitude;
        }


        ///-- 縦軸入力(上下移動)のピッチを制御する --///

        // 縦軸の入力に応じたピッチの回転量分を加算する
        _currentAircraftAngle.y += rollMagnitude * -turningVolume.y;
        _currentAircraftAngle.x -= Mathf.Abs(rollRemainderMagnitude) * turningVolume.y;

        // 現在ピッチが最大ピッチを超えないようにする処理
        if (_currentAircraftAngle.x > _maxPitchAngle)
        {
            _currentAircraftAngle.x = _maxPitchAngle;
        }
        // 現在ピッチが最小ピッチを未満にならないようにする処理
        else if (_currentAircraftAngle.x < -_maxPitchAngle)
        {
            _currentAircraftAngle.x = -_maxPitchAngle;
        }


        ///-- 横軸入力(左右移動)のピッチとヨーを制御する処理 --///

        // ロールの傾きに応じた最大倍率
        float a = (2 - Mathf.Abs(rollMagnitude - rollRemainderMagnitude));

        // ロールの傾きに応じた、ピッチとヨーの回転量を求める。
        float horizontalPitchVolume = rollMagnitude * turningVolume.x * a;
        float horizontalYawVolume = rollRemainderMagnitude * turningVolume.x * a;

        // 横軸の入力に応じたピッチを求める
        _currentAircraftAngle.y += rollMagnitude * horizontalPitchVolume;
        _currentAircraftAngle.x -= Mathf.Abs(rollRemainderMagnitude) * Mathf.Abs(horizontalPitchVolume);

        // 横軸の入力に応じたヨーを求める
        _currentAircraftAngle.y += rollRemainderMagnitude * horizontalYawVolume;
        _currentAircraftAngle.x += Mathf.Abs(rollMagnitude) * Mathf.Abs(horizontalYawVolume);


        // 求めたロール、ピッチ、ヨーの回転を行う
        _aircraftTransform.rotation = Quaternion.Euler(_currentAircraftAngle);
    }


    /// <summary>
    /// 機体の移動に関する処理
    /// </summary>
    private void AircraftMovementControl(float targetMovementSpeed)
    {
        ///-- 加減速度を用いて、現在移動速度を目標速度に近づける処理 --///

        // 現在速度が目標速度より大きいときの処理
        if (_currentMovementSpeed > targetMovementSpeed)
        {
            // 現在速度から減速度を引く
            _currentMovementSpeed -= _aircraftParameter._movementDeceleration;

            // 現在速度が目標速度より小さくなったら、目標速度にする
            if (_currentMovementSpeed < targetMovementSpeed)
            {
                _currentMovementSpeed = targetMovementSpeed;
            }
        }
        // 現在速度が目標速度より小さいときの処理
        else if (_currentMovementSpeed < targetMovementSpeed)
        {
            // 現在速度から加速度を足す
            _currentMovementSpeed += _aircraftParameter._movementDeceleration;

            // 現在速度が目標速度を超えたら、目標速度にする
            if (_currentMovementSpeed > targetMovementSpeed)
            {
                _currentMovementSpeed = targetMovementSpeed;
            }
        }

        // 機体の移動量を求める処理
        Vector3 movementVolume = transform.rotation * AIRCRAFT_FRONT_DIRECTION;

        // 機体の移動を行う
        CollisionProcessing.PhysicsCollision(_aircraftCollider, _aircraftTransform, movementVolume * _currentMovementSpeed);
    }                      
}
