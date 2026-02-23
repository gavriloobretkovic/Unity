using UnityEngine;

public class CellaLabirinto : MonoBehaviour
{
    public int x;
    public int z;
    [SerializeField]
    private GameObject _murosinistro;
    [SerializeField]
    private GameObject _murodestro;
    [SerializeField]
    private GameObject _murofrontale;
    [SerializeField]
    private GameObject _muroposteriore;
    [SerializeField]
    private GameObject _cellanonvisitata;
    public bool visitata { get; private set; }

    public void Visita()
    {
        visitata = true;
        _cellanonvisitata.SetActive(false);
    }

    public void RimuoviMuroSinistro()
    {
        _murosinistro.SetActive(false);
    }

    public void RimuoviMuroDestro()
    {
        _murodestro.SetActive(false);
    }

    public void RimuoviMuroFrontale()
    {
        _murofrontale.SetActive(false);
    }

    public void RimuoviMuroPosteriore()
    {
        _muroposteriore.SetActive(false);
    }
}
