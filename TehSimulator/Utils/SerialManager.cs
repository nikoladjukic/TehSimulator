using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace TehSimulator.Utils
{
    class SerialManager
    {
        public delegate void DataReceivedDelegate(byte[] data);
        public static event DataReceivedDelegate DataReceived;

        private static SerialPort serialPort;

        public SerialManager()
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort();
                openSerialPort();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            }
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int brojBajtova = 0;
            byte[] buffer = new byte[128]; // TODO: Proveriti koliki nam bafer treba

            byte byteRead = (byte)serialPort.ReadByte();
            try
            {
                // Da li je pocetak poruke?
                if (byteRead == 0xFE)   //TODO: Sta ako jedna poruka ne stigne u jednom paketu (u dva poziva stigne)?
                {
                    while ((byteRead = (byte)serialPort.ReadByte()) != 0xFF)
                    {
                        buffer[brojBajtova++] = byteRead;
                    }
                }

                if (brojBajtova > 0)
                {
                    byte[] receivedMsg = new byte[brojBajtova];
                    for (int i = 0; i < brojBajtova; i++)
                    {
                        receivedMsg[i] = buffer[i];
                    }
                    serialPort.DiscardInBuffer();
                    if (DataReceived != null)
                    {
                        DataReceived(receivedMsg);
                    }
                }
                else
                {
                    serialPort.DiscardInBuffer();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Greška u serijskoj komunikaciji: " + exc.Message);
            }
        }

        public void writeData(byte[] msg)
        {
            openSerialPort();
            serialPort.Write(msg, 0, msg.Length);
        }

        public void openSerialPort()
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = "COM7";
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = System.IO.Ports.Parity.None;
                    serialPort.StopBits = System.IO.Ports.StopBits.One;
                    serialPort.Handshake = System.IO.Ports.Handshake.None;

                    serialPort.Open();
                    serialPort.DiscardInBuffer();
                }
                catch (Exception e)
                {
                    //TODO da li logovati ili ispisati drugu gresku
                    MessageBox.Show("Greška prilikom otvaranja porta: \n" + e.Message);
                }
            }
            else
            {
                //TODO Sta da radim ako je port vec otvoren?
                //1. Zatvori-Otvori ponovo?
                //2. Ignorisi OK je stanje tako?
            }
        }

        public void closeSerialPort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}
