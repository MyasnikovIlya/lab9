using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Chat.Forms;
using System.Threading;

namespace Chat
{
    
    public delegate void SendHere(string text, string sender, Color? color);
    public delegate void voidDelegate();
    public partial class Chat : Form
    {
        private int chatBots = 5;
        private int delay = 1000;
        private int msgCount = 5;
        SendHere send;
        

        // private Thread thread2 = null;
        public Chat()
        {
            InitializeComponent();
            send = receiveMessage;
        }
        public void receiveMessage(string text, string sender, Color? color)
        {
            if (!color.HasValue) color = Color.Black;
            voidDelegate del = delegate ()
            {
                this.chatBox.SelectionStart = this.chatBox.TextLength;
                this.chatBox.SelectionLength = 0;

                this.chatBox.SelectionColor = color.Value;
                this.chatBox.AppendText(sender + ": " + text + Environment.NewLine);
                this.chatBox.SelectionColor = this.chatBox.ForeColor;
            };
            chatBox.Invoke(del);
        }

        //функция для получения рандомных слов
        public void startChatBots()
        {
            for (int i = 1; i <= chatBots; i++)
            {
                ChatBot bot = new ChatBot("Bot_" + i.ToString(), msgCount, delay, send);
                Thread thread = new Thread(bot.start);
                thread.Start();
            }
        }

        private void Chat_Load(object sender, EventArgs e)
        {

        }

        private void iFeelLonelyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startChatBots();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings form = new Settings(this, chatBots.ToString(), delay.ToString(), msgCount.ToString());
            form.Show();
        }
        public void applyOptions(int chatBots, int delay, int msgCount)
        {
            this.chatBots = chatBots;
            this.delay = delay;
            this.msgCount = msgCount;

        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chatBox.Visible = !chatBox.Visible;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chatBox.Text = "";
        }
    }
    public class ChatBot
    {
        object __lockObj;
        Color color;
        string name;
        int count;
        int delay;
        SendHere send;
        Random rng;

        public ChatBot(string name, int count, int delay, SendHere send)
        {
            __lockObj = new object();
            rng = new Random(name.GetHashCode());
            color = Color.FromArgb(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256));
            this.name = name;
            this.count = count;
            this.delay = delay;
            this.send = send;
        }
        public void start()
        {
            lock (__lockObj)
            {
                for (int i = 0; i < count; i++)
                {
                    send(TextGenerator.generate(rng.Next(1, 13), rng.Next(1, 14)), name, color);
                    Thread.CurrentThread.Join(delay);
                }
            }
        }
    }
    public static class TextGenerator
    {
        static Random rng = new Random();
        public static string generate(int lengthWord, int lengthSentance)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string ending = "!?."; 
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < lengthSentance; i++)
            {
                res.Append(Enumerable.Repeat(chars, rng.Next(1, lengthWord))
                    .Select(s => s[rng.Next(s.Length)]).ToArray<char>());
                res.Append(' ');
            }
            res.Replace(' ', ending[rng.Next(2)], res.Length - 1, 1);
            return res.ToString();
        }
    }
}