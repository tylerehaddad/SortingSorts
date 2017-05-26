using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour
{
    /// <summary>
    /// Array of sprites that contain every letter
    /// sprite. Letter character to hold the letter
    /// passed from the Phrases script to determine
    /// the sprite to be used. Alphabet string
    /// stores the alphabet to use as a reference
    /// when assigning the sprite.
    /// </summary>
    

    public Sprite[] letterSprites;
    public char letter =' ';
    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	public Vector2 startPos;
    /// <summary>
    /// Iterates through alphabet in order to find
    /// which letter was assigned. Then sets
    /// SpriteRenderer of object to proper sprite.
    /// </summary>

	void Start()
	{
		gameObject.GetComponent<SpriteRenderer> ().flipY = false;
		gameObject.GetComponent<SpriteRenderer> ().flipX = true;
	}

    public void AssignLetter()
    {
		//startPos = transform.position;
        for (int i = 0; i < 26; i++)
        {
            if (letter == alphabet[i])
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = letterSprites[i];
            }
        }
    }
}
