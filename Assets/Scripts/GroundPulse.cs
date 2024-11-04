using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundPulse : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] bool enableTilemap = true;
    private TilemapRenderer _tilemapRenderer;
    private TilemapCollider2D _tilemapCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        _tilemapRenderer = _tilemap.GetComponent<TilemapRenderer>();
        _tilemapCollider2D = _tilemap.GetComponent<TilemapCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        flipState();
    }

    public void flipTilemap ()
    {
        enableTilemap = !(enableTilemap);
    }
    void flipState ()
    {
        if (enableTilemap)
        {
            _tilemapRenderer.enabled = true;
            _tilemapCollider2D.enabled = true;
            //Debug.Log("Enable tilemap");
        }
        else
        {
            _tilemapRenderer.enabled = false;
            _tilemapCollider2D.enabled = false;
            //Debug.Log("Disable tilemap");
        }
        
    }
}
