using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeHorizontal = 5f;
    public float limiteLateral = 1.3f;

    void Update()
    {
        // Lê as setas do teclado ou teclas A e D (-1 para esquerda, 1 para direita)
        float direcaoX = Input.GetAxisRaw("Horizontal");

        // Calcula a nova posição horizontal
        Vector3 novaPosicao = transform.position;
        novaPosicao.x += direcaoX * velocidadeHorizontal * Time.deltaTime;

        // Trava o carro para não sair do asfalto
        novaPosicao.x = Mathf.Clamp(novaPosicao.x, -limiteLateral, limiteLateral);

        // Aplica a posição final ao carro
        transform.position = novaPosicao;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que colidiu possui a tag configurada no Prefab do inimigo
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Fim de Jogo! Colisão detectada com: " + other.name);
            
            // Pausa o tempo do jogo para demonstrar o impacto (temporário até criarmos a UI de Game Over)
            Time.timeScale = 0f; 
        }
    }
}