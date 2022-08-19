using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SingleTon<GameManager>
{
    public delegate void CoinChange();
    public CoinChange coinChanged = null;
    private MapCreator mapCreator = null;
    private UIManager ui = null;
    //private InputManager input;

    //public InputManager GetInput { get { return input; } }
    private GameObject PlayerCharacter = null;
    [SerializeField]
    private Player player = null;

    private Vector3 PlayerStartPos;
    private CameraControl cam = null;

    private int CoinCount = 0;
    public bool isAdr = false;
    private List<GameObject> CoinPosition = new List<GameObject>();

    public Player MyPlayer { get { return player; } set { player = value; } }
    public Vector3 GetStartPos { get { return PlayerStartPos; } }
    public MapCreator GetCreator { get { return mapCreator; } }
    public List<GameObject> GetCoinList { get { return CoinPosition; } set { CoinPosition = value; } }
    public int TotalCoinCount { get { return CoinCount; } set { CoinCount = value; } }

    private GameObject SmokeEffect = null;
    public GameObject smokeEffect { get { return SmokeEffect; } }

    private GameObject RockBlock = null;
    public GameObject GetRock { get { return RockBlock; } }

    private GameObject Coin = null;
    public GameObject GetCoin { get { return Coin; } }
    private GameObject Star = null;
    public GameObject GetStar { get { return Star; } }


    public override void Awake()
    {
        GameManager.Instance.TotalCoinCount = CoinCount;
        GameManager.Instance.GetCoinList = CoinPosition;
        Debug.Log("GameManager Awake : " + GetInstanceID());
        base.Awake();
        //input = GameObject.Find("InputManager").GetComponent<InputManager>();
        mapCreator = GameObject.FindObjectOfType<MapCreator>().GetComponent<MapCreator>();
        cam = GameObject.FindObjectOfType<CameraControl>().GetComponent<CameraControl>();
        ui = GameObject.FindObjectOfType<UIManager>().GetComponent<UIManager>();

        PlayerCharacter = Resources.Load<GameObject>("Player");
        SmokeEffect = Resources.Load<GameObject>("Smoke");
        RockBlock = Resources.Load<GameObject>("Rock");
        Coin = Resources.Load<GameObject>("Coin");
        Star = Resources.Load<GameObject>("Star");
    }
    private void Start()
    {
        mapCreator.Create();
        PlayerStartPos = new Vector3(mapCreator.MaxRow / 2, 0f, mapCreator.MaxCol / 2);
        SpawnPlayer();
        Debug.Log("Player ID Normal : " + player.GetInstanceID());

        Debug.Log("Player ID Instance : " + GameManager.Instance.MyPlayer.GetInstanceID());
        cam.FindPlayer();
        ui.InitUIManager();
    }
    private void OnDestroy()
    {
        Debug.Log("GameManager Destroyed : " + GetInstanceID());
    }
    private void Update()
    {
        //input.KeyUpdate();
    }
    private void SpawnPlayer()
    {
        Instantiate(PlayerCharacter, PlayerStartPos, Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log("Spawn Time Player Normal ID : " + player.GetInstanceID());
        GameManager.Instance.MyPlayer = player;
        Debug.Log("Spawn Time Player Instance ID : " + GameManager.Instance.MyPlayer.GetInstanceID());
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
    public void GetItem()
    {
        ui.ChangeAdr(true);
        GameManager.Instance.isAdr = true;
    }
    public void LostItem()
    {
        ui.ChangeAdr(false);
        GameManager.Instance.isAdr = false;
    }
    public void LoadEndScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}