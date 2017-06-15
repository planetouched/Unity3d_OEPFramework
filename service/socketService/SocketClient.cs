using System;
using System.Net;
using System.Net.Sockets;

namespace OEPFramework.service.socketService
{
    public class SocketClient
    {
        private Socket socket;
        public bool isConnected { get { return socket.Connected; } }
        readonly byte[] buffer;
        public event Action<byte[]> onIncomingData;
        public event Action onConnect;
        public event Action onDisconnect;
        private readonly IPEndPoint remoteEndPoint;

        public SocketClient(byte[] ip, int port, int bufferSize = 1024)
        {
            buffer = new byte[bufferSize];
            remoteEndPoint = new IPEndPoint(new IPAddress(ip), port);
        }

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(remoteEndPoint, ConnectCallback, null);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            socket.EndConnect(ar);
            StartListering();

            if (onConnect != null)
                onConnect();
        }

        public void Send(byte[] bytes)
        {
            socket.BeginSend(bytes, 0, bytes.Length, 0, SendCallback, socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            socket.EndSend(ar);
        }

        void StartListering()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, 0, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = socket.EndReceive(ar);

            if (bytesRead > 0)
            {
                var bytes = new byte[bytesRead];
                Array.Copy(buffer, bytes, bytesRead);

                if (onIncomingData != null)
                    onIncomingData(bytes);

                socket.BeginReceive(buffer, 0, buffer.Length, 0, ReceiveCallback, null);
            }
        }

        public void Disconnect()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(true, Callback, socket);
        }

        private void Callback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndDisconnect(ar);

            if (onDisconnect != null)
                onDisconnect();
        }
    }
}
