using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RunTimer : MonoBehaviour
{
    float position;

    Rigidbody2D rigi;

    [SerializeField]
    GameObject character;
    [SerializeField]
    GameObject caber;
    [SerializeField]
    Slider power;

    //UI
    [SerializeField]
    GameObject winPanel;
    [SerializeField]
    GameObject noFlip;
    [SerializeField]
    GameObject fellOver;
    [SerializeField]
    Text winDistance;
    [SerializeField]
    GameObject begin;
    [SerializeField]
    GameObject faller;
    [SerializeField]
    GameObject receiver;

    float charSpeed = 0;

    float fallen;

    bool thrown = false;

    float powerTimer = 0;

    bool standing = true;

    bool flipped;
    float flip;
    Rigidbody2D caberRigi;

    [SerializeField]
    Animator animator;
    Rigidbody2D charRigi;

    bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        rigi = faller.GetComponent<Rigidbody2D>();
        caberRigi = caber.GetComponent<Rigidbody2D>();
        charRigi = character.GetComponent<Rigidbody2D>();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            if (Input.anyKeyDown)
            {                
                Time.timeScale = 1;
                started = true;
                begin.SetActive(false);
            }
        }

        if (started)
        {
            animator.SetFloat("Velocity", charRigi.velocity.magnitude *100);

            flip = caber.transform.eulerAngles.z;
            fallen = character.transform.eulerAngles.z;

            if (character.transform.position.x < 0)
            {
                
                if (fallen < 75f)
                {
                    character.transform.Translate(Vector3.right * charSpeed * Time.deltaTime, Space.World);
                    position = faller.transform.position.y;

                    if (Input.anyKeyDown && Time.deltaTime >0)
                    {
                        
                        if (Mathf.Round(position) == -4)
                        {
                            float accuracy = 1 - Mathf.Abs(position + 4) * 2;
                            //Debug.Log("Good! " + accuracy);
                            character.transform.Rotate(transform.forward, accuracy / 20);
                        }
                        else
                        {
                            //Debug.Log("wobbly");
                            character.transform.Rotate(transform.forward, 1);
                        }
                        charSpeed++;
                        Respawn();
                    }
                }
                else if (standing)
                {
                    caber.transform.parent = null;
                    caber.AddComponent(typeof(BoxCollider2D));
                    caber.AddComponent(typeof(Rigidbody2D));
                    standing = false;
                    fellOver.SetActive(true);
                }
            }
            else if (!thrown)
            {
                faller.SetActive(false);
                receiver.SetActive(false);
                if (powerTimer <= 3)
                {
                    Time.timeScale = 0.1f;
                    if (Input.anyKeyDown)
                    {
                        power.value += 10;
                    }
                    power.value -= Time.deltaTime * 300;
                    powerTimer += Time.deltaTime * 20;
                    //Debug.Log(powerTimer);

                }
                else
                {
                    Time.timeScale = 1;
                    /*/character.transform.eulerAngles = Vector3.zero;
                    Rigidbody2D charRigi = character.GetComponent<Rigidbody2D>();
                    charRigi.simulated = false;/*/
                    caber.AddComponent(typeof(BoxCollider2D));
                    caber.AddComponent(typeof(Rigidbody2D));
                    caberRigi = caber.GetComponent<Rigidbody2D>();
                    caber.transform.parent = null;
                    caberRigi.AddForce((Vector3.right + (Vector3.up)) * power.value * 5);
                    caberRigi.AddTorque(-200);
                    thrown = true;
                    standing = false;
                }
            }

            if (thrown)
            {
                Vector3 tempPos = caber.transform.position;
                tempPos.z = -10;
                Camera.main.transform.position = tempPos;
                if (Mathf.Round(flip) == 90)
                {
                    flipped = true;
                    //Debug.Log("flipped = " + flipped);
                }
                if (caber.transform.position.x > 0.5f)
                {
                    if (caberRigi.velocity.magnitude == 0)
                    {
                        if (flipped)
                        {
                            winPanel.SetActive(true);
                            winDistance.text = "Distance: " + (caber.transform.position.x);
                        }
                        else
                        {
                            noFlip.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void Respawn()
    {
        Vector3 origin = faller.transform.position;
        origin.y = 4;
        faller.transform.position = origin;
        rigi.velocity = Vector2.zero;
        rigi.gravityScale = charSpeed;
    }

    public void LoadScene(int sceneNum)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNum);
    }
}
