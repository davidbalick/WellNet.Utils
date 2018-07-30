using System;
using System.Windows;
using System.Windows.Controls;

namespace WellNet.Utils
{
    public partial class QuestionAsker : Window
    {
        private MessageBoxResult _messageBoxResult = MessageBoxResult.Cancel;

        public static MessageBoxResult Ask(string title, string question)
        {
            var questionAnswer = new QuestionAsker(title, question);
            questionAnswer.ShowDialog();
            return questionAnswer._messageBoxResult;
        }

        private QuestionAsker(string title, string question)
        {
            InitializeComponent();
            TbTitle.Text = title;
            TbQuestion.Text = question;
            BtnYes.Click += Btn_Click;
            BtnNo.Click += Btn_Click;
            BtnCancel.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            if (button == null)
                throw new Exception("carp");
            var result = button.Name.Substring(3);
            _messageBoxResult = (MessageBoxResult) Enum.Parse(typeof(MessageBoxResult), result);
            Close();
        }
    }
}
