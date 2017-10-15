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

        private ManualStateFuture startFuture;
        private ManualStateFuture stopFuture;

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
            startFuture = base.Start();
            startFuture = new ManualStateFuture();
            socketClient.Connect();
            socketClient.onIncomingData += OnIncomingData;
            socketClient.onConnect += OnConnect;
            socketClient.onDisconnect += OnDisconnect;
            return startFuture;
        }
        public override ManualStateFuture Stop()
        {
            stopFuture = base.Stop();
            socketClient.Disconnect();
            socketClient.onIncomingData -= OnIncomingData;
            return stopFuture;
        }

        private void OnDisconnect()
        {
            socketClient.onDisconnect -= OnDisconnect;
            stopFuture.Complete();
            stopFuture = null;
        }

        private void OnConnect()
        {
            socketClient.onConnect -= OnConnect;
            startFuture.Complete();
            startFuture = null;
        }
    }
}
