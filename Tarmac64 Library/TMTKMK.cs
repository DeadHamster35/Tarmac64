using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cereal64.Common.Utils;
using System.Collections.ObjectModel;

namespace Tarmac64_Library
{
    /// <summary>
    /// Code for encoding/decoding images in the TKMK00 format
    /// </summary>
    public class TKMK00Encoder
    {
        /// <summary>
        /// Huffman trees have a left/right node and a value. In TKMK00, if the left or right node exists, then it's just a traversal
        ///  node, and will not be an output. Traversal nodes will have a value >= 0x20, while output nodes will have a value between
        ///  0x00 - 0x1F (all values for a 5-bit number).
        /// </summary>
        public class TKMK00HuffmanTreeNode
        {
            public TKMK00HuffmanTreeNode Left;
            public TKMK00HuffmanTreeNode Right;
            public int Value;

            public bool IsEndNode
            { get { return Left == null && Right == null; } }
        }

        /// <summary>
        /// The 0x2C size header for the TKMK00 data. Starts with "TKMK00".
        /// </summary>
        public class TKMK00Header
        {
            public byte RepeatModeMask;
            public byte Unknown;
            public ushort Width;
            public ushort Height;
            public int[] ChannelPointers = new int[8];

            public TKMK00Header()
            {
            }

            public TKMK00Header(byte[] headerData)
            {
                RepeatModeMask = ByteHelper.ReadByte(headerData, 0x6);
                Unknown = ByteHelper.ReadByte(headerData, 0x7);
                Width = ByteHelper.ReadUShort(headerData, 0x8);
                Height = ByteHelper.ReadUShort(headerData, 0xA);
                for (int i = 0; i < 8; i++)
                    ChannelPointers[i] = ByteHelper.ReadInt(headerData, 0xC + 4 * i);
            }

            public byte[] GetAsBytes()
            {
                return ByteHelper.CombineIntoBytes((byte)'T', (byte)'K', (byte)'M', (byte)'K', (byte)'0', (byte)'0',
                    RepeatModeMask, Unknown, Width, Height, ChannelPointers);
            }

            /// <summary>
            /// Reads the RepeatModeMask for the specific channel's bit, and returns if it's flipped on or off.
            /// </summary>
            public bool RepeatEnabledFor(int channelNum)
            {
                int maskCompare = 0x1 << channelNum;
                return ((RepeatModeMask & maskCompare) != 0);
            }

            public static int DataSize { get { return 0x2C; } }
        }

        #region Encode

        /// <summary>
        /// The reverse of the TKMK00CommandReader, it takes in a series of bits and, when finished, formats the data into two forms,
        ///  one for each of the different modes (repeating and non-repeating). The user may decide which of the two forms to use in the end.
        /// </summary>
        public class TKMK00CommandWriter
        {
            private List<byte> _data; //Data being written to
            private int _remainingBits; //Remaining bits in the current byte (last byte in the data list)

            private List<byte> _finalData; //Final data for the non-repeating mode
            private List<byte> _finalRepeatingData; //Final data for the repeating mode

            private bool _finished; //True when Finished() is called, signifies if _finalData and _finalRepeatingData have data in them.

            public TKMK00CommandWriter()
            {
                _data = new List<byte>();
                _data.Add(0);
                _remainingBits = 8;

                _finished = false;
            }

            /// <summary>
            /// Write X bits from command to the byte list, which will add bytes as necessary to continue adding.
            /// </summary>
            public void Write(int command, int bitCount)
            {
                if (_finished) //Don't allow more writing after finishing
                    return;

                int commandFlag = 0x1 << (bitCount - 1); //Command flag points to the current bit in command
                for (int i = 0; i < bitCount; i++)
                {
                    _data[_data.Count - 1] <<= 1; //Add the current bit to the data's current byte
                    _data[_data.Count - 1] |= (byte)((commandFlag & command) == 0 ? 0 : 1);
                    commandFlag >>= 1;

                    _remainingBits--;

                    if (_remainingBits == 0) //Time to add another byte
                    {
                        _data.Add(0);
                        _remainingBits = 8;
                    }
                }
            }

            //When finished, returns the bytes saved by using the repeating mode rather than the non-repeating mode
            public int BytesSavedInRepeat { get { if (!_finished) return 0; return _finalData.Count - _finalRepeatingData.Count; } }

            //Public accessors
            public ReadOnlyCollection<byte> FinalData { get { return _finalData.AsReadOnly(); } }
            public ReadOnlyCollection<byte> FinalRepeatingData { get { return _finalRepeatingData.AsReadOnly(); } }

