using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 5f;
    public float limiteEsquerda = -1.3f;
    public float limiteDireitaNormal = 1.3f;
    public float limiteDireitaPosto = 3.7f; // Permite chegar até X = 3.5 (em cima da bomba do posto)

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector3 novaPosicao = transform.position + new Vector3(moveX * speed * Time.deltaTime, 0, 0);

        // Se houver um posto ativo na tela, libera o lado direito para entrar na baia
        float limiteDireitoAtual = (GameManager.Instance != null && GameManager.Instance.isGasStationOnScreen) 
            ? limiteDireitaPosto 
            : limiteDireitaNormal;

        novaPosicao.x = Mathf.Clamp(novaPosicao.x, limiteEsquerda, limiteDireitoAtual);
        transform.position = novaPosicao;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (GameManager.Instance != null) GameManager.Instance.GameOver();
        }
        else if (other.CompareTag("PitStop"))
        {
            if (GameManager.Instance != null && !GameManager.Instance.isPitStopping)
            {
                GameManager.Instance.isPitStopping = true;
                other.gameObject.SetActive(false);
            }
        }
    }
}