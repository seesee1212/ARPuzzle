using UnityEngine;

public class TileData : MonoBehaviour
{
    public Vector3 answerPos;
    public GameObject outline;

    void Start()
    {
        outline = transform.GetChild(0).gameObject;
    }

    void OnMouseDown()
    {
        if (!GeneratePuzzle.isSelected)
        {
            outline.SetActive(true);
            GeneratePuzzle.isSelected = true;
            GeneratePuzzle.selectedTile = this.gameObject;
        }
        else
        {
            float dist = Vector3.Distance(transform.position, GeneratePuzzle.selectedTile.transform.position);

            if (dist < 0.3f)
            {
                GeneratePuzzle.selectedTile.GetComponent<TileData>().outline.SetActive(false);

                var temp = transform.position;
                transform.position = GeneratePuzzle.selectedTile.transform.position;
                GeneratePuzzle.selectedTile.transform.position = temp;

                GeneratePuzzle.isSelected = false;
                GeneratePuzzle.selectedTile = null;

                GeneratePuzzle.checkAction?.Invoke();
            }
        }
    }
}