            /// <summary>
            /// Called to close the TKMK00CommandWriter to further writing, and to add the current data to
            /// _finalData and _finalRepeatingData.
            /// </summary>
            public void Finish()
            {
                _finished = true; //Regardless of error or not, close it to further writing

                //Bump up the final bits
                _data[_data.Count - 1] <<= _remainingBits;

                //Non-repeating just needs to be using 4-byte blocks
                _finalData = new List<byte>(_data);
                int remainingBytes = 4 - _finalData.Count % 4;
                for (int i = 0; i < remainingBytes; i++)
                    _finalData.Add(0);

                //Now we need to determine where we can repeat bytes in the data
                List<Tuple<int, int>> repeatZones = new List<Tuple<int, int>>(); //Item1 = offset of byte to start repeat, Item2 = length of repeat bytes (must be >= 3)

                bool inRepeatZone = false;
                int startRepeatOffset = 0;

                //Go through each byte. When a byte and its next byte are identical, flip the inRepeatZone flag. If they are not identical
                // and the inRepeatZone flag is flipped, we've hit the end of a repeat zone. If the # of bytes repeating is 3 or more,
                // save the zone start/length to repeatZones.
                for (int i = 0; i < _data.Count - 1; i++)
                {
                    if (!inRepeatZone)
                    {
                        if (_data[i] == _data[i + 1])
                        {
                            inRepeatZone = true;
                            startRepeatOffset = i;
                        }
                    }
                    else
                    {
                        if (_data[i] != _data[i + 1])
                        {
                            int repeatCount = i - startRepeatOffset + 1;
                            if (repeatCount >= 3)
                            {
                                repeatZones.Add(new Tuple<int, int>(startRepeatOffset, repeatCount));
                            }
                            inRepeatZone = false;
                        }
                    }
                }

                //Finish up the end, if we're still in a repeatZone
                if (inRepeatZone)
                {
                    int repeatCount = _data.Count - startRepeatOffset;
                    if (repeatCount >= 3)
                    {
                        repeatZones.Add(new Tuple<int, int>(startRepeatOffset, repeatCount));
                    }
                }

                //Write the data out to _finalRepeatingData
                _finalRepeatingData = new List<byte>();

                if (repeatZones.Count > 0)
                {
                    bool ignoreFlagOn = (repeatZones[0].Item1 != 0); //Ignore flag determines if inside or outside a repeat zone
                    int byteOffset = 0;
                    int repeatZoneIndex = 0;

                    //Tricky algorithm here, but essentially switch between outputting non-repeating data and repeating data. Look
                    // at TKMK00CommandReader for a better description of how repeating data is formatted.
                    while (byteOffset < _data.Count)
                    {
                        //0x80 = ignore on, 0x7F + 1 = count
                        if (ignoreFlagOn)
                        {
                            int endZone = (repeatZoneIndex < repeatZones.Count ? repeatZones[repeatZoneIndex].Item1 : _data.Count);
                            int count = Math.Min(128, (endZone - byteOffset)); //Max of 128 non-repeating bytes before a new command byte
                            if (count < 0) //Shouldn't happen, but this is just in case
                            {
                                repeatZoneIndex++;
                                continue;
                            }
                            byte commandByte = (byte)((count - 1) + 0x80);
                            _finalRepeatingData.Add(commandByte);
                            for (int i = 0; i < count; i++)
                            {
                                _finalRepeatingData.Add(_data[byteOffset]);
                                byteOffset++;
                            }

                            if (endZone == byteOffset)
                            {
                                ignoreFlagOn = false;
                            }
                        }
                        else //0x00 = ignore off, 0x7F + 3 = count
                        {
                            int nextCount = (repeatZones[repeatZoneIndex].Item1 + repeatZones[repeatZoneIndex].Item2 - byteOffset);
                            int count = Math.Min(130, nextCount); //Max of 130 repeating bytes before a new command byte
                            if (nextCount != count && nextCount - count < 3) //Would portion out less than 3 repeating bits the next go
                            {
                                count -= 3;
                            }
                            byte commandByte = (byte)(count - 3);
                            _finalRepeatingData.Add(commandByte);
                            _finalRepeatingData.Add(_data[byteOffset]);
                            byteOffset += count;

                            if (repeatZones[repeatZoneIndex].Item1 + repeatZones[repeatZoneIndex].Item2 == byteOffset)
                            {
                                repeatZoneIndex++;
                                ignoreFlagOn = (repeatZoneIndex >= repeatZones.Count || repeatZones[repeatZoneIndex].Item1 != byteOffset);
                                if (ignoreFlagOn)
                                {
                                    repeatZoneIndex++;
                                    repeatZoneIndex--;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Just write a bunch of non-repeating bytes
                    int byteOffset = 0;
                    while (byteOffset < _data.Count)
                    {
                        int count = Math.Min(128, _data.Count - byteOffset); //Max of 128 non-repeating bytes before a new command byte
                        byte commandByte = (byte)((count - 1) + 0x80);
                        _finalRepeatingData.Add(commandByte);
                        for (int i = 0; i < count; i++)
                        {
                            _finalRepeatingData.Add(_data[byteOffset]);
                            byteOffset++;
                        }
                    }
                }

                remainingBytes = 4 - _finalRepeatingData.Count % 4; //Add bytes to line it up to a 4-byte address
                for (int i = 0; i < remainingBytes; i++)
                    _finalRepeatingData.Add(0);
            }
        }

        /// <summary>
        /// Encodes RGBA5551 formatted data into the TKMK00 format. Uses a Huffman tree to store
        ///  color values that are added/subtracted/sometimes overwrite a predicted color for the
        ///  current pixel. Look up DPCM for a conceptual idea of what it's doing (the Huffman tree
        ///  stands in place for entropy coding.
        /// </summary>
        public byte[] Encode(byte[] imageData, int width, int height, ushort alphaColor)
        {
            //ushort[] colors = new ushort[width * height]; //Color map that gets written to mid-conversion
            ushort[] colorsRef = new ushort[width * height]; //Color map that has all image data from the beginning
            byte[] colorChangeMap = new byte[width * height]; // 
            byte[] predictedColorsGreen = new byte[width * height]; //Contains predicted green values for all pixels
            byte[] predictedColorsRed = new byte[width * height]; //Contains predicted red values for all pixels
            byte[] predictedColorsBlue = new byte[width * height]; //Contains predicted blue values for all pixels
            byte[] nearestIndeticalPixel = new byte[width * height]; //Used for finding pixels to copy the color down to, 0 - none, 1-5 - varying positions down one row
            int[] colorCounts = new int[32]; //Contains # of times each huffman tree value is used. This is for constructing the Huffman tree to be most efficient
            bool[] usedIdenticalColors = new bool[colorsRef.Length]; //True if the current pixel was copied from one a row up

            //First, convert all colors from 2-bytes into 1 ushort value (colorsRef), then calculate the predicted color values
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int currentPixel = row * width + col;
                    colorsRef[currentPixel] = ByteHelper.ReadUShort(imageData, currentPixel * 2);

                    //Predicted colors
                    int actualGreen = ((colorsRef[currentPixel] & 0x7C0) >> 6);
                    int actualRed = ((colorsRef[currentPixel] & 0xF800) >> 11);
                    int actualBlue = ((colorsRef[currentPixel] & 0x3E) >> 1);

                    int rgbaTop = (row == 0 ? 0 : colorsRef[currentPixel - width]);
                    int rgbaLeft = (row == 0 && col == 0 ? 0 : colorsRef[currentPixel - 1]);


                    ushort greenTop = (byte)((rgbaTop & 0x7C0) >> 6);
                    ushort greenLeft = (byte)((rgbaLeft & 0x7C0) >> 6);
                    int greenPrediction = (greenTop + greenLeft) / 2;

                    //Use the change between the old & new green values to project expected
                    // values for the red & blue colors
                    predictedColorsGreen[currentPixel] = (byte)ReverseColorCombine(greenPrediction, actualGreen);

                    int greenChange = actualGreen - greenPrediction;

                    //Combine red values of the pixels to make our predicted pixel color
                    ushort redTop = (byte)((rgbaTop & 0xF800) >> 11);
                    ushort redLeft = (byte)((rgbaLeft & 0xF800) >> 11);
                    int redPrediction = greenChange + (redTop + redLeft) / 2;
                    redPrediction = Math.Max(0, Math.Min(0x1F, redPrediction)); //Keep between 0 and 0x1F

                    predictedColorsRed[currentPixel] = (byte)ReverseColorCombine(redPrediction, actualRed);

                    //Combine blue values of the pixels to make our predicted pixel color
                    ushort blueTop = (byte)((rgbaTop & 0x3E) >> 1);
                    ushort blueLeft = (byte)((rgbaLeft & 0x3E) >> 1);
                    int bluePrediction = greenChange + (blueTop + blueLeft) / 2;
                    bluePrediction = Math.Max(0, Math.Min(0x1F, bluePrediction)); //Keep between 0 and 0x1F

                    predictedColorsBlue[currentPixel] = (byte)ReverseColorCombine(bluePrediction, actualBlue);
                }
            }

            //Set up an rgbaBuffer to hold the last 0x40 color values used
            ushort[] rgbaBuffer = new ushort[0x40];
            for (int i = 0; i < 0x40; i++)
                rgbaBuffer[i] = 0xFF;

            ushort lastColor = 0; //Last color used

            //Set up the master writer & 8 channel command writers
            TKMK00CommandWriter masterWriter = new TKMK00CommandWriter();
            TKMK00CommandWriter[] channelWriters = new TKMK00CommandWriter[8];
            for (int i = 0; i < 8; i++)
                channelWriters[i] = new TKMK00CommandWriter();

            List<byte> huffmanValues = new List<byte>(); //Contains the huffman commands that will be written to channelWriters[0], but only
                                                         // after the Huffman tree is constructed.

            //Main loop for determining the TKMK00 commands
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int currentPixel = row * width + col;

                    //If this pixel has been colored already, then no commands are necessary
                    if (usedIdenticalColors[currentPixel])
                    {
                        lastColor = colorsRef[currentPixel];
                        continue;
                    }

                    //Channel index is determined from the colorChangeMap
                    int channelIndex = colorChangeMap[currentPixel] + 1;

                    if (colorsRef[currentPixel] == lastColor)
                    {
                        channelWriters[channelIndex].Write(0, 1); //Write the 'last color' command to the channel writer
                    }
                    else
                    {
                        channelWriters[channelIndex].Write(1, 1); //Write the 'new color' command to the channel writer

                        if (rgbaBuffer.Contains(colorsRef[currentPixel]))
                        {
                            masterWriter.Write(0, 1); //Write the 'use existing color' command to the master writer
                            int index = 0;
                            for (; index < rgbaBuffer.Length; index++)
                            {
                                if (rgbaBuffer[index] == colorsRef[currentPixel]) break;
                            }

                            masterWriter.Write(index, 6); //Write the index of the existing color to the master writer

                            //Move the selected color to the front of the rgba buffer
                            for (int k = index; k > 0; k--)
                            {
                                rgbaBuffer[k] = rgbaBuffer[k - 1];
                            }
                            rgbaBuffer[0] = colorsRef[currentPixel];
                        }
                        else
                        {
                            masterWriter.Write(1, 1); //Write the 'new color' command to the master writer

                            //3 color commands
                            huffmanValues.Add(predictedColorsGreen[currentPixel]);
                            huffmanValues.Add(predictedColorsRed[currentPixel]);
                            huffmanValues.Add(predictedColorsBlue[currentPixel]);

                            //Update the color counts
                            colorCounts[predictedColorsGreen[currentPixel]]++;
                            colorCounts[predictedColorsRed[currentPixel]]++;
                            colorCounts[predictedColorsBlue[currentPixel]]++;

                            //Add the new color to the front of the buffer
                            for (int k = rgbaBuffer.Length - 1; k > 0; k--)
                            {
                                rgbaBuffer[k] = rgbaBuffer[k - 1];
                            }
                            rgbaBuffer[0] = colorsRef[currentPixel];
                        }

                        //Add nearby pixels to the colorChangeMap
                        bool hasLeftCol = (col != 0);
                        bool hasRightCol = (col < (width - 1));
                        bool has2RightCols = (col < (width - 2));
                        bool hasDownRow = (row < (height - 1));
                        bool has2DownRows = (row < (height - 2));

                        //Right 1
                        if (hasRightCol)
                            colorChangeMap[currentPixel + 1]++;
                        //Right 2
                        if (has2RightCols)
                            colorChangeMap[currentPixel + 2]++;
                        //Down 1 Left 1
                        if (hasDownRow && hasLeftCol)
                            colorChangeMap[currentPixel + width - 1]++;
                        //Down 1
                        if (hasDownRow)
                            colorChangeMap[currentPixel + width]++;
                        //Down 1 Right 1
                        if (hasDownRow && hasRightCol)
                            colorChangeMap[currentPixel + width + 1]++;
                        //Down 2
                        if (has2DownRows)
                            colorChangeMap[currentPixel + width * 2]++;

                        lastColor = colorsRef[currentPixel];

                        //Determine if the current pixel has a pixel beneath it that it can be copied to
                        nearestIndeticalPixel[currentPixel] = setNearestPixel(currentPixel, width, colorsRef, usedIdenticalColors);

                        if (nearestIndeticalPixel[currentPixel] == 0)
                        {
                            masterWriter.Write(0, 1); //Write the 'don't copy' command to the master writer
                        }
                        else
                        {
                            //There's still a chance that we won't actually copy the pixel down, so don't write the 'copy' command yet
                            bool hasntWrittenFirstCommand = true;

                            int pixelOffset = currentPixel; //Contains the new pixel location to copy into

                            //Loop here, to continue copying down until it hits the end of the chain.
                            while (nearestIndeticalPixel[pixelOffset] != 0)
                            {
                                int pixelValue = nearestIndeticalPixel[pixelOffset];
                                switch (pixelValue)
                                {
                                    case 1: //back 2
                                        pixelOffset -= 2;
                                        break;
                                    case 2: //back 1
                                        pixelOffset--;
                                        break;
                                    case 3://stay
                                        break;
                                    case 4://forward 1
                                        pixelOffset++;
                                        break;
                                    case 5://forward 2
                                        pixelOffset += 2;
                                        break;
                                }

                                pixelOffset += width; //Down one row

                                if (usedIdenticalColors[pixelOffset])
                                    break;//If the pixel in questinon has already been copied to, quit out of the loop
                                else
                                {
                                    usedIdenticalColors[pixelOffset] = true; //Mark this pixel as having been copied to

                                    if (hasntWrittenFirstCommand)
                                    {
                                        masterWriter.Write(1, 1); //Write the 'copy' command to the master writer
                                        hasntWrittenFirstCommand = false;
                                    }

                                    //Write the pixel offset commands to the master writer (see the decoding process for the
                                    // logic behind these numbers)
                                    switch (pixelValue)
                                    {
                                        case 1: //back 2
                                            masterWriter.Write(2, 4); //0010
                                            break;
                                        case 2: //back 1
                                            masterWriter.Write(1, 2); //01
                                            break;
                                        case 3://stay
                                            masterWriter.Write(2, 2); //10
                                            break;
                                        case 4://forward 1
                                            masterWriter.Write(3, 2); //11
                                            break;
                                        case 5://forward 2
                                            masterWriter.Write(3, 4); //0011
                                            break;
                                    }
                                }

                                nearestIndeticalPixel[pixelOffset] = setNearestPixel(pixelOffset, width, colorsRef, usedIdenticalColors);
                            }

                            if (hasntWrittenFirstCommand)
                            {
                                masterWriter.Write(0, 1); //Write the 'don't copy' command to the master writer
                            }
                            else
                            {
                                //Write the quit command to the master writer (see the decoding process for the
                                // logic behind this number)
                                masterWriter.Write(0, 3); //000
                            }
                        }
                    }
                }
            }

