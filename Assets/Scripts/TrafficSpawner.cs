using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("Prefabs e Faixas")]
    public GameObject[] enemyPrefabs;
    public float[] lanesX = { -0.45f, 0.45f };
    public float spawnInterval = 1.2f;

    [Header("Velocidades por Mão de Direção")]
    public float leftLaneSpeed = 6f;  // Contramão (esquerda): desce mais rápido
    public float rightLaneSpeed = 3f; // Mão certa (direita): anda a favor do player, desce mais devagar

    [Header("Opcional: Variações de Sprites por Mão")]
    [Tooltip("Sprites dos carros olhando para BAIXO (ex: NPC_cars _2, _6, _10, _14)")]
    public Sprite[] leftLaneSprites;
    [Tooltip("Sprites dos carros olhando para CIMA (ex: NPC_cars _0, _4, _8, _12)")]
    public Sprite[] rightLaneSprites;

    private float timer;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPitStopping) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemyCar();
            timer = 0f;
        }
    }

    void SpawnEnemyCar()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || lanesX.Length == 0) return;

        float chosenX = lanesX[Random.Range(0, lanesX.Length)];
        bool isRightLane = chosenX > 0f;

        Vector3 spawnPosition = new Vector3(chosenX, transform.position.y, 0f);
        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject car = Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);

        // Ajusta a velocidade conforme a faixa (contramão na esquerda vs. mão certa na direita)
        EnemyCarMovement movement = car.GetComponent<EnemyCarMovement>();
        if (movement != null)
        {
            movement.speed = isRightLane ? rightLaneSpeed : leftLaneSpeed;
        }

        // Ajusta o visual do carro (direção e cor sorteada)
        SpriteRenderer sr = car.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Sprite[] laneSprites = isRightLane ? rightLaneSprites : leftLaneSprites;
            if (laneSprites != null && laneSprites.Length > 0)
            {
                // Usa o sprite nativo desenhado para cima (direita) ou para baixo (esquerda)
                sr.sprite = laneSprites[Random.Range(0, laneSprites.Length)];
                sr.flipY = false;
            }
            else if (isRightLane)
            {
                // Fallback: se não preencheu os arrays de sprites, apenas inverte o carro na faixa da direita
                sr.flipY = true;
            }
        }
    }
}
