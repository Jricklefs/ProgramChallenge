using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ProgramChallenge.Enums;

using ProgramChallenge.Models;

namespace ProgramChallenge
{
    public  class EmailConvoThread 
    {

       

        /// <summary>
        /// Determine if the Length is 22 & if it is, Does it valid kids/Replys
        /// Valid Kids/Replys are 5 bites long. 
        /// </summary>
        /// <param name="bits"></param>
        
        /// <returns></returns>
        private ConvoResultEnum IsValidlength( byte[] myBytes)
            {
                int len = myBytes.Length;
               
                if(len < 22)
                    return ConvoResultEnum.Incomplete;
                            
                

                int replyLen = len - 22;
                if (replyLen == 0)
                    return ConvoResultEnum.ValidWithoutReplys;


                if (replyLen % 5 != 0)
                    return ConvoResultEnum.ReplysAreIncomplete;

                return ConvoResultEnum.ValidWithReplys;
                 

            }
            /// <summary>
            /// Get Thread Guid
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns></returns>
            private Guid GetThreadId(byte[] bytes)
            {
                return new Guid(bytes.Skip(6).Take(16).ToArray());

            }


            /// <summary>
            /// Identifies if there are Replys to thsi thread to process
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns></returns>
            private int NumberOfReplys(byte[]bytes)
            {
                return (bytes.Length - 22) / 5;
            }


            /// <summary>
            /// Converts thread to byte[] for processing
            /// </summary>
            /// <param name="threadIndex"></param>
            /// <returns></returns>
            private byte[] GetBytesFromString( string threadIndex)
            {

                return Convert.FromBase64String(threadIndex);
            }


            /// <summary>
            /// Gets the datetime of Main Thread
            /// </summary>
            /// <param name="mybytes"></param>
            /// <returns></returns>
            private DateTime GetDatTimeForMainThread(byte[] mybytes)
            {
                var ticks = mybytes
                    .Take(6)
                    .Select(b => (long)b).Aggregate((l1, l2) => (l1 << 8) + l2) << 16;

                return new DateTime(ticks).AddYears(1600);

           
                
      
            }



            /// <summary>
            /// Loops through and gets the Dates of the replys. 
            /// </summary>
            /// <param name="replyBytes"></param>
            /// <param name="mainThreadDateTime"></param>
            /// <returns></returns>
            private List<ChildReply> GetReplys(byte[] replyBytes, DateTime mainThreadDateTime)
            {
                int replyCount = NumberOfReplys(replyBytes);

                List<ChildReply> listCR = new List<ChildReply>();
                for (var i = 0; i < replyCount; i++)
                {

                    var childTicks = replyBytes
                                         .Skip(22 + i * 5).Take(4)
                                         .Select(b => (long) b)
                                         .Aggregate((l1, l2) => (l1 << 8) + l2)
                                     << 18;

                    childTicks &= ~((long) 1 << 50);
                        listCR.Add(new ChildReply()
                            {
                             ReplyDate = mainThreadDateTime.AddTicks(childTicks)
                            });
                       
                }

                return listCR;
            }

        /// <summary>
        /// Process the ThreadIndex and identifies if there are children or not. 
        /// </summary>
        /// <param name="threadIndex"></param>
        /// <param name="pto"></param>
        /// <returns></returns>
            public ProcessThreadObject ProcessThreadObj(string threadIndex, ProcessThreadObject pto)
            {
               
                
                pto.RawInput = threadIndex;
                pto.Bytes = GetBytesFromString(threadIndex);
                pto.RawThreadStatus = IsValidlength(pto.Bytes);

                // if Thead is messed up kick it. 
                if (pto.RawThreadStatus == ConvoResultEnum.ReplysAreIncomplete ||
                    pto.RawThreadStatus == ConvoResultEnum.Incomplete)
                return pto;

                pto.ThreadId = GetThreadId(pto.Bytes);

                pto.ThreadDateTime = GetDatTimeForMainThread(pto.Bytes);


                if (pto.RawThreadStatus == ConvoResultEnum.ValidWithReplys)
                {
                    pto.Replys = GetReplys(pto.Bytes, pto.ThreadDateTime);
                }

                return pto;
            }

            
    }
}
