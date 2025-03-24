using System;
using System.Collections.Generic;

namespace PixelPuzzle
{
    [Serializable]
    public class SaveData
    {
        public string AppVersion;
        public List<int> SolvedImagesIDs;

        public SaveData()
        {
            SolvedImagesIDs = new List<int>();
        }
    }
}
