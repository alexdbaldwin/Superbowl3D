using UnityEngine;
using System.Collections;

/// <summary>
/// Splash screen. Attach to your gameobject (or GlobalStorage for general use)
/// Call Show()/Hide() at will.
/// Set text or texture at runtime or in Unity
/// </summary>
public class SplashScreen : MonoBehaviour {
	
	public GUIStyle TextStyle;
	private const string defaultText = "SplashScreen";
	public string text = null;
	public bool ShowAtStart = false;
	private bool showing, hasTexture = false, hasText = false;
	private Vector2 centerScreen;
	public Texture texture = null;
	private float textScale;

	// Use this for initialization
	void Start () {
		textScale = (TextStyle.fontSize * (Screen.width * 0.001f));
		TextStyle.fontSize = (int)textScale;
		centerScreen = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
		TextStyle.alignment = TextAnchor.MiddleCenter;
		showing = ShowAtStart;
		if (texture != null) {
			hasTexture = true;
				}
		if (text != null) {
			hasText = true;
				}
	}

	public void SetText(string text)
	{
		this.text = text;
		hasText = true;
	}

	public void SetTexture(Texture texture)
	{
		this.texture = texture;
		hasTexture = true;
	}

	public void Show()
	{
		showing = true;
	}

	public void Hide()
	{
		showing = false;
	}

	public bool IsActive()
	{
		return showing;
	}

	void OnGUI()
	{
		if (showing) {
			if(hasTexture)
			{
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
			}

			if(hasText)
			{
				GUI.Label(new Rect(centerScreen.x, centerScreen.y, 0, 0), text, TextStyle);
			}

		}
	}
}