            //Post pixel loop, now we need to finalize and save all the data

            //Load the Huffman tree
            TKMK00HuffmanTreeNode headNode = ConstructTree(colorCounts);

            int[] huffmanTreeTraversalCommands = new int[0x20];
            int[] huffmanTreeTraversalBitCounts = new int[0x20];

            //Get the bits for each huffman value
            GetHuffmanTreeTraversalCommands(headNode, 0, 0, huffmanTreeTraversalCommands, huffmanTreeTraversalBitCounts);

            //Write in the Huffman tree here
            WriteHuffmanTree(headNode, channelWriters[0]);

            //Write the convertedHuffmanValues
            foreach (byte index in huffmanValues)
                channelWriters[0].Write(huffmanTreeTraversalCommands[index], huffmanTreeTraversalBitCounts[index]);

            //Here the writers should be good, finish and combine into a single byte[]
            masterWriter.Finish();
            foreach (TKMK00CommandWriter writer in channelWriters)
                writer.Finish();

            //Set up the header here
            TKMK00Header header = new TKMK00Header();
            int channelOffset = 0x2C + masterWriter.FinalData.Count;
            List<byte[]> channelDatas = new List<byte[]>();
            byte repeatEnabledMask = 0;
            for (int i = 0; i < 8; i++)
            {
                header.ChannelPointers[i] = channelOffset;
                bool useRepeat = channelWriters[i].BytesSavedInRepeat > 0;
                if (useRepeat)
                {
                    channelOffset += channelWriters[i].FinalRepeatingData.Count;
                    channelDatas.Add(channelWriters[i].FinalRepeatingData.ToArray());
                    repeatEnabledMask |= (byte)(1 << i);
                }
                else
                {
                    channelOffset += channelWriters[i].FinalData.Count;
                    channelDatas.Add(channelWriters[i].FinalData.ToArray());
                }
            }
            header.Width = (ushort)width;
            header.Height = (ushort)height;
            header.RepeatModeMask = repeatEnabledMask;
            header.Unknown = 0x0F;

