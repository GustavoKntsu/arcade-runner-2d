using UnityEngine;

public class EnemyCarMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade de descida do carro inimigo. Deve ser diferente da velocidade da pista para dar sensação de movimento próprio.")]
    public float speed = 5f;

    [Header("Limpeza de Memória")]
    [Tooltip("Posição Y onde o carro será destruído após sair da tela.")]
    public float destroyYPosition = -7f;

    void Update()
    {
        // 1. Move o carro para baixo no espaço global (World Space)
        // Usamos Space.World para que, mesmo que o sprite esteja rotacionado em 180 graus, ele continue descendo na tela.
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // 2. Garbage Collection / Descarte
        // Como a câmera tem Size 5 (vai de Y: +5 até Y: -5), em Y: -7 o carro já saiu completamente da visão.
        if (transform.position.y <= destroyYPosition)
        {
            Destroy(gameObject);
        }
    }
}