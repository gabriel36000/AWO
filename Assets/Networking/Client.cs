using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {
	private const int MAX_CONNECTION = 100;

	private int port = 5701;


	private int hostId;
	private int webHostId;

	private int reliableChannel;
	private int unreliableChannel;

	private int connectionId;
	private float connectionTime;
	private bool isConnected = false;
	private bool isStarted = false;
	private byte error;

	public void Connect()
	{
		NetworkTransport.Init();
		ConnectionConfig cc = new ConnectionConfig();

		reliableChannel = cc.AddChannel(QosType.Reliable);
		unreliableChannel = cc.AddChannel(QosType.Unreliable);

		HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

		hostId = NetworkTransport.AddHost(topo, 0);
		connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

		connectionTime = Time.time;
		isConnected = true;

	}

	private void Update()
	{
		if (!isConnected)
		return;


		int recHostId;
		int connectionId;
		int channelId;
		byte[] recBuffer = new byte[1024];
		int bufferSize = 1024;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		switch (recData) {
			case NetworkEventType.Nothing: break;
			case NetworkEventType.ConnectEvent: break;
			case NetworkEventType.DataEvent: break;
			case NetworkEventType.DisconnectEvent: break;

			case NetworkEventType.BroadcastEvent:

				break;
		}
	}
}
