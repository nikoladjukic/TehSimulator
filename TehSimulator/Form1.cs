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
            string hex = BitConverter.ToString(msg);
            hex.Replace("-", " ");
            logString("RCVD: " + hex, true);
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
            String date = DateTime.Now.ToString("HH:mm:ss.fff");
            txtLog.AppendText(date + " " + msg + Environment.NewLine);
        }

        private void logString(String msg, bool rcvd)
        {
            //setColor(rcvd);
            logString(msg);
        }

        private void setColor(Boolean rcvd)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Boolean>(setColor), new Object[] { rcvd });
                return;
            }
            if(rcvd)
            {
                txtLog.ForeColor = Color.Red;
            }
            else
            {
                txtLog.ForeColor = Color.Blue;
            }
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

        private void startMasureTmr()
        {
            if(InvokeRequired)
            {
                this.Invoke(new Action(startMasureTmr));
                return;
            }
            tmrMeasurements.Interval = 100;
            tmrMeasurements.Start();
        }

        private void startDelayTmr()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(startDelayTmr));
                return;
            }
            tmrDelay.Interval = 5000;
            tmrDelay.Start();
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
                values[0] = 20 + 148 * (index - 40) - 6 * (index - 40) ^ 2 + rnd.Next(1, 150);
                values[1] = 20 + 153 * (index - 40) - 6 * (index - 40) ^ 2 + rnd.Next(1, 150);
            }
            index++;
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
            index = 0;
            logString("START MERENJA", false);
            status = TehStatus.TEH_IN_PROGRESS;
            startDelayTmr();
            byte[] msg = getWeightMsg();
            writeWithLog(msg);
        }

        private void stopMerenja()
        {
            tmrMeasurements.Stop();
            status = TehStatus.IDLE;
            logString("STOP MERENJA", false);
        }

        private void zaustaviMerenje()
        {
            logString("ZAUSTAVI MERENJE", false);
            tmrMeasurements.Stop();
            byte[] msg = new byte[] { 0xFE, 0x4B, 0x02, 0xFF};
            writeWithLog(msg);
            status = TehStatus.IDLE;
        }

        private void odgovoriTaraMasa()
        {
            logString("ODGOVOR TARA MASA", false);
            byte[] msg = new byte[] {0xFE, 0x4B, 0x4D, 0x30, 0xFF };
            writeWithLog(msg);
        }

        private void odgovoriTaraSila()
        {
            logString("ODGOVOR TARA SILA", false);
            byte[] msg = new byte[] { 0xFE, 0x4B, 0x46, 0x30, 0x2D, 0x30, 0xFF};
            writeWithLog(msg);
        }

        private void tmrDelay_Tick(object sender, EventArgs e)
        {
            tmrDelay.Stop();
            startMasureTmr();
        }

        private void tmrMeasurements_Tick(object sender, EventArgs e)
        {
            byte[] msg = getForceMsg(calculateForce());
            writeWithLog(msg);
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
            if (status == TehStatus.IDLE)
            {
                writeWithLog(msg);
            }
        }

        private void bClearLog_Click(object sender, EventArgs e)
        {
            clearLog();
        }

        private void writeWithLog(byte[] msg)
        {
            logString("SEND: " + stringFromHex(msg), false);
            serManager.writeData(msg);
        }

        private string stringFromHex(byte[] msg)
        {
            string hex = BitConverter.ToString(msg);
            hex.Replace("-", " ");
            return hex;
        }
    }
}
