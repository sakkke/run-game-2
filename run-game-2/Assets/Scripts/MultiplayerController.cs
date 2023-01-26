using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerController : MonoBehaviour
{
    public enum GameEventType {
        Dive,
        Jump,
        MoveBreak,
        MoveLeft,
        MoveRight,
        Squat,
        StandUp,
        Attack,
    }

    [SerializeField]
    GameObject _player;

    [SerializeField]
    GameObject _playerAttackCircle;

    public string ClientId { get; set; }

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
        if (Clients.ContainsKey(clientId))
        {
            return;
        }

        var vec = new Vector2(GameController.ScreenCenter(), 5);
        var client = Instantiate(_player, vec, Quaternion.identity);

        if (Clients.Count == 0)
        {
            ClientId = clientId;
        }

        Clients[clientId] = client;
    }

    public class GameEvent
    {
        public GameEventType type;
        public string clientId;
    }

    public void EmitEvent(GameEventType type, string clientId)
    {
        Exports.EmitEvent(type, clientId);
    }

    public void ParseEvent(string gameEventJson)
    {
        var ev = JsonUtility.FromJson<GameEvent>(gameEventJson);

        switch (ev.type)
        {
            case GameEventType.Dive:
                PlayerDive(ev.clientId);
                break;

            case GameEventType.Jump:
                PlayerJump(ev.clientId);
                break;

            case GameEventType.MoveBreak:
                PlayerMoveBreak(ev.clientId);
                break;

            case GameEventType.MoveLeft:
                PlayerMoveLeft(ev.clientId);
                break;

            case GameEventType.MoveRight:
                PlayerMoveRight(ev.clientId);
                break;

            case GameEventType.Squat:
                PlayerSquat(ev.clientId);
                break;

            case GameEventType.StandUp:
                PlayerStandUp(ev.clientId);
                break;

            case GameEventType.Attack:
                PlayerAttack(ev.clientId);
                break;
        }
    }

    public void PlayerAttack(string clientId)
    {
        var client = Clients[clientId];
        Instantiate(_playerAttackCircle, client.transform.position, Quaternion.identity);
    }

    public void PlayerDive(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        if (controller.Jumping)
        {
            controller.Dive();
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

    public void PlayerMoveBreak(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        controller.MoveBreak();
    }

    public void PlayerMoveLeft(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        controller.MoveLeft();
    }

    public void PlayerMoveRight(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        controller.MoveRight();
    }

    public void PlayerSquat(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        if (!controller.Jumping)
        {
            controller.Squat();
        }
    }

    public void PlayerStandUp(string clientId)
    {
        var client = Clients[clientId];
        var controller = client.GetComponent<PlayerController>();

        if (!controller.Jumping)
        {
            controller.StandUp();
        }
    }

    public void RemoveClient(string clientId)
    {
        Destroy(Clients[clientId]);
        Clients.Remove(clientId);
    }
}
