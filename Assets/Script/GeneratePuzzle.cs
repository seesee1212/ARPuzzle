using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratePuzzle : MonoBehaviour
{
    public Texture2D puzzleImage;
    public List<Sprite> slicedImages = new List<Sprite>();
    public GameObject tilePrefab;
    public GameObject completedPuzzleImage;

    public List<GameObject> tiles = new List<GameObject>();

    public int rows = 4, cols = 4;
    public static bool isSelected = false;
    public static GameObject selectedTile;
    public static Action checkAction;

    void Start()
    {
        ImageSlice();
        checkAction = CheckPuzzle;
    }

    private void ImageSlice()
    {
        int width = puzzleImage.width / rows;
        int height = puzzleImage.height / cols;

        for (int x = 0; x < rows; x++)
        {
            for (int y = cols - 1; y >= 0; y--)
            {
                Rect rect = new Rect(x * width, y * height, width, height);
                Sprite sprite = Sprite.Create(puzzleImage, rect, new Vector2(0.5f, 0.5f));
                slicedImages.Add(sprite);
            }
        }

        SetTileTexture();
    }

    private void SetTileTexture()
    {
        GameObject tileAnchor = new GameObject("Tile Anchor");

        int index = 0;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                GameObject tile = Instantiate(tilePrefab);
                tile.name = "Tile_" + index;
                tile.transform.SetParent(tileAnchor.transform);
                tile.transform.localPosition = new Vector3(x, -y) * 2.5f;

                tile.GetComponent<TileData>().answerPos = tile.transform.localPosition;
                SpriteRenderer sRenderer = tile.GetComponent<SpriteRenderer>();
                sRenderer.sprite = slicedImages[index];
                tiles.Add(tile);
                index++;
            }
        }

        tileAnchor.transform.SetParent(this.transform);
        tileAnchor.transform.localPosition = new Vector3(-0.35f, 0, 0.35f);
        tileAnchor.transform.localRotation = Quaternion.Euler(90, 0, 0);
        tileAnchor.transform.localScale = Vector3.one * 0.1f;

        StartCoroutine(PuzzleShuffle());
    }

    IEnumerator PuzzleShuffle()
    {
        for (int i = 0; i < 100; i++)
        {
            int a = Random.Range(0, tiles.Count);
            int b = Random.Range(0, tiles.Count);

            var temp = tiles[a];
            tiles[a] = tiles[b];
            tiles[b] = temp;

            ResetPosition();
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void ResetPosition()
    {
        int index = 0;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                tiles[index].transform.localPosition = new Vector3(x, -y) * 2.5f;
                index++;
            }
        }
    }

    private void CheckPuzzle()
    {
        bool isClear = true;

        foreach (var tile in tiles)
        {
            float dist = Vector3.Distance(tile.transform.localPosition, tile.GetComponent<TileData>().answerPos);
            if (dist > 0.1f)
                isClear = false;
        }

        if (isClear)
        {
            foreach (var tile in tiles)
            {
                tile.GetComponent<TileData>().outline.SetActive(true);
                tile.GetComponent<TileData>().outline.GetComponent<SpriteRenderer>().color = Color.green;
                tile.GetComponent<BoxCollider2D>().enabled = false;
            }

            Invoke("CompletedPuzzle", 2f);
        }
    }

    private void CompletedPuzzle()
    {
        foreach (var tile in tiles)
            tile.SetActive(false);

        completedPuzzleImage.SetActive(true);
    }
}
