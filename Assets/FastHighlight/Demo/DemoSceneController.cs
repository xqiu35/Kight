using UnityEngine;
using Merlin.FastHighlight;

public class DemoSceneController : MonoBehaviour 
{
	void Start () 
	{
    	Highlighter[] highlightersOnScene = FindObjectsOfType(typeof (Highlighter)) as Highlighter[];
    	foreach (Highlighter highlighter in highlightersOnScene)
    	{
    		highlighter.ConstantOn(Color.red);
    	}
	}
}
