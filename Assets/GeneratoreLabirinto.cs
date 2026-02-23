using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneratoreLabirinto : MonoBehaviour
{
    [SerializeField] private CellaLabirinto _cellaPrefabbricata;
    [SerializeField] private GameObject _tokenPrefab;
    [SerializeField] private GameObject _traguardoPrefab;
    [SerializeField] private int _larghezzalabirinto = 10;
    [SerializeField] private int _lunghezzalabirinto = 10;

    private CellaLabirinto[,] _griglialabirinto;

    void Start()
    {
        _griglialabirinto = new CellaLabirinto[_larghezzalabirinto, _lunghezzalabirinto];
        float cellSize = 4f;
        List<Vector3> posizioniPerMonete = new List<Vector3>();

        for (int i = 0; i < _larghezzalabirinto; i++)
        {
            for (int j = 0; j < _lunghezzalabirinto; j++)
            {
                Vector3 pos = transform.position + new Vector3(i * cellSize, 0, j * cellSize);
                CellaLabirinto nuovacella = Instantiate(_cellaPrefabbricata, pos, Quaternion.identity);
                nuovacella.x = i; nuovacella.z = j;
                _griglialabirinto[i, j] = nuovacella;

                // Escludiamo l'inizio e la fine per le monete
                if (!(i == 0 && j == 0) && !(i == _larghezzalabirinto - 1 && j == _lunghezzalabirinto - 1))
                    posizioniPerMonete.Add(pos);
            }
        }

        // Spawn ESATTAMENTE 5 Monete
        if (_tokenPrefab != null)
        {
            var casuali = posizioniPerMonete.OrderBy(x => Random.value).Take(5);
            foreach (var p in casuali)
                Instantiate(_tokenPrefab, p + new Vector3(0, 1.2f, 0), Quaternion.identity);
        }

        StartCoroutine(GeneraLabirinto(null, _griglialabirinto[0, 0]));
        CreaUscita();
    }

    private IEnumerator GeneraLabirinto(CellaLabirinto prec, CellaLabirinto corr)
    {
        corr.Visita();
        RimuoviMuro(prec, corr);
        yield return new WaitForSeconds(0.01f);
        CellaLabirinto prossima;
        do
        {
            prossima = OttieniVicino(corr);
            if (prossima != null) yield return StartCoroutine(GeneraLabirinto(corr, prossima));
        } while (prossima != null);
    }

    private CellaLabirinto OttieniVicino(CellaLabirinto corr)
    {
        int x = corr.x; int z = corr.z;
        List<CellaLabirinto> vicini = new List<CellaLabirinto>();
        if (x + 1 < _larghezzalabirinto && !_griglialabirinto[x + 1, z].visitata) vicini.Add(_griglialabirinto[x + 1, z]);
        if (x - 1 >= 0 && !_griglialabirinto[x - 1, z].visitata) vicini.Add(_griglialabirinto[x - 1, z]);
        if (z + 1 < _lunghezzalabirinto && !_griglialabirinto[x, z + 1].visitata) vicini.Add(_griglialabirinto[x, z + 1]);
        if (z - 1 >= 0 && !_griglialabirinto[x, z - 1].visitata) vicini.Add(_griglialabirinto[x, z - 1]);
        return vicini.OrderBy(v => Random.value).FirstOrDefault();
    }

    private void RimuoviMuro(CellaLabirinto a, CellaLabirinto b)
    {
        if (a == null) return;
        if (a.x < b.x) { a.RimuoviMuroDestro(); b.RimuoviMuroSinistro(); }
        else if (a.x > b.x) { a.RimuoviMuroSinistro(); b.RimuoviMuroDestro(); }
        else if (a.z < b.z) { a.RimuoviMuroFrontale(); b.RimuoviMuroPosteriore(); }
        else if (a.z > b.z) { a.RimuoviMuroPosteriore(); b.RimuoviMuroFrontale(); }
    }

    private void CreaUscita()
    {
        _griglialabirinto[0, 0].RimuoviMuroPosteriore();
        CellaLabirinto f = _griglialabirinto[_larghezzalabirinto - 1, _lunghezzalabirinto - 1];
        f.RimuoviMuroFrontale();
        if (_traguardoPrefab != null)
            Instantiate(_traguardoPrefab, f.transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
    }
}