using UnityEngine;

public class MovimientoOpcional : MonoBehaviour
{
    public GameObject jugador;
    public GameObject jugadorEspejado;
    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;
    [SerializeField] private float velocidadMovimiento = 5f;

    private readonly KeyCode[] teclasMovimiento = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
    private readonly Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        rb = jugador.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorEspejado.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame


    void Update()
    {
        Vector2 direccionMovimiento = Vector2.zero;

        // Detectar entrada de teclas y calcular dirección de movimiento
        if (Input.GetKey(KeyCode.W)) direccionMovimiento += Vector2.up;
        if (Input.GetKey(KeyCode.S)) direccionMovimiento += Vector2.down;
        if (Input.GetKey(KeyCode.A)) direccionMovimiento += Vector2.left;
        if (Input.GetKey(KeyCode.D)) direccionMovimiento += Vector2.right;

        // Normalizar dirección para mantener velocidad constante en diagonales
        direccionMovimiento = direccionMovimiento.normalized;

        // Aplicar movimiento libre
        rb.linearVelocity = direccionMovimiento * velocidadMovimiento;
        rbEspejado.linearVelocity = new Vector2(-direccionMovimiento.x, direccionMovimiento.y) * velocidadMovimiento;
    }
}
