using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Telegram.Bot;
using System.Data.SqlClient;

namespace Task3
{
    class TelegramBot
    {
        private string token;

        public string Token => token;

        private TelegramBotClient botClient;

        public TelegramBot()
        {

        }

        ~TelegramBot()
        {
            botClient.StopReceiving();
        }

        public TelegramBot(string token)
        {
            this.token = token;
            InitializeBot();
        }

        public void InitializeBot()
        {
            botClient = new TelegramBotClient(token);
            botClient.OnMessage += OnMessage;
            //another magic for waiting event from bot
            Thread.Sleep(500);
        }

        public void Start()
        {
            botClient.StartReceiving();
            //Console.WriteLine("Bot is alive");
        }

        public void Stop()
        {
            botClient.StopReceiving();
            //Console.WriteLine("Bot is dead");
        }

        public void WriteUserToDb(string username, int id)
        {

            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS;Initial Catalog=BotsDB;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO TelegramBotUsers (id,login) VALUES " +
                    "(@id,@login)", cnn);
                command.Parameters.Add("@id", id);
                command.Parameters.Add("@login", username);
                command.ExecuteNonQuery();
                Console.WriteLine($"User {username} with chat id:{id} was successfylly writen to databse");

                cnn.Close();
            }
            catch (Exception e) { }
        }

        private int getId(string username)
        {

            string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS;Initial Catalog=BotsDB;Integrated Security=True";
            string Query = "select * from TelegramBotUsers";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(Query, Conn);
                SqlDataReader R = Comm.ExecuteReader();
                while (R.Read())
                {

                    if (R[1].ToString() == username)
                    {
                        return Convert.ToInt32(R[0]);
                    }
                }
                Conn.Close();
            }

            return -1;
        }

        public void SendMessage(int id, string message)
        {
            botClient.SendTextMessageAsync(id, message);
        }

        public void SendMessage(string username, string message)
        {
            int id = getId(username);
            if (id != -1)
                botClient.SendTextMessageAsync(id, message);
            else
                Console.WriteLine($"ERROR: user {username} not found");
        }

        void OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {

            Console.WriteLine(e.Message.From.Username+" "+e.Message.From.Id);
            WriteUserToDb(e.Message.From.Username, e.Message.From.Id);
            //botClient.SendTextMessageAsync(e.Message.From.Id, "response");
           
        }
    }

    class TelegramSender
    {

        private TelegramBot tb;

        public void SendMessage(string receiver,string message)
        {
            tb.SendMessage(receiver, $"Message:\n{message}");
        }

        public TelegramSender()
        {
            tb = new TelegramBot("727963015:AAES9Q_dFuT171PNSbBc_kW6htbT3Y-rf-k");
            tb.Start();
        }

        ~TelegramSender()
        {
            tb.Stop();
        }

    }
}
