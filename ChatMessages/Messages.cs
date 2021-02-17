using System;
using System.Collections.Generic;

namespace ChatMessages
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    public abstract class Message
    {
        public MessageTypes MessageType;
        public abstract byte[] ToArray();
    }

    public class MessageLogin : Message
    {
        const int passwordLength = 32;
        public MessageLogin()
        {
            MessageType = MessageTypes.Login;
        }
        public byte[] Password;
        public string UserName;

        private int Size => 1 + passwordLength + UserName.Length;

        public override byte[] ToArray()
        {
            byte[] data = new byte[Size];
            Array.Copy(Password, 0, data, 1, passwordLength);
            var NameArray = UserName.ConvertToArray();
            Array.Copy(NameArray, 0, data, 1 + passwordLength, NameArray.Length);
            return data;
        }

        public MessageLogin(byte[] Data)
        {
            if (Data == null || Data.Length < 1 + passwordLength) throw new ArgumentException("Datenpaket ungültig");
            MessageType = MessageTypes.Login;
            Password = Data[1..(passwordLength + 1)];
            UserName = Data[(1 + passwordLength)..].ConvertToString();
        }
    }

    public class MessageLoginSuccessful : Message
    {
        //TODO: add sessionIDs to identify clients
        //byte[] SessionID;
        //const int sessionIDLength = 4;

        public MessageLoginSuccessful()
        {
            MessageType = MessageTypes.LoginSuccessful;
        }

        public MessageLoginSuccessful(byte[] Data)
        {
            if (Data == null || Data.Length != 1) throw new ArgumentException("Datenpaket ungültig");
            MessageType = (MessageTypes)Data[0];
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[1];
            data[0] = (byte)MessageType;
            return data;
        }
    }

    public class MessageLoginFail : Message
    {
        public LoginFailReason Reason;
        public MessageLoginFail()
        {
            MessageType = MessageTypes.LoginFail;
        }

        public MessageLoginFail(byte[] Data)
        {
            if (Data == null || Data.Length != 2) throw new ArgumentException("Datenpaket ungültig");
            MessageType = MessageTypes.LoginFail;
            Reason = (LoginFailReason)Data[1];
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[2];
            data[0] = (byte)MessageType;
            data[1] = (byte)Reason;
            return data;
        }
    }

    public class MessageLogout : Message
    {
        public MessageLogout()
        {
            MessageType = MessageTypes.Logout;
        }

        public override byte[] ToArray()
        {
            return new byte[1] { (byte)MessageType };
        }
    }

    public class MessageDirect : Message
    {
        public string SourceName;
        public string DestinationName;
        public DataType ContentType;
        public byte[] Content;

        public MessageDirect()
        {
            MessageType = MessageTypes.DirectMessage;
        }
        public MessageDirect(byte[] Data)
        {
            if (Data == null || Data.Length < 4) throw new ArgumentException("Datenpaket ungültig");
            MessageType = MessageTypes.DirectMessage;
            ContentType = (DataType)Data[1];
            SourceName = Data[4..(Data[2] + 4)].ConvertToString();
            DestinationName = Data[(4 + SourceName.Length)..(4 + SourceName.Length + Data[3])].ConvertToString();
            Content = Data[(4 + SourceName.Length + Data[3])..];
        }

        public int Size => 4 + DestinationName.Length + SourceName.Length + Content.Length;

        public override byte[] ToArray()
        {
            byte[] data = new byte[Size];
            data[1] = (byte)ContentType;
            data[2] = (byte)SourceName.Length;
            data[3] = (byte)DestinationName.Length;
            var buffer = SourceName.ConvertToArray();
            Array.Copy(buffer, 0, data, 4, buffer.Length);
            buffer = DestinationName.ConvertToArray();
            Array.Copy(buffer, 0, data, 4 + SourceName.Length, buffer.Length);
            Array.Copy(Content, 0, data, 4 + SourceName.Length + DestinationName.Length, Content.Length);
            return data;
        }
    }

    public class MessageRoom : Message
    {
        public string RoomID;
        public string SourceName;
        public DataType ContentType;
        public byte[] Content;

        public MessageRoom()
        {
            MessageType = MessageTypes.RoomMessage;
            RoomID = string.Empty;
            SourceName = string.Empty;
        }
        public MessageRoom(byte[] Data)
        {
            if (Data == null || Data.Length < 4) throw new ArgumentException("Datenpaket ungültig");
            MessageType = MessageTypes.RoomMessage;
            ContentType = (DataType)Data[1];
            RoomID = Data[4..(4 + Data[2])].ConvertToString();
            SourceName = Data[(4 + RoomID.Length)..(4 + RoomID.Length + Data[3])].ConvertToString();
            Content = Data[(4 + RoomID.Length + SourceName.Length)..];
        }

        public int Size => 4 + RoomID.Length + SourceName.Length + Content.Length;

        public override byte[] ToArray()
        {
            byte[] data = new byte[Size];
            data[0] = (byte)MessageType;
            data[1] = (byte)ContentType;
            data[2] = (byte)RoomID.Length;
            data[3] = (byte)SourceName.Length;
            byte[] buffer = RoomID.ConvertToArray();
            Array.Copy(buffer, 0, data, 4, buffer.Length);
            buffer = SourceName.ConvertToArray();
            Array.Copy(buffer, 0, data, 4 + RoomID.Length, buffer.Length);
            Array.Copy(Content, 0, data, 4 + RoomID.Length + buffer.Length, Content.Length);
            return data;
        }
    }

    public class MessageBroadcast : Message
    {
        public DataType ContentType;
        public byte[] Content;

        public MessageBroadcast()
        {
            MessageType = MessageTypes.Broadcast;
        }
        public MessageBroadcast(byte[] Data)
        {
            if (Data == null || Data.Length < 2) throw new ArgumentException("Datenpaket ungültig");
            MessageType = MessageTypes.Broadcast;
            ContentType = (DataType)Data[1];
            Content = Data[2..];
        }

        private int Size => 2 + Content.Length;

        /// <summary>
        /// Never used
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            byte[] data = new byte[Size];
            data[1] = (byte)ContentType;
            Array.Copy(Content, 0, data, 2, Content.Length);
            return data;
        }
    }

    public class MessageKick : Message
    {
        public MessageKick()
        {
            MessageType = MessageTypes.KickFromServer;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[1];
            data[0] = (byte)MessageType;
            return data;
        }
    }

    public class MessageRequestUserList : Message
    {
        public MessageRequestUserList()
        {
            MessageType = MessageTypes.RoomUserList;
        }
        public override byte[] ToArray()
        {
            return new byte[1] { (byte)MessageType };
        }
    }

    public class MessageUserList : Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Must be generic list because of index operations")]
        public List<string> UserList;

        public MessageUserList()
        {
            MessageType = MessageTypes.RoomUserList;
            UserList = new List<string>();
        }

        public MessageUserList(byte[] Data)
        {
            if (Data == null || Data.Length < 4) throw new ArgumentException("Datenpaket ungültig");
            MessageType = MessageTypes.RoomUserList;
            UserList = new List<string>(Data[1..^1].ConvertToString().Split(';'));
        }
        public override byte[] ToArray()
        {
            byte[] data = new byte[Size];
            data[0] = (byte)MessageType;
            int writePos = 1;
            for (int counter = 0; counter < UserList.Count; counter++)
            {
                byte[] nameArray = UserList[counter].ConvertToArray();
                Array.Copy(nameArray, 0, data, writePos, nameArray.Length);
                writePos += nameArray.Length;
                data[writePos] = ";".ConvertToArray()[0]; //TODO ggfs bessere konvertierung möglich?
                writePos++;
            }
            return data;
        }

        private int Size
        {
            get
            {
                int sum = 1;
                foreach (var name in UserList)
                    sum += name.Length + 1;
                return sum;
            }
        }
    }
#pragma warning restore CA1051 // Do not declare visible instance fields
}
