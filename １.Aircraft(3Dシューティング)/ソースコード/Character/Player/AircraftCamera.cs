
using UnityEngine;

/// --------------------------------------------------
/// #AircraftCamera.cs
/// �쐬��:�g�c�Y��
/// 
/// �v���C���[�̃J�����𐧌䂷��X�N���v�g
/// --------------------------------------------------

public class AircraftCamera : MonoBehaviour
{
    [SerializeField, Label("�@�̂̃p�����[�^")]
    private AircraftParameter _aircraftParameter;

    // �@�̂̈ړ��Ȃǂ̋������s���X�N���v�g
    private AircraftAction _aircraftAction;

    [SerializeField, Header("�q��@��ǂ��J����")]
    private Transform _mainCameraTransform;

    [Header("")]
    [SerializeField, Label("�������̃J�����̋���")]
    private float _minCameraDistance = 1;

    [SerializeField, Label("�������̃J�����̋���")]
    private float _maxCameraDistance = 6;

    [Header("")]
    [SerializeField, Label("�@�̂���J�����̒��S���ǂꂭ�炢���炷��")]
    private Vector3 _offCenterOfCamera;

    [SerializeField, Label("���񎞂ɂǂꂭ�炢�^��납�牡�ɂ���邩"),Range(0.01f, 2f)]
    private float _shiftsSidewaysInTurning = 1;

    // �@�̂�Transform
    private Transform _aircraftTransform = default;

    // ���x������̋@�̂ɑ΂��ăJ��������������
    private float _cameraDistancePerSpeed = default;


    /// <summary>
    /// �ϐ��̏����l���i�[����
    /// </summary>
    private void Start()
    {
        // �@�̂�Transform��Script���擾
        _aircraftTransform = this.GetComponent<Transform>();
        _aircraftAction = this.GetComponent<AircraftAction>();

        // �@�̂Ɠ����������J�����̏����l�Ƃ���
        _mainCameraTransform.rotation = _aircraftTransform.rotation;

        // ���x������̃J�����̈������������߂�
        _cameraDistancePerSpeed = (_maxCameraDistance - _minCameraDistance) / (_aircraftParameter._boostMovementSpeed - _aircraftParameter._slowMovementSpeed);
    }


    /// <summary>
    /// �J�����̈ʒu��ς���
    /// </summary>
    private void LateUpdate()
    {
        // ���ݑ��x�ɉ������A�@�̂ɑ΂��ăJ�����̈������������߂�
        float currentCameraDistance = _aircraftAction._currentMovementSpeed * _cameraDistancePerSpeed + _minCameraDistance;

        // �X��������(0)�`����(1)�Ƃ����Ƃ��́A���݂̌X���̓x���������߂�
        float rollMagnitude = Mathf.Abs(_aircraftAction._aircraftAngleGetter.z) / _aircraftAction._maxRollAngle;

        // �q��@�̌��̕����Ɖ��̕���
        Vector3 behindDirection = new Vector3(0, 0, -1);
        Vector3 sidewayDirection = new Vector3(0, _shiftsSidewaysInTurning, 0);

        // �@�̂̌X���ɉ������J�������������������߂�
        Vector3�@cameraPullDirection = (sidewayDirection * rollMagnitude) + (behindDirection);

        // ���߂��������J����������
        _mainCameraTransform.position = _aircraftTransform.position + _aircraftTransform.rotation * (cameraPullDirection * currentCameraDistance);

        // �J�����̉�]���s��
        _mainCameraTransform.rotation = Quaternion.Euler(_aircraftTransform.eulerAngles.x, _aircraftTransform.eulerAngles.y, 0);

        // �J�����𒆐S���班�����炷
        _mainCameraTransform.position += _aircraftTransform.rotation * _offCenterOfCamera;

        // ���炵�����̃J�����̊p�x���C������
        _mainCameraTransform.rotation *= Quaternion.Euler(_offCenterOfCamera.y, _offCenterOfCamera.x * 20, _offCenterOfCamera.z);
    }
}