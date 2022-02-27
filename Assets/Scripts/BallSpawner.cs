using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    static public BallSpawner instance;
    private void Start()
    {
        instance = this;
        Balls balls;
        try
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://localhost:3000/balls");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                balls = JsonUtility.FromJson<Balls>(reader.ReadToEnd());
            }

            foreach (var ball in balls.balls)
            {
                Instantiate(ballPrefab, new Vector3(ball.posX,ball.posY,ball.posZ),Quaternion.identity);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("Impossibile connettersi");
        }

    }
    public void StartRemoveFunction(float posX, float posY, float posZ)
    {
        StartCoroutine(RemoveBallFromServer(posX, posY, posZ));
    }
    IEnumerator RemoveBallFromServer(float posX,float posY,float posZ)
    {
        Dictionary<string, string> requestParams = new Dictionary<string, string>();
        requestParams.Add("posX", posX.ToString());
        requestParams.Add("posY", posY.ToString());
        requestParams.Add("posZ", posZ.ToString());

        UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/remove-balls", requestParams);

        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);
    }
}
