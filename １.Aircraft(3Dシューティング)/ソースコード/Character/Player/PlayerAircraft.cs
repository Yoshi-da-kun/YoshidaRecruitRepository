
using UnityEngine;
using ControllerInput;

/// --------------------------------------------------
/// #PlayerAircraft.cs
/// 作成者:吉田雄伍
/// 
/// プレイヤーの機体を制御するスクリプト
/// --------------------------------------------------

public class PlayerAircraft : CharacterBase
{
    [SerializeField, Label("機体のパラメータ")]
    private AircraftParameter _aircraftParameter = default;

    [SerializeField, Label("プレイヤーのパラメータ")]
    private PlayerParameter _playerParameter = default;

    // 各種スクリプト
    private AircraftAction _aircraftAction = default;

    private AircraftBullet _aircraftBullet = default;

    private WarpSkill _warpSkill = default;


    // 機体の正面方向のベクトル
    private Vector3Int AIRCRAFT_FRONT_DIRECTION { get { return new Vector3Int(0, 0, 1); } }

    // 機体のTransform
    private Transform _aircraftTransform = default;

    // 旋回用の入力値
    private Vector2 _turningInputVolume = default;

    // 速度を調整する入力の値
    private float _speedControlInput = default;


    private void Start()
    {
        // 航空機関連のスクリプトを取得
        _aircraftAction = this.GetComponent<AircraftAction>();
        _aircraftBullet = this.GetComponent<AircraftBullet>();

        // プレイヤー関連のスクリプトを取得
        _warpSkill = this.GetComponent<WarpSkill>();

        // 機体のTransformを取得
        _aircraftTransform = this.GetComponent<Transform>();
    }


    /// <summary>
    /// プレイヤーの入力や状態ごとの処理を行う
    /// </summary>
    private void Update()
    {
        // 旋回量を格納する
        _turningInputVolume = PlayerInput.TurningInput();

        // 速度調整入力の値を取得する
        _speedControlInput = PlayerInput.SpeedControlInput();

        // 弾発射の入力を取得する
        if (PlayerInput.BulletShotInput())
        {
            // 機体の移動方向を求める処理
            Vector3 movementDirection = transform.rotation * AIRCRAFT_FRONT_DIRECTION;

            // 弾を発射する
            _aircraftBullet.AircraftMachinegunShot(movementDirection);
        }

        // 入力に応じてワープスキルの設置を行う
        if (PlayerInput.TimeWarpBombInput())
        {
            _warpSkill.WarpSkillStart();
        }

        // 体力がなくなった時キャラクターを消す
        if (_isDead)
        {
            // エフェクトを出す
            Instantiate(_playerParameter._breakedEffectPrefab, this.transform.position, Quaternion.identity);
            
            this.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 機体の移動や挙動に関する処理を行う
    /// </summary>
    private void FixedUpdate()
    {
        // ワープ中のときの処理
        if (_warpSkill._isWarping)
        {
            _warpSkill.WarpingProcessing(_aircraftTransform);

            return;
        }

        // ワープスキル発動中の処理
        if (_warpSkill._inWarpSkill)
        {
            _warpSkill.WarpSkillProcessing(_aircraftTransform.position, _aircraftTransform.rotation);
        }

        // 移動に関する処理を行う
        MoveProcess();
    }


    /// <summary>
    /// 移動に関する処理を行う
    /// </summary>
    private void MoveProcess()
    {
        // 現在の入力に応じた移動速度の最大値
        float _currentMaxMovementSpeed = default;

        // ブースト時の機体最大速度を格納する
        if (_speedControlInput > 0)
        {
            _currentMaxMovementSpeed = _aircraftParameter._boostMovementSpeed;
        }
        // 減速時の機体の最大速度を格納する
        else if (_speedControlInput < 0)
        {
            _currentMaxMovementSpeed = _aircraftParameter._slowMovementSpeed;
        }
        // 現在の機体の速度を格納する
        else
        {
            _currentMaxMovementSpeed = _aircraftAction._currentMovementSpeed;
        }

        // 機体の移動と旋回を行う
        _aircraftAction.AircraftMoving(_currentMaxMovementSpeed, _turningInputVolume);
    }
}