            //Combine all the data together
            byte[] finalData = ByteHelper.CombineIntoBytes(header.GetAsBytes(), masterWriter.FinalData,
                channelDatas[0], channelDatas[1], channelDatas[2], channelDatas[3],
                channelDatas[4], channelDatas[5], channelDatas[6], channelDatas[7]);

            return finalData;

        }

        /// <summary>
        /// Small function that takes a Huffman tree & a TKMK00CommandWriter and writes out the tree construction information to the writer.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="writer"></param>
        private static void WriteHuffmanTree(TKMK00HuffmanTreeNode node, TKMK00CommandWriter writer)
        {
            if (!node.IsEndNode)
            {
                writer.Write(1, 1);
                WriteHuffmanTree(node.Left, writer);
                WriteHuffmanTree(node.Right, writer);
            }
            else
            {
                writer.Write(0, 1);
                writer.Write(node.Value, 5);
            }
        }

        /// <summary>
        /// This will check for identical pixel colors below the current pixel (the five closest pixels that are 1 row down from the
        ///  current pixel) in an image that occur after a differently colored pixel, and allow the program to 
        ///  return the identical pixel location. Previously selected locations are kept track of through usedIdenticalColors.
        ///  This is used during encoding to keep track of where pixels can be copied down rows.
        /// </summary>
        private static byte setNearestPixel(int currentPixel, int width, ushort[] colors, bool[] usedIdenticalColors)
        {
            //For output: 0 - no nearest pixel, 1-5 - pixels 1 through 5 beneath the current pixel

            //     - - - - P - - - -  [P = current pixel]
            //     - - 1 2 3 4 5 - -  [1-5 = pixels below current pixel]

            //Down 1, back 2
            int checkingPixel = currentPixel + width - 2;
            if (checkingPixel >= colors.Length)
                return 0;
            if (colors[currentPixel] == colors[checkingPixel] && colors[currentPixel] != colors[checkingPixel - 1] &&
                !usedIdenticalColors[currentPixel + width - 2])
            {
                return 1;
            }

            //Down 1, back 1
            checkingPixel++;
            if (checkingPixel >= colors.Length)
                return 0;
            if (colors[currentPixel] == colors[checkingPixel] && colors[currentPixel] != colors[checkingPixel - 1] &&
                !usedIdenticalColors[currentPixel + width - 1])
            {
                return 2;
            }

            //Down 1
            checkingPixel++;
            if (checkingPixel >= colors.Length)
                return 0;
            if (colors[currentPixel] == colors[checkingPixel] && colors[currentPixel] != colors[checkingPixel - 1] &&
                !usedIdenticalColors[currentPixel + width])
            {
                return 3;
            }

            //Down 1, forward 1
            checkingPixel++;
            if (checkingPixel >= colors.Length)
                return 0;
            if (colors[currentPixel] == colors[checkingPixel] && colors[currentPixel] != colors[checkingPixel - 1] &&
                !usedIdenticalColors[currentPixel + width + 1])
            {
                return 4;
            }

            //Down 1, forward 2
            checkingPixel++;
            if (checkingPixel >= colors.Length)
                return 0;
            if (colors[currentPixel] == colors[checkingPixel] && colors[currentPixel] != colors[checkingPixel - 1] &&
                !usedIdenticalColors[currentPixel + width + 2])
            {
                return 5;
            }

            return 0;
        }

