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
    public delegate void PostToThirdWT(BitArray[] message);
    public delegate void PostToFourthWT(BitArray[] message);

    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.WriteToConsole("Главный поток", "");
            Semaphore firstReceiveSemaphore = new Semaphore(0, 1);
            Semaphore secondReceiveSemaphore = new Semaphore(0, 1);
            Semaphore thirdReceiveSemaphore = new Semaphore(0, 1);
            Semaphore fourthReceiveSemaphore = new Semaphore(0, 1);

            FirstThread firstThread = new FirstThread(ref secondReceiveSemaphore, ref firstReceiveSemaphore);
            SecondThread secondThread = new SecondThread(ref firstReceiveSemaphore, ref secondReceiveSemaphore);
            ThirdThread thirdThread = new ThirdThread(ref fourthReceiveSemaphore, ref thirdReceiveSemaphore);
            FourthThread fourthThread = new FourthThread(ref thirdReceiveSemaphore, ref fourthReceiveSemaphore);

            Thread threadFirst = new Thread(new ParameterizedThreadStart(firstThread.FirstThreadMain));
            Thread threadSecond = new Thread(new ParameterizedThreadStart(secondThread.SecondThreadMain));
            Thread threadThird = new Thread(new ParameterizedThreadStart(thirdThread.ThirdThreadMain));
            Thread threafFourth = new Thread(new ParameterizedThreadStart(fourthThread.FourthThreadMain));

            PostToFirstWT postToFirstWt = new PostToFirstWT(firstThread.ReceiveData);
            PostToSecondWT postToSecondWt = new PostToSecondWT(secondThread.ReceiveData);
            PostToThirdWT postToThirdWt = new PostToThirdWT(thirdThread.ReceiveData);
            PostToFourthWT postToFourthWt = new PostToFourthWT(fourthThread.ReceiveData);


            threadFirst.Start(postToSecondWt);
            threadSecond.Start(postToFirstWt);
            threadThird.Start(postToFourthWt);
            threafFourth.Start(postToThirdWt);
            Console.ReadLine();

        }
    }

}




