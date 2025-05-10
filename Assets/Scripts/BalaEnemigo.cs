using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemigo : MonoBehaviour
{
    public float velocidad;
    public int daño;

    void Start()
    {

    }


    private void Update()
    {
        transform.Translate(Time.deltaTime * velocidad * Vector2.right);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        if (collision.gameObject.CompareTag("Walls"))
        {
            Debug.Log("bala toco pared");
            Destroy(gameObject);
        }


    }
}
