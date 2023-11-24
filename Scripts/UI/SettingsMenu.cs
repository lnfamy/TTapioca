using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

	public Text MusicIcon;
	public Text NoMusicText;
	public Text SFXIcon;
	public Text noSFXText;
	public Button back;
	public Image bubble;
	public Text bubbleText;

	private bool musicOn = true; //thats the default
	private bool SFXOn = true; 

	void Awake(){
		noSFXText.GetComponent<Text> ().enabled = false;
		NoMusicText.GetComponent<Text> ().enabled = false;

	}

	public void SFXOnOff(){
		if (noSFXText.GetComponent<Text>().enabled==true) {
			noSFXText.GetComponent<Text> ().enabled = false;
		} else {
			noSFXText.GetComponent<Text> ().enabled = true;
		}
	}

	public void MusicOnOff ()
	{
		if (NoMusicText.GetComponent<Text>().enabled == true) {
			NoMusicText.GetComponent<Text> ().enabled = false;
		} else {
			NoMusicText.GetComponent<Text> ().enabled = true;
		}
	}

	public void EnableDisable2(bool enable){
		this.gameObject.SetActive (enable);
	}

//	public void EnableDisable(bool enableBool){
//		GetComponent<Image> ().enabled = enableBool;
//		NoMusicText.GetComponent<Text> ().enabled = enableBool;
//		MusicIcon.GetComponent<Text> ().enabled = enableBool;
//		SFXIcon.GetComponent<Text> ().enabled = enableBool;
//		noSFXText.GetComponent<Text> ().enabled = enableBool;
//		back.GetComponent<Image> ().enabled = enableBool;
//		back.GetComponent<Button> ().enabled = enableBool;
//		bubble.GetComponent<Image> ().enabled = enableBool;
//		bubbleText.GetComponent<Text> ().enabled = enableBool;
//	}

}
