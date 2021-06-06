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
        private BitArray _receivedMessage;
        private BitArray _sendMessage;
        private BitArray _sendReceipt;
        private PostToFirstWT _post;
        private PostDataToFirstWT _postData;

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

            ConsoleHelper.WriteToConsoleRequest("2 поток","connect",_receivedMessage);
            _sendReceipt = new BitArray(1);
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);

            _sendSemaphore.Release();
            //2
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleRequest("2 поток", "", _receivedMessage);
            ConsoleHelper.WriteToConsole("2 поток", "Подготавливаю данные.");

            FileStream fls;
            string data;
            fls = new FileStream("C:/Users/Ekate/Downloads/ConsoleApp/1.txt", FileMode.Open);
            StreamReader fstr_in = new StreamReader(fls);
            data = fstr_in.ReadLine();
            fstr_in.Close();

            _post(Frame.GenerateData(data));

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();

            Buffer buffer = new Buffer();
            ConsoleHelper.WriteToConsoleArray("2 поток", _receivedMessage);
            ConsoleHelper.WriteTextMessageToConsole("2 поток переданный текст: ",_receivedMessage);
            Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
            _post(_sendReceipt);

            _sendSemaphore.Release();
            ////4
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);

            _sendSemaphore.Release();
            ////5
            _receiveSemaphore.WaitOne();
            if (_receivedMessage.Length > 1)
            {  
                ConsoleHelper.WriteToConsoleArray("2 поток", _receivedMessage);
                ConsoleHelper.WriteTextMessageToConsole("2 поток переданный текст: ", _receivedMessage);
            }

            _sendSemaphore.Release();

            //6
            _receiveSemaphore.WaitOne();

            if (_receivedMessage.Length > 1)
            {
                Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
            }
            else
            {
                Frame.GenerateReceipt(_sendReceipt, true);
            }
            _post(_sendReceipt);

            _sendSemaphore.Release();

            //7

            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleReceipt("2 поток", _receivedMessage);
            Frame.GenerateRequest(_sendReceipt, true);
            _post(_sendReceipt);

            _sendSemaphore.Release();

            //8
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleDisconnect("2 поток", "disconnect", _receivedMessage);
            ConsoleHelper.WriteToConsole("2 поток", "Заканчиваю работу");

            _sendSemaphore.Release();

        }
        public void SendData(object obj)
        {
            _postData = (PostDataToFirstWT)obj;
            _sendReceipt = new BitArray(1);
            Frame.GenerateReceipt(_sendReceipt, true);
            _postData(_sendReceipt);
            _sendSemaphore.Release();
        }
        public void ReceiveData(BitArray array)
        {
            _receivedMessage = array;
        }
        public void ResendData(PostToFirstWT _postTo, BitArray array)
        {
            if (array[0] == false || array == null)
            {
                ConsoleHelper.WriteToConsoleReceipt("2 поток", array);
                ConsoleHelper.WriteToConsole("2 поток", "Отправляю повторно");
                FileStream fls;
                string data;
                fls = new FileStream("C:/Users/Ekate/Downloads/ConsoleApp/1.txt", FileMode.Open);
                StreamReader fstr_in = new StreamReader(fls);
                data = fstr_in.ReadLine();
                fstr_in.Close();
                _postTo(Frame.GenerateData(data));

            }
            
        }
    }
}
