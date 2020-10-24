using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : MonoBehaviour
{
    public Text countText;
    public Text livesText;
    public Text winText;
    private Rigidbody2D rd2d;
    public float speed;
    private int count = 0;
    private int lives;
    private bool jumpcheck;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    private bool facingRight = true;
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
       rd2d = GetComponent<Rigidbody2D>();
       anim = GetComponent<Animator>();
       countText.text = count.ToString();
       count = 0;
       lives = 3;
       SetCountText();
       SetLivesText();
       winText.text = "";
       musicSource.clip = musicClipOne;
       musicSource.Play(); 
       isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
       anim.SetInteger("State", 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius,allGround);
        rd2d.AddForce(new Vector2 (hozMovement * speed, verMovement * speed));
        if (facingRight == false && hozMovement > 0)
        {
             Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.A)))
        {
            jumpcheck = true;
            anim.SetInteger("State", 1);
        }
         else if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.D)))
            {
                anim.SetInteger("State", 0);
                jumpcheck = false;
            }  
        if (jumpcheck == false)
        {
        if (isOnGround == false)
        {
            anim.SetInteger("State",2);
        }
        else
        {
            anim.SetInteger("State", 0);
        }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            count += 1;
            countText.text = count.ToString();
            SetCountText();
            Destroy(collision.collider.gameObject);
        if (count == 4)
        {
            lives = 3;
            SetLivesText();
            transform.position = new Vector2 (118.0f, 2.5f);
        }
        }
        else if (collision.collider.tag == "Enemy")
        {
            lives -= 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);
        }
        if (lives == 0)
       {
           this.gameObject.SetActive(false);
       }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            } 
        }
    }
    void SetCountText ()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            winText.text ="You Win. Game Created by James Hilley.";
            musicSource.clip = musicClipTwo;
            musicSource.Play(); 
        }
    }
    void SetLivesText ()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            winText.text = "You Lose. Game Created by James Hilley.";
        }
    }
void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
