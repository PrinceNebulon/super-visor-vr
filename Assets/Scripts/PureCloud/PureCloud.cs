﻿using Newtonsoft.Json;
using PureCloudPlatform.Client.V2.Model;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.PureCloud
{
    public class PureCloud
    {
        private RestClient client = new RestClient("https://api.mypurecloud.com");
        private RestClient authClient = new RestClient("https://login.mypurecloud.com");

        private string accessToken { get; set; }

        public List<User> Users
        {
            get
            {
                return _users;
            }
            private set
            {
                _users = value;
            }
        }


        private static PureCloud _instance;
        private List<User> _users = new List<User>();

        public static PureCloud Instance
        {
            get
            {
                if (_instance == null) _instance = new PureCloud();

                return _instance;
            }
        }

        private PureCloud()
        {
            ServicePointManager.MaxServicePointIdleTime = 0;
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            //var auth = System.Convert.ToBase64String(
            //        System.Text.Encoding.GetEncoding("ISO-8859-1")
            //          .GetBytes("7de3af06-c0b3-4f9b-af45-72f4a14037cc:1duphi_YtswNjN2GXOg_APY-KKTmnYXvfNj7NnHUhnM")
            //        );
            //Debug.Log(auth);

            //Debug.Log("Preparing to log in...");
            //var authRequest = new RestRequest("oauth/token", Method.POST);
            //authRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //authRequest.AddHeader("Authorization", "Basic " + auth);
            //authRequest.AddParameter("grant_type", "client_credentials");

            //Debug.Log("Logging in...");
            //var response = authClient.Execute(authRequest);

            //Debug.Log("Auth done");
            //Debug.Log(response.Content);
            //Debug.Log(response.ResponseStatus);
            //Debug.Log(response.Headers);
            //Debug.Log(response.ErrorMessage);
            //Debug.Log(response.ErrorException);

            //var authResponse = JsonConvert.DeserializeObject<AuthResponse>(response.Content);
            //Debug.Log(authResponse);
            //this.accessToken = authResponse.access_token;
            //Debug.Log("Access token=" + this.accessToken);



            //Debug.Log("Getting users...");
            //var usersRequest = new RestRequest("api/v2/users", Method.POST);
            //usersRequest.AddHeader("authorization", "bearer " +this.accessToken);
            //usersRequest.AddHeader("content-type", "application/json");
            //var response2 = client.Execute(usersRequest);

            //Debug.Log("User list done");
            //Debug.Log(response2.Content);
            //Debug.Log(response2.ResponseStatus);
            //Debug.Log("Response headers: " + response2.Headers.Count);
            //foreach(var header in response2.Headers)
            //{
            //    Debug.Log(header);
            //}
            //Debug.Log(response2.ErrorMessage);
            //Debug.Log(response2.ErrorException);

            //var usersResponse = JsonConvert.DeserializeObject<UserEntityListing>(response2.Content);
            //Debug.Log(usersResponse);
            //Debug.Log(JsonConvert.SerializeObject(usersResponse, Formatting.Indented));
            

            if (String.IsNullOrEmpty(PureCloudSettings.Instance.Get("authToken")))
            {
                Debug.Log("Getting new access token");
                var request = (HttpWebRequest)WebRequest.Create("https://login.mypurecloud.com/oauth/token");

                var postData = "grant_type=client_credentials";
                var data = Encoding.ASCII.GetBytes(postData);


                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                var basicAuth = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(PureCloudSettings.Instance.Get("clientId") + ":" + PureCloudSettings.Instance.Get("clientSecret")));
                request.Headers.Add("Authorization", basicAuth);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseString);
                //Debug.Log(authResponse);
                this.accessToken = authResponse.access_token;
                Debug.Log("Access token=" + this.accessToken);
            }
            else
            {
                this.accessToken = PureCloudSettings.Instance.Get("authToken");
                Debug.Log("Using existing access token");
            }
        }

        public IEnumerator GetUsers(int pageNumber = 1)
        {
            return GetRequest("https://api.mypurecloud.com/api/v2/users?pageSize=100&pageNumber=" + pageNumber);
        }

        IEnumerator GetRequest(string uri)
        {
            if (this.accessToken == null)
            {
                Debug.LogWarning("Access token is not set!");
                yield return null;
            }
            Debug.Log("Making request");
            var myWebRequest = UnityWebRequest.Get(uri);
            myWebRequest.SetRequestHeader("Authorization", "bearer " + this.accessToken);
            yield return myWebRequest.SendWebRequest();

            Debug.Log("isHttpError=" + myWebRequest.isHttpError);
            Debug.Log("isNetworkError=" + myWebRequest.isNetworkError);
            Debug.Log("text=" + myWebRequest.downloadHandler.text);
            Debug.Log("data length=" + myWebRequest.downloadHandler.data.Length);

            var users = JsonConvert.DeserializeObject<UserEntityListing>(myWebRequest.downloadHandler.text);
            this.Users.AddRange(users.Entities);
            Debug.Log("Got " + users.Entities.Count + " users, total: " + this.Users.Count);

            if (users.PageNumber < users.PageCount)
            {
                Debug.Log("Getting more users");
                yield return GetUsers((int)users.PageNumber + 1);
            }
        }

        // https://stackoverflow.com/a/33391290/1124338
        public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Debug.Log("Checking cert");
            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }
    }
}
