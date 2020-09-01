using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{

	private const int MAX_CONNECTION = 100;
	
	private int port = 5701;


	private int hostId;
	private int webHostId;

	private int reliableChannel;
	private int unreliableChannel;

	private bool isStarted = false;
	private byte error;

	private void Start()
	{
	}
	private void update()
	{
		if (!isStarted)
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
