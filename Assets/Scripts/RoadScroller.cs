using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RoadScroller : MonoBehaviour
{
    private const float BlockHeight = 12f;   // Altura de cada bloco de pista
    private const float LoopDistance = 24f;  // Distância que o bloco "pula" para cima ao reciclar
    private const float ResetY = -12f;       // Posição Y em que o bloco sai da tela

    public float speed = 5f;

    private SpriteRenderer sr;
    private GameObject pitStopTrigger;
    private bool isStation;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Busca o gatilho pela Tag "PitStop" dentro do bloco pai (RoadDesert),
        // funcionando mesmo que se chame "PitStopTrigger (1)" ou seja irmão do sprite
        pitStopTrigger = FindPitStopTrigger();
        if (pitStopTrigger != null) pitStopTrigger.SetActive(false);
    }

    GameObject FindPitStopTrigger()
    {
        Transform root = transform.parent != null ? transform.parent : transform;
        foreach (Collider2D col in root.GetComponentsInChildren<Collider2D>(true))
        {
            if (col.CompareTag("PitStop")) return col.gameObject;
        }
        return null;
    }

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.isPitStopping) return;

        Transform block = transform.parent;
        block.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        if (block.position.y > ResetY) return;

        block.position += Vector3.up * LoopDistance;
        if (gm != null) RecycleBlock(gm);
    }

    void RecycleBlock(GameManager gm)
    {
        // Se este bloco que acabou de sair da tela era um posto, avisa que o posto já passou
        if (isStation) gm.ClearGasStation();

        Sprite stationSprite = gm.CanSpawnGasStation() ? gm.GetBiomeSprite(gm.gasStationSprites) : null;
        isStation = stationSprite != null;

        if (isStation) SetupStation(gm, stationSprite);
        else SetupRoad(gm);

        if (pitStopTrigger != null) pitStopTrigger.SetActive(isStation);
    }

    void SetupStation(GameManager gm, Sprite sprite)
    {
        gm.RegisterGasStation();
        sr.sprite = sprite;
        sr.drawMode = SpriteDrawMode.Simple;
        transform.localScale = new Vector3(1f, BlockHeight / sprite.bounds.size.y, 1f);
    }

    void SetupRoad(GameManager gm)
    {
        Sprite sprite = gm.GetBiomeSprite(gm.biomeSprites);
        if (sprite == null) return;

        gm.RegisterNormalBlock();
        sr.sprite = sprite;
        sr.drawMode = SpriteDrawMode.Tiled;
        // Largura nativa do sprite (4 un. para 64px, 6 un. para 96px na Highway)
        sr.size = new Vector2(sprite.bounds.size.x, BlockHeight);
        transform.localScale = Vector3.one;
    }
}
