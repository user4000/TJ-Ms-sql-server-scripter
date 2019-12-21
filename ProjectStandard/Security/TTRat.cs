using System;
using System.Linq;
using System.Text;

namespace ProjectStandard
{
  public class TTRat
  {
    public static byte C8 = 8;
    public static byte C255 = 255;
    public static int CByte1 = 256;
    public static int CByte2 = 65536;
    public static int CByte3 = 16777216;
    public static ulong CByte4 = 4294967296;

    public static int TransformationRound1 = 1000;

    public static byte[] SBox = new byte[]
      {0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
      0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
      0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
      0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
      0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
      0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
      0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
      0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
      0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
      0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
      0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
      0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
      0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
      0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
      0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
      0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16};

    public static byte[] InvSBox = new byte[]
    {0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
     0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
     0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
     0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
     0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
     0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
     0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
     0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
     0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
     0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
     0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
     0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
     0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
     0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
     0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
     0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d};

    public static string Debug(byte[] array)
    {
      string s = string.Empty;
      if (array.Length > 100) return ArrayToSmallString(array);
      for (int i = 0; i < array.Length; i++) s += array[i].ToString() + ", ";
      return s;
    }

    public static string ArrayToSmallString(byte[] array)
    {
      string s = string.Empty;
      for (int i = 0; i < 20; i++) s += array[i].ToString() + ", ";
      s += $" ... array length = {array.Length} ... , ";
      for (int i = array.Length - 20; i < array.Length; i++) s += array[i].ToString() + ", ";
      return s;
    }

    public static int Sum(byte[] array) => array.Sum(x => (int)x);

    public static bool ByteArrayCompare(byte[] a1, byte[] a2)
    {
      if (a1.Length != a2.Length) return false;
      for (int i = 0; i < a1.Length; i++) if (a1[i] != a2[i]) return false;
      return true;
    }

    public static int ByteArrayDX(byte[] a1, byte[] a2)
    {
      int sum = 0;
      if (a1.Length != a2.Length) return 0;
      for (int i = 0; i < a1.Length; i++) sum += Math.Abs(a1[i] - a2[i]);
      return sum;
    }

    public static void CopyArray(byte[] Source, byte[] Destination)
    {
      if (Source.Length != Destination.Length) throw new Exception("Ошибка! Размеры массивов не совпадают.");
      for (int j = 0; j < Source.Length; j++) Destination[j] = Source[j];
    }

    public static byte[] CopyArray(byte[] Source)
    {
      byte[] A = new byte[Source.Length];
      for (int j = 0; j < Source.Length; j++) A[j] = Source[j];
      return A;
    }

    public static byte[] RandomArray(int length)
    {
      byte[] array = new byte[length]; TTSecurityStandard.CryptoRandomLocal.NextBytes(array); return array;
    }

    public static byte[] RijndaelSBox(byte[] array)
    {
      byte[] b = new byte[array.Length];
      for (int i = 0; i < array.Length; i++) b[i] = SBox[array[i]];
      return b;
    }

    public static byte[] RijndaelInvSBox(byte[] array)
    {
      byte[] b = new byte[array.Length];
      for (int i = 0; i < array.Length; i++) b[i] = InvSBox[array[i]];
      return b;
    }

    public static byte[] CodeInversion(byte[] array)
    {
      byte[] b = new byte[array.Length];
      for (int i = 0; i < array.Length; i++) b[i] = (byte)(C255 - array[i]);
      return b;
    }

    public static void TjSerpentF(byte[] array)
    {
      int M = array.Length - 2;
      for (int i = M; i >= 0; i--) array[i] = (byte)(array[i + 1] ^ array[i]);
    }

    public static void TjSerpentB(byte[] array)
    {
      int M = array.Length - 2;
      for (int i = 0; i <= M; i++) array[i] = (byte)(array[i + 1] ^ array[i]);
    }

