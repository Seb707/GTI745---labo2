using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class GameManager : MonoBehaviour
{
    public GameObject ball;
    public Rigidbody ballRB;
    public string messageWB;

    WebSocket websocket;

    // Start is called before the first frame update
    async void Start()
    {
        ball = GameObject.Find("Player");
        ballRB = ball.GetComponent<Rigidbody>();

        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
            messageWB = message;
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        if (!string.IsNullOrEmpty(messageWB))
        {
            Debug.Log(messageWB);

            switch (messageWB)
            {
                case "right":
                    ballRB.AddForce(Vector3.right * 3);
                    break;
                case "left":
                    ballRB.AddForce(Vector3.left * 3);
                    break;
                case "back":
                    ballRB.AddForce(Vector3.forward * 3);
                    break;
                case "front":
                    ballRB.AddForce(Vector3.back * 3);
                    break;
                case "neutral":
                    ballRB.velocity = ballRB.velocity * 0.95f * Time.deltaTime;
                    break;
                case "ok_hand":
                    ballRB.AddForce(Vector3.right * 3);
                    break;
                case "peace_sign":
                    ballRB.AddForce(Vector3.right * 3);
                    break;
                case "boite_grise":
                    ballRB.AddForce(Vector3.right * 3);
                    break;
                case "iphone":
                    ballRB.AddForce(Vector3.right * 3);
                    break;
                case "feuille_blanche":
                    ballRB.AddForce(Vector3.right * 3);
                    break;
                default:
                    break;
            }
        }
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}