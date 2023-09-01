
using UnityEngine;

/// --------------------------------------------------
/// #AircraftBullet.cs
/// 作成者:吉田雄伍
/// 
/// 機体の弾の発射を行うスクリプト
/// --------------------------------------------------

public class AircraftBullet : MonoBehaviour
{
    [SerializeField, Label("弾のパラメータ")]
    private BulletParameter _bulletParameter;

    [SerializeField, Label("音を流すスクリプト")]
    private SoundController _soundController;

    [SerializeField, Header("弾の発射位置")]
    private Transform _bulletShotPoint;

    // 弾のPoolフォルダーの名前
    private const string POOL_FOLDER_NAME = "PoolFolder";

    // 弾のオブジェクトプールのフォルダーのTransform
    private Transform _bulletPoolFolder = default;


    private void Start()
    {
        // 機銃の弾のPool用Folderを生成する
        _bulletPoolFolder = ObjectPoolFolderGenerate(_bulletParameter._bulletPrefab.name);
    }


    /// <summary>
    /// 弾のPool用のフォルダをScene上に生成し、そのフォルダを返す
    /// </summary>
    /// <param name="bulletPrefabName"> 弾のPrefabの名前 </param>
    private Transform ObjectPoolFolderGenerate(string bulletPrefabName)
    {
        // Scene上に弾のObjectPoolフォルダがあるかを検索する
        GameObject bulletPoolFolder = GameObject.Find(bulletPrefabName + POOL_FOLDER_NAME);
        
        // 弾のフォルダが見つからなかったとき、フォルダを作成する
        if (bulletPoolFolder == null)
        {
            bulletPoolFolder = new GameObject(bulletPrefabName + POOL_FOLDER_NAME);
        }

        // フォルダのTransformを返す
        return bulletPoolFolder.transform;
    }


    /// <summary>
    /// 弾を生成し、発射する
    /// </summary>
    public void AircraftMachinegunShot(Vector3 shotDirection)
    {
        // 弾を生成する
        GameObject bulletObject = BulletPool();

        // 弾からMachinegunBulletProcessを取得
        ExplosionBulletProcess bulletInstance = bulletObject.GetComponent<ExplosionBulletProcess>();

        // 弾を発射をする
        bulletInstance.ExplosionBulletShot(shotDirection, _soundController);
    }


    /// <summary>
    /// プールから弾の生成を行う
    /// </summary>
    private GameObject BulletPool()
    {
        // プール内の無効化されている弾を検索する
        for (int i = 0; i < _bulletPoolFolder.childCount; i++)
        {
            // 有効化されているオブジェクトのときは次の要素へ
            if (_bulletPoolFolder.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }

            // プール内の弾を発射位置に移動して、有効化する
            _bulletPoolFolder.GetChild(i).position = _bulletShotPoint.position;
            _bulletPoolFolder.GetChild(i).gameObject.SetActive(true);

            // プール内の弾を返す
            return _bulletPoolFolder.GetChild(i).gameObject;
        }

        // Pool内に無効化された弾がないとき、新たに弾を生成する
        return Instantiate(_bulletParameter._bulletPrefab, _bulletShotPoint.position, Quaternion.identity, _bulletPoolFolder);
    }
}
