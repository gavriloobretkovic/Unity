using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Cerchiamo il gestore livello
        GestoreLivello gestore = Object.FindFirstObjectByType<GestoreLivello>();
        if (gestore == null) return;

        if (other.CompareTag("Token"))
        {
            gestore.AggiungiPunto();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish"))
        {
            // Controllo vittoria: hai tutte le monete?
            if (gestore.HaTutteLeMonete())
            {
                gestore.Vittoria();
            }
            else
            {
                // Se arrivi alla fine senza i 5 token, hai perso!
                gestore.Sconfitta("NON HAI TUTTI I TOKEN! HAI PERSO");
            }
        }
    }
}