using System;
using OEPFramework.common.pipeline;
using OEPFramework.common.service;
using OEPFramework.common.service.future;

namespace OEPFramework.service.socketService
{
    public class SocketService : ServiceBase
    {
        private readonly SocketClient socketClient;
        public event Action<SocketClient, byte[]> onIncomingData;
        private Pipeline pipeline;

        private SocketService(SocketClient socketClient)
        {
            this.socketClient = socketClient;
        }

        public void SetPipeline(Pipeline imcomingDataPipeline)
        {
            pipeline = imcomingDataPipeline;
        }
        
        private void OnIncomingData(byte[] inboundBytes)
        {
            if (onIncomingData != null)
                onIncomingData(socketClient, inboundBytes);

            if (pipeline != null)
                pipeline.DoPipeline(inboundBytes);
        }

        public override ManualStateFuture Start()
        {
            CheckState();
            startFuture = new ManualStateFuture();
            socketClient.Connect();
            socketClient.onIncomingData += OnIncomingData;
            socketClient.onConnect += OnConnect;
            socketClient.onDisconnect += OnDisconnect;
            return startFuture;
        }
        public override ManualStateFuture Stop()
        {
            CheckState();
            stopFuture = new ManualStateFuture();
            socketClient.Disconnect();
            socketClient.onIncomingData -= OnIncomingData;
            return stopFuture;
        }

        public override ManualStateFuture Destroy()
        {
            return Stop();
        }

        private void OnDisconnect()
        {
            socketClient.onDisconnect -= OnDisconnect;
            stopFuture.Complete();
        }

        private void OnConnect()
        {
            socketClient.onConnect -= OnConnect;
            startFuture.Complete();
        }
    }
}
