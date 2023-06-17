using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FreeNet
{
    class CNetworkService
    {
        int connected_count;
        //클라이너트 접속 대기 기체
        CListener client_listener;

        SocketAsyncEventArgsPool receive_event_args_pool;
        SocketAsyncEventArgsPool send_event_args_pool;

        //메시지 수신, 전송 시 닷넷 비동기 소켓에서 사용할 버퍼를 관리하는 객체
        BufferManager buffer_manager;

        //클라이언트의 접속이 이루어졌을 때 호출되는 콜백 델리게이트
        public delegate void SessionHanadler(CUserToken token);
        public SessionHanadler session_created_callback { get; set; }

        //confifs
        int max_connections;
        int buffer_size;
        readonly int pre_alloc_count = 2; // read, write

        public CNetworkService()
        {
            this.connected_count = 0;
            this.session_created_callback = null;
        }

        public void initialize()
        {
            this.max_connections = 10000;
            this.buffer_size = 1024;

            this.buffer_manager = new BufferManager(this.max_connections * this.buffer_size * this.pre_alloc_count, buffer_size);
            this.receive_event_args_pool = new SocketAsyncEventArgsPool(this.max_connections);
            this.send_event_args_pool = new SocketAsyncEventArgsPool(this.max_connections);

            this.buffer_manager.InitBuffer();

            SocketAsyncEventArgs arg;

            for (int i = 0; i < this.max_connections; i++)
            {
                CUserToken token = new CUserToken();

                //receive pool
                {
                    arg = new SocketAsyncEventArgs();
                    arg.Completed += new EventHandler<SocketAsyncEventArgs>(receive_completed);
                    arg.UserToken = token;

                    this.buffer_manager.SetBuffer(arg);

                    this.receive_event_args_pool.Push(arg);
                }

                //send pool
                {
                    arg = new SocketAsyncEventArgs();
                    arg.Completed += new EventHandler<SocketAsyncEventArgs>(send_complete);
                    arg.UserToken = token;

                    this.buffer_manager.SetBuffer(arg);

                    this.send_event_args_pool.Push(arg);
                }
            }
        }

        public void listen(string host, int port, int backlog)
        {
            this.client_listener = new CListener();
            this.client_listener.callback_on_newclient += on_new_client;
            this.client_listener.start(host, port, backlog);
        }

        public void on_connect_completed(Socket socket, CUserToken token)
        {

        }

        public void on_new_client(Socket client_socket, object token)
        {
            Interlocked.Increment(ref this.connected_count);

            Console.WriteLine(string.Format("[{0}] A client connected. handle {1}, count{2}", 
                Thread.CurrentThread.ManagedThreadId, client_socket.Handle, this.connected_count));

            //풀에서 하나 꺼내와 사용한다
            SocketAsyncEventArgs receive_args = this.receive_event_args_pool.Pop();
            SocketAsyncEventArgs send_args = this.send_event_args_pool.Pop();

            //SocketAsync<EventArgs>를 생성할 때 만들어 두었던 CuserToken을 꺼내와서
            //콜백 메서드의 파라미터로 넘겨준다.
            CUserToken user_token = null;
            if(this.session_created_callback != null)
            {
                user_token = receive_args.UserToken as CUserToken;
                this.session_created_callback(user_token);
            }

            //클라이언트로부터 데이터를 수신할 준비를 한다.
            begin_receive(client_socket, receive_args, send_args);
        }

        private void begin_receive(Socket socket, SocketAsyncEventArgs receive_args, SocketAsyncEventArgs send_args)
        {
            CUserToken token = receive_args.UserToken as CUserToken;
        }

        private void receive_completed(object sender, SocketAsyncEventArgs e)
        {

        }

        private void send_complete(object sender, SocketAsyncEventArgs e)
        {

        }

        private void  process_receive(SocketAsyncEventArgs e)
        {

        }

        public void close_clientsocket(CUserToken token)
        {

        }
    }
}
