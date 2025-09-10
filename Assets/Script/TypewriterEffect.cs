using TMPro;
using UnityEngine;
using System.Collections;

public class MultiTypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textBoxes; // 위에서부터 순서대로 넣기
    [SerializeField] private float typingSpeed = 0.05f;   // 글자 출력 속도
    [SerializeField] private AudioClip keySound;          // 키보드 소리

    private AudioSource audioSource;
    private string[] fullTexts; // 원래 텍스트 저장용

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // 원래 텍스트 저장 후, 일단 전부 숨기기
        fullTexts = new string[textBoxes.Length];
        for (int i = 0; i < textBoxes.Length; i++)
        {
            fullTexts[i] = textBoxes[i].text; // 원본 저장
            textBoxes[i].text = "";           // 초기화
        }
    }

    private void Start()
    {
        StartCoroutine(PlayAllTextSequentially());
    }

    private IEnumerator PlayAllTextSequentially()
    {
        for (int t = 0; t < textBoxes.Length; t++)
        {
            var textUI = textBoxes[t];
            string fullText = fullTexts[t];

            for (int i = 0; i < fullText.Length; i++)
            {
                textUI.text = fullText.Substring(0, i + 1);

                // 효과음: 50% 확률로만 재생 + 랜덤 볼륨
                if (keySound != null && Random.value > 0.5f)
                {
                    audioSource.volume = Random.Range(0.2f, 0.5f);
                    audioSource.PlayOneShot(keySound);
                }

                // 출력 속도도 살짝 랜덤하게 주면 자연스러움
                yield return new WaitForSeconds(typingSpeed + Random.Range(-0.01f, 0.02f));
            }

            // 현재 박스 다 끝나고 잠깐 기다린 뒤 다음 박스로
            yield return new WaitForSeconds(0.5f);
        }
    }
}
