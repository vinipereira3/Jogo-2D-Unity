using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ControllerGame : MonoBehaviour
{

    private int score;
    public Text txtScore;
    public GameObject hitprefab;

    public Sprite[] imagensVida;
    public Image barraVida;


    public AudioSource fxGame;
    public AudioClip fxCenouraColetada;
    public AudioClip fxExplosao;
    public AudioClip fxDie;


    public void Pontuacao(int qtdPontos)
    {
        score += qtdPontos;
        txtScore.text = score.ToString();

        fxGame.PlayOneShot(fxCenouraColetada);

    }

    public void BarraVida( int healthVida )
    {
        barraVida.sprite = imagensVida[healthVida];
    }
}
