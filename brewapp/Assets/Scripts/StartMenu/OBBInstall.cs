// This script written by Richard Grable
using UnityEngine;
using System.Collections;

public class OBBInstall : MonoBehaviour
{
#if UNITY_ANDROID
    private string expPath;
    private string logtxt;
    private bool alreadyLogged = false;
    private bool downloadStarted;

    void log(string t)
    {
        logtxt += t + "\n";
        print("MYLOG " + t);
    }
    void OnGUI()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            Application.LoadLevel(1);
            return;
        }


        if (!GooglePlayDownloader.RunningOnAndroid())
        {
            GUI.Label(new Rect(10, 10, Screen.width - 10, 20), "Use GooglePlayDownloader only on Android device!");
            return;
        }
        expPath = GooglePlayDownloader.GetExpansionFilePath();
        if (expPath == null)
        {
            GUI.Label(new Rect(10, 10, Screen.width - 10, 20), "External storage is not available!");
        }
        else
        {
            string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
            if (alreadyLogged == false)
            {
                alreadyLogged = true;
                log("expPath = " + expPath);
                log("Main = " + mainPath);
                log("Main = " + mainPath.Substring(expPath.Length));

                if (mainPath != null)
                    StartCoroutine(loadLevel());

            }
            //GUI.Label(new Rect(10, 10, Screen.width-10, Screen.height-10), logtxt );

            if (mainPath == null)
            {
                GUI.Label(new Rect((Screen.width / 2) - 300, Screen.height / 2, 600, 80), "The game needs to download over 100MB of game content. It's recommanded to use WiFi connection.");
                if (GUI.Button(new Rect((Screen.width / 2) - 150, (Screen.height / 2) + 50, 300, 100), "Start Download !"))
                {
                    GooglePlayDownloader.FetchOBB();
                    StartCoroutine(loadLevel());
                }
            }

        }

    }
    protected IEnumerator loadLevel()
    {
        string mainPath;
        do
        {
            yield return new WaitForSeconds(0.5f);
            mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            log("waiting mainPath " + mainPath);
        }
        while (mainPath == null);

        if (downloadStarted == false)
        {
            downloadStarted = true;

            string uri = "file://" + mainPath;
            log("downloading " + uri);
            WWW www = WWW.LoadFromCacheOrDownload(uri, 0);

            // Wait for download to complete
            yield return www;

            if (www.error != null)
            {
                log("wwww error " + www.error);
            }
            else
            {
                Application.LoadLevel(1);
            }
        }
    }
#endif
}
