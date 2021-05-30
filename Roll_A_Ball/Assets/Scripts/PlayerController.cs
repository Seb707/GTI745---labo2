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
    public float fadeSpeed;

    private bool fadeIn, fadeOut;
    private int objective = 1;
    private GameObject door;
    private Rigidbody rb;
    private int count;
    private int totalPickUps;
    private float movementX;
    private float movementY;

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
        SetCountText();
        winTextObject.SetActive(false);
        SetSpeedText();
        SetObjectifText();
        door = GameObject.Find("Door");

    }

    void Update() {
        SetSpeedText();
        SetObjectifText();
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
        countText.text = "Count: " + count.ToString()+"/"+ totalPickUps.ToString();
        if (count >= 9)
        {
            winTextObject.SetActive(true);
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

        if (col.gameObject.tag == "Enemy") {
            door.SetActive(true);
        }
        Debug.LogFormat("{0} collision enter: {1}", this, col.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "doorHitBox")
        {
            print("Woohoo");
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
