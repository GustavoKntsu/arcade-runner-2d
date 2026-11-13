using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progresso e Fases (Biomas)")]
    public float score;
    public TextMeshProUGUI scoreText;
    public float scoreToNextPhase = 300f;
    public int currentBiomeIndex = 0;
    private int lastBiomeIndex = 0;
    public Sprite[] biomeSprites; 
    public Sprite[] gasStationSprites;

    [Header("Controle de Frequência dos Postos")]
    public int maxGasStationsPerBiome = 2; // Menos de 3 vezes no mesmo bioma
    public int gasStationsInCurrentBiome = 0;
    public int normalBlocksSinceLastStation = 0;
    public int minNormalBlocksBetweenStations = 4; // Distância mínima entre postos
    public bool isGasStationOnScreen = false; // Avisa o PlayerController para liberar a entrada
    [Range(0f, 1f)]
    public float secondStationChance = 0.25f; // 1º posto do bioma = 100%; 2º posto = 25% de chance

    [Header("Fundo da Tela")]
    public SpriteRenderer backgroundRenderer;
    public Sprite[] backgroundSprites;
    public float backgroundScrollSpeed = 5f;
    private static readonly Vector2 BackgroundTiledSize = new Vector2(24f, 14f);

    [Header("Combustível e HUD")]
    public float maxFuel = 100f;
    public float currentFuel;
    public float fuelDrainRate = 8f;
    public Image fuelBarFill;
    public TextMeshProUGUI fuelText;

    [Header("Game Over e Pit Stop")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText; // Opcional: mostra pontuação final na tela de Game Over
    private bool isGameOver = false;
    public bool isPitStopping = false;

    void Awake() { if (Instance == null) Instance = this; }

    void Start()
    {
        Time.timeScale = 1f;
        currentFuel = maxFuel;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateBackgroundSprite();
    }

    void Update()
    {
        if (isGameOver) return;

        // Pausa o mundo e abastece no Pit Stop
        if (isPitStopping)
        {
            currentFuel += 40f * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            UpdateFuelUI();

            if (currentFuel >= maxFuel)
            {
                isPitStopping = false;
            }
            return;
        }

        // Pontuação e troca de bioma
        score += Time.deltaTime * 10f;
        if (scoreText != null) scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();

        if (biomeSprites != null && biomeSprites.Length > 0)
        {
            currentBiomeIndex = Mathf.Min(Mathf.FloorToInt(score / scoreToNextPhase), biomeSprites.Length - 1);
            
            // Se mudou de bioma, zera o contador de postos daquela fase
            if (currentBiomeIndex != lastBiomeIndex)
            {
                gasStationsInCurrentBiome = 0;
                lastBiomeIndex = currentBiomeIndex;
            }

            UpdateBackgroundSprite();
        }

        ScrollBackground();

        // Consumo de combustível
        currentFuel -= fuelDrainRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateFuelUI();

        if (currentFuel <= 0) GameOver();
    }

    // ---------- Regras dos Postos (usadas pelo RoadScroller) ----------

    // Regras para poder aparecer um posto (por bioma, independente do combustível):
    // 1. Menos de maxGasStationsPerBiome postos neste bioma
    // 2. Pelo menos minNormalBlocksBetweenStations blocos normais desde o último posto
    // 3. Nenhum posto na tela no momento
    // 4. O 1º posto do bioma tem 100% de chance; o 2º tem 25% (secondStationChance)
    public bool CanSpawnGasStation()
    {
        bool baseRulesMet = gasStationsInCurrentBiome < maxGasStationsPerBiome
                         && normalBlocksSinceLastStation >= minNormalBlocksBetweenStations
                         && !isGasStationOnScreen;

        if (!baseRulesMet) return false;

        return gasStationsInCurrentBiome == 0 || Random.value <= secondStationChance;
    }

    // Retorna o sprite do bioma atual, ou null se não existir
    public Sprite GetBiomeSprite(Sprite[] sprites) =>
        sprites != null && currentBiomeIndex < sprites.Length ? sprites[currentBiomeIndex] : null;

    public void RegisterGasStation()
    {
        gasStationsInCurrentBiome++;
        normalBlocksSinceLastStation = 0;
        isGasStationOnScreen = true;
    }

    public void RegisterNormalBlock() => normalBlocksSinceLastStation++;

    public void ClearGasStation() => isGasStationOnScreen = false;

    void UpdateBackgroundSprite()
    {
        Sprite bgSprite = GetBiomeSprite(backgroundSprites);
        if (backgroundRenderer == null || bgSprite == null) return;

        if (backgroundRenderer.sprite != bgSprite)
        {
            backgroundRenderer.sprite = bgSprite;
            backgroundRenderer.drawMode = SpriteDrawMode.Tiled;
            backgroundRenderer.size = BackgroundTiledSize;
            backgroundRenderer.transform.localScale = Vector3.one;
            backgroundRenderer.sortingOrder = -2;
        }
    }

    void ScrollBackground()
    {
        if (backgroundRenderer == null) return;

        Transform bg = backgroundRenderer.transform;
        bg.Translate(Vector3.down * backgroundScrollSpeed * Time.deltaTime, Space.World);

        // Cada tile de detalhe tem 16px (1 unidade em PPU 16), então recicla a cada 1 unidade
        if (bg.position.y <= -1f)
        {
            bg.position += Vector3.up * 1f;
        }
    }

    void UpdateFuelUI()
    {
        if (fuelBarFill != null) fuelBarFill.fillAmount = currentFuel / maxFuel;
        if (fuelText != null)
        {
            int percent = Mathf.FloorToInt((currentFuel / maxFuel) * 100f);
            fuelText.text = percent.ToString() + "%";
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (finalScoreText != null)
            finalScoreText.text = "Score Final: " + Mathf.FloorToInt(score);

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}