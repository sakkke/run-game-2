using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerController : MonoBehaviour
{
    enum GameEventType {
        Jump,
    }

    [SerializeField]
    GameObject _player;

    public Dictionary<string, GameObject> Clients;

    // Start is called before the first frame update
    void Start()
    {
        Clients = new Dictionary<string, GameObject>();
        Exports.InitializeMultiplayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateClient(string clientId)
    {
        var vec = new Vector2(GameController.ScreenCenter(), 5);
        var client = Instantiate(_player, vec, Quaternion.identity);
        Clients[clientId] = client;
    }

    class GameEvent
    {
        public GameEventType type;
        public string clientId;
    }

    public void ParseEvent(string gameEventJson)
    {
        var ev = JsonUtility.FromJson<GameEvent>(gameEventJson);

        switch (ev.type)
        {
            case GameEventType.Jump:
                PlayerJump(ev.clientId);
                break;
        }
    }

    public void PlayerJump(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        if (!controller.Jumping)
        {
            controller.Jump();
        }
    }

    public void RemoveClient(string clientId)
    {
        Destroy(Clients[clientId]);
        Clients.Remove(clientId);
    }
}
