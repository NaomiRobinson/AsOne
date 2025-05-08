using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarNivel : MonoBehaviour
{

    void Start()
    {

    }


    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Se murio");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    }
}
