namespace ChatMessages
{
    public enum MessageTypes : byte
    {
        // client sending
        Login,
        Logout,
        RoomChange,
        DirectMessage,
        RoomMessage,

        // admin client sending
        KickToLobby,
        KickFromServer,
        BanFromServer,

        // server sending
        LoginSuccessful = 127,
        LoginFail,
        UserEntersRoom,
        UserLeavesRoom,
        RoomCreated,
        RoomDeleted,
        RoomUserList,
        RoomChangeDenied,
        ServerShutdown,
        Broadcast
    }

    public enum DataType : byte
    {
        Text,
        Image,
        File
    }

    public enum LoginFailReason : byte
    {
        UnknownUsername,
        IncorrectPassword,
        Banned,
        AlreadyLoggedIn
    }
}