        /// <summary>
        /// Returns the differential code used to turn the predicted color to the actual color
        /// </summary>
        private static int ReverseColorCombine(int predictedColor, int actualColor)
        {
            //Detect if it's trying to be too big or too small
            int spaceFromEdge = predictedColor;
            if (predictedColor >= 0x10)
                spaceFromEdge = 0x1F - spaceFromEdge;

            int diff = actualColor - predictedColor;
            if (spaceFromEdge < Math.Abs(diff))
            {
                if (predictedColor >= 0x10)
                    return 0x1F - actualColor;

                return actualColor;
            }

            if (diff < 0)
            {
                return Math.Abs(diff) << 1;
            }
            if (diff > 0)
            {
                return ((Math.Abs(diff - 1)) << 1) | 0x1;
            }

            //diff == 0, return 0
            return 0;
        }

        /// <summary>
        /// Returns the differential code used to turn the predicted color to the actual color
        /// </summary>
        private static TKMK00HuffmanTreeNode ConstructTree(int[] colorCounts)
        {
            //Link to Huffman balancing document: https://www.siggraph.org/education/materials/HyperGraph/video/mpeg/mpegfaq/huffman_tutorial.html

            List<Tuple<int, TKMK00HuffmanTreeNode>> frequenciesAndValues = new List<Tuple<int, TKMK00HuffmanTreeNode>>();

            for (int i = 0; i < colorCounts.Length; i++)
            {
                TKMK00HuffmanTreeNode node = new TKMK00HuffmanTreeNode();
                node.Value = i;
                frequenciesAndValues.Add(new Tuple<int, TKMK00HuffmanTreeNode>(colorCounts[i], node));
            }

            while (frequenciesAndValues.Count > 1)
            {
                frequenciesAndValues.Sort((s1, s2) => s2.Item1.CompareTo(s1.Item1));

                //Merge two into a new Huffman tree (NOTE: LEFT IS LESS THAN RIGHT)
                TKMK00HuffmanTreeNode node = new TKMK00HuffmanTreeNode();
                node.Left = frequenciesAndValues[frequenciesAndValues.Count - 1].Item2;
                node.Right = frequenciesAndValues[frequenciesAndValues.Count - 2].Item2;
                node.Value = 64; //Invalid value to denote a traversal node rather than an end node

                int newCount = frequenciesAndValues[frequenciesAndValues.Count - 1].Item1 + frequenciesAndValues[frequenciesAndValues.Count - 2].Item1;
                frequenciesAndValues.RemoveAt(frequenciesAndValues.Count - 1);
                frequenciesAndValues.RemoveAt(frequenciesAndValues.Count - 1);

                frequenciesAndValues.Add(new Tuple<int, TKMK00HuffmanTreeNode>(newCount, node));
            }

            return frequenciesAndValues[0].Item2;
        }

        /// <summary>
        /// Uses the existing Huffman tree and the command that is being looked for, and saves the combination of bits used to
        ///  reach that command in the Huffman tree into outCommands (the value) and outBits (# of bits used)
        /// </summary>
        private static void GetHuffmanTreeTraversalCommands(TKMK00HuffmanTreeNode node, int commandTotal, int bitCount,
            int[] outCommands, int[] outBits)
        {
            if (node.IsEndNode)
            {
                outCommands[node.Value] = commandTotal;
                outBits[node.Value] = bitCount;
            }
            else
            {
                bitCount++;
                commandTotal <<= 1;
                GetHuffmanTreeTraversalCommands(node.Left, commandTotal, bitCount, outCommands, outBits);
                commandTotal++;
                GetHuffmanTreeTraversalCommands(node.Right, commandTotal, bitCount, outCommands, outBits);
            }
        }

