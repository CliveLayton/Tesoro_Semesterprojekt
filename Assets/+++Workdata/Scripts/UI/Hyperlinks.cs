using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlinks : MonoBehaviour
{
    /// <summary>
    /// opens link 
    /// </summary>
    /// <param name="link">URL to site</param>
    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }
}
