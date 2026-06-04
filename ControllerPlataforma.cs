using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public Transform plataforma, pontoA, pontoB;
    public float velocidadePlataforma;

    public Vector3 pontoDestino;

    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plataforma.position = pontoA.position;
        pontoDestino = pontoB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (plataforma.position == pontoA.position)
        {
            pontoDestino = pontoB.position;
        }

        if (plataforma.position == pontoB.position)
        {
            pontoDestino = pontoA.position;
        }

        plataforma.position = Vector3.MoveTowards(plataforma.position, pontoDestino, velocidadePlataforma);
    }
}