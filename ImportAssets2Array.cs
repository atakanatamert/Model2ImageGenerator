using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ImportAssets2Array : MonoBehaviour {

    public GameObject[] importArray;

	// Use this for initialization
	void Start () {
        ImportAssets();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ImportAssets(){
        //var info = new DirectoryInfo(PlayerPrefs.GetString("3DFilePath"));
        var info = new DirectoryInfo("/users/AtakanAtamert/Downloads/FBX-Files");
        var fileInfo = info.GetFiles();
        foreach (FileInfo currFile in fileInfo){
            Debug.Log(currFile);
           // GameObject x = AssetDatabase.LoadAssetAtPath<GameObject>(currFile.ToString());
           // Debug.Log(x.name);
           // AssetDatabase.ImportAsset(currFile.ToString(), ImportAssetOptions.Default);
        }
        //AssetDatabase.ImportAsset("", ImportAssetOptions.Default);
    }
    
}
