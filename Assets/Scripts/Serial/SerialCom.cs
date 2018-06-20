using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

namespace SERIAL_ARDUINO_
{
	public class SerialCom
	{

		public static int [] previousData;


		public static void setup()
		{
			previousData = new int[2];
			previousData [0] = 0;
			previousData [1] = 0;

			createCom();
			//getData();
		}

		public static SerialPort _serialPort;

		public static void createCom()
		{
			bool flagSerial = false;
			int portCOM = 1;

//			_serialPort = new SerialPort("COM5", 9600);
//			_serialPort.ReadTimeout = 1000;
//			_serialPort.WriteTimeout = 1000;
//			_serialPort.Open();


			_serialPort = new SerialPort();
			_serialPort.BaudRate = 9600;
			while (!flagSerial) {
				portCOM++;
				_serialPort.PortName = "COM" + portCOM.ToString();
				try {
					_serialPort.Open();
					flagSerial = true;
				}
				catch{
					closePort ();
				}
			}
		}

		public static void closePort ()
		{
			_serialPort.Close ();
		}

		public static void getData()
		{
			int[] data = new int [2];
			_serialPort.Write("?");
			Debug.Log (_serialPort.IsOpen);
			byte[] bytes = new byte[10];
			_serialPort.Read (bytes, 0, 10);

			if ((char)bytes [3] == '_' && (char)bytes [7] == '#')
			{
				string light =((char) bytes [0]).ToString () + ((char) bytes [1]).ToString () + ((char) bytes [2]).ToString ();
				string sound =((char) bytes [4]).ToString () + ((char) bytes [5]).ToString () + ((char) bytes [6]).ToString ();
				data [0] = Convert.ToInt32 (light);
				data [1] = Convert.ToInt32 (sound);
			}
			else
			{
				data = previousData;
			}


			previousData = data;
//			Debug.Log ("light: " + data[0]);
//			Debug.Log ("sound: " + data[1]);
//
//			for (int index = 0; index < 10; index++)
//			{
//				char letra = (char)bytes [index];
//				Debug.Log (index + ": " + letra);
//			}
		}   
	}
}