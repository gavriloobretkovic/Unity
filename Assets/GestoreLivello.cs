using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GestoreLivello : MonoBehaviour
{
    public TextMeshProUGUI testoTempo;
    public TextMeshProUGUI testoPunti;
    public TextMeshProUGUI testoMessaggio;

    [Header("Impostazioni Sfida")]
    public float tempoMassimo = 60f;
    private float tempo;
    private int punti;
    private bool sfidaAttiva = false;
    private int puntiVittoria = 5;

    void Start()
    {
        testoMessaggio.text = "";
        // All'inizio forziamo tutto a spento
        testoTempo.gameObject.SetActive(false);
        testoPunti.gameObject.SetActive(false);
    }

    void Update()
    {
        if (sfidaAttiva)
        {
            tempo += Time.deltaTime;
            testoTempo.text = "Tempo: " + tempo.ToString("F2") + " / " + tempoMassimo;

            if (tempo >= tempoMassimo)
            {
                Sconfitta("TEMPO SCADUTO! HAI PERSO");
            }
        }
    }

    // Questa viene chiamata quando passi attraverso la porta del labirinto
    public void IniziaSfida(int w = 0, int l = 0)
    {
        sfidaAttiva = true;
        tempo = 0;
        punti = 0;
        testoMessaggio.text = "";

        // ACCENDIAMO i testi solo ora che la sfida è iniziata!
        testoTempo.gameObject.SetActive(true);
        testoPunti.gameObject.SetActive(true);

        AggiornaUI();
    }

    public void AggiungiPunto()
    {
        if (!sfidaAttiva) return;
        punti++;
        AggiornaUI();
    }

    public bool HaTutteLeMonete() => punti >= puntiVittoria;

    public void Vittoria()
    {
        if (!sfidaAttiva) return;
        FinePartita("HAI VINTO!", Color.green);
    }

    public void Sconfitta(string motivo)
    {
        if (!sfidaAttiva) return;
        FinePartita(motivo, Color.red);
    }

    private void FinePartita(string msg, Color colore)
    {
        sfidaAttiva = false;

        // SPEGNIAMO tutto tranne il messaggio finale
        testoTempo.gameObject.SetActive(false);
        testoPunti.gameObject.SetActive(false);

        testoMessaggio.text = msg;
        testoMessaggio.color = colore;

        StartCoroutine(RicominciaGiocoDopoTempo(5f));
    }

    IEnumerator RicominciaGiocoDopoTempo(float secondi)
    {
        yield return new WaitForSeconds(secondi);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void AggiornaUI()
    {
        testoPunti.text = "Punti: " + punti + "/" + puntiVittoria;
    }
}