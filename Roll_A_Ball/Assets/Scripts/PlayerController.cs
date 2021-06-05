using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public TextMeshProUGUI speedTextObject;
    public TextMeshProUGUI ObjectiveTextObject;
    public TextMeshProUGUI LifePointsTextObject;
    public float fadeSpeed;
    public AudioClip EnemyHitclip;

    private bool fadeIn, fadeOut;
    private int objective = 1;
    private GameObject door;
    private GameObject Player;
    private Rigidbody rb;
    private int count;
    private int lifePoints;
    private bool isPlayerInv;
    private int invTimer;
    private int totalPickUps;
    private float movementX;
    private float movementY;
    private Color originalColors;

    //private int lifeCounter = 4;
    //public Image[] hearts;
    //public Sprite fullHeart;
    //public Sprite emptyHeart;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        totalPickUps = 9;
        lifePoints = 6;
        invTimer = 0;

        isPlayerInv = false;
        SetCountText();
        winTextObject.SetActive(false);
        SetSpeedText();
        SetObjectifText();
        Player = GameObject.Find("Player");
        door = GameObject.Find("Door");
        originalColors = Player.GetComponent<Renderer>().material.color;

    }

    void Update() {
        SetSpeedText();
        SetObjectifText();
        SetLifePointText();
        //print("update");
        if (fadeOut)
        {
            //j.GetComponent<MeshRenderer>().material.color = Color.Lerp(obj.GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
            Color objectColor = door.GetComponent<Renderer>().material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            door.GetComponent<Renderer>().material.color = objectColor;

            if (objectColor.a <= 0)
            {
                print("Fading out finished");
                fadeOut = false;
                door.SetActive(false);
            }
        }
        else if (fadeIn) {
            //j.GetComponent<MeshRenderer>().material.color = Color.Lerp(obj.GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
            Color objectColor = door.GetComponent<Renderer>().material.color;
            float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            door.GetComponent<Renderer>().material.color = objectColor;

            if (objectColor.a > 0)
            {
                door.SetActive(true);
            }
            if (objectColor.a >= 1)
            {
                print("Fading in finished");
                fadeIn = false;
            }
        }
    }

    void SetObjectifText()
    {
        
        

        switch (objective)
        {
            case 1:
                ObjectiveTextObject.text = "Veuillez explorer la zone et ramasser les cube jaune!";
                break;
            case 2:
                ObjectiveTextObject.text = "La porte c'est ouverte!";
                break;
            case 3:
                ObjectiveTextObject.text = "Ball Speed: ";
                break;
            default:

                break;
        }

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString() + "/" + totalPickUps.ToString();
        if (count >= 9)
        {
            winTextObject.SetActive(true);
        }
    }

    void SetLifePointText()
    {
        Color playerColor = Player.GetComponent<Renderer>().material.color;

        if (isPlayerInv) {
            invTimer++;
            if (invTimer == 800) {
                isPlayerInv = false;
                invTimer = 0;
            }
        }
        if (invTimer % 100 >= 50)
        {
            playerColor = new Color(208, 64, 57, 1.0f);
            LifePointsTextObject.text = "Point de vie:   /6";
            Player.GetComponent<Renderer>().material.color = playerColor;
        }
        else {
            playerColor = new Color(originalColors.r, originalColors.g, originalColors.b, originalColors.a);

            LifePointsTextObject.text = "Point de vie: " + lifePoints.ToString() + "/6";
            Player.GetComponent<Renderer>().material.color = playerColor;

        }
    }

    void SetSpeedText()
    {
        float speed = rb.velocity.magnitude;
        if (speed < 0.1) {
            rb.velocity = Vector3.zero;
            speed = rb.velocity.magnitude;
        }
        speedTextObject.text = "Ball Speed: " + speed.ToString();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision col) {

        if (!isPlayerInv) {
            if (col.gameObject.tag == "Enemy") {
                isPlayerInv = true;
                lifePoints--;
                print("colision detected");
                AudioSource.PlayClipAtPoint(EnemyHitclip, col.gameObject.transform.position);
                SetLifePointText();
            }
        }
        Debug.LogFormat("{0} collision enter: {1}", this, col.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "doorHitBox")
        {
            fadeIn = true;
        }

        if (other.gameObject.CompareTag("PickUp"))
        { 
            other.gameObject.SetActive(false);
            count++;

            if (count == 1){
                print("fadeOut true");
                fadeOut = true;
            }

            SetCountText();
        }
    }
}
