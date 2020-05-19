using System;
using System.Windows.Forms;

namespace Chat.Forms
{
    public partial class Settings : Form
    {
        Chat parent;
        public Settings(Chat form, string bots, string delay, string count)
        {
            parent = form;
            InitializeComponent();
            botCountParam.Text = bots;
            delayParam.Text = delay;
            msgCountParam.Text = count;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                parent.applyOptions(Int32.Parse(botCountParam.Text), Int32.Parse(delayParam.Text), Int32.Parse(msgCountParam.Text));
                parent.startChatBots();
            } catch (FormatException)
            {
                MessageBox.Show("Wrong options format! (all ints btw)");
            }
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
