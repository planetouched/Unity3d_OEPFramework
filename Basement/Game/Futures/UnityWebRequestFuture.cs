using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using UnityEngine;
using UnityEngine.Networking;

namespace Basement.Game.Futures
{
    public class UnityWebRequestFuture : Future
    {
        public UnityWebRequest request { get; private set; }
        
        private int _attemptsCount;
        private int _resendCounter;
        private AsyncOperation _asyncOperation;
        
        private readonly string _method;
        private readonly string _uri;
        private Dictionary<string, string> _headers;
        private byte[] _bodyData;
        private Dictionary<string, string> _formFields;
        private string _stringBodyData;
        private WWWForm _wwwForm;

        public UnityWebRequestFuture(string uri, string method)
        {
            _uri = uri;
            _method = method; 
        }

        public UnityWebRequestFuture SetAttemptsCount(int attemptsCount)
        {
            _attemptsCount = attemptsCount;
            return this;
        }
        
        public UnityWebRequestFuture SetHeaders(Dictionary<string, string> headers)
        {
            _headers = headers;
            return this;
        }
        
        public UnityWebRequestFuture SetBodyData(byte [] bodyData)
        {
            _bodyData = bodyData;
            return this;
        }
        
        public UnityWebRequestFuture SetStringBodyData(string stringBodyData)
        {
            _stringBodyData = stringBodyData;
            return this;
        }
        
        public UnityWebRequestFuture SetFormFields(Dictionary<string, string> formFields)
        {
            _formFields = formFields;
            return this;
        }
        
        public UnityWebRequestFuture SetWwwForm(WWWForm wwwForm)
        {
            _wwwForm = wwwForm;
            return this;
        }

        protected override void OnRun()
        {
            SendRequest();
        }

        private void SendRequest()
        {
            request?.Dispose();
            request = CreateRequest();

            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            _asyncOperation = request.SendWebRequest();
            _asyncOperation.completed += AsyncOperationOnCompleted;
        }

        private UnityWebRequest CreateRequest()
        {
            switch (_method)
            {
                case UnityWebRequest.kHttpVerbGET:
                    return UnityWebRequest.Get(_uri);

                case UnityWebRequest.kHttpVerbPUT:
                {
                    if (_bodyData != null)
                    {
                        return UnityWebRequest.Put(_uri, _bodyData);
                    }

                    if (_stringBodyData != null)
                    {
                        return UnityWebRequest.Put(_uri, _stringBodyData);
                    }
                    
                    throw new Exception("PUT without parameters");
                }

                case UnityWebRequest.kHttpVerbHEAD:
                    return UnityWebRequest.Head(_uri);

                case UnityWebRequest.kHttpVerbPOST:
                {
                    if (_stringBodyData != null)
                    {
                        return UnityWebRequest.Post(_uri, _stringBodyData);
                    }
                    
                    if (_formFields != null)
                    {
                        return UnityWebRequest.Post(_uri, _formFields);                        
                    }

                    if (_wwwForm != null)
                    {
                        return UnityWebRequest.Post(_uri, _wwwForm);
                    }
                    
                    throw new Exception("POST without parameters");
                }

                case UnityWebRequest.kHttpVerbDELETE:
                    return UnityWebRequest.Delete(_uri);
                
                default:
                    throw new Exception("Unknown method");
            }
        }

        private void AsyncOperationOnCompleted(AsyncOperation asyncOperation)
        {
            if (request.isHttpError || !string.IsNullOrEmpty(request.error))
            {
                if (_resendCounter < _attemptsCount)
                {
                    _resendCounter++;
                    SendRequest();
                }
                else
                {
                    Cancel();
                }
            }
            else
            {
                Complete();
            }
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                request.Dispose();
            }
        }
    }
}