using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;


namespace mkdb.Python
{
    class FileHelper
    {
        private const int KB = 1024;
        private const int MB = 1024 * 1024;
        private const int GB = 1024 * 1024 * 1024;

        /// <summary>
        /// Writes bytes[] into file referred to by <paramref name="filename"/> at <paramref name="position"/>, 
        /// increasing length of file by length of inserted bytes.
        /// </summary>
        /// <param name="bytes">Array of bytes to insert into file</param>
        /// <param name="filename">Target file to receive <paramref name="bytes[]"/></param>
        /// <param name="position">Position in file at which to insert <paramref name="bytes[]"/></param>
        public static void InsertBytes(byte[] bytes, string filename, long position)
        {
            InsertBytes(bytes, filename, position, 32 * KB);
        }

        /// <summary>
        /// Identical to the other InsertBytes() overload, except it adds the bfrSz argument
        /// to force a specific read/write buffer size when Transpose or transposeReverse is 
        /// called. Also, removes the redundant length comparison logic in the other overload, 
        /// since Transpose() now supports that internally:
        /// </summary>
        public static void InsertBytes(byte[] bytes, string filename, long position, int bfrSz)
        {
            // Length of file before insert:
            long fileLen = new FileInfo(filename).Length;
            // Extend the target file to accomodate bytes[]:
            SetFileLen(filename, fileLen + bytes.Length);
            // Move the bytes after our insert position to make room for 
            // the bytes we're inserting, in one fell swoop:
            Transpose(filename, position, position + bytes.Length, fileLen - position, bfrSz);
            // Then insert the desired bytes and we're done:
            WriteBytes(bytes, filename, position);
        }

        /// <summary>
        /// *** NOT FOR PRODUCTION USE, LACKS ERROR CHECKING ***
        /// Allows performance testing of all insert stragegies
        /// *** NOT FOR PRODUCTION USE, LACKS ERROR CHECKING ***
        /// </summary>
        public static void InsertBytes(byte[] bytes, string filename, long position, int bfrSz, InsertStrategies strategy)
        {
            // Length of file before insert:
            long fileLen = new FileInfo(filename).Length;            
            
            // Move the bytes after our insert position to make room for 
            // the bytes we're inserting, in one fell swoop:
            switch(strategy)
            {
                case InsertStrategies.UseTransposeForward:
                    // Extend the target file to accomodate bytes[]:
                    SetFileLen(filename, fileLen + bytes.Length);
                    transposeForward(filename, position, position + bytes.Length, fileLen - position, bfrSz);
                    break;
                case InsertStrategies.UseTargetAsTempFile:
                    InsertBytesUsingEOFTemp(bytes, filename, position);
                    break;
                case InsertStrategies.UseTransposeReverse:
                    // Extend the target file to accomodate bytes[]:
                    SetFileLen(filename, fileLen + bytes.Length);
                    // Transpose will automatically use TransposeReverse:
                    transposeReverse(filename, position, position + bytes.Length, fileLen - position, bfrSz);
                    break;
            }
            // Then write in the desired bytes and we're done:
            WriteBytes(bytes, filename, position);
        }

        /// <summary>
        /// Inserts bytes into a file while avoiding any external memory or disk buffers;
        /// when needed, the target file provides its own temp space:
        /// </summary>
        /// <param name="bytes">Bytes to be inserted</param>
        /// <param name="filename">Target file</param>
        /// <param name="position">Insertion position</param>
        public static void InsertBytesUsingEOFTemp(byte[] bytes, string filename, long position)
        {
            long fileLen = new FileInfo(filename).Length;
            long suffixLen = fileLen - position;
            long suffixTempPosition;
            long tempLen;
            // Is the Inserted text inter or shorter than the right segment?

            long compare = suffixLen.CompareTo(bytes.Length);
            // If we're shifting the RH segment right by its own length or more, 
            // then we have it easy; shift it exactly enough to accomodate the
            // inserted bytes...
            if (compare < 0)
            {
                suffixTempPosition = position + bytes.Length;
                tempLen = (suffixTempPosition + suffixLen);
                SetFileLen(filename, tempLen);
                Transpose(filename, position, suffixTempPosition, suffixLen);
                WriteBytes(bytes, filename, position);
            }
            // Otherwise, if we're shifting the RH segment right by less than 
            // its own length, we'll encounter a write/read collision, so
            // we would need to preserve the RH segment by buffering [1]:
            else
            {
                suffixTempPosition = fileLen;
                tempLen = (fileLen + suffixLen);
                SetFileLen(filename, tempLen);
                Transpose(filename, position, suffixTempPosition, suffixLen);
                WriteBytes(bytes, filename, position);
                Transpose(filename, suffixTempPosition, position + bytes.Length, suffixLen);
                SetFileLen(filename, fileLen + bytes.Length);
            }
            // [1] See InsertBytes() and transposeReverse() for a more efficient approach;
        }

