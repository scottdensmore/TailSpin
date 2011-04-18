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
    public class WaveHeader
    {
        public readonly string GroupId; // RIFF    
        public readonly uint FileLength; // total file length minus 8, which is taken up by RIFF    
        public readonly string RiffType; // always WAVE     

        /// <summary>    
        /// Initializes a WaveHeader object with the default values.    
        /// </summary>    
        public WaveHeader()
        {
            this.FileLength = 0;
            this.GroupId = "RIFF";
            this.RiffType = "WAVE";
        }
    }
}