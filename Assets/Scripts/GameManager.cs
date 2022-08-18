using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SingleTon<GameManager>
{
    public delegate void CoinChange();
    public CoinChange coinChanged;
    private MapCreator mapCreator = null;
    public MapCreator GetCreator { get { return mapCreator; } }
    private GameObject PlayerCharacter = null;
    private Player player = null;
    public Player MyPlayer { get { return player; } }
    private Vector3 PlayerStartPos;
    public Vector3 GetStartPos { get { return PlayerStartPos; } }

    private CameraControl cam = null;

    private RaycastHit hit;
    private Vector3 desiredPos;
    private GameObject hitObj;
    private Coroutine FallCoroutine;

    private int CoinCount = 0;
    private List<GameObject> CoinPosition = new List<GameObject>();
    public List<GameObject> GetCoinList { get { return CoinPosition; } }
    public int TotalCoinCount { get { return CoinCount; } set { CoinCount = value; } }
    public Coroutine GetFall { get { return FallCoroutine; } set { FallCoroutine = value; } }

    private GameObject SmokeEffect = null;
    public GameObject smokeEffect { get { return SmokeEffect; } }

    private GameObject RockBlock = null;
    public GameObject GetRock { get { return RockBlock; } }

    private GameObject Coin = null;
    public GameObject GetCoin { get { return Coin; } }


    public override void Awake()
    {
        base.Awake();
        mapCreator = GameObject.FindObjectOfType<MapCreator>().GetComponent<MapCreator>();
        PlayerCharacter = Resources.Load<GameObject>("Player");

        cam = GameObject.FindObjectOfType<CameraControl>().GetComponent<CameraControl>();
        SmokeEffect = Resources.Load<GameObject>("Smoke");
        RockBlock = Resources.Load<GameObject>("Rock");
        Coin = Resources.Load<GameObject>("Coin");
    }
    private void Start()
    {
        mapCreator.Create();
        PlayerStartPos = new Vector3(mapCreator.MaxRow / 2, 0f, mapCreator.MaxCol / 2);
        SpawnPlayer();
        cam.FindPlayer();
        UIManager.Instance.InitUIManager();
        UIManager.Instance.UpdateBar();
    }
    private void Update()
    {
        InputManager.Instance.KeyUpdate();
    }
    private void SpawnPlayer()
    {
        player = Instantiate(PlayerCharacter, PlayerStartPos, Quaternion.identity).GetComponent<Player>();
    }
    public IEnumerator Fall(Vector3 myPos)
    {
        if (Physics.Raycast(myPos, Vector3.up, out hit, 0.5f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Rock")
            {
                desiredPos = myPos;
                hitObj = hit.transform.gameObject;
            }
        }
        while (hitObj != null)
        {
            desiredPos.y = Mathf.Lerp(hitObj.transform.position.y, hitObj.transform.position.y - 1f, 0.5f);
            hitObj.transform.position = desiredPos;
            if (desiredPos.y == hitObj.transform.position.y)
            {
                StopCoroutine(FallCoroutine);
            }
            yield return null;
        }
    }
    public void FoundCoin(GameObject coin)
    {
        CoinCount--;
        if (CoinPosition.Contains(coin))
        {
            CoinPosition.Remove(coin);
        }
        else Debug.LogError("Tried Destroy Null Coin : Coin List");
        coinChanged();
        if (CoinCount <= 0)
        {
            LoadEndScene();
        }
    }
    public void LoadEndScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
