﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Octarine_Core.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.12.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/auth/registration")]
        public string ApiUrlRegister {
            get {
                return ((string)(this["ApiUrlRegister"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/auth/confirm")]
        public string ApiUrlConfirm {
            get {
                return ((string)(this["ApiUrlConfirm"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/auth/login")]
        public string ApiUrl {
            get {
                return ((string)(this["ApiUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/auth/send_code")]
        public string ApiUrlSendCode {
            get {
                return ((string)(this["ApiUrlSendCode"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string JwtToken {
            get {
                return ((string)(this["JwtToken"]));
            }
            set {
                this["JwtToken"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/")]
        public string ApiAll {
            get {
                return ((string)(this["ApiAll"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/request/send")]
        public string ApiAddFriend {
            get {
                return ((string)(this["ApiAddFriend"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/search?currentUserId=")]
        public string SearchFriend {
            get {
                return ((string)(this["SearchFriend"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/request/accept")]
        public string AcceptFriend {
            get {
                return ((string)(this["AcceptFriend"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/friend-requests?userId=")]
        public string GetUsersRuquests {
            get {
                return ((string)(this["GetUsersRuquests"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/create-private-chat")]
        public string CreatePrivateChate {
            get {
                return ((string)(this["CreatePrivateChate"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/get-messages")]
        public string GetMessagies {
            get {
                return ((string)(this["GetMessagies"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/get-chats/")]
        public string GetChats {
            get {
                return ((string)(this["GetChats"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int UserID {
            get {
                return ((int)(this["UserID"]));
            }
            set {
                this["UserID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int IdActiveChat {
            get {
                return ((int)(this["IdActiveChat"]));
            }
            set {
                this["IdActiveChat"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/send-message")]
        public string PostApi {
            get {
                return ((string)(this["PostApi"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FriendId {
            get {
                return ((int)(this["FriendId"]));
            }
            set {
                this["FriendId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Nik")]
        public string UserName {
            get {
                return ((string)(this["UserName"]));
            }
            set {
                this["UserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool InColling {
            get {
                return ((bool)(this["InColling"]));
            }
            set {
                this["InColling"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5001/voice/start-call")]
        public string StartCallAPI {
            get {
                return ((string)(this["StartCallAPI"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5001/voice/confirm-call")]
        public string ConfirmCallAPI {
            get {
                return ((string)(this["ConfirmCallAPI"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int UserPort {
            get {
                return ((int)(this["UserPort"]));
            }
            set {
                this["UserPort"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5001/voice/reject-call")]
        public string RejectCall {
            get {
                return ((string)(this["RejectCall"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5001/voice/end-call")]
        public string EndCall {
            get {
                return ((string)(this["EndCall"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/getPrivateChat")]
        public string GetChatID {
            get {
                return ((string)(this["GetChatID"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/UsersSettings/ChangeUserName")]
        public string ChangeNameApi {
            get {
                return ((string)(this["ChangeNameApi"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Windows.Controls.Grid BorderForEror {
            get {
                return ((global::System.Windows.Controls.Grid)(this["BorderForEror"]));
            }
            set {
                this["BorderForEror"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public Octarine_Core.Classic.ChatController ChatController
        {
            get
            {
                return ((Octarine_Core.Classic.ChatController)(this["ChatController"]));
            }
            set {
                this["ChatController"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/create-group")]
        public string CreateGroupChatId {
            get {
                return ((string)(this["CreateGroupChatId"]));
            }
            set {
                this["CreateGroupChatId"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/get-messages-byIdChat/")]
        public string LoadGroupChat {
            get {
                return ((string)(this["LoadGroupChat"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/get-chat-participants/")]
        public string LoadPartikals {
            get {
                return ((string)(this["LoadPartikals"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/add-users-to-chat")]
        public string AddUserinChat {
            get {
                return ((string)(this["AddUserinChat"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/delete-chat/")]
        public string DeleteChat {
            get {
                return ((string)(this["DeleteChat"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/get-creator-chat-ID/")]
        public string GetCreatorId {
            get {
                return ((string)(this["GetCreatorId"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/Chats/delete-user-from-chat")]
        public string DeleteUserFromChat {
            get {
                return ((string)(this["DeleteUserFromChat"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/remove")]
        public string DeleteFriendship {
            get {
                return ((string)(this["DeleteFriendship"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/auth/ping")]
        public string PingApi {
            get {
                return ((string)(this["PingApi"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://147.45.175.135:5000/api/friends/getUsersParts?userIds=")]
        public string GetUserData {
            get {
                return ((string)(this["GetUserData"]));
            }
            set {
                this["GetUserData"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("M")]
        public string MuteMicroHotKey {
            get {
                return ((string)(this["MuteMicroHotKey"]));
            }
            set {
                this["MuteMicroHotKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C")]
        public string MuteAllVoiceHotKey {
            get {
                return ((string)(this["MuteAllVoiceHotKey"]));
            }
            set {
                this["MuteAllVoiceHotKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.5")]
        public float ClientVolume {
            get {
                return ((float)(this["ClientVolume"]));
            }
            set {
                this["ClientVolume"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.5")]
        public float ReciverVolume {
            get {
                return ((float)(this["ReciverVolume"]));
            }
            set {
                this["ReciverVolume"] = value;
            }
        }
    }
}