        #endregion

        #region Decode

        /// <summary>
        /// This class is combined from two different bit readers specified in the decoding assembly. The first form of it
        ///  (no repeat mode) reads in 4 bytes at a time, and reads 1 bit at a time. It can also return multi-bit sized
        ///  outputs (the decoding reads up to 6 at a time).
        ///  
        /// The second form is repeat mode. In this form it loads 1 byte at a time and always returns 1 bit, but there's 
        ///  an extra ignoreRepeat flag. If the flag is off (repeat mode is on), then when it hits the end of the byte it 
        ///  re-uses the current byte. When RemainingBits reaches zero, it reads in a byte that tells it if the IgnoreRepeat
        ///  flag is on or off, and then the number of RemainingBits (will almost always be bigger than the size of a byte).
        /// </summary>
        public class TKMK00CommandReader
        {
            private byte[] _data; //Rom data, TKMK00 data, whatever you're reading from
            private int _dataPointer; //Reading offset in the data
            private int _buffer; //Last byte data loaded from _data
            private int _remainingBits; //Remaining bits before loading the next byte[s]
            private bool _repeatEnabled; //Repeat mode
            private bool _ignoreRepeatFlag; //Flag used in repeat mode

            public TKMK00CommandReader(byte[] data, int dataPointer, bool repeatEnabled)
            {
                _data = data;
                _buffer = 0;
                _dataPointer = dataPointer;
                _remainingBits = 0;
                _repeatEnabled = repeatEnabled;
                _ignoreRepeatFlag = false;

                //Here, we set back the pointer 1 load's worth so when it reads the first value it moves up and reads the correct location
                if (!repeatEnabled)
                    _dataPointer -= 4;
                else
                    _dataPointer--;
            }

            public int ReadBits()
            {
                return ReadBits(1);
            }

            public int ReadBits(int bitCount) //Bitcount only counts for non-repeating readers
            {
                int commandValue = 0;//clear the command value

                if (!_repeatEnabled) //Not repeat mode
                {
                    while (bitCount > 0) //Read out the # of bits in the parameter
                    {
                        if (_remainingBits == 0) //If the end of the current buffer has been reached
                        {
                            //Load the next 4 bytes and progress the pointer/reset the remaining bits
                            _dataPointer += 4;
                            _buffer = ByteHelper.ReadInt(_data, _dataPointer);
                            _remainingBits = 0x20;
                        }

                        //Append the next bit in the buffer to the end of the commandValue
                        commandValue = ((_buffer >> (_remainingBits - 1)) & 0x1) | (commandValue << 1);
                        _remainingBits--;

                        bitCount--;
                    }
                }
                else //Repeat mode
                {
                    if (_remainingBits == 0) //out of data
                    {
                        _dataPointer++;
                        byte repeatInfo = ByteHelper.ReadByte(_data, _dataPointer);

                        _ignoreRepeatFlag = ((repeatInfo & 0x80) != 0); //The new ignoreRepeatFlag is stored in the top bit

                        if (_ignoreRepeatFlag)
                        {
                            _remainingBits = (ushort)(((repeatInfo & 0x7F) + 1) * 8); //Read new byte (repeatInfo + 1) times
                        }
                        else
                        {
                            _remainingBits = (ushort)((repeatInfo + 3) * 8); //Repeat byte (repeatInfo + 3) times
                        }

                        _dataPointer++;
                        _buffer = ByteHelper.ReadByte(_data, _dataPointer); //Read in next byte
                    }

                    int byteWrappedRemainingBits = (_remainingBits & 0x7);
                    if (byteWrappedRemainingBits == 0)
                        byteWrappedRemainingBits = 8; //Remaining bits on CURRENT byte

                    //Set the commandValue to the bit in the buffer
                    commandValue = ((_buffer >> (byteWrappedRemainingBits - 1)) & 0x1);
                    _remainingBits--;

                    byteWrappedRemainingBits--;

                    //Check if we've reached the end of the bit but not the remaining bits
                    if (byteWrappedRemainingBits == 0 && _remainingBits > 0)
                    {
                        if (_ignoreRepeatFlag) //Load in next byte if ignoreRepeatMode
                        {
                            _dataPointer++;
                            _buffer = ByteHelper.ReadByte(_data, _dataPointer);
                        }
                    }
                }
                return commandValue;
            }
        }

