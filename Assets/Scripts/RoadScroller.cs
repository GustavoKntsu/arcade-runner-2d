using UnityEngine;

public class RoadScroller : MonoBehaviour
{
    [Header("Configuração da Pista")]
    public float velocidadePista = 6f;
    public float alturaDoBloco = 12f;

    private SpriteRenderer spriteRenderer;
    private Vector2 tamanhoInicial;
    private Vector3 posicaoInicial;
    private float deslocamento;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tamanhoInicial = spriteRenderer.size;
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Move o bloco inteiro para baixo continuamente
        transform.Translate(Vector3.down * velocidadePista * Time.deltaTime);

        // Quando o bloco descer completamente (Y <= -12), joga ele de volta para o topo (+24 unidades para cima)
        if (transform.position.y <= -alturaDoBloco)
        {
            transform.position += new Vector3(0f, alturaDoBloco * 2f, 0f);
        }
    }
}