using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundLayer;
    //public Sprite marioSmall;
    //public Sprite marioBig;
    //public AnimatorController marioSmallAnim;
    //public AnimatorController marioBigAnim;
    public GameObject fire;
    //public AnimatorController flowerAnimator;


    public float moveSpeed;
    public float acceleration;
    public float deceleration;
    public float velPow;
    public static bool grown = false;
    public static bool flower = false;
    public static int dir = 1;
    public bool isSitting = false;
    public bool pipeCheck = false;


    private float jumpingForce = 20.5f;
    public float moveInput;
    public float moveInputY;

    private bool isGrounded = true;

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D marioCollider;
    private AudioSource audioSource;

    public AudioClip jumpSound;
    public AudioClip fireSound;
    public AudioClip deathSound;
    public AudioClip mushroomSound;

    public Animator animator;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        marioCollider = GetComponent<CapsuleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isGrounded", true);
        grown = false;
        flower = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", rigidbody2d.velocity.x);
    }

    private void FixedUpdate()
    {
        if (isSitting) moveInput = 0;
        float targetSpeed = moveSpeed * moveInput;
        float speedDif = targetSpeed - rigidbody2d.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif);

        rigidbody2d.AddForce(movement * Vector2.right);
    }

    private bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        return isGrounded;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        moveInputY = context.ReadValue<Vector2>().y;
        animator.SetFloat("dir", moveInput);
        if (context.started)
        {
            if (moveInput > 0.01f || moveInput < -0.01f)
            {
                dir = (int)Mathf.Sign(moveInput);
            }
        }
    }

    public void Sit(InputAction.CallbackContext context)
    {
        if (context.performed && grown)
        {
            animator.SetBool("isSitting", true);
            isSitting = true;
            moveInput = 0;

            marioCollider.size = new Vector2(1f, 1f);
            marioCollider.offset = new Vector2(0f, 0.5f);
        }
        if (context.canceled && grown)
        {
            animator.SetBool("isSitting", false);
            isSitting = false;

            marioCollider.size = new Vector2(1f, 2f);
            marioCollider.offset = new Vector2(0f, 1f);
        }
        if (context.performed)
        {
            pipeCheck = true;
            print(pipeCheck);
        }
        if (context.canceled)
        {
            pipeCheck = false;
            print(pipeCheck);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !isSitting)
        {
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
            rigidbody2d.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
            animator.SetFloat("dirJump", moveInput);
            //dir = (int)Mathf.Sign(moveInput);
            if (moveInput > 0.01f || moveInput < -0.01f)
            {
                dir = (int)Mathf.Sign(moveInput);
            }
        }
        if (context.canceled && rigidbody2d.velocity.y > 0f)
        {
            rigidbody2d.AddForce(Vector2.down * rigidbody2d.velocity.y * 0.5f, ForceMode2D.Impulse);
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && flower)
        {
            Instantiate(fire, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            if (fireSound != null)
            {
                audioSource.PlayOneShot(fireSound);
            }
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveSpeed *= 1.2f;
        }
        if (context.canceled)
        {
            moveSpeed /= 1.2f;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("isGrounded", true);

        // Здесь надо переписать в виде case для оптимизации. 

        if (collision.collider.CompareTag("enemyWeakness"))
        {
            rigidbody2d.AddForce(Vector2.up * jumpingForce * 0.3f, ForceMode2D.Impulse);
            GameManager.score += 100;
        }

        if (collision.gameObject.CompareTag("mushroom"))
        {
            if (!flower && mushroomSound != null)
            {
                audioSource.PlayOneShot(mushroomSound);
            }
            if (!grown)
            {
                MarioGrow();
            }
            else
            {
                MarioEatFlower();
                flower = true;
            }

            Destroy(collision.gameObject);
        }

        if (collision.collider.CompareTag("enemy") && collision.otherCollider.GetType() == typeof(CapsuleCollider2D))
        {
            if (grown)
            {
                MarioShrink();
            }
            else
            {
                animator.SetBool("death", true);
                StartCoroutine(Death());
            }
        }

    }

    public void MarioEatFlower()
    {
        flower = true;
        spriteRenderer.material.color = Color.red;
    }


    /// <summary>
    /// Функции для анимации
    /// </summary>
    public void MarioGrow()
    {
        grown = true;
        animator.SetBool("grown", grown);
        //spriteRenderer.sprite = marioBig;
        marioCollider.size = new Vector2(1f, 2f);
        marioCollider.offset = new Vector2(0f, 1f);
    }
    public void MarioShrink()
    {
        grown = false;
        flower = false;
        animator.SetBool("grown", grown);
        //spriteRenderer.sprite = marioSmall;
        marioCollider.size = new Vector2(1f, 1f);
        marioCollider.offset = new Vector2(0f, 0.5f);
        spriteRenderer.material.color = Color.white;
    }

    public void StopMotion()
    {
        rigidbody2d.simulated = false;
    }
    public void ResumeMotion()
    {
        rigidbody2d.simulated = true;
    }

    public void ColorTransparent()
    {
        spriteRenderer.color = new Color(0, 0, 0, 0);
    }
    public void ColorWhite()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void EnemyStop()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {

            enemy.GetComponent<Rigidbody2D>().simulated = false;
            if (enemy.GetComponent<Animator>() != null)
            {
                enemy.GetComponent<Animator>().enabled = false;
            }

        }
    }
    public void EnemyResume()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().simulated = true;
            if (enemy.GetComponent<Animator>() != null)
            {
                enemy.GetComponent<Animator>().enabled = true;
            }
        }
    }

    public void InvulnerabilityOn()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }
    public void InvulnerabilityOff()
    {
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
    public IEnumerator Death()
    {
        if (deathSound != null)
        {
            Camera.main.GetComponent<AudioSource>().Stop();
            audioSource.PlayOneShot(deathSound);
        }
        gameObject.GetComponent<PlayerInput>().enabled = false;
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.gravityScale = 0f;
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSecondsRealtime(1);
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        rigidbody2d.gravityScale = 4f;
        rigidbody2d.AddForce(Vector2.up * jumpingForce * 0.7f, ForceMode2D.Impulse);
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
        {
            collider.isTrigger = true;
        }

        yield return new WaitForSeconds(2);
        GameManager.LoadScreenOn();

        yield return new WaitForSeconds(2);
        GameManager.LoadScreenOff();

        if (GameManager.health > 0)
        {
            GameManager.RestartLevel();
            GameManager.health--;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
            //SceneManager.LoadScene("1-1", LoadSceneMode.Additive);
        }
    }



}
