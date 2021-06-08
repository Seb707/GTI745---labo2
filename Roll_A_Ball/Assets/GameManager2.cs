using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public GameObject ball;
    private string messageWB;
    public GameObject RestartTextObject;
    public GameObject RestartBgObject;
    public GameObject RestartImageObject;
    public GameObject pauseMenuCanvas;

    private Rigidbody ballRB;
    private bool awaitConfirm;
    private bool awaitRestartConfirm;
    private int awaitTimer = 0;
    private GameObject[] enemies;
    private bool isDisabled;
    int updateCounterDelay = 0;
    WebSocket websocket;

    // Start is called before the first frame update
    async void Start()
    {
        RestartTextObject.SetActive(false);
        RestartBgObject.SetActive(false);
        RestartImageObject.SetActive(false);
        ball = GameObject.Find("Player");
        pauseMenuCanvas = GameObject.Find("Canvas_Pause");
        ballRB = ball.GetComponent<Rigidbody>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

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
        print("test");
        if (!string.IsNullOrEmpty(messageWB))
        {
            Debug.Log(messageWB);

            if (updateCounterDelay >= 1)
            {
                if (updateCounterDelay == 500)
                {
                    updateCounterDelay = 0;
                }
                else
                {
                    updateCounterDelay++;
                }
            }

            if (awaitConfirm)
            {
                awaitTimer++;
                switch (messageWB)
                {
                    case "ok_hand":
                        if (awaitRestartConfirm)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            Time.timeScale = 1;
                        }
                        break;
                }
                if (awaitTimer == 2000)
                {
                    if (awaitRestartConfirm)
                    {
                        rsumeFromRestart();
                        awaitTimer = 0;
                    }
                }
            }
            else
            {
                switch (messageWB)
                {
                    case "right":
                        ballRB.AddForce(Vector3.right * 2);
                        break;
                    case "left":
                        ballRB.AddForce(Vector3.left * 2);
                        break;
                    case "back":
                        ballRB.AddForce(Vector3.forward * 2);
                        break;
                    case "front":
                        ballRB.AddForce(Vector3.back * 2);
                        break;
                    case "neutral":
                        ballRB.velocity = ballRB.velocity * 0.95f * Time.deltaTime;
                        break;
                    case "ok_hand":
                        //ballRB.AddForce(Vector3.right * 3);
                        break;
                    case "peace_sign":
                        if (updateCounterDelay == 0)
                        {
                            updateCounterDelay = 1;
                            disableEnemies();
                        }
                        break;
                    case "boite_grise":
                        break;
                    case "iphone":
                        pauseMenuCanvas.GetComponent<PauseMenu>().testPause();
                        break;
                    case "feuille_blanche":
                        pauseMenuCanvas.GetComponent<PauseMenu>().removePauseUI();
                        confirmRestart();
                        break;
                    default:
                        break;
                }
            }

        }
    }

    private void confirmRestart()
    {
        Time.timeScale = 0;

        awaitConfirm = true;
        awaitRestartConfirm = true;
        RestartTextObject.SetActive(true);
        RestartBgObject.SetActive(true);
        RestartImageObject.SetActive(true);

    }

    private void rsumeFromRestart()
    {

        awaitConfirm = false;
        awaitRestartConfirm = false;
        RestartTextObject.SetActive(false);
        RestartBgObject.SetActive(false);
        RestartImageObject.SetActive(false);

        Time.timeScale = 1;
    }

    private void disableEnemies()
    {
        if (!isDisabled)
        {
            foreach (GameObject enemy in enemies)
            {
                print("disabled");
                enemy.SetActive(false);
                isDisabled = true;
            }
        }
        else if (isDisabled)
        {
            foreach (GameObject enemy in enemies)
            {
                print("enableing");
                enemy.SetActive(true);
                isDisabled = false;
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