        /// <summary>
        /// Decodes the TKMK00 format into RGBA5551 formatted data. Uses a Huffman tree to store
        ///  color values that are added/subtracted/sometimes overwrite a predicted color for the
        ///  current pixel. Look up DPCM for a conceptual idea of what it's doing (the Huffman tree
        ///  stands in place for entropy coding.
        /// </summary>
        public byte[] Decode(byte[] data, int tkmk00Offset, ushort alphaColor)
        {
            //Initialize the header & readers
            byte[] headerBytes = new byte[TKMK00Header.DataSize];
            Array.Copy(data, tkmk00Offset, headerBytes, 0, TKMK00Header.DataSize);
            TKMK00Header header = new TKMK00Header(headerBytes);
            TKMK00CommandReader masterReader = new TKMK00CommandReader(data, tkmk00Offset + TKMK00Header.DataSize, false);
            TKMK00CommandReader[] channelReaders = new TKMK00CommandReader[8];
            for (int i = 0; i < 8; i++)
                channelReaders[i] = new TKMK00CommandReader(data, tkmk00Offset + header.ChannelPointers[i], header.RepeatEnabledFor(i));

            //Set up the image data/buffers
            ushort[] rgbaBuffer = new ushort[0x40];
            for (int i = 0; i < 0x40; i++)
                rgbaBuffer[i] = 0xFF;
            byte[] colorChangeMap = new byte[header.Width * header.Height];
            byte[] imageData = new byte[header.Width * header.Height * 2];
            int pixelIndex = 0;

            //Set up the Huffman binary tree
            TKMK00HuffmanTreeNode headTreeNode = SetUpHuffmanTree(0x20, data, channelReaders[0]);

            ushort lastPixelColor = 0;

            //Iterate through each pixel in order left to right, top to bottom
            for (int row = 0; row < header.Height; row++)
            {
                for (int col = 0; col < header.Width; col++)
                {
                    //Look at the current pixel's color. If it's not empty, then it's already been
                    // set to its correct value, and we can skip to the next pixel
                    ushort currentPixelColor = ByteHelper.ReadUShort(imageData, pixelIndex * 2);

                    if (currentPixelColor != 0) //Color already exists
                    {
                        lastPixelColor = currentPixelColor;

                        //Test to make sure that the curent color is not the alpha value with the incorrect alpha channel value
                        ushort currentPixelWithoutAlpha = (ushort)(currentPixelColor & 0xFFFE);
                        if (currentPixelWithoutAlpha == alphaColor)
                        {
                            ByteHelper.WriteUShort(alphaColor, imageData, pixelIndex * 2);
                            lastPixelColor = alphaColor;
                        }

                        //Done, go to end of the loop
                    }
                    else
                    {
                        //Load up the channel reader that is associated with the given color change value in the
                        // colorChangeMap (low values = not much change around that pixel, high values = lots of change)
                        byte channelIndex = (byte)(colorChangeMap[pixelIndex] + 1);

                        int command = channelReaders[channelIndex].ReadBits(1); // 0 - Use the previous color, 1 - Use new color

                        if (command == 0)
                        {
                            ByteHelper.WriteUShort(lastPixelColor, imageData, pixelIndex * 2);
                            //End of this line
                        }
                        else
                        {
                            command = masterReader.ReadBits(1); // 1 - Create new RGBA, 0 - Use existing RGBA
                            if (command != 0)
                            {
                                //Load in the huffman values for the new green, red and blue. These are combined
                                // with a predicted pixel value for them later on.
                                int newGreen = RetrieveHuffmanTreeValue(data, headTreeNode, channelReaders[0]);
                                int newRed = RetrieveHuffmanTreeValue(data, headTreeNode, channelReaders[0]);
                                int newBlue = RetrieveHuffmanTreeValue(data, headTreeNode, channelReaders[0]);

                                //Retreive the pixel colors from the pixel above and the pixel to the left
                                ushort rgbaTop, rgbaLeft;

                                if (row != 0)
                                {
                                    rgbaTop = ByteHelper.ReadUShort(imageData, (pixelIndex - header.Width) * 2);
                                    rgbaLeft = ByteHelper.ReadUShort(imageData, (pixelIndex - 1) * 2);
                                }
                                else
                                {
                                    rgbaTop = 0;
                                    if (col != 0)
                                        rgbaLeft = ByteHelper.ReadUShort(imageData, (pixelIndex - 1) * 2);
                                    else
                                        rgbaLeft = 0;
                                }

                                //Combine green values of the pixels to make our predicted pixel color
                                ushort greenTop = (byte)((rgbaTop & 0x7C0) >> 6);
                                ushort greenLeft = (byte)((rgbaLeft & 0x7C0) >> 6);
                                int greenPrediction = (greenTop + greenLeft) / 2;

                                //Combine the prediction & huffman value to make the output color
                                ColorCombine(greenPrediction, ref newGreen);

                                //Use the change between the old & new green values to project expected
                                // values for the red & blue colors
                                int greenChange = newGreen - greenPrediction;

                                //Combine red values of the pixels to make our predicted pixel color
                                ushort redTop = (byte)((rgbaTop & 0xF800) >> 11);
                                ushort redLeft = (byte)((rgbaLeft & 0xF800) >> 11);
                                int redPrediction = greenChange + (redTop + redLeft) / 2;
                                redPrediction = Math.Max(0, Math.Min(0x1F, redPrediction)); //Keep between 0 and 0x1F

                                //Combine the prediction & huffman value to make the output color
                                ColorCombine(redPrediction, ref newRed);

                                //Combine blue values of the pixels to make our predicted pixel color
                                ushort blueTop = (byte)((rgbaTop & 0x3E) >> 1);
                                ushort blueLeft = (byte)((rgbaLeft & 0x3E) >> 1);
                                int bluePrediction = greenChange + (blueTop + blueLeft) / 2;
                                bluePrediction = Math.Max(0, Math.Min(0x1F, bluePrediction)); //Keep between 0 and 0x1F

                                //Combine the prediction & huffman value to make the output color
                                ColorCombine(bluePrediction, ref newBlue);

                                //Make the newpixel color
                                currentPixelColor = (ushort)((newRed << 11) | (newGreen << 6) | (newBlue << 1));
                                if (currentPixelColor != alphaColor) //Only transparent if it matches the transparency pixel
                                    currentPixelColor |= 0x1;

                                //Add to the front of the color buffer
                                for (int i = rgbaBuffer.Length - 1; i > 0; i--)
                                    rgbaBuffer[i] = rgbaBuffer[i - 1];
                                rgbaBuffer[0] = currentPixelColor;
                            }
                            else //Use existing RGBA
                            {
                                command = masterReader.ReadBits(6); // Returns index of color in color buffer to use

                                currentPixelColor = rgbaBuffer[command];
                                if (command != 0)
                                {
                                    //Bump the selected color to the front of the buffer
                                    for (int i = command; i > 0; i--)
                                        rgbaBuffer[i] = rgbaBuffer[i - 1];
                                    rgbaBuffer[0] = currentPixelColor;
                                }
                            }

                            //Write the RGBA to the imageData
                            ByteHelper.WriteUShort(currentPixelColor, imageData, pixelIndex * 2);
                            lastPixelColor = currentPixelColor;

                            //Add nearby pixels to the colorChangeMap
                            bool hasLeftCol = (col != 0);
                            bool hasRightCol = (col < (header.Width - 1));
                            bool has2RightCols = (col < (header.Width - 2));
                            bool hasDownRow = (row < (header.Height - 1));
                            bool has2DownRows = (row < (header.Height - 2));

                            //Right 1
                            if (hasRightCol)
                                colorChangeMap[pixelIndex + 1]++;
                            //Right 2
                            if (has2RightCols)
                                colorChangeMap[pixelIndex + 2]++;
                            //Down 1 Left 1
                            if (hasDownRow && hasLeftCol)
                                colorChangeMap[pixelIndex + header.Width - 1]++;
                            //Down 1
                            if (hasDownRow)
                                colorChangeMap[pixelIndex + header.Width]++;
                            //Down 1 Right 1
                            if (hasDownRow && hasRightCol)
                                colorChangeMap[pixelIndex + header.Width + 1]++;
                            //Down 2
                            if (has2DownRows)
                                colorChangeMap[pixelIndex + header.Width * 2]++;

                            //Now test to see if we need to continue writing this color down the column
                            command = masterReader.ReadBits(1);//1 - repeat color, 0 - continue

                            if (command == 1) //Repeat color
                            {
                                //Basically move down one row each repeat, possibly moving to the side, and write the color again
                                int pixelOffset = 0;
                                ushort currentPixelColorOpaque = (ushort)(currentPixelColor | 0x1); //Not sure why this is the case, is it to catch it in the first if statement?

                                while (true) //I hate while(true)
                                {
                                    command = masterReader.ReadBits(2);//0 - advanced move, 1 - back one, 2 - no lateral move, 3 - forward one
                                    if (command == 0)
                                    {
                                        //Advanced move
                                        command = masterReader.ReadBits(1);//0 - stop, 1 - advanced move

                                        if (command == 0)
                                        {
                                            break;
                                        }

                                        command = masterReader.ReadBits(1); //0 - move back 2, 1 - move forward 2
                                        if (command == 0)
                                        {
                                            pixelOffset -= 2;
                                        }
                                        else
                                        {
                                            pixelOffset += 2;
                                        }
                                    }
                                    else if (command == 1)
                                    {
                                        pixelOffset--;
                                    }
                                    else if (command == 3)
                                    {
                                        pixelOffset++;
                                    }

                                    pixelOffset += header.Width; //move down a row
                                    ByteHelper.WriteUShort(currentPixelColorOpaque, imageData, (pixelIndex + pixelOffset) * 2);
                                }


                            }
                        }
                    }

                    //Next pixel
                    pixelIndex++;
                }
            }

            return imageData;
        }

