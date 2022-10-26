using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool jump;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(HorizontalInput * speed, body.velocity.y);

        //Flip player when moving left-right
        if (HorizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(10, 10, 10);
        }
        else if (HorizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-10, 10, 10);
        }

        if (Input.GetKey(KeyCode.Space) && jump)
        {
            Jump();
        }

        //Set animator parameters
        anim.SetBool("run", HorizontalInput != 0);
        anim.SetBool("jump", jump);

        SetAnimationState();
    }

    void SetAnimationState()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            anim.SetBool("slide", true);
        else
            anim.SetBool("slide", false);

        if (body.velocity.y < 0)
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", true);
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jump = true;
        }
    }
}
