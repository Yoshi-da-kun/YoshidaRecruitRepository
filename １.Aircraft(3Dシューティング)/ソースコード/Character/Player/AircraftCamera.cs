
using UnityEngine;

/// --------------------------------------------------
/// #AircraftCamera.cs
/// 作成者:吉田雄伍
/// 
/// プレイヤーのカメラを制御するスクリプト
/// --------------------------------------------------

public class AircraftCamera : MonoBehaviour
{
    [SerializeField, Label("機体のパラメータ")]
    private AircraftParameter _aircraftParameter;

    // 機体の移動などの挙動を行うスクリプト
    private AircraftAction _aircraftAction;

    [SerializeField, Header("航空機を追うカメラ")]
    private Transform _mainCameraTransform;

    [Header("")]
    [SerializeField, Label("減速時のカメラの距離")]
    private float _minCameraDistance = 1;

    [SerializeField, Label("加速時のカメラの距離")]
    private float _maxCameraDistance = 6;

    [Header("")]
    [SerializeField, Label("機体からカメラの中心をどれくらいずらすか")]
    private Vector3 _offCenterOfCamera;

    [SerializeField, Label("旋回時にどれくらい真後ろから横にずれるか"),Range(0.01f, 2f)]
    private float _shiftsSidewaysInTurning = 1;

    // 機体のTransform
    private Transform _aircraftTransform = default;

    // 速度あたりの機体に対してカメラを引く距離
    private float _cameraDistancePerSpeed = default;


    /// <summary>
    /// 変数の初期値を格納する
    /// </summary>
    private void Start()
    {
        // 機体のTransformとScriptを取得
        _aircraftTransform = this.GetComponent<Transform>();
        _aircraftAction = this.GetComponent<AircraftAction>();

        // 機体と同じ方向をカメラの初期値とする
        _mainCameraTransform.rotation = _aircraftTransform.rotation;

        // 速度あたりのカメラの引く距離を求める
        _cameraDistancePerSpeed = (_maxCameraDistance - _minCameraDistance) / (_aircraftParameter._boostMovementSpeed - _aircraftParameter._slowMovementSpeed);
    }


    /// <summary>
    /// カメラの位置を変える
    /// </summary>
    private void LateUpdate()
    {
        // 現在速度に応じた、機体に対してカメラの引く距離を求める
        float currentCameraDistance = _aircraftAction._currentMovementSpeed * _cameraDistancePerSpeed + _minCameraDistance;

        // 傾きが水平(0)～垂直(1)としたときの、現在の傾きの度合いを求める
        float rollMagnitude = Mathf.Abs(_aircraftAction._aircraftAngleGetter.z) / _aircraftAction._maxRollAngle;

        // 航空機の後ろの方向と横の方向
        Vector3 behindDirection = new Vector3(0, 0, -1);
        Vector3 sidewayDirection = new Vector3(0, _shiftsSidewaysInTurning, 0);

        // 機体の傾きに応じたカメラを引く方向を求める
        Vector3　cameraPullDirection = (sidewayDirection * rollMagnitude) + (behindDirection);

        // 求めた距離分カメラを引く
        _mainCameraTransform.position = _aircraftTransform.position + _aircraftTransform.rotation * (cameraPullDirection * currentCameraDistance);

        // カメラの回転を行う
        _mainCameraTransform.rotation = Quaternion.Euler(_aircraftTransform.eulerAngles.x, _aircraftTransform.eulerAngles.y, 0);

        // カメラを中心から少しずらす
        _mainCameraTransform.position += _aircraftTransform.rotation * _offCenterOfCamera;

        // ずらした分のカメラの角度を修正する
        _mainCameraTransform.rotation *= Quaternion.Euler(_offCenterOfCamera.y, _offCenterOfCamera.x * 20, _offCenterOfCamera.z);
    }
}