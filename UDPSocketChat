using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDPSocketChat
{
    internal class UDPChat
    {
        string? username;
        IPAddress localAddress;
        int localPort;
        int remotePort;

        public void handlerUDPChat()
        {
            localAddress = IPAddress.Parse("127.0.0.1");
            Console.Write("Введите свое имя: ");
            username = Console.ReadLine();
            Console.Write("Введите порт для приема сообщений: ");
            if (!int.TryParse(Console.ReadLine(), out localPort)) return;
            Console.Write("Введите порт для отправки сообщений: ");
            if (!int.TryParse(Console.ReadLine(), out remotePort)) return;
            Console.WriteLine();

            // запускаем получение сообщений
            Task.Run(ReceiveMessageAsync);
            // запускаем ввод и отправку сообщений
            SendMessageAsync();
        }

        // отправка сообщений в группу
        async Task SendMessageAsync()
        {
            using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
            // отправляем сообщения
            while (true)
            {
                var message = Console.ReadLine(); // сообщение для отправки
                                                  // если введена пустая строка, выходим из цикла и завершаем ввод сообщений
                if (string.IsNullOrWhiteSpace(message)) break;
                // иначе добавляем к сообщению имя пользователя
                message = $"{username}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                // и отправляем на 127.0.0.1:remotePort
                await sender.SendToAsync(data, SocketFlags.None, new IPEndPoint(localAddress, remotePort));
            }
        }
        // отправка сообщений
        async Task ReceiveMessageAsync()
        {
            byte[] data = new byte[65535]; // буфер для получаемых данных
                                           // сокет для прослушки сообщений
            using Socket receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // запускаем получение сообщений по адресу 127.0.0.1:localPort
            receiver.Bind(new IPEndPoint(localAddress, localPort));
            while (true)
            {
                // получаем данные в массив data
                var result = await receiver.ReceiveFromAsync(data, SocketFlags.None, new IPEndPoint(IPAddress.Any, 0));
                var message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);
                // выводим сообщение
                Console.WriteLine(message);
            }
    }   }
}
