using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

	Rigidbody2D rb;
	Animator anim;
	float dirX, moveSpeed = 7f;
	int  healthPoints = 3;
	bool isHurting, isDead;
	bool facingRight = true;
	Vector3 localScale;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {		

		if (Input.GetButtonDown ("Jump") && !isDead && rb.velocity.y == 0)
			rb.AddForce (Vector2.up * 600f);

		//if (Input.GetKey (KeyCode.LeftShift))
		//	moveSpeed = 10f;
		//else
		//	moveSpeed = 5f;

		SetAnimationState();

		if (!isDead)
			dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed;
	}

	void FixedUpdate()
	{
		if (!isHurting)
			rb.velocity = new Vector2 (dirX, rb.velocity.y);
	}

	void LateUpdate()
	{
		CheckWhereToFace();
	}

	void SetAnimationState()
	{
		if (dirX == 0) {
			anim.SetBool ("run", false);
		}

		if (rb.velocity.y == 0) {
			anim.SetBool ("jump", false);
			anim.SetBool ("falling", false);
		}

		if (Mathf.Abs(dirX) == 7 && rb.velocity.y == 0)
			anim.SetBool ("run", true);
		else
			anim.SetBool ("run", false);

		if (Input.GetKey (KeyCode.LeftControl))
			anim.SetBool ("slide", true);
		else
			anim.SetBool ("slide", false);

		if (rb.velocity.y > 0)
			anim.SetBool ("jump", true);
		
		if (rb.velocity.y < 0) {
			anim.SetBool ("jump", false);
			anim.SetBool ("falling", true);
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {

			anim.SetTrigger("dash-attack");
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			anim.SetTrigger("attack");
		}

	}

	void CheckWhereToFace()
	{
		if (dirX > 0)
			facingRight = true;
		else if (dirX < 0)
			facingRight = false;

		if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name.Equals ("Fire")) {
			healthPoints -= 1;
		}

		if (col.gameObject.name.Equals ("Fire") && healthPoints > 0) {
			anim.SetTrigger ("hurt");
			StartCoroutine ("Hurt");
		} else {
			dirX = 0;
			isDead = true;
			anim.SetTrigger ("dead");
		}
	}

	IEnumerator Hurt()
	{
		isHurting = true;
		rb.velocity = Vector2.zero;

		if (facingRight)
			rb.AddForce (new Vector2(-300f, 300f));
		else
			rb.AddForce (new Vector2(300f, 300f));
		
		yield return new WaitForSeconds (0.5f);

		isHurting = false;
	}

}
