using ConsoleApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadsEdu
{
    public delegate void PostToFirstWT(BitArray[] message);
    public delegate void PostToSecondWT(BitArray[] message);
    public delegate void PostDataToFirstWT(BitArray[] message);
    public delegate void PostDataToSecondWT(BitArray[] message);
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.WriteToConsole("Главный поток", "");

            Semaphore firstReceiveSemaphore = new Semaphore(0, 1);
            Semaphore secondReceiveSemaphore = new Semaphore(0, 1);

            FirstThread firstThread = new FirstThread(ref secondReceiveSemaphore, ref firstReceiveSemaphore);
            SecondThread secondThread = new SecondThread(ref firstReceiveSemaphore, ref secondReceiveSemaphore);

            Thread threadFirst = new Thread(new ParameterizedThreadStart(firstThread.FirstThreadMain));
            Thread threadSecond = new Thread(new ParameterizedThreadStart(secondThread.SecondThreadMain));
            Thread thirdThread = new Thread(new ParameterizedThreadStart(firstThread.SendData));
            Thread fouthThread = new Thread(new ParameterizedThreadStart(secondThread.SendData));

            PostToFirstWT postToFirstWt = new PostToFirstWT(firstThread.ReceiveData);
            PostToSecondWT postToSecondWt = new PostToSecondWT(secondThread.ReceiveData);
            PostDataToFirstWT postDataToFirstWT = new PostDataToFirstWT(firstThread.ReceiveData);
            PostDataToSecondWT postDataToSecondWT = new PostDataToSecondWT(secondThread.ReceiveData);

            thirdThread.Start(postDataToSecondWT);
            fouthThread.Start(postDataToFirstWT);
            threadFirst.Start(postToSecondWt);
            threadSecond.Start(postToFirstWt);

            Console.ReadLine();

        }
    }

}



