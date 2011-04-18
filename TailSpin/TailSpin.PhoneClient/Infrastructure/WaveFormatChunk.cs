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
    public class WaveFormatChunk
    {
        /// <summary>    /// Initializes a format chunk with the following properties:    
        /// Sample rate: 44100 Hz    
        /// Channels: Stereo    
        /// Bit depth: 16-bit    
        /// </summary>    
        public WaveFormatChunk()
        {
            this.ChunkId = "fmt ";
            this.ChunkSize = 16;
            this.FormatTag = 1;
            this.Channels = 2;
            this.SamplesPerSec = 44100;
            this.BitsPerSample = 16;
            this.BlockAlign = (ushort)(this.Channels * (this.BitsPerSample / 8));
            this.AvgBytesPerSec = this.SamplesPerSec * this.BlockAlign;
        }

        public string ChunkId { get; set; }         // Four bytes: "fmt "    
        public uint ChunkSize { get; set; }       // Length of header in bytes    
        public ushort FormatTag { get; set; }       // 1 (MS PCM)    
        public ushort Channels { get; set; }        // Number of channels    
        public uint SamplesPerSec { get; set; }    // Frequency of the audio in Hz... 44100    
        public uint AvgBytesPerSec { get; set; }   // for estimating RAM allocation    
        public ushort BlockAlign { get; set; }      // sample frame size, in bytes    
        public ushort BitsPerSample { get; set; }    // bits per sample     
    }
}