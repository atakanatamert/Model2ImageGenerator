using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class Process : MonoBehaviour {

    public List<GameObject> TestInput;
    List<string> files;
    List<int> numbers;
    public int RotationAmount = 5;
    GameObject tempObj;
    public Camera currentCamera;
    int nameCounter, fileCounter, rotationCounter, objCounter;
    bool flag, continueFlag, rotX, rotY, rotZ, rotXY, rotXZ, rotYZ, rotXYZ;
    Coroutine takeSamples;
    const int captureWidth = 1920;
    const int captureHeight = 1080;
    Rect rect;
    RenderTexture renderTexture;
    Texture2D screenShot;
    byte[] bytes;
    string DesktopPath, currDirectory;

    // Use this for initialization
    void Start(){
        rotationCounter = 2520;
        //tempObj = Instantiate(TestInput[fileCounter], new Vector3(0, 0, 0), TestInput[fileCounter].transform.rotation);
        //tempObj.name = "RotationObject";
        numbers = new List<int>();
        takeSamples = StartCoroutine(TakeScreenshotSamples());
        rotX = rotY = rotZ = rotXY = rotXZ = rotYZ = rotXYZ = true;
    }

    void Update(){
        if(Input.GetMouseButtonDown(0) && flag){
            Time.timeScale = 1;
            continueFlag = true;
            flag = false;
        }

        if(continueFlag){
            for (int i = 0; i < 7; i++){
                files = new List<string>();

                var info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/TestScreenshots");

                FileInfo[] fileInfo = info.GetFiles();
                foreach (FileInfo file in fileInfo){
                    files.Add(file.Name);
                    // Debug.Log(file);
                }
                numbers.Add(i);
            }

            if (files.Contains(".DS_Store")){
                files.Remove(".DS_Store");
            }

            List<int> itemsToRemove = new List<int>();
            int[] referenceNumStorage = new int[PlayerPrefs.GetInt("FileCountReference")];

            for (int x = 0; x < referenceNumStorage.Length; x++){
                referenceNumStorage[x] = x;
            }

            foreach (string currFile in files){
                int parsed;
                string tmp = currFile.Replace(".png", "");
                int.TryParse(tmp, out parsed);

                if (numbers.Contains(parsed)){
                    itemsToRemove.Add(parsed);
                }
            }

            //var intStorage = new int[] {0, 1, 2, 3, 4, 5};

            List<int> difference = referenceNumStorage.Except(itemsToRemove).ToList();

            //Debug.Log("Size of difference is: " + difference.Count);

            foreach (int remove in difference.OrderByDescending(v => v)){
                //Those are the items LEFT
                //Debug.Log("One of the element to be removed is: " + remove);
                if (TestInput.ElementAt(remove) != null){
                    TestInput.RemoveAt(remove);
                }
            }

            StopCoroutine(takeSamples);
            Destroy(tempObj);
            StartCoroutine(TakeScreenshotRotational());
            //Debug.Log("Not Continueing!");
            continueFlag = false;
        }
    }

    IEnumerator TakeScreenshotSamples(){
        yield return new WaitForEndOfFrame();

        //yield return new WaitForSeconds(2);

        if(fileCounter == 0){
            tempObj = Instantiate(TestInput[fileCounter], new Vector3(0, 0, 0), TestInput[fileCounter].transform.rotation);
        }

        rect = new Rect(0, 0, captureWidth, captureHeight);
        renderTexture = new RenderTexture(captureWidth, captureHeight, 32);
        screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGBA32, false);

        currentCamera.targetTexture = renderTexture;
        currentCamera.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        currentCamera.targetTexture = null;
        RenderTexture.active = null;

        bytes = screenShot.EncodeToPNG();
        DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        currDirectory = DesktopPath + "/TestScreenshots/";

        if (!Directory.Exists(currDirectory))
            Directory.CreateDirectory(DesktopPath + "/TestScreenshots/");


        File.WriteAllBytes(DesktopPath + "/TestScreenshots/" + fileCounter + ".png", bytes);

        Destroy(tempObj);

        fileCounter++;

        if(fileCounter < TestInput.Count){
            tempObj = Instantiate(TestInput[fileCounter], new Vector3(0, 0, 0), TestInput[fileCounter].transform.rotation);
            //tempObj.name = "RotationObject";
            StartCoroutine(TakeScreenshotSamples());
        }else{
            Time.timeScale = 0;
            flag = true;
            Debug.Log("-----> Clear the invalid images from Desktop/TestScreenshots, after that click on editor to continue");
            PlayerPrefs.SetInt("FileCountReference", fileCounter);
            yield return null;
            //StartCoroutine(QuitApplication());
        }
    }

    IEnumerator TakeScreenshotRotational(){
        while(true){
            if (GameObject.Find("RotationObject") == null){
                Debug.Log("Created initial object!");
                GameObject tmpObj = Instantiate(TestInput[objCounter], new Vector3(0, 0, 0), transform.rotation);
                tmpObj.name = "RotationObject";
            }
            yield return new WaitForEndOfFrame();

            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 32);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGBA32, false);

            currentCamera.targetTexture = renderTexture;
            currentCamera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            currentCamera.targetTexture = null;
            RenderTexture.active = null;

            bytes = screenShot.EncodeToPNG();
            DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            currDirectory = DesktopPath + "/Screenshots/" + objCounter + "/";

            if (!Directory.Exists(currDirectory))
                Directory.CreateDirectory(DesktopPath + "/Screenshots/" + objCounter + "/");
            File.WriteAllBytes(DesktopPath + "/Screenshots/" + objCounter + "/" + nameCounter + ".png", bytes);

            nameCounter++;

            if (rotationCounter > 0){
                //rotationCounter--;
                rotationCounter -= RotationAmount;
                Resources.UnloadUnusedAssets();
                //Debug.Log(rotationCounter);
                if (rotX){
                    RotateWithAxis("X", GameObject.Find("RotationObject"));
                    rotX = !(rotationCounter % 360 == 0);
                }
                else if (rotY){
                    RotateWithAxis("Y", GameObject.Find("RotationObject"));
                    rotY = !(rotationCounter % 360 == 0);
                }
                else if (rotZ){
                    RotateWithAxis("Z", GameObject.Find("RotationObject"));
                    rotZ = !(rotationCounter % 360 == 0);
                }
                else if (rotXY){
                    RotateWithAxis("XY", GameObject.Find("RotationObject"));
                    rotXY = !(rotationCounter % 360 == 0);
                }
                else if (rotYZ){
                    RotateWithAxis("YZ", GameObject.Find("RotationObject"));
                    rotYZ = !(rotationCounter % 360 == 0);
                }
                else if (rotXZ){
                    RotateWithAxis("XZ", GameObject.Find("RotationObject"));
                    rotXZ = !(rotationCounter % 360 == 0);
                }
                else if (rotXYZ){
                    RotateWithAxis("XYZ", GameObject.Find("RotationObject"));
                    rotXYZ = !(rotationCounter % 360 == 0);
                }

            }
            else{
                nameCounter = 0;
                if (!rotX && !rotY && !rotZ){
                    if (TestInput.Count - 1 > objCounter){
                        objCounter++;
                        Destroy(GameObject.Find("RotationObject"));

                        //Destroy(rect);
                        try{
                            GameObject temp2 = Instantiate(TestInput[objCounter], new Vector3(0, 0, 0), TestInput[objCounter].transform.rotation);
                            temp2.name = "RotationObject";
                            //objCounter++;
                            rotationCounter = 2520;
                            rotX = rotY = rotZ = rotXY = rotXZ = rotYZ = rotXYZ = true;

                            RotateWithAxis("X", temp2);
                            /*
                            if (rotX){
                                RotateX(temp2);
                                rotX = !(rotationCounter % 360 == 0);
                            }
                            else if (rotY){
                                RotateY(temp2);
                                rotY = !(rotationCounter % 360 == 0);
                            }
                            else if (rotZ){
                                RotateZ(temp2);
                                rotZ = !(rotationCounter % 360 == 0);
                            }*/

                        }
                        catch (IndexOutOfRangeException){
                            Debug.Log("Requsted array component do not exist!");
                            UnityEditor.EditorApplication.isPlaying = false;
                            Application.Quit();
                        }
                        catch (ArgumentOutOfRangeException){
                            //UnityEditor.EditorApplication.isPlaying = false;
                            //Application.Quit();
                            //Debug.Log(currDirectory);
                            //DeleteDirectory(currDirectory);
                            System.Diagnostics.Process.Start(@"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal", @"/Users/AtakanAtamert/Desktop/ImageEdgeDetection.sh");
                            StartCoroutine(QuitApplication());
                            continue;
                        }


                    }
                    else{
                        Debug.Log(currDirectory);
                        DeleteDirectory(currDirectory);
                        System.Diagnostics.Process.Start(@"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal", @"/Users/AtakanAtamert/Desktop/ImageEdgeDetection.sh");
                        StartCoroutine(QuitApplication());
                        yield return null;
                        break;
                    }
                }
       //
            }
        }
    }

    /*
    void RotateX(GameObject obj)
    {
        obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(5, 0, 0));
    }

    void RotateY(GameObject obj){
        //GameObject.Find("RotationObject").transform.rotation = GameObject.Find("RotationObject").transform.rotation * Quaternion.Euler(new Vector3(0, 5, 0));
        obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(0, 5, 0));
        //StopCoroutine(takeRotational);
        //((IDisposable)TakeScreenshotRotational()).Dispose();
        //takeRotational = StartCoroutine(TakeScreenshotRotational());
    }

    void RotateZ(GameObject obj){
        obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 5));
    }
    */


    void RotateWithAxis(string axis, GameObject obj){
        switch (axis){
            case "X":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(5, 0, 0));
                break;
            case "Y":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(0, 5, 0));
                break;
            case "Z":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 5));
                break;
            case "XY":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(5, 5, 0));
                break;
            case "XZ":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(5, 0, 5));
                break;
            case "YZ":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(0, 5, 5));
                break;
            case "XYZ":
                obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(new Vector3(5, 5, 5));
                break;
            default:
                Debug.Log("An error occured while coosing rotation!");
                break;
        }

    }

    public static void DeleteDirectory(string target){
        string[] files = Directory.GetFiles(target);
        string[] dirs = Directory.GetDirectories(target);

        foreach (string file in files){
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs){
            DeleteDirectory(dir);
        }

        Directory.Delete(target, false);
    }

    IEnumerator QuitApplication(){
        yield return new WaitForSeconds(3);
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


}
