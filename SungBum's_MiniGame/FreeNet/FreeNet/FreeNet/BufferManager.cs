using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FreeNet
{
    /// <summary>
    /// 이 클래스는 나누어 사용하기 위해 SocketAsyncEventArgs 개체에 할당할 수 있는 하나의 큰 버퍼를 만듭니다.
    /// 각 소켓 I/O 작업과 함께.이를 통해 버퍼를 쉽게 재사용하고 힙 메모리 조각화를 방지할 수 있습니다.
    /// BufferManager 클래스에 노출된 작업은 스레드로부터 안전하지 않습니다.
    /// </summary>
    class BufferManager
    {
        int m_numBytes;                     // 버퍼 풀이 제어하는 총 바이트 수
        byte[] m_buffer;                    // 버퍼 관리자가 유지 관리하는 기본 바이트 배열
        Stack<int> m_freeIndexPool;         //
        int m_currentIndex;
        int m_bufferSize;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// 버퍼 풀에서 사용하는 버퍼 공간을 할당합니다.
        /// </summary>
        public void InitBuffer()
        {
            // 하나의 큰 대형 버퍼를 만들고 이를 각 SocketAsyncEventArg 개체로 나눕니다.
            m_buffer = new byte[m_numBytes];
        }

        /// <summary>
        /// 지정된 SocketAsyncEventArgs 개체에 버퍼 풀의 버퍼를 할당합니다.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if(m_freeIndexPool.Count > 0) 
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        /// <summary>
        /// SocketAsyncEventArgs 개체에서 버퍼를 제거합니다. 이렇게 하면 버퍼가
        /// 버퍼 풀로 다시 해제됩니다.
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffe(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
