using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestServerSample {
    public partial class Form1 : Form {
        
        Mock.Server _mockServer = new Mock.Server(80);

        public Form1() {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) {

            var server = "weather.livedoor.com";
            server = "127.0.0.1";
            var url = textBox2.Text;

            HttpClient cl = new HttpClient();
            var str = await cl.GetStringAsync(string.Format("http://{0}{1}",server,url));
            textBox1.Text = str;

        }
    }
}
