using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour
{
    public char character;

    public Letter()
    {
        character = 'A';
    }

    public Letter(char ch)
    {
        character = ch;
    }

    public void SetChar(char ch)
    {
        character = ch;
    }
}
