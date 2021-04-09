using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class IPC : MonoBehaviour
{
    private Text rectangleText;
    private string rectangleContent;
    static Socket listener;
    private CancellationTokenSource source;
    public ManualResetEvent allDone;

    public static readonly int PORT = 9600;
    public static readonly int WAITTIME = 1;

    private string pointX;
    private Text PointText;


    IPC()
    {
        source = new CancellationTokenSource();
        allDone = new ManualResetEvent(false);
    }

    async void Start()
    {
        PointText = GameObject.Find("Canvas/PointText").GetComponent<Text>();
        rectangleText = GameObject.Find("Canvas/RectangleText").GetComponent<Text>();
        await Task.Run(() => ListenEvents(source.Token));
    }
    void Update()
    {
        rectangleText.text = rectangleContent;
        DrawPoint(int.Parse(pointX));
    }
    void DrawPoint(int x)
    {
        PointText.transform.position = new Vector3(x, Screen.height / 2, 0);
    }
    private void ListenEvents(CancellationToken token)
    {

        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, PORT);
        Debug.Log(ipAddress.ToString());
        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (!token.IsCancellationRequested)
            {
                allDone.Reset();

                print("Waiting for a connection... host :" + ipAddress.MapToIPv4().ToString() + " port : " + PORT);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                while (!token.IsCancellationRequested)
                {
                    if (allDone.WaitOne(WAITTIME))
                    {
                        break;
                    }
                }

            }

        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    void AcceptCallback(IAsyncResult ar)
    {
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        allDone.Set();

        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
    }

    void ReadCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        int read = handler.EndReceive(ar);

        if (read > 0)
        {
            state.locationDatas.Append(Encoding.ASCII.GetString(state.buffer, 0, read));
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        else
        {
            if (state.locationDatas.Length > 1)
            {
                string content = state.locationDatas.ToString();
                string[] infos = content.Split(',');
                Debug.Log(infos[0] + "   "+ infos[1]);
                rectangleContent = "X: " + infos[0] + "  Angle: " + infos[1];
                pointX = infos[0];







            }
            handler.Close();
        }
    
    }


    private void OnDestroy()
    {
        source.Cancel();
    }
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder locationDatas = new StringBuilder();
    }

}
