using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticVariables;

public class ReiniciarNivel : MonoBehaviour
{
    public bool modoInvencible { get; private set; }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") ) & modoInvencible == false || (collision.gameObject.CompareTag("Obstacle") & modoInvencible == false))
        {
           Reiniciar();
        }
    }

    public void  Reiniciar(){
          Debug.Log("GameOver");
          
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
