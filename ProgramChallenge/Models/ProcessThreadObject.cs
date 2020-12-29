using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using ProgramChallenge.Enums;

namespace ProgramChallenge.Models
{

    /// <summary>
    /// Returns the results for the Email Parsed stream
    /// </summary>
    public class ProcessThreadObject
    {
        public string RawInput { get; set; }
        public  ConvoResultEnum  RawThreadStatus { get; set; }
        public byte[] Bytes { get; set; }
        public Guid ThreadId { get; set; }
        public int ReplyCount { get; set; }
        public DateTime  ThreadDateTime { get; set; }
        public List<ChildReply> Replys { get; set; }

        
        
    }
}
