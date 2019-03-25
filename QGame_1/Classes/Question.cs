using System;
using System.Collections.Generic;
using System.Text;

    internal class Question
    {

        public string question;
        public string answer;
        public List<string> answers = new List<string>();
        public bool correctAnswer = false;
        public string cAnswer = "";
        public Question()
        {

        }
        public Question(string q, List<string> a)
        {
            question = q;
            answers = a;
        }
        public Question(string q, List<string> a,string ca)
        {
            question = q;
            foreach(string s in a)
            {
            //Console.WriteLine(s);
                answers.Add(s);
            }
            cAnswer = ca;
        }
        public Question(string q, string a)
        {
            question = q;
            answer = a;
        }
        public string info()
        {
            return question + "\nОтвет: " + answer;
        }
        public void addAnswer(string a)
    {
        answers.Add(a);
    }
        public void addCorrectAnswer(string a)
    {
        cAnswer = a;

    }
    }
