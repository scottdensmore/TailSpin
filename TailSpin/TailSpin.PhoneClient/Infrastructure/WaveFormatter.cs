//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Infrastructure
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;

    public class WaveFormatter : IDisposable
    {
        private Stream outputStream;
        private BinaryWriter outputWriter;
        private IsolatedStorageFile fileSystem;

        public WaveFormatter(string outFile, uint sampleRate, ushort bitsPerSample, ushort channels)
        {
            this.OpenStreams(outFile);
            this.WriteHeaderAndFormat(sampleRate, bitsPerSample, channels);
        }

        public void WriteDataChunk(byte[] data)
        {
            var bufferSize = (uint)data.Length;
            this.outputWriter.Write("data".ToCharArray());
            this.outputWriter.Write(bufferSize);
            this.outputWriter.Write(data);
        }

        public void Dispose()
        {
            if (this.outputStream != null)
            {
                this.Close();
            }
        }

        private void Close()
        {
            // write out the final length to the header.
            this.outputWriter.Seek(4, SeekOrigin.Begin);
            var filesize = (uint)this.outputWriter.BaseStream.Length;
            this.outputWriter.Write(filesize - 8);
            this.outputWriter.Close();
            this.outputStream.Close();
            this.outputStream.Dispose();
            this.fileSystem.Dispose();
            this.outputWriter = null;
            this.outputStream = null;
            this.fileSystem = null;
        }

        private void OpenStreams(string outFile)
        {
            this.fileSystem = IsolatedStorageFile.GetUserStoreForApplication();

            if (this.fileSystem.FileExists(outFile))
            {
                this.fileSystem.DeleteFile(outFile);
            }

            this.outputStream = this.fileSystem.OpenFile(outFile, FileMode.Create);
            this.outputWriter = new BinaryWriter(this.outputStream);
        }

        private void WriteHeaderAndFormat(uint sampleRate, ushort bitsPerSample, ushort channels)
        {
            var header = new WaveHeader();
            var wavFormat = new WaveFormatChunk
            {
                SamplesPerSec = sampleRate,
                Channels = channels,
                BitsPerSample = bitsPerSample
            };

            wavFormat.BlockAlign = (ushort)(wavFormat.Channels * (wavFormat.BitsPerSample / 8));
            wavFormat.AvgBytesPerSec = wavFormat.SamplesPerSec * wavFormat.BlockAlign;

            // Write the header  
            this.outputWriter.Write(header.GroupId.ToCharArray());
            this.outputWriter.Write(header.FileLength);
            this.outputWriter.Write(header.RiffType.ToCharArray());

            // Write the format chunk  
            this.outputWriter.Write(wavFormat.ChunkId.ToCharArray());
            this.outputWriter.Write(wavFormat.ChunkSize);
            this.outputWriter.Write(wavFormat.FormatTag);
            this.outputWriter.Write(wavFormat.Channels);
            this.outputWriter.Write(wavFormat.SamplesPerSec);
            this.outputWriter.Write(wavFormat.AvgBytesPerSec);
            this.outputWriter.Write(wavFormat.BlockAlign);
            this.outputWriter.Write(wavFormat.BitsPerSample);
        }
    }
}
