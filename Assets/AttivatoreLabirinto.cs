using UnityEngine;

public class AttivatoreLabirinto : MonoBehaviour
{
    public int tempoDisponibile = 60;
    public int puntiDaRaccogliere = 5;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ho toccato qualcosa: " + other.name); // Questo scriverà in console cosa ti tocca

        if (other.CompareTag("Player"))
        {
            Debug.Log("È il Player! Faccio partire la sfida.");
            FindObjectOfType<GestoreLivello>().IniziaSfida(tempoDisponibile, puntiDaRaccogliere);
        }
    }
}