    public static void HTF(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    { // Hadamard Transformation Forward //
      ulong A1 = (ulong)(N1 * CByte3 + N2 * CByte2 + N3 * CByte1 + N4);
      ulong A2 = (ulong)(N5 * CByte3 + N6 * CByte2 + N7 * CByte1 + N8);

      ulong B1 = (ulong)(2 * A1 + A2) % CByte4;
      ulong B2 = (ulong)(A1 + A2) % CByte4;

      N1 = (byte)(B1 >> 24);
      N2 = (byte)((B1 << 8) >> 24);
      N3 = (byte)((B1 << 16) >> 24);
      N4 = (byte)((B1 << 24) >> 24);

      N5 = (byte)(B2 >> 24);
      N6 = (byte)((B2 << 8) >> 24);
      N7 = (byte)((B2 << 16) >> 24);
      N8 = (byte)((B2 << 24) >> 24);
    }

    public static void HTB(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    { // Hadamard Transformation Backward //
      ulong B1 = (ulong)(N1 * CByte3 + N2 * CByte2 + N3 * CByte1 + N4);
      ulong B2 = (ulong)(N5 * CByte3 + N6 * CByte2 + N7 * CByte1 + N8);

      ulong A1 = (ulong)((B1 + CByte4 - B2) % CByte4);
      ulong A2 = (ulong)((2 * B2 + CByte4 - B1) % CByte4);

      N1 = (byte)(A1 >> 24);
      N2 = (byte)((A1 << 8) >> 24);
      N3 = (byte)((A1 << 16) >> 24);
      N4 = (byte)((A1 << 24) >> 24);

      N5 = (byte)(A2 >> 24);
      N6 = (byte)((A2 << 8) >> 24);
      N7 = (byte)((A2 << 16) >> 24);
      N8 = (byte)((A2 << 24) >> 24);
    }

    public static byte[] RandomSmallArray(bool LengthInTwoFirstBytes, int MinArrayLength, int MaxArrayLength)
    {
      if (MinArrayLength < 4) MinArrayLength = 4;
      if (MaxArrayLength < MinArrayLength) MaxArrayLength = MinArrayLength;

      ushort Length = (ushort)TTSecurityStandard.CryptoRandomLocal.Next(MinArrayLength, MaxArrayLength);
      byte UpperByte = (byte)(Length >> 8);
      byte LowerByte = (byte)(Length & 0xff);
      byte[] array = RandomArray(Length);
      if (LengthInTwoFirstBytes)
      {
        array[0] = UpperByte;
        array[1] = LowerByte;
      }
      else
      {
        array[Length - 2] = UpperByte;
        array[Length - 1] = LowerByte;
      }
      return array;
    }

    public static byte[] AddRandomArraysFromRightAndFromLeft(byte[] array, int LeftMin, int LeftMax, int RightMin, int RightMax)
    { // К массиву добавим слева и справа случайные данные //
      byte[] ArrayLeft = RandomSmallArray(true, LeftMin, LeftMax);
      byte[] ArrayRight = RandomSmallArray(false, RightMin, RightMax);
      byte[] ArrayResult = new byte[ArrayLeft.Length + array.Length + ArrayRight.Length];
      for (int i = 0; i < ArrayLeft.Length; i++) ArrayResult[i] = ArrayLeft[i];
      for (int i = 0; i < array.Length; i++) ArrayResult[ArrayLeft.Length + i] = array[i];
      for (int i = 0; i < ArrayRight.Length; i++) ArrayResult[ArrayLeft.Length + array.Length + i] = ArrayRight[i];
      return ArrayResult;
    }

    public static byte[] RemoveRandomArraysFromRightAndFromLeft(byte[] array)
    { // Убираем слева и справа случайные данные и восстанавливаем исходную информацию в середине массива //
      int L = array.Length;

      byte UpperByte1 = array[0];
      byte LowerByte1 = array[1];

      byte UpperByte2 = array[L - 2];
      byte LowerByte2 = array[L - 1];

      int L1 = UpperByte1 * CByte1 + LowerByte1;
      int L2 = UpperByte2 * CByte1 + LowerByte2;

      int ResultLength = L - L1 - L2;

      byte[] ResultArray = new byte[ResultLength];

      for (int i = L1; i < L1 + ResultLength; i++) ResultArray[i - L1] = array[i];

      return ResultArray;
    }

    public static void HTF(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;
      for (int i = 1; i <= M; i++)
      {
        j = (uint)(i - 1) * C8;
        HTF(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
      if (L % C8 > 0)
        HTF(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);
    }

    public static void HTB(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;

      if (L % C8 > 0)
        HTB(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);

      for (int i = (int)M; i >= 1; i--)
      {
        j = (uint)(i - 1) * C8;
        HTB(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
    }

    public static void CodeShiftF(byte[] array, byte[] shift)
    {
      int L = array.Length;
      int K = shift.Length;
      if ((L < 1) || (K < 2)) return;
      for (int i = 0; i < L; i++) array[i] = (byte)((array[i] + shift[i % K]) % CByte1);
    }

    public static void XOR(byte[] array, byte[] shift)
    {
      int L = array.Length;
      int K = shift.Length;
      if ((L < 1) || (K < 2)) return;
      for (int i = 0; i < L; i++) array[i] = (byte)((array[i] ^ shift[i % K]));
    }

    public static void CodeShiftB(byte[] array, byte[] shift)
    {
      int L = array.Length;
      int K = shift.Length;
      if ((L < 1) || (K < 2)) return;
      for (int i = 0; i < L; i++) array[i] = (byte)((array[i] + CByte1 - shift[i % K]) % CByte1);
    }

    public static void ShiftF(byte[] array, int shift)
    {
      int L = array.Length;
      if (L < 2) return;
      shift = (Math.Abs(shift) % L);
      if (shift == 0) return;

      byte[] EE = CopyArray(array);

      int X = shift - 1;
      int Y = L - shift;
      for (int i = 0; i <= X; i++) array[i] = EE[i + Y];
      for (int i = shift; i < L; i++) array[i] = EE[i - shift];

    }

    public static void ShiftB(byte[] array, int shift)
    {
      int L = array.Length;
      if (L < 2) return;
      shift = (Math.Abs(shift) % L);
      if (shift == 0) return;

      shift = L - shift;
      byte[] EE = CopyArray(array);

      int X = shift - 1;
      int Y = L - shift;
      for (int i = 0; i <= X; i++) array[i] = EE[i + Y];
      for (int i = shift; i < L; i++) array[i] = EE[i - shift];
    }

    public static void Tj1F(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    {
      byte x1 = N1; byte x2 = N2; byte x3 = N3; byte x4 = N4; byte x5 = N5; byte x6 = N6; byte x7 = N7; byte x8 = N8;
      N1 = (byte)(x2 ^ x4 ^ x8);
      N2 = (byte)(x1 ^ x3 ^ x7);
      N3 = (byte)(x2 ^ x3 ^ x6);
      N4 = (byte)(x1 ^ x4 ^ x5);
      N5 = (byte)(N4 ^ x3 ^ x6 ^ x7);
      N6 = (byte)(N3 ^ x4 ^ x5 ^ x8);
      N7 = (byte)(x3 ^ x5 ^ x7 ^ x8);
      N8 = (byte)(x4 ^ x6 ^ x7 ^ x8);
    }

    public static void Tj1B(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    {
      byte x1 = N1; byte x2 = N2; byte x3 = N3; byte x4 = N4; byte x5 = N5; byte x6 = N6; byte x7 = N7; byte x8 = N8;
      N1 = (byte)(x1 ^ x2 ^ x3 ^ x8);
      N2 = (byte)(x1 ^ x2 ^ x4 ^ x7);
      N5 = (byte)(x1 ^ x3 ^ x6 ^ N2);
      N4 = (byte)(x4 ^ N1 ^ N5);
      N8 = (byte)(x1 ^ N2 ^ N4);
      N3 = (byte)(x1 ^ x8 ^ x5 ^ N1 ^ N2 ^ N4 ^ N5);
      N7 = (byte)(x2 ^ N1 ^ N3);
      N6 = (byte)(x3 ^ N2 ^ N3);
    }

    public static void Tj2F(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    {
      byte x1 = N1; byte x2 = N2; byte x3 = N3; byte x4 = N4; byte x5 = N5; byte x6 = N6; byte x7 = N7; byte x8 = N8;
      N1 = (byte)(x3 ^ x5 ^ x7);
      N2 = (byte)(x4 ^ x6 ^ x8);
      N3 = (byte)(x1 ^ x5 ^ x8);
      N4 = (byte)(x2 ^ x6 ^ x7);
      N5 = (byte)(x3 ^ x4 ^ x2);
      N6 = (byte)(x1 ^ x3 ^ x8);
      N7 = (byte)(x2 ^ x4 ^ x6);
      N8 = (byte)(x1 ^ x5 ^ x7);
    }

    public static void Tj2B(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    {
      byte x1 = N1; byte x2 = N2; byte x3 = N3; byte x4 = N4; byte x5 = N5; byte x6 = N6; byte x7 = N7; byte x8 = N8;
      N8 = (byte)(x1 ^ x8 ^ x6);
      N7 = (byte)(x3 ^ x8 ^ N8);
      N4 = (byte)(x4 ^ x7 ^ N7);
      N2 = (byte)(x2 ^ x4 ^ N4 ^ N7 ^ N8);
      N6 = (byte)(x4 ^ N2 ^ N7);
      N3 = (byte)(x5 ^ N2 ^ N4);
      N5 = (byte)(x1 ^ N3 ^ N7);
      N1 = (byte)(x6 ^ N3 ^ N8);
    }

    public static void Tj3F(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    {
      byte x1 = N1; byte x2 = N2; byte x3 = N3; byte x4 = N4; byte x5 = N5; byte x6 = N6; byte x7 = N7; byte x8 = N8;
      N1 = (byte)(x8 ^ x5);
      N2 = (byte)(x6 ^ x7);
      N5 = (byte)(x1 ^ x5);
      N6 = (byte)(x2 ^ x6);
      N7 = (byte)(N6 ^ x3 ^ x7);
      N8 = (byte)(N5 ^ x4 ^ x8);
      N3 = (byte)(N8 ^ x7);
      N4 = (byte)(N7 ^ x8);
    }

    public static void Tj3B(ref byte N1, ref byte N2, ref byte N3, ref byte N4, ref byte N5, ref byte N6, ref byte N7, ref byte N8)
    {
      byte x1 = N1; byte x2 = N2; byte x3 = N3; byte x4 = N4; byte x5 = N5; byte x6 = N6; byte x7 = N7; byte x8 = N8;
      N8 = (byte)(x4 ^ x7);
      N7 = (byte)(x3 ^ x8);
      N5 = (byte)(N8 ^ x1);
      N1 = (byte)(N5 ^ x5);
      N6 = (byte)(N7 ^ x2);
      N2 = (byte)(N6 ^ x6);
      N3 = (byte)(N7 ^ x6 ^ x7);
      N4 = (byte)(N8 ^ x5 ^ x8);
    }

    public static void Tj1F(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;
      for (int i = 1; i <= M; i++)
      {
        j = (uint)(i - 1) * C8;
        Tj1F(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
      if (L % C8 > 0)
        Tj1F(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);
    }

    public static void Tj2F(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;
      for (int i = 1; i <= M; i++)
      {
        j = (uint)(i - 1) * C8;
        Tj2F(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
      if (L % C8 > 0)
        Tj2F(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);
    }

    public static void Tj3F(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;
      for (int i = 1; i <= M; i++)
      {
        j = (uint)(i - 1) * C8;
        Tj3F(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
      if (L % C8 > 0)
        Tj3F(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);
    }

    public static void Tj1B(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;

      if (L % C8 > 0)
        Tj1B(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);

      for (int i = (int)M; i >= 1; i--)
      {
        j = (uint)(i - 1) * C8;
        Tj1B(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
    }

    public static void Tj2B(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;

      if (L % C8 > 0)
        Tj2B(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);

      for (int i = (int)M; i >= 1; i--)
      {
        j = (uint)(i - 1) * C8;
        Tj2B(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
    }

    public static void Tj3B(byte[] array)
    {
      uint L = (uint)array.Length;
      if (L < C8) return;
      uint M = L / C8;
      uint j = 0;

      if (L % C8 > 0)
        Tj3B(ref array[L - 8], ref array[L - 7], ref array[L - 6], ref array[L - 5], ref array[L - 4], ref array[L - 3], ref array[L - 2], ref array[L - 1]);

      for (int i = (int)M; i >= 1; i--)
      {
        j = (uint)(i - 1) * C8;
        Tj3B(ref array[j], ref array[j + 1], ref array[j + 2], ref array[j + 3], ref array[j + 4], ref array[j + 5], ref array[j + 6], ref array[j + 7]);
      }
    }

    public static string TransformF(string s)
    {   
      byte[] AA = Encoding.UTF8.GetBytes(s);    
      //---------------------------------------------------------------------------------------------------
      XOR(AA, TTConstArray128.ListArray[0]);
      Tj3F(AA);
      AA = RijndaelSBox(AA);
      Tj2F(AA);
      AA = AddRandomArraysFromRightAndFromLeft(AA, 117, 229, 117, 229);
      Tj1F(AA);
      //---------------------------------------------------------------------------------------------------
      for ( int i=1; i <= TransformationRound1; i++ )
      {
        TjSerpentF(AA);
        Tj1F(AA);
        HTF(AA);
        Tj2F(AA);
        XOR(AA, TTConstArray128.ListArray[i % TTConstArray128.Array.Length]);
        Tj3F(AA);
        ShiftF(AA, i);
        AA = RijndaelSBox(AA);
      }
      //---------------------------------------------------------------------------------------------------
      s = Convert.ToBase64String(AA);
      return s;
    }

    public static string TransformB(string s)
    {   
      byte[] AA = Convert.FromBase64String(s);    
      //---------------------------------------------------------------------------------------------------
      for ( int i = TransformationRound1; i >= 1; i-- )
      {
        AA = RijndaelInvSBox(AA);
        ShiftB(AA, i);
        Tj3B(AA);
        XOR(AA, TTConstArray128.ListArray[i % TTConstArray128.Array.Length]);
        Tj2B(AA);
        HTB(AA);
        Tj1B(AA);
        TjSerpentB(AA);
      }
      //---------------------------------------------------------------------------------------------------
      Tj1B(AA);
      AA = RemoveRandomArraysFromRightAndFromLeft(AA);
      Tj2B(AA);
      AA = RijndaelInvSBox(AA);
      Tj3B(AA);
      XOR(AA, TTConstArray128.ListArray[0]);
      //---------------------------------------------------------------------------------------------------
      s = Encoding.UTF8.GetString(AA);
      return s;
    }
  }
}

/*



*/

