using ChatMessages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace ChatTests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void Login()
        {
            MessageLogin ml = new();
            using (SHA256 hash = SHA256.Create())
                ml.Password = hash.ComputeHash("1234567890".ConvertToArray());
            ml.UserName = "abcdefghij";

            byte[] arr = ml.ToArray();
            MessageLogin ml2 = new(arr);

            Assert.IsTrue(ml.MessageType == MessageTypes.Login);
            Assert.IsTrue(ml.UserName == ml2.UserName);
            for (int counter = 0; counter < ml.Password.Length; counter++)
                Assert.IsTrue(ml.Password[counter] == ml2.Password[counter]);
            Assert.IsTrue(ml.MessageType == ml2.MessageType);
        }

        [TestMethod]
        public void Direct()
        {
            MessageDirect md = new();
            md.SourceName = "abcdefgh";
            md.DestinationName = "jklmnop";
            md.ContentType = DataType.Text;
            md.Content = "Hallo Welt!".ConvertToArray();

            MessageDirect md2 = new(md.ToArray());

            Assert.IsTrue(md.MessageType == MessageTypes.DirectMessage);
            Assert.IsTrue(md.SourceName == md2.SourceName);
            Assert.IsTrue(md.DestinationName == md2.DestinationName);
            Assert.IsTrue(md.ContentType == md2.ContentType);
            for (int counter = 0; counter < md.Content.Length; counter++)
                Assert.IsTrue(md.Content[counter] == md2.Content[counter]);
        }

        [TestMethod]
        public void LoginSuccessful()
        {
            MessageLoginSuccessful mls = new();
            MessageLoginSuccessful mls2 = new(mls.ToArray());

            Assert.IsTrue(mls.MessageType == MessageTypes.LoginSuccessful);
            Assert.IsTrue(mls2.MessageType == MessageTypes.LoginSuccessful);        
        }
        
        [TestMethod]
        public void LoginFail()
        {
            MessageLoginFail mlf = new();
            mlf.Reason = LoginFailReason.IncorrectPassword;
            MessageLoginFail mlf2 = new(mlf.ToArray());

            Assert.IsTrue(mlf.MessageType ==  MessageTypes.LoginFail);
            Assert.IsTrue(mlf2.MessageType == MessageTypes.LoginFail);
            Assert.IsTrue(mlf.Reason == mlf2.Reason);
        }

        [TestMethod]
        public void Logout()
        {
            MessageLogout mlo = new();
            Assert.IsTrue(mlo.MessageType == MessageTypes.Logout);
        }
        
        [TestMethod]
        public void Broadcast()
        {
            MessageBroadcast mb = new();
            mb.ContentType = DataType.Text;
            mb.Content = "abcdefghi".ConvertToArray();

            MessageBroadcast mb2 = new(mb.ToArray());
            Assert.IsTrue(mb.MessageType == MessageTypes.Broadcast);
            Assert.IsTrue(mb2.MessageType == MessageTypes.Broadcast);
            Assert.IsTrue(mb.ContentType == mb2.ContentType);
            for (int counter = 0; counter < mb2.Content.Length; counter++)
                Assert.IsTrue(mb.Content[counter] == mb2.Content[counter]);
        }

        [TestMethod]
        public void Room()
        {
            MessageRoom mr = new();
            mr.ContentType = DataType.Text;
            mr.RoomID = "lmnopqr";
            mr.SourceName = "zyxwvuts";
            mr.Content = "abcdefghi".ConvertToArray();

            MessageRoom mr2 = new(mr.ToArray());
            Assert.IsTrue(mr.MessageType == MessageTypes.RoomMessage);
            Assert.IsTrue(mr2.MessageType == MessageTypes.RoomMessage);
            Assert.IsTrue(mr.ContentType == mr2.ContentType);
            Assert.IsTrue(mr.SourceName == mr2.SourceName);
            Assert.IsTrue(mr.RoomID == mr2.RoomID);
            for (int counter = 0; counter < mr2.Content.Length; counter++)
                Assert.IsTrue(mr.Content[counter] == mr2.Content[counter]);
        }

        [TestMethod]
        public void UserList()
        {
            MessageUserList mul = new();
            mul.UserList.Add("alpha");
            mul.UserList.Add("bravo");
            mul.UserList.Add("charly");
            mul.UserList.Add("delta");
            mul.UserList.Add("echo");

            MessageUserList mul2 = new(mul.ToArray());

            Assert.IsTrue(mul.MessageType == MessageTypes.RoomUserList);
            Assert.IsTrue(mul2.MessageType == MessageTypes.RoomUserList);
            Assert.IsTrue(mul2.UserList.Count == mul.UserList.Count);

            for (int counter = 0; counter < mul.UserList.Count; counter++)
            {
                Assert.AreEqual(mul.UserList[counter], mul2.UserList[counter]);
            }
        }
    }
}
