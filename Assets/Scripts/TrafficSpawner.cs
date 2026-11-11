using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Lista de Prefabs de carros inimigos para variar as cores/modelos no trânsito.")]
    public GameObject[] enemyPrefabs;

    [Header("Configuração das Faixas (Lanes)")]
    [Tooltip("Coordenadas X exatas de cada faixa da pista (baseado no limite de -1.3 a +1.3).")]
    public float[] lanesX = { -1.3f, -0.45f, 0.45f, 1.3f };

    [Header("Controle de Tempo (Spawn Rate)")]
    [Tooltip("Tempo em segundos entre a geração de cada carro.")]
    public float spawnInterval = 1.2f;

    private float timer;

    void Update()
    {
        // Cronômetro simples baseado no tempo real entre frames
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemyCar();
            timer = 0f; // Reseta o cronômetro após instanciar
        }
    }

    void SpawnEnemyCar()
    {
        // Validação de segurança caso o aluno esqueça de arrastar o Prefab no Inspector
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Nenhum Prefab de carro inimigo atribuído ao TrafficSpawner!");
            return;
        }

        // 1. Sorteia qual faixa (eixo X) o carro vai nascer
        int randomLaneIndex = Random.Range(0, lanesX.Length);
        float chosenX = lanesX[randomLaneIndex];

        // 2. Define a posição de nascimento usando o X sorteado e o Y atual do Spawner (Y = 7)
        Vector3 spawnPosition = new Vector3(chosenX, transform.position.y, 0f);

        // 3. Sorteia qual modelo de carro da lista será instanciado
        int randomCarIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomCarIndex];

        // 4. Instancia o carro mantendo a rotação original configurada no Prefab
        Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);
    }
}