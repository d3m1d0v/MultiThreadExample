using System;
using System.Threading;
using System.Windows.Forms;

namespace InvokeExample
{
    public partial class InvokeExampleForm : Form
    {
        private Worker _worker;

        public InvokeExampleForm()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            startBtn.Enabled = false;
            stopBtn.Enabled = true;
            stopBtn.Focus();

            progressLbl.Text = "Progress: 0%";

            _worker = new Worker();
            _worker.ProcessChanged += _worker_ProcessChanged;
            _worker.WorkCompleted += _worker_WorkCompleted;
            
            Thread thread = new Thread(_worker.Work);
            thread.Start();
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            Action action = () =>
            {
                string message = cancelled ? "Process canceled" : "Process completed";
                MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                stopBtn.Enabled = false;
                startBtn.Enabled = true;
                startBtn.Focus();
            };

            this.InvokeEx(action);
        }

        private void _worker_ProcessChanged(int progress)
        {
            Action action = () =>
            {
                progressBar.Value = progress;
                progressLbl.Text = string.Format("Progress: {0}%", progress);
            };

            this.InvokeEx(action);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            _worker.Cancel();
        }
    }
}
