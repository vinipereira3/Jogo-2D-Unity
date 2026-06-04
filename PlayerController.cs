using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbory2d;
    private SpriteRenderer srPlayer;
    private bool playerInvencivel;

    public GameObject PlayerDie;

    public Transform groundCheck;
    public bool isGround = false;

    public float speed;

    public float touchRun = 0.0f;

    public bool facingRight = true;

    public int vidas = 3;
    public Color hitcolor;
    public Color noHitcolor;

    public bool Jump = false;
    public int numberJumps = 0;
    public int maximoJump = 2;
    public float JumpForce;

    private ControllerGame _ControleGame;

    public AudioSource fxGame;
    public AudioClip fxPulo;
    public AudioClip fxCenouraColetada;

    public ParticleSystem _poeira;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbory2d = GetComponent<Rigidbody2D>();
        srPlayer = GetComponent<SpriteRenderer>();

        _ControleGame = FindFirstObjectByType<ControllerGame>();
    }

    void Update()
    {
        isGround = Physics2D.Linecast(
            transform.position,
            groundCheck.position,
            1 << LayerMask.NameToLayer("Ground")
        );

        playerAnimator.SetBool("isGrounded", isGround);

        touchRun = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }

        SetaMovimentos();
    }

    private void FixedUpdate()
    {
        MovePlayer(touchRun);

        if (Jump)
        {
            JumpPlayer();
        }
    }

    void MovePlayer(float movimentoH)
    {
        playerRigidbory2d.linearVelocity = new Vector2(
            movimentoH * speed,
            playerRigidbory2d.linearVelocity.y
        );

        if ((movimentoH < 0 && facingRight) ||
            (movimentoH > 0 && !facingRight))
        {
            Flip();
        }
    }

    void JumpPlayer()
    {
        if (isGround)
        {
            numberJumps = 0;
            CriarPoeira();
        }

        if (isGround || numberJumps < maximoJump)
        {
            playerRigidbory2d.AddForce(new Vector2(0f, JumpForce));

            isGround = false;
            numberJumps++;

            if (fxGame != null && fxPulo != null)
            {
                fxGame.PlayOneShot(fxPulo);

                CriarPoeira();
            }
        }

        Jump = false;
    }

    void Flip()
    {
        CriarPoeira();

        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        transform.localScale = theScale;
    }

    void SetaMovimentos()
    {
        playerAnimator.SetBool(
            "Walk",
            playerRigidbory2d.linearVelocity.x != 0 && isGround
        );

        playerAnimator.SetBool("Jump", !isGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coletaveis":

                if (_ControleGame != null)
                {
                    _ControleGame.Pontuacao(1);
                }

                if (fxGame != null && fxCenouraColetada != null)
                {
                    fxGame.PlayOneShot(fxCenouraColetada);
                }

                Destroy(collision.gameObject);
                break;

            case "Inimigo":

                if (_ControleGame != null && _ControleGame.hitprefab != null)
                {
                    GameObject tempExplosao = Instantiate(
                        _ControleGame.hitprefab,
                        transform.position,
                        transform.rotation
                    );

                    Destroy(tempExplosao, 0.5f);
                }

                Rigidbody2D rb = GetComponent<Rigidbody2D>();

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(new Vector2(0, 900));

                if (_ControleGame != null &&
                    _ControleGame.fxGame != null &&
                    _ControleGame.fxExplosao != null)
                {
                    _ControleGame.fxGame.PlayOneShot(_ControleGame.fxExplosao);
                }

                Destroy(collision.gameObject);
                break;

            case "Damage":
                Hurt();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {

            case "Plataforma":
                this.transform.parent = collision.transform;
                break;

            case "Inimigo":
                Hurt();
                break;
        }
    }

    private void OnCollisionExit(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Plataforma":
                this.transform.parent = null;
                break;
        }
    }

    void Hurt()
    {
        if (!playerInvencivel)
        {
            playerInvencivel = true;

            vidas--;

            StartCoroutine(Dano());

            if (_ControleGame != null)
            {
                _ControleGame.BarraVida(vidas);
            }

            Debug.Log("Perdeu uma vida!");

            if (vidas < 1)
            {
                GameObject pDieTemp = Instantiate(PlayerDie, transform.position, Quaternion.identity);

                Rigidbody2D rbDie = pDieTemp.GetComponent<Rigidbody2D>();
                rbDie.AddForce(new Vector2(150f, 500f));

                _ControleGame.fxGame.PlayOneShot(_ControleGame.fxDie);


                Invoke(nameof(CarregarJogo), 4f);

                gameObject.SetActive(false);
            }
        }
    }

    void CarregarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Dano()
    {
        srPlayer.color = noHitcolor;

        yield return new WaitForSeconds(0.1f);

        for (float i = 0; i < 1; i += 0.1f)
        {
            srPlayer.enabled = false;
            yield return new WaitForSeconds(0.1f);

            srPlayer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        srPlayer.color = Color.white;
        playerInvencivel = false;
    }

    void CriarPoeira()
    {
        _poeira.Play();
    }
}