        /// <summary>
        /// Within <paramref name="filename"/>, moves a range of <paramref name="Len"/> bytes 
        /// starting at <paramref name="SourcePos"/> to <paramref name="DestPos"/>.
        /// </summary>
        /// <param name="filename">The target file</param>
        /// <param name="SourcePos">The starting position of the byte range to move</param>
        /// <param name="DestPos">The destination position of the byte range</param>
        /// <param name="Len">The number of bytes to move</param>
        public static void Transpose(string filename, long SourcePos, long DestPos, long Len)
        {
            // 32KB is consistently among the most efficient buffer sizes:
            Transpose(filename, SourcePos, DestPos, Len, 32 * KB);
        }

        /// <summary>
        /// Identical to Transpose(), but allows the caller to specify a read/write buffer
        /// size if transposeReverse is called:
        /// </summary>
        public static void Transpose(string filename, long SourcePos, long DestPos, long Len, int bfrSz)
        {
            if (DestPos > SourcePos && Len > (DestPos - SourcePos))
            {
                // Delegate work to transposeReverse, telling it to use a
                // specified read/write buffer size:
               transposeForward(filename, SourcePos, DestPos, Len, bfrSz);
            }
            else
            {
                using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                {
                    using (FileStream fsr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fsr))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fsw))
                            {
                                sr.BaseStream.Position = SourcePos;
                                bw.BaseStream.Seek(DestPos, SeekOrigin.Begin);
                                //bw.Seek(int.Parse(DestPos.ToString()), SeekOrigin.Begin);
                                for (long i = 0; i < Len; i++)
                                {
                                    bw.Write((byte)sr.Read());
                                }
                                bw.Close();
                                sr.Close();
                            }
                        }
                    }
                }
            }
        }

        private static void transposeForward(string filename, long SourcePos, long DestPos, long Length, int bfrSz)
        {
            long distance = DestPos - SourcePos;
            if (distance < 1)
            {
                throw new ArgumentOutOfRangeException
                    ("DestPos", "DestPos is less than SourcePos, and this method can only copy byte ranges to the right.\r\n" +
                    "Use the public Transpose() method to copy a byte range to the left of itself.");
            }
            long readPos = SourcePos;// +Length;
            long writePos = DestPos;// +Length;
            bfrSz = bfrSz < 1 ? 32 * KB :
                (int)Math.Min(bfrSz, Length);
            // more than 40% of available memory poses a high risk of
            // OutOfMemoryExceptions when allocating 2x buffer, and
            // saps performance anyway:
            bfrSz=(int)Math.Min(bfrSz, (My.ComputerInfo.AvailablePhysicalMemory * .4));

            long numReads = Length / bfrSz;
            byte[] buff = new byte[bfrSz];
            byte[] buff2 = new byte[bfrSz];
            int remainingBytes = (int)Length % bfrSz;
            using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                using (FileStream fsr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fsr))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fsw))
                        {
                            sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                            bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                            // prime Buffer B:
                            sr.BaseStream.Read(buff2, 0, bfrSz);
                            for (long i = 1; i < numReads; i++)
                            {
                                buff2.CopyTo(buff,0);
                                sr.BaseStream.Read(buff2, 0, bfrSz);
                                bw.Write(buff, 0, bfrSz);                                
                                                                
                            }
                            buff2.CopyTo(buff,0);
                            if (remainingBytes > 0)
                            {
                                buff2 = new byte[remainingBytes];
                                sr.BaseStream.Read(buff2, 0, remainingBytes);
                                bw.Write(buff, 0, bfrSz);
                                bfrSz = remainingBytes;
                                buff = new byte[bfrSz];
                                buff2.CopyTo(buff,0);
                            }
                            bw.Write(buff, 0, bfrSz);
                            bw.Close();
                            sr.Close();
                            buff = null;
                            buff2 = null;
                        }
                    }
                }
            }
            GC.Collect();
        }

        private static void transposeReverse(string filename, long SourcePos, long DestPos, long Length, int bfrSz)
        {
            long distance = DestPos - SourcePos;
            if (distance < 1)
            {
                throw new ArgumentOutOfRangeException
                    ("DestPos", "DestPos is less than SourcePos, and this method can only copy byte ranges to the right.\r\n" +
                    "Use the public Transpose() method to copy a byte range to the left of itself.");
            }
            long readPos = SourcePos + Length;
            long writePos = DestPos + Length;
            bfrSz = bfrSz < 1 ? (int)Math.Min(My.ComputerInfo.AvailablePhysicalMemory * .9, Length) : (int)Math.Min(bfrSz, Length);

            long numReads = Length / bfrSz;
            byte[] buff = new byte[bfrSz];
            int remainingBytes = (int)Length % bfrSz;
            using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                using (FileStream fsr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fsr))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fsw))
                        {
                            sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                            bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                            for (long i = 0; i < numReads; i++)
                            {
                                readPos -= bfrSz;
                                writePos -= bfrSz;
                                sr.DiscardBufferedData();
                                sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                                sr.BaseStream.Read(buff, 0, bfrSz);
                                bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                                bw.Write(buff, 0, bfrSz);
                            }
                            if (remainingBytes > 0)
                            {
                                bfrSz = remainingBytes;
                                readPos -= bfrSz;
                                writePos -= bfrSz;
                                sr.DiscardBufferedData();
                                sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                                sr.BaseStream.Read(buff, 0, bfrSz);
                                bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                                bw.Write(buff, 0, bfrSz);
                            }
                            bw.Close();
                            sr.Close();
                            buff = null;
                        }
                    }
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// Writes bytes[] into <paramref name="file"/> at [position], overwriting existing contents
        /// </summary>
        /// <param name="bytes">Array of bytes to write into <paramref name="file"/></param>
        /// <param name="filename">Target file to be written to
        /// <param name="position">Position at which to begin writing
        public static void WriteBytes(byte[] bytes, string filename, long position)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.BaseStream.Seek(position, SeekOrigin.Begin);
                bw.Write(bytes);
                bw.Close(); fs.Close();
            }
        }

        /// <summary>
        /// Wrapper for FileStream.SetLength().
        /// </summary>
        /// <remarks>
        /// When lengthening a file, this method appends null characters to it which 
        /// does NOT leave it in an XML-parseable state. After all your transpositions, 
        /// ALWAYS come back and truncate the file unless you've overwritten the 
        /// appended space with valid characters.
        /// </remarks>
        /// <param name="filename">Name of file to resize</param>
        /// <param name="len">New size of file</param>
        public static void SetFileLen(string filename, long len)
        {
            using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                fsw.SetLength(len);
                fsw.Close();
            }
        }

        /// <summary>
        /// Overwrites <paramref name="length"/> bytes in <paramref name="filename"/> 
        /// with spaces, beginning at <paramref name="start"/>.
        /// </summary>
        /// <param name="filename">The target file</param>
        /// <param name="start">The position at which to begin writing spaces</param>
        /// <param name="length">How many spaces to write</param>
        public static void WriteSpaces(string filename, int start, int length)
        {
            using (FileStream fs = new FileStream
                (filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Seek(start, SeekOrigin.Begin);
                for (int i = 0; i < length; i++)
                {
                    bw.Write(" ");
                }
                bw.Flush(); bw.Close(); fs.Close();
            }
        }

        /// <summary>
        /// Grab the desired number of bytes from the beginning of a file;
        /// useful, e.g. for files too large to open in Notepad.
        /// </summary>
        /// <param name="filename">Target file</param>
        /// <param name="lines">Number of lines to grab</param>
        /// <returns>First n lines from the file</returns>
        public static string Head(string filename, int bytes)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                char[] buffer = (char[])Array.CreateInstance(typeof(char), bytes); 
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fs))
                {
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    sr.ReadBlock(buffer, 0, buffer.Length);
                    return new string(buffer);
                }
            }
        }

        /// <summary>
        /// Grab the desired number of kilobytes from the end of a file;
        /// useful, e.g. for files too large to open in Notepad.
        /// </summary>
        /// <param name="filename">Target file</param>
        /// <param name="kb">Number of kilobytes to grab</param>
        /// <returns>Last kb bytes from the file</returns>
        public static string Tail(string filename, int bytes)
        {
            using (FileStream fs = new FileStream(filename,FileMode.Open,FileAccess.Read))
            {
                char[] buffer = (char[])Array.CreateInstance(typeof(char), bytes);
                string txt;
                using (StreamReader sr = new StreamReader(fs))
                {
                    sr.BaseStream.Seek((-1024 * bytes), SeekOrigin.End);
                    sr.ReadBlock(buffer, 0, buffer.Length);
                    txt = new string(buffer);
                    sr.Close(); fs.Close(); 
                }
                return txt;
            }
        }

        /// <summary>
        /// Returns the position of the first occurrence of <paramref name="FindWhat"/> 
        /// within <paramref name="InStream"/>, or -1 if <paramref name="FindWhat"/> is
        /// not found.
        /// </summary>
        /// <param name="FindWhat">The string being sought</param>
        /// <param name="InStream">The stream in which to search (must be readable & seekable)</param>
        /// <returns>The position of the first occurrence of <paramref name="FindWhat"/> 
        /// within <paramref name="InStream"/>, or -1 if <paramref name="FindWhat"/> is
        /// not found
        /// </returns>
        public static int Find(string FindWhat, Stream InStream)
        {
            // TODO: Investigate performance optimizations using a smart string-search
            // algorithm, like Boyer-Moore, Knuth-Morris-Pratt, etc. Automate choice of
            // brute force vs. smart algorithm; Optionally, run performance tests & save 
            // results to a configuration file indicating, e.g., where the tradeoff between
            // algorithms would be for a given length of FindWhat & file size.
            int streamPos = 0;
            int findPos;
            bool found = true;
            char findChar;
            StreamReader sr = new StreamReader(InStream);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            // Outer loop for entire file stream reader...
            while (sr.Peek() >= 0)
            {
                findPos = 0;
                findChar = Convert.ToChar(FindWhat.Substring(findPos, 1));
                found = findChar == (char)sr.Read();
                // Per MSDN:
                // "StreamReader might buffer input such that the position of the
                //  underlying stream will not match the StreamReader position."
                //  Since sr.BaseStream.Position is not an accurate indicator
                //  for determining streamPos, we'll track it ourselves...
                streamPos += 1;
                findPos += 1;
                // Inner loop for comparing findwhat to candidate 
                //  when we hit a potential match...
                while (found)
                {
                    while (findPos <= FindWhat.Length)
                    {
                        findChar = Convert.ToChar(FindWhat.Substring(findPos, 1));
                        found = findChar == (char)sr.Read();
                        if (!found)
                            break;
                        streamPos += 1;
                        findPos += 1;
                        if (findPos == FindWhat.Length) return streamPos - findPos;
                    }
                }
            }
            // No luck finding it?
            return -1;
        }

        /// <summary>
        /// Experimental; researching various approaches to quickly validating
        /// very large XML files, avoiding XmlDocument and XmlSchema instances
        /// </summary>
        /// <param name="filename">The XML file to be validated</param>
        /// <returns>True if valid, False if not. Capiche?</returns>
        public static bool IsValidXmlFile(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationEventHandler += new ValidationEventHandler(_validationHandler);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema;
                XmlReader xr = XmlReader.Create(stream, settings);
                while (xr.Read())
                { }//do nothing, just read; if there's a validation error, it'll hit the callback.
                xr.Close(); stream.Close();
                return _validationErrorsCount == 0;
            }
        }

        private static int _validationErrorsCount;// = 0;
        private static void _validationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity != XmlSeverityType.Warning &&
                args.Exception.Message.IndexOf
                    ("An element or attribute information item has already been validated from the '' namespace")
                    < 0)
            {
                Console.WriteLine(args.Exception.ToString());
                _validationErrorsCount++;
            }
        }
    }
}
