﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snap7;
using System.Threading;


namespace WpfApplication1
{

    public class PlcDb
    {
        private static PlcDb instance;

        static S7Client Client;
        static int ok = 0, ko = 0;
        static int SampleDB = -1;
        static int SampleDBSize = 0;
        static int AsyncResult;
        static bool AsyncDone;
        private static S7Client.S7CliCompletion Completion;


        private PlcDb() { }

        public static PlcDb Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlcDb();
                }
                return instance;
            }
        }

        static void CompletionProc(IntPtr usrPtr, int opCode, int opResult)
        {
            AsyncResult = opResult;
            AsyncDone = true;
        }


        //-------------------------------------------------------------------------  
        // Summary
        //-------------------------------------------------------------------------  
        static void Summary()
        {
            Console.WriteLine();
            Console.WriteLine("+-----------------------------------------------------");
            Console.WriteLine("| Test Summary");
            Console.WriteLine("+-----------------------------------------------------");
            Console.WriteLine("| Performed : " + (ok + ko).ToString());
            Console.WriteLine("| Passed    : " + ok.ToString());
            Console.WriteLine("| Failed    : " + ko.ToString());
            Console.WriteLine("+----------------------------------------[press a key]");
        }
        //------------------------------------------------------------------------------
        // HexDump, a very nice function, it's not mine.
        // I found it on the net somewhere some time ago... thanks to the author ;-)
        //------------------------------------------------------------------------------
        static void HexDump(byte[] bytes, int Size)
        {
            if (bytes == null)
                return;
            int bytesLength = Size;
            int bytesPerLine = 16;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
#if __MonoCS__
            result.Append('\n');
#endif
            }
            Console.WriteLine(result.ToString());
        }
        //------------------------------------------------------------------------------
        // Check error (simply writes an header)
        //------------------------------------------------------------------------------
        static bool Check(int Result, string FunctionPerformed)
        {
            Console.WriteLine();
            Console.WriteLine("+-----------------------------------------------------");
            Console.WriteLine("| " + FunctionPerformed);
            Console.WriteLine("+-----------------------------------------------------");
            if (Result == 0)
            {
                int ExecTime = Client.ExecTime();
                Console.WriteLine("| Result         : OK");
                Console.WriteLine("| Execution time : " + ExecTime.ToString() + " ms"); //+ Client.getex->ExecTime());
                Console.WriteLine("+-----------------------------------------------------");
                ok++; // one more test passed
            }
            else
            {
                Console.WriteLine("| ERROR !!! \n");
                if (Result < 0)
                    Console.WriteLine("| Library Error (-1)\n");
                else
                    Console.WriteLine("| " + Client.ErrorText(Result));
                Console.WriteLine("+-----------------------------------------------------\n");
                ko++;
            }
            return Result == 0;
        }




        #region [Info]
        //------------------------------------------------------------------------------
        // CPU Info : unit info
        //------------------------------------------------------------------------------
        static void CpuInfo()
        {
            S7Client.S7CpuInfo Info = new S7Client.S7CpuInfo();
            int res = Client.GetCpuInfo(ref Info);
            if (Check(res, "Unit Info"))
            {
                Console.WriteLine("  Module Type Name : " + Info.ModuleTypeName);
                Console.WriteLine("  Serial Number    : " + Info.SerialNumber);
                Console.WriteLine("  AS Name          : " + Info.ASName);
                Console.WriteLine("  Module Name      : " + Info.ModuleName);
            };
        }
        //------------------------------------------------------------------------------
        // List blocks in AG and chose a DB for next tests
        //------------------------------------------------------------------------------
        static void ListBlocks()
        {
            S7Client.S7BlocksList List = new S7Client.S7BlocksList();
            ushort[] DBList = new ushort[0x4000];
            int ItemsCount = DBList.Length;

            int res = Client.ListBlocks(ref List);
            if (Check(res, "List Blocks in AG"))
            {
                Console.WriteLine("  OBCount  : " + List.OBCount.ToString());
                Console.WriteLine("  FBCount  : " + List.FBCount.ToString());
                Console.WriteLine("  FCCount  : " + List.FCCount.ToString());
                Console.WriteLine("  SFBCount : " + List.SFBCount.ToString());
                Console.WriteLine("  SFCCount : " + List.SFCCount.ToString());
                Console.WriteLine("  DBCount  : " + List.DBCount.ToString());
                Console.WriteLine("  SDBCount : " + List.SDBCount.ToString());
            }
            else
                return;
            // List Blocks of Type (DB)
            // Note about ItemsCount: 
            //   on input must contain the buffer items count available 
            //   on output it contains the number of the items found
            //   
            res = Client.ListBlocksOfType(S7Client.Block_DB, DBList, ref ItemsCount);
            if (Check(res, "DB List in AG"))
            {
                if (ItemsCount > 0)
                {
                    for (int i = 0; i < ItemsCount; i++)
                        Console.WriteLine("  DB " + DBList[i].ToString());
                    SampleDB = DBList[0]; // Choose the 1st DB as Sample
                    Console.WriteLine();
                    Console.WriteLine("  DB " + SampleDB.ToString() + " was chosen to make some tests..");
                }
                else
                    Console.WriteLine("NO DB found, please load at least 1 DB to perform some tests...");
            }
        }
        //-------------------------------------------------------------------------  
        // Read a SZL block : ID 0x0011 IDX 0x0000
        //-------------------------------------------------------------------------  
        static void ReadSZL_0011_0000()
        {
            S7Client.S7SZL SZL = new S7Client.S7SZL();
            int Size = 0x8000;
            int res = Client.ReadSZL(0x0011, 0x000, ref SZL, ref Size);
            if (Check(res, "Read SZL - ID : 0x0011, IDX 0x0000"))
            {
                Console.WriteLine("  LENTHDR : " + SZL.Header.LENTHDR.ToString());
                Console.WriteLine("  N_DR    : " + SZL.Header.N_DR.ToString());
                Console.WriteLine("Dump : " + Size.ToString() + " bytes");
                HexDump(SZL.Data, Size);
            }
        }
        #endregion

        //------------------------------------------------------------------------------
        // DB Dump using DB Get                            
        //------------------------------------------------------------------------------
        static void DBGetAndDump()
        {
            if (SampleDB == -1)
                return; // we didn't find any DB in AG

            byte[] Buffer = new byte[0x10000]; // 64k buffer (max size allowed for a DB in S7400)
            // Note about Size: 
            //   on input must contain the buffer size (in bytes) available 
            //   on output it contains the bytes read 
            int Size = Buffer.Length;
            int res = Client.DBGet(SampleDB, Buffer, ref Size);
            if (Check(res, "DB Get (DB " + SampleDB.ToString() + ")"))
            {
                Console.WriteLine("Dump : " + Size.ToString() + " bytes");
                SampleDBSize = Size; // Store it for next test ;-)
                HexDump(Buffer, Size);
            }
        }
        //------------------------------------------------------------------------------
        // DB Dump using DB Read                            
        //------------------------------------------------------------------------------
        static void DBReadAndDump()
        {
            if ((SampleDB == -1) || (SampleDBSize == 0))
                return; // we didn't find any DB in AG

            byte[] Buffer = new byte[SampleDBSize]; // 64k buffer (max size allowed for a DB in S7400)
            int res = Client.DBRead(SampleDB, 0, SampleDBSize, Buffer);
            if (Check(res, "DB Read (DB = " + SampleDB.ToString() + ", Start = 0, Size = " + SampleDBSize.ToString() + ", Buffer)"))
            {
                Console.WriteLine("Dump : " + SampleDBSize.ToString() + " bytes");
                HexDump(Buffer, SampleDBSize);
            }
        }
        //------------------------------------------------------------------------------
        // Sync Upload SDB0 
        //------------------------------------------------------------------------------
        static void UploadSDB0()
        {
            byte[] Buffer = new byte[0x10000]; // 64k buffer (max size allowed for a DB in S7400)
            int Size = Buffer.Length;
            // Note about Size: 
            //   on input must contain the buffer size (in bytes) available 
            //   on output it contains the bytes read 
            int res = Client.Upload(S7Client.Block_SDB, 0, Buffer, ref Size);
            if (Check(res, "Block Upload (SDB 0)"))
            {
                Console.WriteLine("Dump : " + Size.ToString() + " bytes");
                HexDump(Buffer, Size);
            }
        }
        //------------------------------------------------------------------------------
        // Async Upload SDB0 using callback as "done" trigger 
        //------------------------------------------------------------------------------
        static void AsyncUploadCB_SDB0()
        {
            byte[] Buffer = new byte[0x10000]; // 64k buffer (max size allowed for a DB in S7400)
            int Size = Buffer.Length;
            AsyncDone = false;
            int res = Client.AsUpload(S7Client.Block_SDB, 0, Buffer, ref Size);
            if (res == 0) // this res refers only to the async job start
            {
                // this is a simply text mode demo : use callback to set a flag
                while (!AsyncDone)
                {
                    System.Threading.Thread.Sleep(50);
                }
                res = AsyncResult; // this is the operation result
            };
            if (Check(res, "Async Block Upload (CallBack) (SDB 0)"))
            {
                Console.WriteLine("Dump : " + Size.ToString() + " bytes");
                HexDump(Buffer, Size);
            }
        }
        //------------------------------------------------------------------------------
        // Async Upload SDB0 using Wait completion with timeout 
        //------------------------------------------------------------------------------
        static void AsyncUploadWC_SDB0()
        {
            byte[] Buffer = new byte[0x10000]; // 64k buffer (max size allowed for a DB in S7400)
            int Size = Buffer.Length;
            int res = Client.AsUpload(S7Client.Block_SDB, 0, Buffer, ref Size);
            if (res == 0) // this res refers only to the async job start
            {
                res = Client.WaitAsCompletion(3000);
            };
            if (Check(res, "Async Block Upload (Wait completion) (SDB 0)"))
            {
                Console.WriteLine("Dump : " + Size.ToString() + " bytes");
                HexDump(Buffer, Size);
            }
        }
        //------------------------------------------------------------------------------
        // Async Upload SDB0 using Check completion (polling)    
        //------------------------------------------------------------------------------
        static void AsyncUploadCC_SDB0()
        {
            byte[] Buffer = new byte[0x10000]; // 64k buffer (max size allowed for a DB in S7400)
            int Size = Buffer.Length;
            int res = Client.AsUpload(S7Client.Block_SDB, 0, Buffer, ref Size);
            if (res == 0) // this res refers only to the async job start
            {
                while (!Client.CheckAsCompletion(ref res))
                {
                    System.Threading.Thread.Sleep(50);
                }
            };
            if (Check(res, "Async Block Upload (Polling) (SDB 0)"))
            {
                Console.WriteLine("Dump : " + Size.ToString() + " bytes");
                HexDump(Buffer, Size);
            }
        }

        //-------------------------------------------------------------------------  
        // PLC connection
        //-------------------------------------------------------------------------  
        static bool PlcConnect(string Address, int Rack, int Slot)
        {
            int res = Client.ConnectTo(Address, Rack, Slot);
            if (Check(res, "UNIT Connection"))
            {
                int Requested = Client.RequestedPduLength();
                int Negotiated = Client.NegotiatedPduLength();
                Console.WriteLine("  Connected to   : " + Address + " (Rack=" + Rack.ToString() + ", Slot=" + Slot.ToString() + ")");
                Console.WriteLine("  PDU Requested  : " + Requested.ToString());
                Console.WriteLine("  PDU Negotiated : " + Negotiated.ToString());
            }
            return res == 0;
        }
        //-------------------------------------------------------------------------  
        // Perform some safe (readonly) tests
        //-------------------------------------------------------------------------  
        static void PerformTests()
        {
                /*
            CpuInfo();
            ListBlocks();
            DBGetAndDump();
            DBReadAndDump();
            UploadSDB0();
            ReadSZL_0011_0000();
            // Async functions
            AsyncUploadCB_SDB0();
            AsyncUploadWC_SDB0();
            AsyncUploadCC_SDB0();
                 * */
        }

        public static UInt16 getLungime()
        {
            byte[] b0 = new byte[2];
            Client.DBRead(100, 4, 2, b0);
            UInt16 lung = b0[0];
            lung = (ushort)(lung << 8);
            lung = (ushort)(lung | b0[1]);

            return lung;
        }

        public static UInt16 getGrosime()
        {
            byte[] b0 = new byte[2];
            Client.DBRead(100, 2, 2, b0);
            UInt16 grosime = b0[0];
            grosime = (ushort)(grosime << 8);
            grosime = (ushort)(grosime | b0[1]);

            return grosime;
        }

        public static void POS_Connect(){
                byte[] b18=new byte[5];
                Client.DBRead(100, 18,1,b18);
                b18[0] = (byte)(b18[0] | 0x04);
                Client.DBWrite(100, 18, 1, b18);
        }

        public static void POS_Reset()
        {
            byte[] b18 = new byte[5];
            
            Client.DBRead(100, 18, 1, b18);
        //    Console.Out.WriteLine("Reset inainte" + b18[0]);
            //b18[0] = (byte)(b18[0] | 0x); 
            b18[0] = (byte)(b18[0] | 0x01);
            Client.DBWrite(100, 18, 1, b18);
            Thread.Sleep(50);
            Client.DBRead(100, 18, 1, b18);
            b18[0] = (byte)(b18[0] & (~0x01));
            Client.DBWrite(100, 18, 1, b18);
            POS_Manual();
            Console.Out.WriteLine("Reset efectuat. Trecut in manual");
        }



        public static void POS_Disconect()
        {
            byte[] b18 = new byte[5];
            Client.DBRead(100, 18, 1, b18);
            Console.Out.WriteLine("Byte 18 inainte " + b18[0]);
            b18[0] = (byte)(b18[0] | (0x20));
            Console.Out.WriteLine("Byte 18 dupa " + b18[0]);
            Client.DBWrite(100, 18, 1, b18);
        }

        public static void POS_Start()
        {
            byte[] b18 = new byte[5];
            Client.DBRead(100, 18, 1, b18);
            b18[0] = (byte)(b18[0] | (0x20));
            Client.DBWrite(100, 18, 1, b18);

            Thread.Sleep(50);

            Client.DBRead(100, 18, 1, b18);
            b18[0] = (byte)(b18[0] & (~0x20));
            Client.DBWrite(100, 18, 1, b18);
        }

        public static void POS_Auto()
        {
            byte[] b18 = new byte[5];
            Client.DBRead(100, 18, 1, b18);
            Console.Out.WriteLine("Byte 18 inainte " + b18[0]);
            b18[0] = (byte)(b18[0] & (0xF7));
            Console.Out.WriteLine("Byte 18 intermediar" + b18[0]);
            b18[0] = (byte)(b18[0] | (0x10));
            Console.Out.WriteLine("Byte 18 dupa " + b18[0]);
            Client.DBWrite(100, 18, 1, b18);
        }

        public static void POS_Manual()
        {
            byte[] b18 = new byte[5];
            Client.DBRead(100, 18, 1, b18);
            b18[0] = (byte)(b18[0] | (0x08));// 18.3-H, 18.4-L
            b18[0] = (byte)(b18[0] & (0xEF));
                // 1110 1111
            Client.DBWrite(100, 18, 1, b18);
        }

        public static void POS_NextPlanck()
        {
            byte[] b18 = new byte[5];
            Client.DBRead(100, 18, 1, b18);
            b18[0] = (byte)(b18[0] | (0x40));
            Client.DBWrite(100, 18, 1, b18);

            Thread.Sleep(50);

            Client.DBRead(100, 18, 1, b18);
            b18[0] = (byte)(b18[0] & (~0x40));
            Client.DBWrite(100, 18, 1, b18);
        }

        public void PLC_Connect_Handler(String ip){
             int Rack = 0, Slot = 0; // default for S71200
             Client = new S7Client();
             Completion = new S7Client.S7CliCompletion(CompletionProc);
             Client.SetAsCallBack(Completion, IntPtr.Zero);
             if (PlcConnect(ip, Rack, Slot))
             {
                 POS_Connect();
                 Client.Disconnect();
             }
       // Summary();
        }
        public void PLC_Disconnect_Handler(String ip)
        {
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                POS_Disconect();
                Client.Disconnect();
            }
            // Summary();
        }

        public void PLC_Reset_Handler(String ip)
        {
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                POS_Reset();
                Client.Disconnect();
            }
            // Summary();
        }

        public void PLC_Auto_Handler(String ip)
        {
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                POS_Auto();
                Client.Disconnect();
            }
            // Summary();
        }

        public void PLC_Manual_Handler(String ip)
        {
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                POS_Manual();
                Client.Disconnect();
            }
            // Summary();
        }

        public void PLC_Start_Handler(String ip)
        {
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                POS_Start();
                Client.Disconnect();
            }
            // Summary();
        }

        public void PLC_NextPlanck_Handler(String ip)
        {
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                POS_NextPlanck();
                Client.Disconnect();
            }
            // Summary();
        }

        public UInt16 readGrosime(String ip)
        {
            UInt16 resultat = 0;
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                resultat = getGrosime();
                Client.Disconnect();
            }
            // Summary();
            return resultat;
        }

        public UInt16 readLungime(String ip)
        {
            UInt16 resultat = 0;
            int Rack = 0, Slot = 0; // default for S71200
            Client = new S7Client();
            Completion = new S7Client.S7CliCompletion(CompletionProc);
            Client.SetAsCallBack(Completion, IntPtr.Zero);
            if (PlcConnect(ip, Rack, Slot))
            {
                resultat = getLungime();
                Client.Disconnect();
            }
            // Summary();
            return resultat;
        }
    }
}
