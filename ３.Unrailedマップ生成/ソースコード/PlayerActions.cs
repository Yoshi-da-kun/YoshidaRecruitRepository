
using UnityEngine;

/// --------------------------------------------------------
/// #PlayerActions.cs
/// 
/// プレイヤーの動作を行うスクリプト
/// --------------------------------------------------------

public class PlayerActions : MonoBehaviour
{
    [SerializeField, Label("プレイヤーのパラメータ")]
    private PlayerParameter _playerParameter;

    [SerializeField, Label("フィールドのパラメータ")]
    private FieldParameter _fieldParameter;

    // このキャラ(プレイヤー)のTransformとRigidbody
    private Transform _playerTransform = default;
    private Rigidbody2D _playerRigidbody = default;

    // プレイヤーの当たり判定の半径
    private float _playerRadius = default;

    // マップ上の移動上限と下限の座標
    private float _upperLimitOfMovement = default, _underLimitOfMovement = default;

    // プレイヤーの向いている方向
    private Vector2 _playerFacingDirection = default;

    // ものを壊す用のRayにHitしたCollider
    private Collider2D _breakingCollider = default;

    // ものを壊す間隔の計測時間
    private float _breakIntervalElapsedTime;


    // 移動用の入力を格納する変数
    private float horizontalInput = default;
    private float verticalInput = default;

    // 壊せるオブジェクトのタグ
    private const string BREAKABLE_OBJECTS_TAG = "BreakableObject";


    private void Start()
    {
        // キャラクターのTransformとRigidbodyを取得
        _playerTransform = this.GetComponent<Transform>();
        _playerRigidbody = this.GetComponent<Rigidbody2D>();

        // キャラクターの半径を取得
        _playerRadius = this.GetComponent<CircleCollider2D>().radius * _playerTransform.localScale.z;

        // マップの上端と下端の座標を取得
        float topEdgeOfField = _fieldParameter._fieldGenerateStartPosition.y + _fieldParameter._oneBlockSize.y * _fieldParameter._fieldColumnSize;
        float bottomEdgeOfField = _fieldParameter._fieldGenerateStartPosition.y;

        // キャラクターの移動上限と下限を求める
        _upperLimitOfMovement = topEdgeOfField - _playerRadius;
        _underLimitOfMovement = bottomEdgeOfField + _playerRadius;
    }


    private void Update()
    {
        // 入力を受け取る処理
        RecieveInput();

        // 破壊のアクションを行う
        PlayerBreakAction();
    }


    /// <summary>
    /// コントローラとキーボードの入力を受け取る
    /// </summary>
    private void RecieveInput()
    {
        // 移動の入力を受け取る
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }



    private void FixedUpdate()
    {
        // プレイヤーの移動を行う
        PlayerMoveProcess();


    }


    /// <summary>
    /// プレイヤーの移動を行う処理
    /// </summary>
    private void PlayerMoveProcess()
    {
        // 移動用の入力されていなければ処理しない
        if (horizontalInput == 0 && verticalInput == 0)
        {
            return;
        }

        // プレイヤーの移動予定場所
        Vector3 playerMoveDestinationPos = _playerTransform.position + new Vector3(horizontalInput, verticalInput, 0) * _playerParameter._moveSpeed;

        // プレイヤーの向いている方向を格納する
        _playerFacingDirection = new Vector2(horizontalInput, verticalInput);

        // プレイヤーがフィールドの下の端を超えたとき、移動先を下の端にする
        if (playerMoveDestinationPos.y < _underLimitOfMovement)
        {
            playerMoveDestinationPos.y = _underLimitOfMovement;
        }
        // プレイヤーがフィールドの上の端を超えたとき、移動先を上の端にする
        else if (playerMoveDestinationPos.y > _upperLimitOfMovement)
        {
            playerMoveDestinationPos.y = _upperLimitOfMovement;
        }

        // プレイヤーの移動を行う
        _playerRigidbody.MovePosition(playerMoveDestinationPos);
    }


    /// <summary>
    /// プレイヤーの破壊アクションを行う処理
    /// </summary>
    private void PlayerBreakAction()
    {
        // rayの可視化(デバッグ)を行う
        DebugPlayerActionRay();


        // Rayの発射位置を求める
        Vector2 rayStartPos = _playerFacingDirection * _playerRadius;
        rayStartPos += new Vector2(_playerTransform.position.x, _playerTransform.position.y);

        // プレイヤーを中心に向いている方向にRayを出す
        RaycastHit2D facingDirectionRayHit = Physics2D.Raycast(rayStartPos, _playerFacingDirection, _playerParameter._breakingDistance);

        // Rayが当たっていないとき、処理を終了する
        if (!facingDirectionRayHit)
        {
            return;
        }

        // Rayが壊せるオブジェクトにあたったときの処理
        if (facingDirectionRayHit.collider.CompareTag(BREAKABLE_OBJECTS_TAG))
        {
            // 破壊対象のオブジェクトが変わったとき
            if (_breakingCollider != facingDirectionRayHit.collider)
            {
                // 破壊対象のコライダーを格納する
                _breakingCollider = facingDirectionRayHit.collider;

                // 破壊間隔の計測時間を初期化
                _breakIntervalElapsedTime = 0;

                return;
            }
            // 破壊対象のオブジェクトが変わらないとき
            else
            {            
                // 破壊間隔時間の計測
                _breakIntervalElapsedTime += Time.deltaTime;
            }

            // 破壊間隔時間が経過していなければ処理を終了する
            if (_breakIntervalElapsedTime < _playerParameter._breakIntervalTime)
            {
                return;
            }

            // オブジェクトのHPを格納するスクリプト取得
            BreakableObjects breakableObjects = facingDirectionRayHit.collider.GetComponent<BreakableObjects>();

            // オブジェクトのHPを減少させる
            breakableObjects.DamagedProcess(_playerParameter._attackVolume);
        }
        else
        {
            // 破壊中のオブジェクトを空にする
            _breakingCollider = null;
        }
    }


    /// <summary>
    /// プレイヤーから出るRayを可視化する処理(デバッグ)
    /// </summary>
    private void DebugPlayerActionRay()
    {
        // Rayの発射位置を求める
        Vector2 rayStartPos = _playerFacingDirection * _playerRadius;
        rayStartPos += new Vector2(_playerTransform.position.x, _playerTransform.position.y);

        // 修正する倍率を求めるための除数
        Vector2 correctionDivisor = _playerFacingDirection;

        if (_playerFacingDirection.x < 0)
        {
            correctionDivisor.x = -_playerFacingDirection.x;
        }
        if (_playerFacingDirection.y < 0)
        {
            correctionDivisor.y = -_playerFacingDirection.y;
        }
        // プレイヤーの向いている方向の値の合計が１になるように修正する倍率
        float directionCorrectionMultiplier = 1 / (correctionDivisor.x + correctionDivisor.y);

        // 向いている方向の値を修正する
        _playerFacingDirection = new Vector2(_playerFacingDirection.x, _playerFacingDirection.y) * directionCorrectionMultiplier;

        Debug.DrawRay(rayStartPos, _playerFacingDirection * 3, Color.red);
    }
}
