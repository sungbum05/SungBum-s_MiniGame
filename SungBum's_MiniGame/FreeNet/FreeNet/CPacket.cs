using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeNet
{
    /// <summary>
    /// byte[] 버퍼를 참조로 보관하여 pop_xxx 메소드 호출 순서대로 데이터 변환을 수행한다.
    /// </summary>
    public class CPacket
    {
        public IPeer owner { get; private set; }
        public byte[] buffer { get; private set; }
        public int position { get; private set; }

        public Int16 protocol_id { get; private set; }

        public static CPacket create(Int16 protocol_id)
        {
            //CPacket packet = new CPacket();
            CPacket packet = CPacketBufferManager.pop();
            packet.set_protocol(protocol_id);
            return packet;
        }

        public static void destroy(CPacket packet)
        {
            CPacketBufferManager.push(packet);
        }

        public CPacket(byte[] buffer, IPeer owner)
        {
            this.buffer = buffer;

            this.position = Defines.HEADRSIZE;
            this.owner = owner;
        }

        public CPacket()
        {
            this.buffer = new byte[1024];
        }

        public Int16 pop_protocol_id()
        {
            return pop_int16();
        }

        public void Copy_to(CPacket target)
        {
            target.set_protocol(this.protocol_id);
            target.overwrite(this.buffer, this.position);
        }

        public void overwrite(byte[] source, int position)
        {
            Array.Copy(source, this.buffer, source.Length);
            this.position = position;
        }

        public Byte pop_byte()
        {
            byte data = (byte)BitConverter.ToInt16(this.buffer, this.position);
            this.position += sizeof(byte);
            return data;
        }

        public Int16 pop_int16()
        {
            Int16 data = BitConverter.ToInt16(this.buffer, this.position);
            this.position += sizeof(Int16);
            return data;
        }

        public Int32 pop_int32()
        {
            Int32 data = BitConverter.ToInt32(this.buffer, this.position);
            this.position += sizeof(Int32);
            return data;
        }

        public string pop_string()
        {
            //문자열 길이는 최대 2바이트 까지. 0~ 23767
            Int16 len = BitConverter.ToInt16(this.buffer, this.position);
            this.position += sizeof(Int16);

            //인코징은 utf8로 통일
            string data = Encoding.UTF8.GetString(this.buffer, this.position, len);
            this.position += len;

            return data;
        }

        public void set_protocol(Int16 protocol_id)
        {
            this.protocol_id = protocol_id;
            //this.buffer = new byte[1024];

            // 헤더는 나중에 넣을것이므로 데이터 부터 넣을 수 있도록 위치를 점프시켜놓는다.
            this.position = Defines.HEADRSIZE;
            push_int16(protocol_id);
        }

        public void record_size()
        {
            Int16 body_size = (Int16)(this.position - Defines.HEADRSIZE);
            byte[] header = BitConverter.GetBytes(body_size);
            header.CopyTo(this.buffer, 0);
        }

        public void push_int16(Int16 data)
        {
            byte[] temp_buffer = BitConverter.GetBytes(data);
            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += temp_buffer.Length;
        }

        public void push(byte data)
        {
            byte[] temp_buffer = BitConverter.GetBytes(data);
            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += sizeof(byte);
        }

        public void push(Int16 data)
        {
            byte[] temp_buffer = BitConverter.GetBytes(data);
            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += temp_buffer.Length;
        }

        public void push(Int32 data)
        {
            byte[] temp_buffer = BitConverter.GetBytes(data);
            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += temp_buffer.Length;
        }

        public void push(string data)
        {
            byte[] temp_buffer = Encoding.UTF8.GetBytes(data);

            Int16 len = (Int16)temp_buffer.Length;
            byte[] len_buffet = BitConverter.GetBytes(len);
            len_buffet.CopyTo(this.buffer, this.position);
            this.position += sizeof(Int16);

            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += temp_buffer.Length;
        }
    }
}
