using UnityEngine;
using UnityEngine.SceneManagement;

public class RecolectarFragmento : MonoBehaviour
{
  public string idGrupo;

  public bool juntoFragmento { get; private set; } = false;
  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("JugadorIzq") || collision.CompareTag("JugadorDer"))
    {
      juntoFragmento = true;
      string idNivel = SceneManager.GetActiveScene().name;
      ChequeoLlaves.AgregarFragmento(idGrupo, idNivel);

      SoundManager.instancia.ReproducirSonido(SoundManager.instancia.llave_recolectada);
      gameObject.SetActive(false);
    }

  }
}