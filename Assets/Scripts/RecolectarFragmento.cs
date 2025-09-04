using UnityEngine;
using UnityEngine.SceneManagement;


public class RecolectarFragmento : MonoBehaviour
{
  public string idGrupo;
  public GameObject ranura;

  public GameObject jugadorRecolecta;

 public bool estaEnTecho = false;

  float duracion = 2f;
  private Vector3 posInicial;
  public float altura = 0.2f;
  public float duracionFlotar = 1f;
  public float velocidadRotacion = 45f;
  public bool juntoFragmento { get; private set; } = false;


  private Vector3 escalaOriginal;



  void Start()
  {
    posInicial = transform.position;
    escalaOriginal = transform.localScale;

    Flotar();
  }


  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("JugadorIzq") || collision.CompareTag("JugadorDer"))
    {
      RecogerFragmento(collision.gameObject);
    }
  }

  void RecogerFragmento(GameObject jugadorRecolecta)
  {
    juntoFragmento = true;

    string idNivel = SceneManager.GetActiveScene().name;
    ChequeoLlaves.AgregarFragmento(idGrupo, idNivel);


    ReproducirSonido();

    GetComponent<Collider2D>().enabled = false;

    AnimacionRecogida(jugadorRecolecta);
  }





  void ReproducirSonido()
  {
    SoundManager.instancia.ReproducirSonido(SoundManager.instancia.llave_recolectada);
  }

  void AnimacionRecogida(GameObject jugadorRecolecta)
  {
    LeanTween.cancel(gameObject);

    Vector3 offset = estaEnTecho ? Vector3.down * 1.5f : Vector3.up * 1.5f;
    Vector3 posDestino = jugadorRecolecta.transform.position + offset;


    LeanTween.move(gameObject, posDestino, 0.3f)
             .setEase(LeanTweenType.easeOutBack)
             .setIgnoreTimeScale(true);

    LeanTween.scale(gameObject, escalaOriginal * 2f, 0.3f)
             .setEase(LeanTweenType.easeOutBack)
             .setOnComplete(() =>
             {

               LeanTween.scale(gameObject, escalaOriginal, 0.2f)
                        .setEase(LeanTweenType.easeInBack)
                        .setOnComplete(() =>
                        {

                          moverARanura(ranura.transform.position, duracion);
                        });
             });
  }


  void moverARanura(Vector3 destino, float duracion)
  {
    if (gameObject.LeanIsTweening())
      gameObject.LeanCancel();
    gameObject.LeanMove(destino, duracion)
              .setEase(LeanTweenType.easeInOutBack)
              .setIgnoreTimeScale(true)
              .setDelay(0.25f)
              .setOnComplete(() => { });
  }

  void Flotar()
  {

    LeanTween.moveY(gameObject, posInicial.y + altura, duracionFlotar)
                .setEase(LeanTweenType.easeInOutSine)
                .setLoopPingPong();
  }

}