using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public GameObject jugadorAsignado;

    private bool LlavesRecolectadas = false;

    public string id;

    void Start()
    {
        if (PlayerPrefs.GetInt("llave_" + id, 0) == 1)
        {
            gameObject.gameObject.SetActive(false);
        }
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JugadorIzq") || other.CompareTag("JugadorDer"))
        {
            PlayerPrefs.SetInt("llave_" + id, 1);
            PlayerPrefs.Save();

            SoundManager.instancia.ReproducirSonido(SoundManager.instancia.llave_recolectada);
            Destroy(gameObject);

        }
    }



}
