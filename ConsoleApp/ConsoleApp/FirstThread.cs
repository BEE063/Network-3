using Microsoft.VisualBasic.CompilerServices;
using PP_lab1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ThreadsEdu;
using Buffer = PP_lab1.Buffer;

namespace ConsoleApp
{
    public class FirstThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray[] _receivedMessage;
        private BitArray[] _sendMessage;
        private BitArray[] _sendReceipt;
        private PostToSecondWT _post;

        public FirstThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void FirstThreadMain(object obj)
        {
            //1
            _post = (PostToSecondWT)obj;
            ConsoleHelper.WriteToConsole("1 поток", "Начинаю работу.Готовлю данные для передачи.");
            _sendReceipt = new BitArray[1];
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);
            _sendSemaphore.Release();

            //2
            _receiveSemaphore.WaitOne();
            ConsoleHelper.WriteToConsoleRequest("1 поток", "", _receivedMessage);
            var fileBytes =
                File.ReadAllBytes("C:/Users/Ekate/Downloads/ConsoleApp/1.txt");
           
            _post(Frame.GenerateData(fileBytes));

            _sendMessage = new BitArray[2];

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);

            _sendSemaphore.Release();
            //4
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);
            ConsoleHelper.WriteToConsoleReceipt("1 поток", _receivedMessage);
            _sendSemaphore.Release();
            //5
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleDisconnect("1 поток", "", _receivedMessage);
            ConsoleHelper.WriteToConsole("1 поток", "Заканчиваю работу");

            _sendSemaphore.Release();


        }
        public void ReceiveData(BitArray[] array)
        {
            _receivedMessage = array;
        }
        public void ResendData(PostToSecondWT _postTo, BitArray[] array)
        {
            if (array[0][0] == false || array == null)
            {
                ConsoleHelper.WriteToConsoleReceipt("1 поток", array);
                ConsoleHelper.WriteToConsole("1 поток", "Отправляю повторно");
                var fileBytes =
                File.ReadAllBytes("C:/Users/Ekate/Downloads/ConsoleApp/1.txt");
                _postTo(Frame.GenerateData(fileBytes));

            }

        }


    }
}
