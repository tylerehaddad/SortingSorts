using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PhraseCreator : EditorWindow
{
	public GameManager gameManager;
	public List<int> visible;

	string newPhrase;
	string newHint;
	int newDifficulty;

	#if UNITY_EDITOR
	[MenuItem ("Window/Phrase Creator")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(PhraseCreator));
	}

	void OnGUI ()
	{
		if (gameManager != null)
		{
			GUILayout.Label ("You can use this editor to add or remove phrases from the game.");

			for(int i = 0; i < gameManager.phrases.Count; i++)
			{
				bool isVisible = false;
				if (visible.Contains (i))
				{
					isVisible = true;
				}

				isVisible = EditorGUILayout.Foldout (isVisible, "Phrase " + i);

				if (!visible.Contains (i) && isVisible)
				{
					visible.Add (i);
				} else if (visible.Contains (i) && !isVisible)
				{
					visible.Remove (i);
				}

				if (visible.Contains (i))
				{
					gameManager.hints [i] = EditorGUILayout.TextField ("Phrase: ", gameManager.hints [i]);
					gameManager.phrases [i] = EditorGUILayout.TextField ("Solution: ", gameManager.phrases [i]).ToUpper();
					GUILayout.Label ("Difficulty:");
					gameManager.difficulty [i] = EditorGUILayout.IntSlider (gameManager.difficulty [i], 0, 3);
					if (GUILayout.Button ("Remove"))
					{
						gameManager.hints.Remove (gameManager.hints[i]);
						gameManager.phrases.Remove (gameManager.phrases[i]);
						gameManager.difficulty.Remove (gameManager.difficulty[i]);
					}
				}
			}

			GUILayout.Label ("New?", EditorStyles.boldLabel);

			newHint = EditorGUILayout.TextField ("Phrase: ", newHint);
			newPhrase = EditorGUILayout.TextField ("Solution: ", newPhrase);
			GUILayout.Label ("Difficulty:");
			newDifficulty = EditorGUILayout.IntSlider (newDifficulty, 0, 3);

			if (GUILayout.Button ("Create"))
			{
				gameManager.hints.Add (newHint);
				gameManager.phrases.Add (newPhrase.ToUpper());
				gameManager.difficulty.Add (newDifficulty);

				newPhrase = null;
				newHint = null;
				newDifficulty = 0;
			}
		} else
		{
			gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
			GUILayout.Label ("I can't access a Game Manager. Try opening a different scene?");
		}
	}
	#endif
}
