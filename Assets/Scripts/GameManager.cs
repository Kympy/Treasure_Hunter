using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    private MapCreator mapCreator = null;
    private GameObject PlayerCharacter = null;
    private Player player = null;
    public Player MyPlayer { get { return player; } }
    private Vector3 PlayerStartPos;
    public Vector3 GetStartPos { get { return PlayerStartPos; } }

    private CameraControl cam = null;

    public override void Awake()
    {
        base.Awake();
        mapCreator = GameObject.FindObjectOfType<MapCreator>().GetComponent<MapCreator>();
        PlayerCharacter = Resources.Load<GameObject>("Player");

        cam = GameObject.FindObjectOfType<CameraControl>().GetComponent<CameraControl>();
    }
    private void Start()
    {
        mapCreator.Create();
        PlayerStartPos = new Vector3(mapCreator.MaxRow / 2, 0f, mapCreator.MaxCol / 2);
        SpawnPlayer();
        Debug.Log(player);
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

}
