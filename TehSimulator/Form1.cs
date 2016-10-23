using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TehSimulator
{
    public partial class Form1 : Form
    {

        private const int STATUS_PERIOD = 3000;
        private enum TehStatus { IDLE, TEH_IN_PROGRESS};
        private TehStatus status = TehStatus.IDLE;

        Utils.SerialManager serManager;

        private int index = 0;
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            serManager = new Utils.SerialManager();
            Utils.SerialManager.DataReceived += new Utils.SerialManager.DataReceivedDelegate(parseData);
            serManager.openSerialPort();
        }

        private void parseData(byte[] msg)
        {
            if (msg[0] == 0x4B)
            {
                switch (msg[1])
                {
                    case 1: // Start/Stop merenja
                        if (msg[2] == 0x30)
                        {
                            stopMerenja();
                        }
                        else if (msg[2] == 0x31)
                        {
                            startMerenja();
                        }
                        break;
                    case 6: // Zahtev za vremenom
                        break;
                    case 7: // Tara Masa
                        odgovoriTaraMasa();
                        break;
                    case 8: // Tara Sila
                        odgovoriTaraSila();
                        break;
                    case 9:     // Kupljenje parametara sa plocice
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // Poruka nije namenjena racunaru
            }
        }

        private void logString(String msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<String>(logString), new Object[] { msg });
                return;
            }
            txtLog.Text += msg + Environment.NewLine;
        }

        private void clearLog()
        {
            if(InvokeRequired)
            {
                this.Invoke(new Action(clearLog));
                return;
            }
            txtLog.Text = "";
        }

        private void bStartStatus_Click(object sender, EventArgs e)
        {
            tmrStatus.Interval = STATUS_PERIOD;
            tmrStatus.Start();
        }

        private void bStopStatus_Click(object sender, EventArgs e)
        {
            tmrStatus.Stop();
        }

        private int[] calculateForce()
        {
            int[] values = new int[2];
            if(index <= 40)
            {
                values[0] = 40 + rnd.Next(1, 10);
                values[1] = 40 + rnd.Next(1, 10);
            }
            else
            {
                values[0] = 20 + 150 * (index - 40) - 6 * (index - 40) ^ 2 + rnd.Next(1, 40);
                values[1] = 20 + 150 * (index - 40) - 6 * (index - 40) ^ 2 + rnd.Next(1, 40);
            }
            return values;
        }

        private byte[] getForceMsg(int[] forces)
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(0xFE);
            bytes.Add(0x4B);
            bytes.Add(0x46);
            bytes.AddRange(Encoding.ASCII.GetBytes(forces[0].ToString()));
            bytes.Add(0x2D);
            bytes.AddRange(Encoding.ASCII.GetBytes(forces[1].ToString()));
            bytes.Add(0xFF);

            return bytes.ToArray();
        }

        private byte[] getWeightMsg()
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(0xFE);
            bytes.Add(0x4B);
            bytes.Add(0x4D);
            bytes.AddRange(Encoding.ASCII.GetBytes((450 + rnd.Next(1, 100)).ToString()));
            bytes.Add(0xFF);

            return bytes.ToArray();
        }

        private void startMerenja()
        {
            status = TehStatus.TEH_IN_PROGRESS;
            tmrDelay.Interval = 5000;
            tmrDelay.Start();
        }

        private void stopMerenja()
        {
            tmrMeasurements.Stop();
            status = TehStatus.IDLE;
        }

        private void zaustaviMerenje()
        {
            tmrMeasurements.Stop();
            byte[] msg = new byte[] { 0xFE, 0x4B, 0x02, 0xFF};
            serManager.writeData(msg);
            status = TehStatus.IDLE;
        }

        private void odgovoriTaraMasa()
        {
            byte[] msg = new byte[] {0xFE, 0x4B, 0x4D, 0x30, 0xFF };
            serManager.writeData(msg);
        }

        private void odgovoriTaraSila()
        {
            byte[] msg = new byte[] { 0xFE, 0x4B, 0x46, 0x30, 0x2D, 0x30, 0xFF};
            serManager.writeData(msg);
        }

        private void tmrDelay_Tick(object sender, EventArgs e)
        {
            tmrDelay.Stop();
            tmrMeasurements.Interval = 100;
            tmrMeasurements.Start();
        }

        private void tmrMeasurements_Tick(object sender, EventArgs e)
        {
            byte[] msg = getForceMsg(calculateForce());
            serManager.writeData(msg);
            tmrMeasurements.Stop();
            tmrMeasurements.Start();
            if (index > 140)
            {
                zaustaviMerenje();
            }
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            byte[] msg;
            if (chbVoziloUValjcima.Checked)
            {
                msg = new byte[] { 0xFE, 0x4B, 0x03, 0x30, 0x30, 0x31, 0x31, 0xFF };
            }
            else
            {
                msg = new byte[] { 0xFE, 0x4B, 0x03, 0x30, 0x30, 0x30, 0x30, 0xFF };
            }
        }
    }
}
