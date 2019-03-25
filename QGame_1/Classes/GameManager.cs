using Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QGame_1.Classes
{
    class GameManager
    {
        private const string CONTINUE_MSG = "Нажмите Enter для продолжения";
        private const int MENU_STATE = 1;
        private const int INSTRUCTIONS_STATE = 2;
        private const int QUESTION_STATE = 3;
        private const int RESULTS_STATE = 4;
        public int currentState = 0;

        const string DEFAULT_FILE = @"\data";
        DataReader reader = null;

        List<Question> data = new List<Question>();
        private void initPath()
        {
            using (FileStream fstream = new FileStream(@"data", FileMode.OpenOrCreate))
            {
                byte[] array = QGame_1.Properties.Resources.data1;
                fstream.Write(array, 0, array.Length);
            }


            Console.Clear();
            string input = "";
            string path = "";
            Console.WriteLine("1 - Использовать путь по умолчанию\n2 - Ввести свой путь\n");
            input = Console.ReadLine();
            while (true)
            {


                if (input == "1")
                {
                    //Console.WriteLine(CONTINUE_MSG);
                    loading();
                    reader = new DataReader(Application.StartupPath+DEFAULT_FILE);
                    break;
                }
                else if (input == "2")
                {
                    Console.Write("\nВведите путь до файла:\n>");
                    path = Console.ReadLine();
                    if (File.Exists(path))
                    {
                        loading();
                        
                        reader = new DataReader(path);
                        break;
                    }
                   
                }

            }
        }
        private void loading()
        {
            Console.Write("Loading...\n");
            for (int i = 0; i < 100; i++)
            {
                Console.Write("=");
                Thread.Sleep(10);
            }
            
        }
        public GameManager()
        {
            initPath();
            printMenu();
            data = reader.getList();
            
        }

        void printInputError()
        {
            Console.Clear();
            Console.WriteLine("Недопустимый ввод."+CONTINUE_MSG);
            Console.ReadLine();
            printMenu();
        }
        public void printSettings()
        {
            Console.Clear();
            int action = 0;
            Console.Write("1.Изменить путь до файла с вопросами\n2.Назад\n>");
            try
            {
                action = Convert.ToInt32(Console.ReadLine());
            }catch(Exception e)
            {
                action = 0;
                printInputError();
            }

            switch(action)
            {
                case 1:
                    initPath();
                    reader.initList();
                    printMenu();
                    break;
                case 2:
                    printMenu();
                    break;
            }
        }
        public void printMenu()
        {

            Console.Clear();
            currentState = MENU_STATE;
            Console.WriteLine("1.Начать игру\n2.Инструкции\n3.Список вопросов\n4.Настройки\n5.Выход");
            int nextAction;
            try
            {
                nextAction = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                nextAction = 0;
            }

            switch (nextAction)
            {
                case 0:
                    printInputError();
                    break;
                case 1:
                    startGame();
                    break;
                case 2:
                    printInstructions();
                    break;
                case 3:
                    printQuestions();
                    break;
                case 4:
                    printSettings();
                    break;
                case 5:
                    Application.Exit();
                    break;
                
            }

        }
        
        public void startGame()
        {
            Console.Clear();
            reader.makeMixList();
            Console.WriteLine("Игра начата, для выхода в главное меню введите 'exit'");
            Console.WriteLine(CONTINUE_MSG);
            data = reader.getList();
            Console.ReadLine();
            foreach (Question q in data)
            {
                Console.Clear();
                printQuestion(q);
                
            }
            Console.Clear();
            Console.WriteLine("Тест завершен.\n"+CONTINUE_MSG);
            Console.Read();
            printResults();
            
        }
        
        public void printInstructions()
        {
            Console.Clear();
            currentState = INSTRUCTIONS_STATE;
            Console.WriteLine("В этой игре вам нужно отвечать на вопросы, для возврата в меню нажмите Enter.");
            Console.ReadLine();
            printMenu();

        }
        public void printQuestionTest()
        {

            foreach (Question q in data)
            {
                Console.Clear();
                currentState = QUESTION_STATE;
                Console.WriteLine(q.question);
                Console.Write("Введите ответ: ");
                string input = Console.ReadLine();
                if (isCorrect(input, q))
                {
                    Console.WriteLine("Ответ верный");
                    Console.ReadLine();
                }
                else if (!isCorrect(input, q))
                {
                    Console.WriteLine("Ответ неверный");
                    Console.ReadLine();

                }
                else if (input == "exit")
                {
                    Console.Clear();
                    printMenu();
                }
            }
        }
        void printQuestions()
        {
            reader.makeMixList();
            //reader.makeMixList();
            Console.Clear();

            data = reader.getList();
            int i = 0;
            
            foreach (Question item in data)
            {
                i++;
                
                Console.WriteLine(i + "." + item.question +"\n"+"\n====================================");
            }
            Console.ReadLine();
            printMenu();
        }
        void printResults()
        {
            Console.Clear();
            int i = 0;
            bool an = false;
            string a;
            foreach (Question item in data)
            {
                i++;
                an = item.correctAnswer;
                if (an)
                {
                    a = "Правильный";
                }
                else
                    a = "Неправильный";
                Console.WriteLine(i + ") вопрос --\t" + item.question + " \n    Ответ--\t" + a);
            }
           

            Console.WriteLine("\n1.Вернуться в главное меню\n2.Выйти");
            Console.Read();

            try
            {

                int action = Convert.ToInt32(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        printMenu();
                        break;
                }
            }
            catch(Exception e)
            {
                printMenu();
            }
        }

        public void printQuestion(Question q)
        {
           
            currentState = QUESTION_STATE;
            Console.WriteLine("Вопрос: "+q.question);
            Console.WriteLine("Ответы: \n");
            int min = 1;
            int max = q.answers.Count;
            bool isFirstQ = true;
            for (int i = 0; i < q.answers.Count;i++)
            {
                
                Console.WriteLine((i+1) + "." + q.answers[i] +"\n");
                
            }
            while (true)
            {
                if(isFirstQ)
                {
                    Console.Write("Введите ответ: ");
                    isFirstQ = false;
                }
                string input = Console.ReadLine();
                int num;
                if(int.TryParse(input,out num))
                {
                    if (num >= min && num <= max)
                    {
                        q.correctAnswer = isCorrect(input, q);
                        if(q.correctAnswer)
                        {
                            Console.Write("\nПравильно!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.Write("\nНеправильно!");
                            Console.ReadKey();

                        }
                        break;
                    }

                }
                
            }
            


        }
        private bool isCorrect(string answerIndex, Question answer)
        {

            
            if (answer.answers[int.Parse(answerIndex)-1] == answer.cAnswer)
            {
                answer.correctAnswer = true;
            }
            return answer.correctAnswer;
        }

        private Question chooseQuestion()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, data.Count);
            op("Осталось вопросов: " + data.Count);
            Question q = data[num];
            //returnData.Add(q);
            data.Remove(q);
            return q;
        }
        void op(object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
