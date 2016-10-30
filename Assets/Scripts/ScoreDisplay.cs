using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
    public Sprite[] digits;
    public GameObject digitPrefab;

    public static float distance;

	// Use this for initialization
	void Start () {
        CreateString(((int)(distance)).ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateString(string s) {
        float x = 0;
        foreach (char c in s) {
            Sprite spr = digits[(int)char.GetNumericValue(c)];

            GameObject img = (GameObject)Instantiate(digitPrefab, transform);
            RectTransform rect = img.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x, 0);
            rect.GetComponent<Image>().sprite = spr;

            x += 40;
        }
    }
}
