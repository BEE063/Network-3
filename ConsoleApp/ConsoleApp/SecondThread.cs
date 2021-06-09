using PP_lab1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ThreadsEdu;
using Buffer = PP_lab1.Buffer;

namespace ConsoleApp
{
    public class SecondThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray[] _receivedMessage;
        private BitArray[] _sendMessage;
        private BitArray[] _sendReceipt;
        private PostToFirstWT _post;

        public SecondThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void SecondThreadMain(Object obj)
        {
            _post = (PostToFirstWT)obj;
            ConsoleHelper.WriteToConsole("2 поток", "Начинаю работу.Жду передачи данных.");
            //1
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleRequest("2 поток", "connect", _receivedMessage);
            _sendReceipt = new BitArray[1];
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);

            _sendSemaphore.Release();
            //2
            _receiveSemaphore.WaitOne();

            Buffer buffer = new Buffer();
            ConsoleHelper.WriteToConsole("2 поток", "Данные полученны");
            ConsoleHelper.WriteToConsoleMatrixBitArray("2 поток ", _receivedMessage);
            bool check = buffer.CheckSum(_receivedMessage);
            Frame.GenerateReceipt(_sendReceipt, check);
            _post(_sendReceipt);

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();

            if (check == false)
            {
                ConsoleHelper.WriteToConsoleMatrixBitArray("2 поток ", _receivedMessage);
                Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
            }

            _sendSemaphore.Release();

            //4
            _receiveSemaphore.WaitOne();

            //ConsoleHelper.WriteToConsoleReceipt("2 поток", _receivedMessage);
            ConsoleHelper.WriteToConsoleDisconnect("2 поток", "disconnect", _receivedMessage);
            Frame.GenerateRequest(_sendReceipt, true);
            _post(_sendReceipt);
            ConsoleHelper.WriteToConsole("2 поток", "Заканчиваю работу");

            _sendSemaphore.Release();

        }
        public void ReceiveData(BitArray[] array)
        {
            _receivedMessage = array;
        }
        
    }
}
