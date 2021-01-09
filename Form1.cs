using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using AForge.Video.DirectShow;
using System.Windows.Forms;


namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FilterInfoCollection filterinfo;
        VideoCaptureDevice videocap;

        private void Form1_Load(object sender, EventArgs e)
        {
            filterinfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filterinfo)
                comboBox1.Items.Add(device.Name);
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            videocap = new VideoCaptureDevice(filterinfo[comboBox1.SelectedIndex].MonikerString);
            videocap.NewFrame += Videocap_NewFrame;
            videocap.Start();
        }

        private void Videocap_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result!= null)
            {
                textBox1.Invoke(new MethodInvoker(delegate ()
                { textBox1.Text = result.ToString(); }
                ));

            }
            pictureBox1.Image = bitmap;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            videocap.Stop();
        }
    }
}
