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
    public class ThirdThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private Semaphore _firstFileSemaphore;
        private BitArray[] _receivedMessage;
        private BitArray[] _sendMessage;
        private BitArray[] _sendReceipt;
        private PostToFourthWT _post;

        public ThirdThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore, ref Semaphore firstFileSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
            _firstFileSemaphore = firstFileSemaphore;
        }
        public void ThirdThreadMain(object obj)
        {
            //1
            _post = (PostToFourthWT)obj;
            ConsoleHelper.WriteToConsole("3 поток", "Начинаю работу.Готовлю данные для передачи.");
            _sendReceipt = new BitArray[1];
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);
            _sendSemaphore.Release();

            //2
            _receiveSemaphore.WaitOne();
            ConsoleHelper.WriteToConsoleRequest("3 поток", "", _receivedMessage);
            FileStream fls;
            string data;
            _firstFileSemaphore.WaitOne();
            fls = new FileStream("C:/Users/Ekate/Downloads/ConsoleApp/1.txt", FileMode.Open);  
            StreamReader fstr_in = new StreamReader(fls);
            data = fstr_in.ReadLine();
            fstr_in.Close();
            _firstFileSemaphore.Release();
            _post(Frame.GenerateData(data));

            _sendMessage = new BitArray[2];

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);

            _sendSemaphore.Release();
            //4
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);
            ConsoleHelper.WriteToConsoleReceipt("3 поток", _receivedMessage);
            ConsoleHelper.WriteToConsoleRequest("3 поток", "connect", _receivedMessage);
            _sendSemaphore.Release();
            //5
            _receiveSemaphore.WaitOne();

            Buffer buffer = new Buffer();
            ConsoleHelper.WriteToConsoleMatrixBitArray("3 поток", _receivedMessage);
            ConsoleHelper.WriteTextMessageToConsole("3 поток переданный текст: ", _receivedMessage);
            Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
            bool check = buffer.CheckSum(_receivedMessage);
            _post(_sendReceipt);
            _sendSemaphore.Release();
            ////6
            _receiveSemaphore.WaitOne();

            if (check == false)
            {
                ConsoleHelper.WriteToConsoleMatrixBitArray("3 поток", _receivedMessage);
                ConsoleHelper.WriteTextMessageToConsole("3 поток переданный текст: ", _receivedMessage);
                Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
                _post(_sendReceipt);
            }

            _sendSemaphore.Release();
            //7
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleDisconnect("3 поток", "", _receivedMessage);
            ConsoleHelper.WriteToConsole("3 поток", "Заканчиваю работу");

            _sendSemaphore.Release();


        }
        public void ReceiveData(BitArray[] array)
        {
            _receivedMessage = array;
        }
        public void ResendData(PostToFourthWT _postTo, BitArray[] array)
        {
            if (array[0][0] == false || array == null)
            {
                ConsoleHelper.WriteToConsoleReceipt("3 поток", array);
                ConsoleHelper.WriteToConsole("3 поток", "Отправляю повторно");
                FileStream fls;
                string data;
                _firstFileSemaphore.WaitOne();
                fls = new FileStream("C:/Users/Ekate/Downloads/ConsoleApp/1.txt", FileMode.Open);
                StreamReader fstr_in = new StreamReader(fls);
                data = fstr_in.ReadLine();
                fstr_in.Close();
                _firstFileSemaphore.Release();
                _postTo(Frame.GenerateData(data));

            }

        }

    }
}
