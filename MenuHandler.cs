using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public void LoadMainGenerator(){
        //Debug.Log(PlayerPrefs.GetString("3DFilePath"));
        SceneManager.LoadScene("SampleScene");
    }

    public void GetTextFieldInput(string input){
        PlayerPrefs.SetString("3DFilePath", input);
    }

}
