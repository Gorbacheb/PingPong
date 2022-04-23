using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPong
{
    public partial class l : Form
    {
        public l()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(PortTextBox.Text, out int port))
            {
                MessageBox.Show("Неверный ввод порта");
                return;
            }

            try
            {
                Form2 form = new Form2(ipTextBox.Text, port);
                form.Show();
                //this.Hide();
                //PongClient client = new PongClient(ipTextBox.Text, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно подключиться: " + ex.Message);
            }
        }
    }
}
