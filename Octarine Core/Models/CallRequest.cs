using System.Collections.Generic;


namespace Octarine_Core.Models
{
    public class CallRequest
    {
        public string CallerId { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public int CallerPoort { get; set; }
        public List<string> ParticipantIds { get; set; } = null;
        public CallRequest(string roomID,string callerId, List<string> participantIds)
        {
            CallerId = callerId;
            RoomId = roomID;
            ParticipantIds = participantIds;
        }
    }
}
