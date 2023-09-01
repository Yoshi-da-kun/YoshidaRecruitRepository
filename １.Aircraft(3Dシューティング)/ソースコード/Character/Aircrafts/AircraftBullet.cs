
using UnityEngine;

/// --------------------------------------------------
/// #AircraftBullet.cs
/// �쐬��:�g�c�Y��
/// 
/// �@�̂̒e�̔��˂��s���X�N���v�g
/// --------------------------------------------------

public class AircraftBullet : MonoBehaviour
{
    [SerializeField, Label("�e�̃p�����[�^")]
    private BulletParameter _bulletParameter;

    [SerializeField, Label("���𗬂��X�N���v�g")]
    private SoundController _soundController;

    [SerializeField, Header("�e�̔��ˈʒu")]
    private Transform _bulletShotPoint;

    // �e��Pool�t�H���_�[�̖��O
    private const string POOL_FOLDER_NAME = "PoolFolder";

    // �e�̃I�u�W�F�N�g�v�[���̃t�H���_�[��Transform
    private Transform _bulletPoolFolder = default;


    private void Start()
    {
        // �@�e�̒e��Pool�pFolder�𐶐�����
        _bulletPoolFolder = ObjectPoolFolderGenerate(_bulletParameter._bulletPrefab.name);
    }


    /// <summary>
    /// �e��Pool�p�̃t�H���_��Scene��ɐ������A���̃t�H���_��Ԃ�
    /// </summary>
    /// <param name="bulletPrefabName"> �e��Prefab�̖��O </param>
    private Transform ObjectPoolFolderGenerate(string bulletPrefabName)
    {
        // Scene��ɒe��ObjectPool�t�H���_�����邩����������
        GameObject bulletPoolFolder = GameObject.Find(bulletPrefabName + POOL_FOLDER_NAME);
        
        // �e�̃t�H���_��������Ȃ������Ƃ��A�t�H���_���쐬����
        if (bulletPoolFolder == null)
        {
            bulletPoolFolder = new GameObject(bulletPrefabName + POOL_FOLDER_NAME);
        }

        // �t�H���_��Transform��Ԃ�
        return bulletPoolFolder.transform;
    }


    /// <summary>
    /// �e�𐶐����A���˂���
    /// </summary>
    public void AircraftMachinegunShot(Vector3 shotDirection)
    {
        // �e�𐶐�����
        GameObject bulletObject = BulletPool();

        // �e����MachinegunBulletProcess���擾
        ExplosionBulletProcess bulletInstance = bulletObject.GetComponent<ExplosionBulletProcess>();

        // �e�𔭎˂�����
        bulletInstance.ExplosionBulletShot(shotDirection, _soundController);
    }


    /// <summary>
    /// �v�[������e�̐������s��
    /// </summary>
    private GameObject BulletPool()
    {
        // �v�[�����̖���������Ă���e����������
        for (int i = 0; i < _bulletPoolFolder.childCount; i++)
        {
            // �L��������Ă���I�u�W�F�N�g�̂Ƃ��͎��̗v�f��
            if (_bulletPoolFolder.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }

            // �v�[�����̒e�𔭎ˈʒu�Ɉړ����āA�L��������
            _bulletPoolFolder.GetChild(i).position = _bulletShotPoint.position;
            _bulletPoolFolder.GetChild(i).gameObject.SetActive(true);

            // �v�[�����̒e��Ԃ�
            return _bulletPoolFolder.GetChild(i).gameObject;
        }

        // Pool���ɖ��������ꂽ�e���Ȃ��Ƃ��A�V���ɒe�𐶐�����
        return Instantiate(_bulletParameter._bulletPrefab, _bulletShotPoint.position, Quaternion.identity, _bulletPoolFolder);
    }
}
