using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PhraseCreator : EditorWindow
{
	public GameManager gameManager;

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
				GUILayout.Label ("Phrase " + i, EditorStyles.boldLabel);
				gameManager.hints [i] = EditorGUILayout.TextField ("Phrase: ", gameManager.hints [i]);
				gameManager.phrases [i] = EditorGUILayout.TextField ("Solution: ", gameManager.phrases [i]);
				GUILayout.Label ("Difficulty:");
				gameManager.difficulty [i] = EditorGUILayout.IntSlider (gameManager.difficulty [i], 0, 3);
			}
		} else
		{
			gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
			GUILayout.Label ("I can't access a Game Manager. Try opening a different scene?");
		}
	}
}
