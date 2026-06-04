using UnityEngine;

public class IASlug : MonoBehaviour
{
    public Transform enemie;
    public SpriteRenderer enemieSprite;
    public Transform[] position;
    public float speed;
    public bool isRight;

    private int idTarget;

    void Start()
    {
        enemieSprite = enemie.GetComponent<SpriteRenderer>();
        enemie.position = position[0].position;
        idTarget = 1;
    }

    void Update()
    {
        if (enemie != null)
        {
            enemie.position = Vector3.MoveTowards(
                enemie.position,
                position[idTarget].position,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(enemie.position, position[idTarget].position) < 0.01f)
            {
                idTarget++;

                if (idTarget >= position.Length)
                {
                    idTarget = 0;
                }
            }

            if (position[idTarget].position.x < enemie.position.x && isRight)
            {
                Flip();
            }
            else if (position[idTarget].position.x > enemie.position.x && !isRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        isRight = !isRight;
        enemieSprite.flipX = !enemieSprite.flipX;
    }
}