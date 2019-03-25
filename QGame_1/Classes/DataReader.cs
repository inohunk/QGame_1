using System;
using System.Collections.Generic;
using System.IO;
namespace Data
{
    class DataReader
    {
        string path = "";
        const string separator = "=============================";
        List<Question> data = new List<Question>();
        
        public DataReader(string p)
        {
            this.path = p;
            initList();
            testInfo();
            Console.ReadKey();
        }
        public List<Question> getList()
        {
            return data;
        }
        public string getPath()
        {
            return path;
        }
        public void testInfo()
        {
            foreach(Question item in data)
            {
                w("Question: " +item.question + "\n" + "Correct Answer: " + item.cAnswer + "\nNumber of questions: " + item.answers.Count);
            }
        }
        public void initList()
        {
            using (StreamReader sr = new StreamReader(path))
            {
                var test = new List<string>();
                bool qu = false,
                     an = false,
                     qan = false;

                string ques = "",
                       cAnswer = "";

                Question q = new Question();
                List<string> answersArr = new List<string>();

                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    test.Add(s);
                }


                foreach (string line in test)
                {
                    if (line == "#")
                    {
                        
                        qu = true;
                        continue;
                    }
                    else if(line == "" || line == null)
                    {
                        continue;
                    }
                    else if (qu == true)
                    {
                        //w("Question: " + line);
                        qu = false;
                        ques = line;
                        continue;

                    }
                    else if (line == "@")
                    {
                        an = true;
                        continue;

                    }
                    else if (line == "$")
                    {
                        Question init = new Question(ques, answersArr, cAnswer);
                        
                        data.Add(init);
                        //w("Counter answers: " + (init.answers.Count));
                        //w(separator);
                        an = false;
                        answersArr.Clear();
                        continue;
                    }
                    else if (line == "+")
                    {

                        qan = true;
                        continue;

                    }
                    else if (qan == true)
                    {
                        //w("Correct answer: " + line);

                        cAnswer = line;
                        answersArr.Add(line);
                        qan = false;
                        continue;
                    }
                    else if (an == true)
                    {
                        //w("Answer: " + line);
                        answersArr.Add(line);
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                //w(separator);
                //testInfo();
                //Console.Read();

            }

        }
        
        public void makeMixList()
        {
            Random rand = new Random();
            for (int i = data.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                Question tmp = data[j];
                
                data[j] = data[i];
                data[i] = tmp;
            }
        }
        private void w(object s)
        {
            Console.WriteLine(s);
        }

    }
}