        /// <summary>
        /// Creates a Huffman-style binary tree that holds the 5-bit color data referenced in the 
        /// TKMK00 decoding process.
        /// </summary>
        private static TKMK00HuffmanTreeNode SetUpHuffmanTree(uint val, byte[] data, TKMK00CommandReader commandReader)
        {
            TKMK00HuffmanTreeNode newTree = new TKMK00HuffmanTreeNode();

            int command = commandReader.ReadBits(1);//1 - Make new branches, 0 - End current branch

            if (command != 0)
            {
                newTree.Value = (int)val; //Not used, but can't hurt to number it
                val++;
                newTree.Left = SetUpHuffmanTree(val, data, commandReader);
                newTree.Right = SetUpHuffmanTree(val, data, commandReader);
                return newTree;
            }

            //Else, return a node with a value
            int value = 0;
            int bitCount = 5;
            do
            {
                command = commandReader.ReadBits(1);
                value = value * 2 + command; //basically bitshifts s0 to the left and adds v0, aka it's loading 5 bytes straight from the comandReader
                bitCount -= 1;
            } while (bitCount > 0);

            newTree.Value = value;
            return newTree;
        }

        //Only called when creating new rgba value. Traverses the Huffman tree to return the correct value
        private static int RetrieveHuffmanTreeValue(byte[] data, TKMK00HuffmanTreeNode currentNode, TKMK00CommandReader commandReader)
        {
            while (!currentNode.IsEndNode)/*currentNode.Value >= 0x20*/
            {
                int command = commandReader.ReadBits(1); // 0 - left, 1 - right
                if (command == 0)
                    currentNode = currentNode.Left;
                else
                    currentNode = currentNode.Right;
            }

            return currentNode.Value;
        }

        /// <summary>
        /// Combines a predicted color with a new color. Sometimes the new color is just something added to/subtracted
        ///  from the predicted color, others it's just used as the new color. Complex behavior, but it seems to work.
        /// </summary>
        private static void ColorCombine(int predictedColor, ref int newColor)
        {
            //See if our numbers could possibly go over 0x1F or below 0x00 if we do the differential method. If they could,
            // then just use the new color.

            if (predictedColor >= 0x10) //Check if it'll go above 0x1F
            {
                int remainingSpace = (0x1F - predictedColor);
                int incrementDecrementValue = newColor >> 1;
                bool isAdding = (newColor & 0x1) != 0;
                if (remainingSpace < incrementDecrementValue || (remainingSpace == incrementDecrementValue && isAdding)) //Since an extra 1 is used when adding
                {
                    newColor = 0x1F - newColor;
                    return;
                }
            }
            else //Check if it'll go below 0
            {
                int incrementDecrementValue = newColor >> 1;
                bool isAdding = (newColor & 0x1) != 0;
                if (predictedColor < incrementDecrementValue || (predictedColor == incrementDecrementValue && isAdding))
                {
                    return;
                }
            }

            bool addToOld = (newColor & 0x1) != 0; //Last bit is used to determine if adding or subtracting the value
            newColor = newColor >> 1;
            if (addToOld)
                newColor += predictedColor + 1;
            else
                newColor = predictedColor - newColor;

            return;
        }

        #endregion

    }
}